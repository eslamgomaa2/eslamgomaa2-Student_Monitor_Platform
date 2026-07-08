using E_Learning.API.Extensions.E_Learning.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Student;
using StudentBehaviorPlatform.Services.Interfaces;

namespace Student_Behavior_Monitoring_Platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        
        [HttpGet("Get All")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await _studentService.GetAllStudentsAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await _studentService.GetStudentByIdAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("{id:int}/attendance")]
        public async Task<IActionResult> GetStudentAttendance(int id)
        {
            var result = await _studentService.GetStudentAttendanceAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("{id:int}/grades")]
        public async Task<IActionResult> GetStudentGrades(int id)
        {
            var result = await _studentService.GetStudentGradesAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        
        [HttpGet("{id:int}/behavior")]
        public async Task<IActionResult> GetStudentBehaviorHistory(int id)
        {
            var result = await _studentService.GetStudentBehaviorHistoryAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpGet("GetAllNotes")]
        public async Task<IActionResult> GetNotes( )
        {
            
            var result = await _studentService.GetNotesAsync();
            return StatusCode((int)result.HttpStatusCode, result);
        }
        [HttpPost("{id:int}/notes")]
        public async Task<IActionResult> AddNote(int id, [FromBody] CreateNoteDto dto)
        {
            var userId = User.GetUserId();
            var result = await _studentService.AddNoteAsync(id, userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }


        [HttpPut("notes/{noteId:int}")]
        public async Task<IActionResult> UpdateNote(int noteId, [FromBody] UpdateNoteDto dto)
        {
            var userId = User.GetUserId();
            var result = await _studentService.UpdateNoteAsync(noteId, userId, dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent( [FromForm] AddStudentDto dto)
        {
            var userId = User.GetUserId();
            var result = await _studentService.AddStudent( dto);
            return StatusCode((int)result.HttpStatusCode, result);
        }

    }
}