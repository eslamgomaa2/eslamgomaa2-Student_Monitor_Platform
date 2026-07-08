
using E_Learning.Core.Base;
using Microsoft.AspNetCore.Identity;
using Services.Dtos.BehaviorRule;
using StudentBehaviorPlatform.Data.Entities;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;

namespace StudentBehaviorPlatform.Core.Services
{
    public class BehaviorRuleService : IBehaviorRuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseHandler _responseHandler;
        private readonly UserManager<ApplicationUser> _userManager;


        public BehaviorRuleService(IUnitOfWork unitOfWork, ResponseHandler responseHandler, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _userManager = userManager;
        }

        public async Task<Response<IEnumerable<BehaviorRuleDto>>> GetAllBehaviorRulesAsync()
        {
            var rules = await _unitOfWork.BehaviorRules.GetAllBehaviorRulesAsync();
            return _responseHandler.Success(rules.Select(MapToDto));
        }

        public async Task<Response<BehaviorRuleDto>> GetBehaviorRuleByIdAsync(int id)
        {
            var rule = await _unitOfWork.BehaviorRules.GetBehaviorRuleByIdAsync(id);
            if (rule is null)
                return _responseHandler.NotFound<BehaviorRuleDto>($"BehaviorRule with ID {id} not found.");

            return _responseHandler.Success(MapToDto(rule));
        }

        public async Task<Response<BehaviorRuleDto>> CreateBehaviorRuleAsync(CreateBehaviorRuleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.CreatedByUserID.ToString());
            if (user is null)
                return _responseHandler.NotFound<BehaviorRuleDto>("User doesnt Exist");


            var rule = new BehaviorRule
            {
                RuleName = dto.RuleName,
                Description = dto.Description,
                Category = dto.Category,
                SeverityLevel = dto.SeverityLevel,
                CreatedByUserID = dto.CreatedByUserID,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BehaviorRules.AddAsync(rule);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Created(MapToDto(rule));
        }

        public async Task<Response<BehaviorRuleDto>> UpdateBehaviorRuleAsync(UpdateBehaviorRuleDto dto)
        {
            var rule = await _unitOfWork.BehaviorRules.GetByIdAsync(dto.RuleID);
            if (rule is null)
                return _responseHandler.NotFound<BehaviorRuleDto>($"BehaviorRule with ID {dto.RuleID} not found.");

            rule.RuleName = dto.RuleName;
            rule.Description = dto.Description;
            rule.Category = dto.Category;
            rule.SeverityLevel = dto.SeverityLevel;
            rule.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BehaviorRules.Update(rule);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(MapToDto(rule));
        }
        public async Task<Response<BehaviorRuleDto>> DeleteBehaviorRuleAsync(int id)
        {
            var rule = await _unitOfWork.BehaviorRules.GetByIdAsync(id);
            if (rule is null)
                return _responseHandler.NotFound<BehaviorRuleDto>($"BehaviorRule with ID {id} not found.");

            _unitOfWork.BehaviorRules.Remove(rule);
            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Deleted<BehaviorRuleDto>();
        }

        public async Task<Response<BehaviorRuleDto>> ToggleActivationAsync(int id)
        {
            var rule = await _unitOfWork.BehaviorRules.GetByIdAsync(id);
            if (rule is null)
                return _responseHandler.NotFound<BehaviorRuleDto>($"BehaviorRule with ID {id} not found.");

            rule.IsActive = !rule.IsActive;
            rule.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.BehaviorRules.Update(rule);
            await _unitOfWork.SaveChangesAsync();

            var message = rule.IsActive ? "Rule activated successfully." : "Rule deactivated successfully.";
            return _responseHandler.Success(MapToDto(rule));
        }

        // ─── MAPPER ─────────────────────────────────────────────────────────────
        private static BehaviorRuleDto MapToDto(BehaviorRule rule) => new()
        {
            RuleID = rule.RuleID,
            RuleName = rule.RuleName,
            Description = rule.Description,
            Category = rule.Category,
            SeverityLevel = rule.SeverityLevel,
            IsActive = rule.IsActive,
            CreatedByUserID = rule.CreatedByUserID,
            CreatedAt = rule.CreatedAt,
            UpdatedAt = rule.UpdatedAt
        };
    }
}