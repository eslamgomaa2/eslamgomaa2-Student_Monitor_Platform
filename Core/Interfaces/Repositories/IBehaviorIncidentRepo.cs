using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBehaviorIncidentRepo:IGenericRepository<BehaviorIncident,int>
    {
        public Task<List<BehaviorIncident>> GetIncidentsByStudentIdAsync(int studentId, int days);
        Task<IEnumerable<BehaviorIncident>> GetAllWithDetailsAsync();
        Task<IEnumerable<BehaviorIncident>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<BehaviorIncident>> GetAllForSummaryAsync();
    }
}
