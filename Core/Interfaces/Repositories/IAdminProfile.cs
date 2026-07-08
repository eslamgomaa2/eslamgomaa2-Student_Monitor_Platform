using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAdminProfileRepository : IGenericRepository<AdminProfile, int>
    {
        Task<AdminProfile?> GetProfileByUserIdAsync(int userId);
    }
}
