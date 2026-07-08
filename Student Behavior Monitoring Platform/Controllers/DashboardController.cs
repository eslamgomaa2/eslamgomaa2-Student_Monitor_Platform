using E_Learning.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Dashboard;

namespace StudentBehaviorPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class DashboardController : ControllerBase
    {
        private readonly IDashboard _dashboardService;

        public DashboardController(IDashboard dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _dashboardService.GetDashboardStatsAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("student/{studentId:int}/academic")]
        public async Task<IActionResult> GetStudentAcademicData(int studentId)
        {
            var result = await _dashboardService.GetStudentAcademicDataAsync(studentId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("attendanceCount/present")]
        public async Task<IActionResult> GetPresentStudentCount([FromQuery] DateOnly? date = null)
        {
            var result = await _dashboardService.GetPresentStudentCountAsync(date);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("attendanceCount/absent")]
        public async Task<IActionResult> GetAbsentStudentCount([FromQuery] DateOnly? date = null)
        {
            var result = await _dashboardService.GetAbsentStudentCountAsync(date);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("attendanceCount/late")]
        public async Task<IActionResult> GetLateStudentCount([FromQuery] DateOnly? date = null)
        {
            var result = await _dashboardService.GetLateStudentCountAsync(date);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}