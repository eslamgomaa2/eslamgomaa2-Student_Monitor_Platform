using E_Learning.Core.Base;
using Services.Dtos.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Grade
{
    public interface IGradeService
    {
        Task<Response<IEnumerable<GradeDto>>> GetAllGradesAsync();
        Task<Response<IEnumerable<GradeDto>>> GetGradesByStudentAsync(int studentId);
        Task<Response<IEnumerable<GradeDto>>> GetGradesBySubjectAsync(string subject);
        Task<Response<IEnumerable<StudentAverageDto>>> GetAveragesAsync();
    }
}
