using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace Repository.Repositoires.GenericRepositories
{
    public class AdminProfileRepository : GenericRepository<AdminProfile,  int>, IAdminProfileRepository
    {

        private readonly AppDbContext _context;
        public AdminProfileRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AdminProfile?> GetProfileByUserIdAsync(int userId)
            => await _context.AdminProfiles.Include(o => o.AppUser).FirstOrDefaultAsync(p => p.AppUserId == userId);



    }
}

