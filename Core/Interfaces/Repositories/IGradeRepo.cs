using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IGradeRepo:IGenericRepository<Grade, int>
    {
        Task<IEnumerable<Grade>> GetAllWithStudentAsync();
        Task<IEnumerable<Grade>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<Grade>> GetBySubjectAsync(string subject);
        Task<IEnumerable<Grade>> GetAllForAverageAsync();
    }
}
