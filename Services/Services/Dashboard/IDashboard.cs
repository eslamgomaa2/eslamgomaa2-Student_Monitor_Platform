using E_Learning.Core.Base;
using Services.Dtos.Dashboard;
using StudentBehaviorPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Dashboard
{
    public interface IDashboard
    {

        Task<Response<DashboardStatsDto>> GetDashboardStatsAsync();

        Task<Response<StudentAcademicDto>> GetStudentAcademicDataAsync(int studentId);

        Task<Response<int>> GetPresentStudentCountAsync(DateOnly? date = null);

        Task<Response<int>> GetAbsentStudentCountAsync(DateOnly? date = null);

        Task<Response<int>> GetLateStudentCountAsync(DateOnly? date = null);
    }
}
