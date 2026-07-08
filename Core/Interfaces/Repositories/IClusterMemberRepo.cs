using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClusterMemberRepository : IGenericRepository<ClusterMember, int>
    {
        Task<IEnumerable<ClusterMember>> GetMembersByGroupIdAsync(int groupId);
        Task<ClusterMember?> GetStudentClusterDetailsAsync(int studentId, int runId);
        IQueryable<ClusterMember> GetMembersByRunIdQueryable(int runId);
    }
}
