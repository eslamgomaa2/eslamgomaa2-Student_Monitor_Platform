using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace SchoolSystem.Infrastructure.Repositories.Clustering.Implementations;

public class ClusterGroupRepository : GenericRepository<ClusterGroup, int>, IClusterGroupRepository
{
    public ClusterGroupRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ClusterGroup>> GetGroupsByRunIdAsync(int runId)
    {
        return await _context.ClusterGroups
            .Where(g => g.RunID == runId)
            .OrderBy(g => g.GroupLabel)
            .ToListAsync();
    }
}