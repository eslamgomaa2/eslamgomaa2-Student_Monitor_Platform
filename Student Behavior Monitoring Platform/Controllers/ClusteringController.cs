using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Cluster;
using Services.Services.Cluster.SchoolSystem.Application.Services.Clustering.Interfaces;
using System.Security.Claims;

namespace StudentBehaviorPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClustersController : ControllerBase
    {
        private readonly IClusterService _clusterService;

        public ClustersController(IClusterService clusterService)
        {
            _clusterService = clusterService;
        }

        // ─────────────────────────────
        // Cluster Summaries
        // ─────────────────────────────
        [HttpGet("summaries")]
        public async Task<IActionResult> GetClusterSummaries([FromQuery] ClusterFilterDto filters)
        {
            var result = await _clusterService.GetClusterSummariesAsync(filters);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────
        // Generate Report
        // ─────────────────────────────
        [HttpPost("generate-report")]
        public async Task<IActionResult> GenerateClusterReport( [FromForm] GenerateClusterReportDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

          

            var result = await _clusterService.GenerateClusterReportAsync(userId,dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────
        // Student Details (match your pattern)
        // ─────────────────────────────
        [HttpGet("{runId:int}/students/{studentId:int}")]
        public async Task<IActionResult> GetStudentDetails([FromRoute] int runId,[FromRoute] int studentId)
        {
            var result = await _clusterService.GetStudentDetailsAsync(studentId, runId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────
        // Visualization
        // ─────────────────────────────
        [HttpGet("visualization/{runId:int}")]
        public async Task<IActionResult> GetVisualizationData([FromRoute] int runId)
        {
            var result = await _clusterService.GetClusterVisualizationDataAsync(runId);
            return StatusCode(200, result);
        }

        // ─────────────────────────────
        // Reset Filters
        // ─────────────────────────────
        [HttpPost("reset-filters")]
        public async Task<IActionResult> ResetFilters()
        {
            var result = await _clusterService.ResetFiltersAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        // ─────────────────────────────
        // Apply Filters
        // ─────────────────────────────
        [HttpPost("apply-filters")]
        public async Task<IActionResult> ApplyFilters([FromBody] ClusterFilterDto filters)
        {
            var result = await _clusterService.GetClusterSummariesAsync(filters);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}