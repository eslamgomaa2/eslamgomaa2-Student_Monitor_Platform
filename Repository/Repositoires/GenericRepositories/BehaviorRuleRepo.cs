
using Microsoft.EntityFrameworkCore;
using Repository.Repositoires.GenericRepositories;
using StudentBehaviorPlatform.Data;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Infrastructure.Repositories
{
    public class BehaviorRuleRepo : GenericRepository<BehaviorRule, int>, IBehaviorRuleRepo
    {
        private readonly AppDbContext _context;

        public BehaviorRuleRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BehaviorRule>> GetAllBehaviorRulesAsync()
        {
            return await _context.BehaviorRules
                .OrderBy(r => r.RuleName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BehaviorRule?> GetBehaviorRuleByIdAsync(int id)
        {
            return await _context.BehaviorRules
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RuleID == id);
        }

        public async Task<BehaviorRule?> GetByBehaviorNameAsync(string name)
        {
           return await _context.BehaviorRules
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RuleName == name);
        }
    }
}