using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IStudentRepo:IGenericRepository<Student,int>
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int studentId);
        Task<IEnumerable<AttendanceRecord>> GetStudentAttendanceAsync(int studentId);
        Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId);
        Task<IEnumerable<BehaviorIncident>> GetStudentBehaviorHistoryAsync(int studentId);
        Task<StudentNote?> GetNoteByIdAsync(int noteId);
        Task AddNoteAsync(StudentNote note);
        void UpdateNote(StudentNote note);

    }
}
