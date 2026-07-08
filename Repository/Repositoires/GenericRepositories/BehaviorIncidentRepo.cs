using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace SchoolSystem.Infrastructure.Repositories
{
    public class BehaviorIncidentRepo : GenericRepository<BehaviorIncident, int>, IBehaviorIncidentRepo
    {
        public BehaviorIncidentRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<List<BehaviorIncident>> GetIncidentsByStudentIdAsync(int studentId, int days)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);

            return await _context.BehaviorIncidents
                .Where(i => i.StudentID == studentId && i.OccurredAt >= startDate)
                .OrderByDescending(i => i.OccurredAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<BehaviorIncident>> GetAllWithDetailsAsync()
        {
            return await _context.BehaviorIncidents
                .Include(b => b.Student)
                .Include(b => b.BehaviorRule)
                .Include(b => b.ReviewedByUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<BehaviorIncident>> GetByStudentIdAsync(int studentId)
        {
            return await _context.BehaviorIncidents
                .Include(b => b.Student)
                .Include(b => b.BehaviorRule)
                .Include(b => b.ReviewedByUser)
                .Where(b => b.StudentID == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<BehaviorIncident>> GetAllForSummaryAsync()
        {
            return await _context.BehaviorIncidents
                .Include(b => b.Student)
                .Include(b => b.BehaviorRule)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}