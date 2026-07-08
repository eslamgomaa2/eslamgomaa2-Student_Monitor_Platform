using Core.Interfaces.Repositories;
using E_Learning.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Repositoires.GenericRepositories;
using SchoolSystem.Infrastructure.Repositories;
using SchoolSystem.Infrastructure.Repositories.Clustering.Implementations;
using StudentBehaviorPlatform.Data.Repositories.Interfaces;
using StudentBehaviorPlatform.Infrastructure.Repositories;

namespace StudentBehaviorPlatform.Data.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        private IStudentRepo? _students;
        private IBehaviorRuleRepo? _behaviorRules;
        private IClusterRunRepository? _clusterRuns;
        private IClusterGroupRepository? _clusterGroups;
        private IClusterMemberRepository? _clusterMembers;
        private IAdminProfileRepository? _adminProfiles;
        private IBehaviorIncidentRepo _behaviorIncidentRepo;
       private IAttendanceRecordRepo _attendanceRecords;
       private IStudentNoteRepo _studentNoteRepo;


        public UnitOfWork(AppDbContext  context)
        {
            _context = context;
        }

       
        public IClusterRunRepository ClusterRuns => _clusterRuns ??= new ClusterRunRepository(_context);
        public IClusterGroupRepository ClusterGroups => _clusterGroups ??= new ClusterGroupRepository(_context);
        public IClusterMemberRepository ClusterMembers => _clusterMembers ??= new ClusterMemberRepository(_context);
        public IStudentRepo Students => _students ??= new StudentRepository(_context);
        public IBehaviorRuleRepo BehaviorRules => _behaviorRules ??= new BehaviorRuleRepo (_context);
        public IAdminProfileRepository AdminProfiles => _adminProfiles ??= new AdminProfileRepository(_context);
        public IBehaviorIncidentRepo BehaviorIncidents => _behaviorIncidentRepo ??= new BehaviorIncidentRepo(_context);
        public IAttendanceRecordRepo AttendanceRecords => _attendanceRecords ??= new AttendanceRecordRepo(_context);

        public IStudentNoteRepo StudentNote =>_studentNoteRepo ??= new StudentNoteRepo(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await _context.Database.BeginTransactionAsync(ct);
        }
    }
}