using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClusterRunRepository : IGenericRepository<ClusterRun, int>
    {
        Task<ClusterRun?> GetLatestRunAsync();
        Task<ClusterRun?> GetLatestClusterRunAsync(string schoolYear, string gradeLevel, DateTime startDate, DateTime endDate);
        Task<ClusterRun?> GetRunWithDetailsAsync(int runId);
    }
}
