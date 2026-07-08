using E_Learning.Core.Base;
using Services.Dtos.Student;

namespace StudentBehaviorPlatform.Services.Interfaces
{
    public interface IStudentService
    {

        Task<Response<IEnumerable<StudentDto>>> AddStudent(AddStudentDto studentDto, CancellationToken ct = default);
        Task<Response<IEnumerable<StudentDto>>> GetAllStudentsAsync();
        Task<Response<StudentDto>> GetStudentByIdAsync(int studentId);
        Task<Response<IEnumerable<AttendanceRecordDto>>> GetStudentAttendanceAsync(int studentId);
        Task<Response<IEnumerable<GradeDto>>> GetStudentGradesAsync(int studentId);
        Task<Response<IEnumerable<BehaviorIncidentDto>>> GetStudentBehaviorHistoryAsync(int studentId);
        Task<Response<StudentNoteDto>> AddNoteAsync(int studentId, int userId, CreateNoteDto dto);
        Task<Response<StudentNoteDto>> UpdateNoteAsync(int noteId, int userId, UpdateNoteDto dto);
        Task<Response<IEnumerable<StudentNoteDto>>> GetNotesAsync();

    }
}