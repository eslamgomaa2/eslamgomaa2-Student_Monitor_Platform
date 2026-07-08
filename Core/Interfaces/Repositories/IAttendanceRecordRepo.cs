using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IAttendanceRecordRepo : IGenericRepository<AttendanceRecord, int>
    {
        Task<double> GetAttendancePercentageLast90DaysAsync(int studentId);

        Task<AttendanceRecord?> GetTodayRecordAsync(int studentId);

        Task<IEnumerable<AttendanceRecord>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<AttendanceRecord>> GetByDateAsync(DateTime date);
        Task<IEnumerable<AttendanceRecord>> GetAllWithStudentAsync();
        Task<IEnumerable<AttendanceRecord>> GetSummaryDataAsync();
    }
}
