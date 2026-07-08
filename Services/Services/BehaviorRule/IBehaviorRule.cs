
using E_Learning.Core.Base;
using Services.Dtos.BehaviorRule;

namespace StudentBehaviorPlatform.Core.Services
{
    public interface IBehaviorRuleService
    {
        Task<Response<IEnumerable<BehaviorRuleDto>>> GetAllBehaviorRulesAsync();
        Task<Response<BehaviorRuleDto>> GetBehaviorRuleByIdAsync(int id);
        Task<Response<BehaviorRuleDto>> CreateBehaviorRuleAsync(CreateBehaviorRuleDto dto);
        Task<Response<BehaviorRuleDto>> UpdateBehaviorRuleAsync(UpdateBehaviorRuleDto dto);
        Task<Response<BehaviorRuleDto>> DeleteBehaviorRuleAsync(int id);
        Task<Response<BehaviorRuleDto>> ToggleActivationAsync(int id);
    }
}