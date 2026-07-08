using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClusterGroupRepository : IGenericRepository<ClusterGroup, int>
    {
        Task<IEnumerable<ClusterGroup>> GetGroupsByRunIdAsync(int runId);
    }
}
