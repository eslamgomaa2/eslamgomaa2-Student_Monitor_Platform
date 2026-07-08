
using E_Learning.Core.Interfaces.Repositories;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Infrastructure.Repositories
{
    public interface IBehaviorRuleRepo : IGenericRepository<BehaviorRule, int>
    {
        Task<IEnumerable<BehaviorRule>> GetAllBehaviorRulesAsync();
        Task<BehaviorRule?> GetBehaviorRuleByIdAsync(int id);
        Task<BehaviorRule?> GetByBehaviorNameAsync(string name);

    }
}