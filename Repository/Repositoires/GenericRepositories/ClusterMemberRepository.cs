using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace SchoolSystem.Infrastructure.Repositories.Clustering.Implementations;

public class ClusterMemberRepository : GenericRepository<ClusterMember, int>, IClusterMemberRepository
{
    public ClusterMemberRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ClusterMember>> GetMembersByGroupIdAsync(int groupId)
    {
        return await _context.ClusterMembers
            .Where(m => m.GroupID == groupId)
            .Include(m => m.Student)
            .ToListAsync();
    }

    public async Task<ClusterMember?> GetStudentClusterDetailsAsync(int studentId, int runId)
    {
        return await _context.ClusterMembers
            .Include(m => m.ClusterGroup)
            .Include(m => m.Student)
                .ThenInclude(s => s.BehaviorIncidents)
                    .ThenInclude(i => i.BehaviorRule)
            .FirstOrDefaultAsync(m => m.StudentID == studentId && m.RunID == runId);
    }

    public IQueryable<ClusterMember> GetMembersByRunIdQueryable(int runId)
    {
        return _context.ClusterMembers
     .Where(m => m.RunID == runId)
     .Include(m => m.Student)
         .ThenInclude(s => s.Grades)

     .Include(m => m.Student)
         .ThenInclude(s => s.AttendanceRecords)

     .Include(m => m.ClusterGroup);
    }
}