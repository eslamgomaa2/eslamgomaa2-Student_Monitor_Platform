
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.BehaviorRule;
using StudentBehaviorPlatform.Core.Services;

namespace StudentBehaviorPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BehaviorRuleController : ControllerBase
    {
        private readonly IBehaviorRuleService _behaviorRuleService;

        public BehaviorRuleController(IBehaviorRuleService behaviorRuleService)
        {
            _behaviorRuleService = behaviorRuleService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllBehaviorRules()
        {
            var result = await _behaviorRuleService.GetAllBehaviorRulesAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBehaviorRuleById(int id)
        {
            var result = await _behaviorRuleService.GetBehaviorRuleByIdAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateBehaviorRule([FromBody] CreateBehaviorRuleDto dto)
        {
            var result = await _behaviorRuleService.CreateBehaviorRuleAsync(dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPut]
        public async Task<IActionResult> UpdateBehaviorRule([FromBody] UpdateBehaviorRuleDto dto)
        {
            var result = await _behaviorRuleService.UpdateBehaviorRuleAsync(dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBehaviorRule([FromRoute]int id)
        {
            var result = await _behaviorRuleService.DeleteBehaviorRuleAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleActivation(int id)
        {
            var result = await _behaviorRuleService.ToggleActivationAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}