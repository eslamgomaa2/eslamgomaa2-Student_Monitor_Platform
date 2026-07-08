using E_Learning.Core.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Dtos.Student;
using Services.Services.FaceRecognition;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using StudentBehaviorPlatform.Services.Interfaces;

namespace StudentBehaviorPlatform.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFaceRecognitionService _faceRecognitionService;
        private readonly ResponseHandler _responseHandler;

        private static readonly string[] AllowedNoteTypes =
            ["Reading", "Assignment", "Assessment", ];

        public StudentService(IUnitOfWork unitOfWork, ResponseHandler responseHandler, IFaceRecognitionService faceRecognitionService)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _faceRecognitionService = faceRecognitionService;
        }

        public async Task<Response<IEnumerable<StudentDto>>> GetAllStudentsAsync()
        {
            var students = await _unitOfWork.Students.GetAllStudentsAsync();
            return _responseHandler.Success(students.Select(MapToStudentDto));
        }

        public async Task<Response<StudentDto>> GetStudentByIdAsync(int studentId)
        {
            if (studentId <= 0)
                return _responseHandler.BadRequest<StudentDto>("Student ID must be a positive number.");

            var student = await _unitOfWork.Students.GetStudentByIdAsync(studentId);
            if (student is null)
                return _responseHandler.NotFound<StudentDto>($"Student with ID {studentId} was not found.");

            return _responseHandler.Success(MapToStudentDto(student));
        }

        public async Task<Response<IEnumerable<AttendanceRecordDto>>> GetStudentAttendanceAsync(int studentId)
        {
            if (studentId <= 0)
                return _responseHandler.BadRequest<IEnumerable<AttendanceRecordDto>>("Student ID must be a positive number.");

            var student = await _unitOfWork.Students.GetStudentByIdAsync(studentId);
            if (student is null)
                return _responseHandler.NotFound<IEnumerable<AttendanceRecordDto>>($"Student with ID {studentId} was not found.");

            var records = await _unitOfWork.Students.GetStudentAttendanceAsync(studentId);
            return _responseHandler.Success(records.Select(a => new AttendanceRecordDto(
                AttendanceID: a.AttendanceID,
                StudentID: a.StudentID,
                AttendanceDate: a.AttendanceDate,
                Status: a.Status,
                ConfidenceScore: a.ConfidenceScore,
                Source: a.Source
            )));
        }

        public async Task<Response<IEnumerable<GradeDto>>> GetStudentGradesAsync(int studentId)
        {
            if (studentId <= 0)
                return _responseHandler.BadRequest<IEnumerable<GradeDto>>("Student ID must be a positive number.");

            var student = await _unitOfWork.Students.GetStudentByIdAsync(studentId);
            if (student is null)
                return _responseHandler.NotFound<IEnumerable<GradeDto>>($"Student with ID {studentId} was not found.");

            var grades = await _unitOfWork.Students.GetStudentGradesAsync(studentId);
            return _responseHandler.Success(grades.Select(g => new GradeDto(
                g.GradeID,
                g.StudentID,
                g.Subject,
                g.Score,
                g.GradeLabel,
                g.Term,
                g.AcademicYear
            )));
        }

        public async Task<Response<IEnumerable<BehaviorIncidentDto>>> GetStudentBehaviorHistoryAsync(int studentId)
        {
            if (studentId <= 0)
                return _responseHandler.BadRequest<IEnumerable<BehaviorIncidentDto>>("Student ID must be a positive number.");

            var student = await _unitOfWork.Students.GetStudentByIdAsync(studentId);
            if (student is null)
                return _responseHandler.NotFound<IEnumerable<BehaviorIncidentDto>>($"Student with ID {studentId} was not found.");

            var incidents = await _unitOfWork.Students.GetStudentBehaviorHistoryAsync(studentId);
            return _responseHandler.Success(incidents.Select(b => new BehaviorIncidentDto(
                b.IncidentID,
                b.StudentID,
                b.RuleID,
                b.Source,
                b.Detail,
                b.Confidence,
                b.OccurredAt,
                b.ReviewedByUserID,
                b.ReviewStatus
            )));
        }

        public async Task<Response<StudentNoteDto>> AddNoteAsync(int studentId, int userId, CreateNoteDto dto)
        {
          
            if (studentId <= 0)
                return _responseHandler.BadRequest<StudentNoteDto>("Student ID must be a positive number.");

            if (userId <= 0)
                return _responseHandler.BadRequest<StudentNoteDto>("User ID must be a positive number.");

            
            if (string.IsNullOrWhiteSpace(dto.NoteText))
                return _responseHandler.BadRequest<StudentNoteDto>("Note text is required.");

            if (dto.NoteText.Trim().Length < 10)
                return _responseHandler.BadRequest<StudentNoteDto>("Note text must be at least 10 characters.");

            if (dto.NoteText.Trim().Length > 1000)
                return _responseHandler.BadRequest<StudentNoteDto>("Note text must not exceed 1000 characters.");

            if (dto.NoteType is not null && !AllowedNoteTypes.Contains(dto.NoteType))
                return _responseHandler.BadRequest<StudentNoteDto>(
                    $"NoteType must be one of: {string.Join(", ", AllowedNoteTypes)}.");

            
            var student = await _unitOfWork.Students.GetStudentByIdAsync(studentId);
            if (student is null)
                return _responseHandler.NotFound<StudentNoteDto>($"Student with ID {studentId} was not found.");

            
            var note = new StudentNote
            {
                StudentID = studentId,
                UserID = userId,
                NoteText = dto.NoteText.Trim(),
                NoteType = dto.NoteType,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Students.AddNoteAsync(note);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Created(MapToNoteDto(note));
        }

        public async Task<Response<StudentNoteDto>> UpdateNoteAsync(int noteId, int userId, UpdateNoteDto dto)
        {
            
            if (noteId <= 0)
                return _responseHandler.BadRequest<StudentNoteDto>("Note ID must be a positive number.");

            if (userId <= 0)
                return _responseHandler.BadRequest<StudentNoteDto>("User ID must be a positive number.");

           
            if (string.IsNullOrWhiteSpace(dto.NoteText))
                return _responseHandler.BadRequest<StudentNoteDto>("Note text is required.");

            if (dto.NoteText.Trim().Length < 10)
                return _responseHandler.BadRequest<StudentNoteDto>("Note text must be at least 10 characters.");

            if (dto.NoteText.Trim().Length > 1000)
                return _responseHandler.BadRequest<StudentNoteDto>("Note text must not exceed 1000 characters.");

            
            if (dto.NoteType is not null && !AllowedNoteTypes.Contains(dto.NoteType))
                return _responseHandler.BadRequest<StudentNoteDto>(
                    $"NoteType must be one of: {string.Join(", ", AllowedNoteTypes)}.");

            
            var note = await _unitOfWork.Students.GetNoteByIdAsync(noteId);
            if (note is null)
                return _responseHandler.NotFound<StudentNoteDto>($"Note with ID {noteId} was not found.");

            
            if (note.UserID != userId)
                return _responseHandler.Forbidden<StudentNoteDto>("You are not allowed to edit this note.");

            note.NoteText = dto.NoteText.Trim();
            note.NoteType = dto.NoteType;

            _unitOfWork.Students.UpdateNote(note);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(MapToNoteDto(note));
        }

       



        public async Task<Response<IEnumerable<StudentNoteDto>>> GetNotesAsync()
        {
            var notes = await _unitOfWork.StudentNote.GetAllAsync();
            return _responseHandler.Success(notes.Select(MapToNoteDto));
        }
        private static StudentNoteDto MapToNoteDto(StudentNote n) => new(
           n.NoteID, n.StudentID, n.UserID,
           n.NoteText, n.NoteType, n.CreatedAt
       );

        public async Task<Response<IEnumerable<StudentDto>>> AddStudent(AddStudentDto studentDto, CancellationToken ct = default)
        {
            if (studentDto == null || studentDto.ImageFile == null)
                return _responseHandler.BadRequest<IEnumerable<StudentDto>>("Student Informations Needed");

            IDbContextTransaction? transaction = null;

            try
            {
                // 1️⃣ فتح الـ Transaction
                transaction = await _unitOfWork.BeginTransactionAsync(ct);

               
                var student = new Student
                {
                    FullName = studentDto.FullName,
                    NationalID = studentDto.NationalID,
                    DateOfBirth = studentDto.DateOfBirth,
                    Gender = studentDto.Gender,
                    GradeLevel = studentDto.GradeLevel,
                    Section = studentDto.Section,
                    AcademicYear = studentDto.AcademicYear,
                    IsActive = true
                };

                await _unitOfWork.Students.AddAsync(student, ct);
                await _unitOfWork.SaveChangesAsync();

                await using var imageStream = studentDto.ImageFile.OpenReadStream();

                if (imageStream.Length == 0)
                   _responseHandler.BadRequest<IEnumerable<StudentDto>>("the file is Empty");

                
                var aiSuccess = await _faceRecognitionService.RegisterFaceAsync(
                    student.StudentID,
                    studentDto.StudentCode,
                    student.FullName,
                    imageStream,
                    ct);

                if (!aiSuccess)
                    _responseHandler.BadRequest<IEnumerable<StudentDto>>("Fail in Register the student in Ai model");

                await transaction.CommitAsync(ct);

                
                var resultDto = MapToStudentDto(student); 
                return _responseHandler.Success<IEnumerable<StudentDto>>(new[] { resultDto });
            }
            catch (Exception ex)
            {
                
                if (transaction != null)
                    await transaction.RollbackAsync(ct);


                return _responseHandler.BadRequest<IEnumerable<StudentDto>>($" Fail To Add Student : {ex.Message}");
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
            }

        }

        // ── Mappers ───────────────────────────────────────────────
        private static StudentDto MapToStudentDto(Student s) => new(
            s.StudentID, s.FullName, s.NationalID,
            s.DateOfBirth, s.Gender, s.GradeLevel,
            s.Section, s.AcademicYear, s.IsActive
        );
    }
}