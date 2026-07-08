using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositoires.GenericRepositories
{
    public class AttendanceRecordRepo : GenericRepository<AttendanceRecord, int>, IAttendanceRecordRepo
    {
        public AttendanceRecordRepo(AppDbContext context) : base(context) { }
        public async Task<double> GetAttendancePercentageLast90DaysAsync(int studentId)
        {
            var fromDate = DateTime.UtcNow.AddDays(-90);

            var query = _context.AttendanceRecords
                .Where(ar => ar.StudentID == studentId && ar.AttendanceDate >= fromDate);

            var totalDays = await query.CountAsync();

            if (totalDays == 0)
                return 0;

            var attendedDays = await query
                .CountAsync();

            return (double)attendedDays / totalDays * 100;
        }

        public Task<AttendanceRecord?> GetTodayRecordAsync(int studentId)
        {
            return _context.AttendanceRecords.Where(ar => ar.StudentID == studentId && ar.AttendanceDate.Date == DateTime.UtcNow.Date).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AttendanceRecord>> GetAllWithStudentAsync()
        {
            return await _context.AttendanceRecords
                .Include(a => a.Student)
                .Include(a => a.VideoSession)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId)
        {
            return await _context.AttendanceRecords
                .Include(a => a.Student)
                .Include(a => a.VideoSession)
                .Where(a => a.StudentID == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceRecord>> GetByDateAsync(DateTime date)
        {
            return await _context.AttendanceRecords
                .Include(a => a.Student)
                .Where(a => a.AttendanceDate.Date == date.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<AttendanceRecord>> GetSummaryDataAsync()
        {
            return await _context.AttendanceRecords
                .Include(a => a.Student)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}