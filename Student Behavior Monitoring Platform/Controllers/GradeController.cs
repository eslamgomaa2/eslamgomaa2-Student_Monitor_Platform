using Microsoft.AspNetCore.Mvc;
using Services.Services.Grade;

namespace Student_Behavior_Monitoring_Platform.Controllers
{

    [ApiController]
    [Route("api/grades")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllGrades()
        {
            var result = await _gradeService.GetAllGradesAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetGradesByStudent(int studentId)
        {
            var result = await _gradeService.GetGradesByStudentAsync(studentId);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("subject/{subject}")]
        public async Task<IActionResult> GetGradesBySubject(string subject)
        {
            var result = await _gradeService.GetGradesBySubjectAsync(subject);
            return StatusCode((int)result.HttpStatusCode, result);
        }

       
        [HttpGet("average")]
        public async Task<IActionResult> GetAverages()
        {
            var result = await _gradeService.GetAveragesAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
