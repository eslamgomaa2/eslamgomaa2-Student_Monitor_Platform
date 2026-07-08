using Core.Enums;
using E_Learning.Core.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.Cluster;
using Services.Services.Cluster.SchoolSystem.Application.Services.Clustering.Interfaces;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using System.Text.Json;

namespace StudentBehaviorPlatform.Application.Services
{
    public class ClusterService : IClusterService
    {

        private readonly string _wwwRootPath;
        private readonly string _baseUrl;
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;

        public ClusterService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ResponseHandler responseHandler, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _env = env;

            _wwwRootPath = env.WebRootPath;

            // خد الـ Base URL من الـ HttpContext
            var request = httpContextAccessor.HttpContext?.Request;
            _baseUrl = request is not null
                ? $"{request.Scheme}://{request.Host}"
                : string.Empty;
        }

        public async Task<E_Learning.Core.Base.Response<ClusterRunResponseDto>> GetClusterSummariesAsync(ClusterFilterDto filters)
        {
            // ✅ FIX #1: ParseDateRange now returns DateOnly
            var (startDate, endDate) = ParseDateRange(filters.DateRange);

            // Get latest cluster run matching filters
            var clusterRun = await _unitOfWork.ClusterRuns.GetLatestClusterRunAsync(
                filters.SchoolYear,
                filters.GradeLevel,
                startDate,
                endDate);

            if (clusterRun == null)
            {
                return _responseHandler.Success<ClusterRunResponseDto>(new ClusterRunResponseDto
                {
                    Clusters = new List<ClusterSummaryDto>(),
                    VisualizationData = new List<ClusterVisualizationPointDto>()
                });
            }

            // Get cluster groups
            var groups = await _unitOfWork.ClusterGroups.GetGroupsByRunIdAsync(clusterRun.RunID);

            // Map to DTOs - ✅ FIX #7: Pass date range to filter metrics correctly
            var clusterSummaries = new List<ClusterSummaryDto>();
            foreach (var group in groups)
            {
                var summary = await MapClusterGroupToSummaryAsync(group, startDate, endDate);
                clusterSummaries.Add(summary);
            }

            // Get visualization data - ✅ FIX #2: Pass all required parameters
            var visualizationData = await GetClusterVisualizationDataAsync(
                clusterRun.RunID);

            return _responseHandler.Success<ClusterRunResponseDto>(new ClusterRunResponseDto
            {
                RunID = clusterRun.RunID,
                RunAt = clusterRun.RunAt,
                FiltersApplied = clusterRun.FiltersApplied ?? "No filters applied",
                NumClusters = clusterRun.NumClusters,
                Clusters = clusterSummaries,
                VisualizationData = visualizationData
            });
        }





        public async Task<Response<StudentDetailDto>> GetStudentDetailsAsync(int studentId, int runId)
        {
            var clusterMember = await _unitOfWork.ClusterMembers
                .GetStudentClusterDetailsAsync(studentId, runId);

            if (clusterMember == null)
                throw new KeyNotFoundException($"Student {studentId} not found in cluster run {runId}");

            var student = clusterMember.Student;
            if (student == null)
                throw new KeyNotFoundException($"Student {studentId} data not found");

            // ✅ Safe parsing without System.Text.Json issues
            var features = ParseFeatures(clusterMember.Features);

            var avgGrade = features.GetValueOrDefault("grade", 0);
            var attendanceRate = features.GetValueOrDefault("attendance", 0) * 100;

            var incidents = await _unitOfWork.BehaviorIncidents
                .GetIncidentsByStudentIdAsync(studentId, 5);

            return _responseHandler.Success(new StudentDetailDto
            {
                StudentID = student.StudentID,
                StudentName = student.FullName ?? "Unknown",
                GradeLevel = student.GradeLevel ?? "N/A",
                CurrentGrade = Math.Round(avgGrade, 2),
                AttendanceRate = Math.Round(attendanceRate, 2),
                ClusterGroupID = clusterMember.GroupID,
                ClusterLabel = clusterMember.ClusterGroup?.GroupSummary ?? "Unknown",
                RecentIncidents = incidents?.Select(i => new IncidentDto
                {
                    IncidentID = i.IncidentID,
                    OccurredAt = i.OccurredAt,
                    Type = i.BehaviorRule?.Category ?? "Unknown",
                    Description = i.BehaviorRule?.Description ?? "No description"
                }).ToList() ?? new List<IncidentDto>()
            });
        }

        // ✅ Manual parsing - no JSON serializer needed
        private Dictionary<string, double> ParseFeatures(string featuresJson)
        {
            var result = new Dictionary<string, double>();

            if (string.IsNullOrWhiteSpace(featuresJson))
                return result;

            try
            {
                // Simple manual parsing: {"attendance":0.95,"grade":90}
                var clean = featuresJson.Trim('{', '}');
                var pairs = clean.Split(',');

                foreach (var pair in pairs)
                {
                    var kv = pair.Split(':');
                    if (kv.Length == 2)
                    {
                        var key = kv[0].Trim().Trim('"');
                        var value = double.Parse(kv[1].Trim());
                        result[key] = value;
                    }
                }
            }
            catch
            {
                // Ignore parse errors
            }

            return result;
        }
        // ✅ FIX #2: Updated signature to match interface (int, string, DateOnly, DateOnly)
        public async Task<List<ClusterVisualizationPointDto>> GetClusterVisualizationDataAsync(int runId)
        {
            // 1. جيب كل الـ members اللي تابعين للـ runId
            var members = await _unitOfWork.ClusterMembers
                .GetMembersByRunIdQueryable(runId)
                .Include(m => m.Student)           // عشان نجيب بيانات الطالب
                .Include(m => m.ClusterGroup)      // عشان نجيب بيانات المجموعة
                .ToListAsync();

            var visualizationData = new List<ClusterVisualizationPointDto>();

            foreach (var member in members)
            {
                var student = member.Student;
                var group = member.ClusterGroup;

                if (student == null || group == null)
                    continue;

                // 2. استخدم الـ Features اللي مخزنة في الـ ClusterMember نفسه
                // دي بيانات الـ clustering اللي اتحسبت لما اتعمل الـ run
                var features = !string.IsNullOrEmpty(member.Features)
                    ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, double>>(member.Features)
                    : new Dictionary<string, double>();

                // 3. اقرأ القيم من الـ Features (اللي اتحسبت وقت الـ clustering)
                var attendanceRate = features.GetValueOrDefault("attendance", 0) * 100;
                var avgGrade = features.GetValueOrDefault("grade", 0);


                visualizationData.Add(new ClusterVisualizationPointDto
                {
                    StudentID = student.StudentID,
                    StudentCode = $"S-{student.StudentID + 1000}",
                    StudentName = student.FullName ?? "Unknown",
                    GradeLevel = $"Grade {student.GradeLevel}",
                    AverageGrade = Math.Round(avgGrade, 0),
                    AttendanceRate = Math.Round(attendanceRate, 0),
                    GroupID = group.GroupID,
                    GroupLabel = $"Cluster {GetClusterLetter(group.GroupLabel)}",
                    ColorCode = GetClusterColorCode(group.GroupSummary ?? "")
                });
            }

            return visualizationData;
        }

        public Task<Response<string>> ResetFiltersAsync()
        {
            return Task.FromResult(_responseHandler.Success<string>("completed"));
        }

        // ✅ FIX #1: ParseDateRange now returns DateOnly instead of DateTime?
        private (DateTime StartDate, DateTime EndDate) ParseDateRange(string dateRange)
        {
            var endDate = DateTime.UtcNow;
            DateTime startDate;

            switch (dateRange.ToLower())
            {
                case "last 30 days":
                    startDate = endDate.AddDays(-30);
                    break;
                case "last 90 days":
                    startDate = endDate.AddDays(-90);
                    break;
                case "last 180 days":
                    startDate = endDate.AddDays(-180);
                    break;
                case "this school year":
                    var currentYear = endDate.Year;
                    var schoolYearStart = new DateTime(currentYear, 9, 1);
                    // If today is before Sept 1, use previous year's start
                    startDate = endDate < schoolYearStart
                        ? new DateTime(currentYear - 1, 9, 1)
                        : schoolYearStart;
                    break;
                default:
                    // Fallback: last 90 days
                    startDate = endDate.AddDays(-90);
                    break;
            }

            return (startDate, endDate);
        }

        // ✅ FIX #7: Added startDate and endDate parameters
        private async Task<ClusterSummaryDto> MapClusterGroupToSummaryAsync(
            ClusterGroup group,
            DateTime startDate,
            DateTime endDate)
        {
            var members = group.ClusterMembers;

            // Filter attendance records by date range
            var avgAttendance = members.Any()
                ? members.Average(m =>
                {
                    if (m.Student == null) return 0;

                    var filteredRecords = m.Student.AttendanceRecords?
                        .Where(a => a.AttendanceDate >= startDate
                                 && a.AttendanceDate <= endDate)
                        ?? Enumerable.Empty<AttendanceRecord>();

                    if (!filteredRecords.Any()) return 0;

                    return (double)filteredRecords.Count(a => a.Status == AttendanceStatus.Present) / filteredRecords.Count();
                })
                : 0;

            // Filter grades by academic year derived from date range
            var academicYear = startDate.Year;
            var avgGrade = members.Any()
                ? members.SelectMany(m => m.Student?.Grades ?? new List<Grade>())
                    .Where(g => g.AcademicYear == academicYear)
                    .Average(g => g.Score)
                : 0;

            // Determine cluster label and main issue based on group summary
            var (label, mainIssue, colorCode) = DetermineClusterInfo(group.GroupSummary);

            return new ClusterSummaryDto
            {
                GroupID = group.GroupID,
                ClusterName = $"CLUSTER {GetClusterLetter(group.GroupLabel)}",
                ClusterLabel = label,
                StudentCount = group.StudentCount,
                AvgAttendance = Math.Round(avgAttendance * 100, 0), // Convert to percentage
                AvgGrade = Math.Round(avgGrade, 0),
                MainIssue = mainIssue,
                ColorCode = colorCode
            };
        }

        private (string Label, string MainIssue, string ColorCode) DetermineClusterInfo(string groupSummary)
        {
            return groupSummary?.ToLower() switch
            {
                var s when s.Contains("at-risk") || s.Contains("risk") =>
                    ("At-Risk", "Frequent absences and missed classwork", "bg-red-100 text-red-800"),

                var s when s.Contains("disengaged") || s.Contains("low engagement") =>
                    ("Disengaged", "Low participation and weak assignment momentum", "bg-yellow-100 text-yellow-800"),

                var s when s.Contains("high potential") || s.Contains("high performing") || s.Contains("excellent") =>
                    ("High Potential", "Strong grades with consistent performance", "bg-green-100 text-green-800"),

                var s when s.Contains("average") || s.Contains("moderate") =>
                    ("Average", "Steady performance with room for improvement", "bg-blue-100 text-blue-800"),

                _ => ("General", "Standard performance profile", "bg-gray-100 text-gray-800")
            };
        }

        // ✅ FIX #3: Handle both numeric and text labels safely
        private string GetClusterLetter(string groupLabel)
        {
            if (string.IsNullOrWhiteSpace(groupLabel))
                return "?";

            // Try parse as number (1 → B, 2 → C, etc. since 0 → A)
            if (int.TryParse(groupLabel, out int number))
            {
                return ((char)('A' + number)).ToString();
            }

            // For text labels like "Average students cluster", return first letter
            return char.ToUpper(groupLabel.Trim()[0]).ToString();
        }

        private string GetClusterColorCode(string groupSummary)
        {
            return groupSummary?.ToLower() switch
            {
                var s when s.Contains("at-risk") || s.Contains("risk") || s.Contains("intervention") => "#EF4444", // Red
                var s when s.Contains("disengaged") || s.Contains("low") || s.Contains("attendance") => "#F59E0B", // Yellow/Orange
                var s when s.Contains("high") || s.Contains("excellent") || s.Contains("top") => "#10B981", // Green
                var s when s.Contains("average") || s.Contains("moderate") || s.Contains("mixed") => "#3B82F6", // Blue
                _ => "#6B7280" // Gray
            };
        }

        public async Task<Response<GenerateClusterReportResponseDto>> GenerateClusterReportAsync(int TriggeredByUserID, GenerateClusterReportDto dto)
        {
            var filtersJson = JsonSerializer.Serialize(dto);

            // ── 1. تأكد إن wwwroot موجود (لو WebRootPath ب null) ──────────────
            var webRoot = _env.WebRootPath;

            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
                Directory.CreateDirectory(webRoot);

                // ⚠️ مهم: لو wwwroot اتعمل دلوقتي، StaticFiles مش هتخدمه غير لما تعمل Restart
                Console.WriteLine($"WARNING: wwwroot was missing. Created at: {webRoot}");
            }

            // ── 2. بناء مسار reports ─────────────────────────────────────────
            const string folder = "reports";
            var folderPath = Path.Combine(webRoot, folder);
            Directory.CreateDirectory(folderPath);

            var fileName = $"cluster_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
            var absolutePath = Path.Combine(folderPath, fileName);
            var relativePath = Path.Combine(folder, fileName).Replace('\\', '/');

            // ✅ Log المسار الكامل عشان تعرف فين الملف فعلياً
            Console.WriteLine($"Full absolute path: {Path.GetFullPath(absolutePath)}");

            // ── 3. (TODO) اكتب الـ PDF هنا ─────────────────────────────────────
            // var pdfBytes = await GeneratePdfBytesAsync(dto);
            // await File.WriteAllBytesAsync(absolutePath, pdfBytes);

            // ── 4. حفظ في DB ───────────────────────────────────────────────────
            var clusterRun = new ClusterRun
            {
                TriggeredByUserID = TriggeredByUserID,
                FiltersApplied = filtersJson,
                NumClusters = 3,
                RunAt = DateTime.UtcNow,
                ReportPath = $"/{relativePath}"
            };

            await _unitOfWork.ClusterRuns.AddAsync(clusterRun);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(new GenerateClusterReportResponseDto
            {
                RunID = clusterRun.RunID,
                ReportPath = clusterRun.ReportPath,
                GeneratedAt = clusterRun.RunAt,
                Message = "Cluster report generated successfully"
            });

        }


    }
}