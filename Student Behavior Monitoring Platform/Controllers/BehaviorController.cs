using Microsoft.AspNetCore.Mvc;
using Services.Dtos.BehaviorRecognation;
using Services.Services.Behavior;

namespace Student_Behavior_Monitoring_Platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BehaviorController : ControllerBase
    {
        private readonly IBehaviorService _behaviorService;

        public BehaviorController(IBehaviorService behaviorService)
        {
            _behaviorService = behaviorService;
        }


        [HttpPost("detect")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> DetectBehavior([FromForm] CameraBehaviorDto dto)
        {
            try
            {
                if (dto.CameraImage == null || dto.CameraImage.Length == 0)
                    return BadRequest(new { error = "❌ No image provided!" });

                await using var stream = dto.CameraImage.OpenReadStream();

                var incidents = await _behaviorService.DetectAndSaveBehaviorAsync(
                    stream);

                if (incidents.Count == 0)
                    return Ok(new { success = true, message = "⚠️ No behavior detected", data = new List<object>() });

                return Ok(new
                {
                    success = true,
                    message = $"{incidents.Count} behavior(s) detected and saved!",
                    data = incidents.Select(i => new
                    {
                        incidentId = i.IncidentID,

                        studentId = i.StudentID,
                        ruleId = i.RuleID,
                        behaviorName = i.BehaviorRule?.RuleName,
                        confidence = i.Confidence,
                        occurredAt = i.OccurredAt,
                        detail = i.Detail,
                        source = i.Source,
                        reviewStatus = i.ReviewStatus.ToString()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBehaviorIncidents()
        {
            var result = await _behaviorService.GetAllBehaviorIncidentsAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetBehaviorByStudent(int studentId)
        {
            var result = await _behaviorService.GetBehaviorByStudentAsync(studentId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("summary")]
        public async Task<IActionResult> GetBehaviorSummary()
        {
            var result = await _behaviorService.GetBehaviorSummaryAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
