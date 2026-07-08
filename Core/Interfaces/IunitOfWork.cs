using Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using StudentBehaviorPlatform.Infrastructure.Repositories;

namespace StudentBehaviorPlatform.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IStudentRepo Students { get; }
        IStudentNoteRepo StudentNote { get; }
        IBehaviorRuleRepo BehaviorRules { get; }
        IClusterRunRepository ClusterRuns { get; }
        IClusterGroupRepository ClusterGroups { get; }
        IClusterMemberRepository ClusterMembers { get; }
        IAdminProfileRepository AdminProfiles { get; }
        IBehaviorIncidentRepo BehaviorIncidents { get; }
        IAttendanceRecordRepo AttendanceRecords { get; }
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);


        Task<int> SaveChangesAsync();
    }
}