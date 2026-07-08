
using E_Learning.Core.Base;
using Services.Dtos.BehaviorRecognation;
using StudentBehaviorPlatform.Data.Entities;

namespace Services.Services.Behavior
{
    public interface IBehaviorService
    {
        Task<List<BehaviorIncident>> DetectAndSaveBehaviorAsync(Stream imageStream, CancellationToken ct = default);
        Task<Response<IEnumerable<BehaviorIncidentDto>>> GetAllBehaviorIncidentsAsync();
        Task<Response<IEnumerable<BehaviorIncidentDto>>> GetBehaviorByStudentAsync(int studentId);
        Task<Response<IEnumerable<BehaviorStudentSummaryDto>>> GetBehaviorSummaryAsync();
    }
}
