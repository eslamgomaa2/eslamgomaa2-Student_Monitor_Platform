using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;
using System.Linq;

namespace SchoolSystem.Infrastructure.Repositories.Clustering.Implementations;

public class ClusterRunRepository : GenericRepository<ClusterRun, int>, IClusterRunRepository
{
    public ClusterRunRepository(AppDbContext context) : base(context) { }

    public async Task<ClusterRun?> GetLatestClusterRunAsync(
    string schoolYear,
    string gradeLevel,
    DateOnly startDate,
    DateOnly endDate)
    {
        // Convert DateOnly to DateTime for proper comparison with RunAt (DateTime)
        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _context.ClusterRuns
            .AsNoTracking()  // Optional: for read-only query
            .Where(r => r.SchoolYear == schoolYear)
            .Where(r => r.GradeLevel == gradeLevel)
            .Where(r => r.RunAt >= startDateTime && r.RunAt <= endDateTime)
            .OrderByDescending(r => r.RunAt)
            .FirstOrDefaultAsync();
    }

    public async Task<ClusterRun?> GetLatestClusterRunAsync(  string schoolYear, string gradeLevel,  DateTime startDate,  DateTime endDate)
    {
        return await _context.ClusterRuns
            .Where(o => o.SchoolYear == schoolYear &&
                        o.GradeLevel == gradeLevel &&
                        o.RunAt >= startDate &&
                        o.RunAt <= endDate)
            .OrderByDescending(r => r.RunAt)
            .FirstOrDefaultAsync();
    }

    public async Task<ClusterRun?> GetLatestRunAsync()
    {
        return await _context.ClusterRuns
            .OrderByDescending(r => r.RunAt)
            .FirstOrDefaultAsync();
    }

    public async Task<ClusterRun?> GetRunWithDetailsAsync(int runId)
    {
        return await _context.ClusterRuns
            .Include(r => r.ClusterGroups)
            .Include(r => r.ClusterMembers)
            .FirstOrDefaultAsync(r => r.RunID == runId);
    }
}