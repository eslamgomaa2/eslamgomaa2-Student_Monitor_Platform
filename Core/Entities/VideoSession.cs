using System;
using System.Collections.Generic;

namespace StudentBehaviorPlatform.Data.Entities
{
    public class VideoSession
    {
        public int SessionID { get; set; }
        public int UploadedByUserID { get; set; }
        public string? FilePath { get; set; }
        public string Status { get; set; } = "Pending";
        public string? ClassroomRef { get; set; }
        public DateTime RecordedAt { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ApplicationUser? UploadedByUser { get; set; }
        public ICollection<AIAnalysisResult> AIAnalysisResults { get; set; } = [];
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
    }
}