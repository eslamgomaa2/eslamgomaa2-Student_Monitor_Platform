
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Data.Repositories
{
    public class StudentRepository : GenericRepository<Student, int>, IStudentRepo
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.FullName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StudentID == studentId && s.IsActive);
        }

        public async Task<IEnumerable<AttendanceRecord>> GetStudentAttendanceAsync(int studentId)
        {
            return await _context.AttendanceRecords
                .Where(a => a.StudentID == studentId)
                .OrderByDescending(a => a.AttendanceDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetStudentGradesAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentID == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<BehaviorIncident>> GetStudentBehaviorHistoryAsync(int studentId)
        {
            return await _context.BehaviorIncidents
                .Where(b => b.StudentID == studentId)
                .OrderByDescending(b => b.OccurredAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<StudentNote?> GetNoteByIdAsync(int noteId)
        {
            return await _context.StudentNotes
                .FirstOrDefaultAsync(n => n.NoteID == noteId);
        }

        public async Task AddNoteAsync(StudentNote note)
        {
            await _context.StudentNotes.AddAsync(note);
        }

        public void UpdateNote(StudentNote note)
        {
            _context.StudentNotes.Update(note);
        }
    }
}