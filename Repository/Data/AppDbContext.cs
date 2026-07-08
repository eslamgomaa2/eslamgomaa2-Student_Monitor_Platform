using E_Learning.Core.Entities.Assessments.Assignments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentBehaviorPlatform.Data.Entities;

namespace StudentBehaviorPlatform.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
        public DbSet<Grade> Grades => Set<Grade>();
        public DbSet<BehaviorRule> BehaviorRules => Set<BehaviorRule>();
        public DbSet<BehaviorIncident> BehaviorIncidents => Set<BehaviorIncident>();
        public DbSet<StudentNote> StudentNotes => Set<StudentNote>();
        public DbSet<VideoSession> VideoSessions => Set<VideoSession>();
        public DbSet<AIAnalysisResult> AIAnalysisResults => Set<AIAnalysisResult>();
        public DbSet<ClusterRun> ClusterRuns => Set<ClusterRun>();
        public DbSet<ClusterGroup> ClusterGroups => Set<ClusterGroup>();
        public DbSet<ClusterMember> ClusterMembers => Set<ClusterMember>();
        public DbSet<HelpContent> HelpContents => Set<HelpContent>();
        public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();
        public DbSet<AssignmentSubmission> assignmentSubmissions => Set<AssignmentSubmission>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rename Identity tables
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });

            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Student configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentID);
                entity.HasIndex(e => e.NationalID).IsUnique();
                entity.HasIndex(e => new { e.GradeLevel, e.Section, e.AcademicYear });

                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.NationalID).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.GradeLevel).HasMaxLength(10);
                entity.Property(e => e.Section).HasMaxLength(10);

                entity.HasMany(e => e.AttendanceRecords)
                    .WithOne(a => a.Student)
                    .HasForeignKey(a => a.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Grades)
                    .WithOne(g => g.Student)
                    .HasForeignKey(g => g.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.BehaviorIncidents)
                    .WithOne(b => b.Student)
                    .HasForeignKey(b => b.StudentID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.StudentNotes)
                    .WithOne(n => n.Student)
                    .HasForeignKey(n => n.StudentID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ClusterMembers)
                    .WithOne(c => c.Student)
                    .HasForeignKey(c => c.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // AttendanceRecord configuration
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.AttendanceID);
                entity.HasIndex(e => new { e.StudentID, e.AttendanceDate }).IsUnique();

                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.Source).HasMaxLength(20);
                entity.Property(e => e.ConfidenceScore).HasPrecision(5, 2);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.AttendanceRecords)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.VideoSession)
                    .WithMany(v => v.AttendanceRecords)
                    .HasForeignKey(e => e.VideoSessionID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Grade configuration
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.GradeID);
                entity.HasIndex(e => new { e.StudentID, e.AcademicYear, e.Term });

                entity.Property(e => e.Subject).HasMaxLength(100);
                entity.Property(e => e.GradeLabel).HasMaxLength(5);
                entity.Property(e => e.Term).HasMaxLength(20);
                entity.Property(e => e.Score).HasPrecision(5, 2);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Grades)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // BehaviorRule configuration
            modelBuilder.Entity<BehaviorRule>(entity =>
            {
                entity.HasKey(e => e.RuleID);

                entity.Property(e => e.RuleName).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Category).HasMaxLength(100);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.BehaviorRules)
                    .HasForeignKey(e => e.CreatedByUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.BehaviorIncidents)
                    .WithOne(b => b.BehaviorRule)
                    .HasForeignKey(b => b.RuleID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // BehaviorIncident configuration
            modelBuilder.Entity<BehaviorIncident>(entity =>
            {
                entity.HasKey(e => e.IncidentID);
                entity.HasIndex(e => new { e.StudentID, e.OccurredAt });
               

                entity.Property(e => e.Source).HasMaxLength(20);
                entity.Property(e => e.Detail).HasMaxLength(1000);
                entity.Property(e => e.Confidence).HasPrecision(5, 2);
                entity.Property(e => e.ReviewStatus).HasMaxLength(20);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.BehaviorIncidents)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.BehaviorRule)
                    .WithMany(b => b.BehaviorIncidents)
                    .HasForeignKey(e => e.RuleID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ReviewedByUser)
                    .WithMany(u => u.ReviewedBehaviorIncidents)
                    .HasForeignKey(e => e.ReviewedByUserID)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<AdminProfile>()
                        .HasOne(a => a.AppUser)
                       .WithOne(u => u.AdminProfile)
                       .HasForeignKey<AdminProfile>(a => a.AppUserId);

            // StudentNote configuration
            modelBuilder.Entity<StudentNote>(entity =>
            {
                entity.HasKey(e => e.NoteID);
                entity.HasIndex(e => new { e.StudentID, e.CreatedAt });

                entity.Property(e => e.NoteText).HasMaxLength(2000);
                entity.Property(e => e.NoteType).HasMaxLength(50);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.StudentNotes)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.StudentNotes)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // VideoSession configuration
            modelBuilder.Entity<VideoSession>(entity =>
            {
                entity.HasKey(e => e.SessionID);

                entity.Property(e => e.FilePath).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.ClassroomRef).HasMaxLength(100);

                entity.HasOne(e => e.UploadedByUser)
                    .WithMany(u => u.VideoSessions)
                    .HasForeignKey(e => e.UploadedByUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.AIAnalysisResults)
                    .WithOne(a => a.VideoSession)
                    .HasForeignKey(a => a.SessionID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.AttendanceRecords)
                    .WithOne(a => a.VideoSession)
                    .HasForeignKey(a => a.VideoSessionID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // AIAnalysisResult configuration
            modelBuilder.Entity<AIAnalysisResult>(entity =>
            {
                entity.HasKey(e => e.ResultID);

                entity.Property(e => e.AnalysisType).HasMaxLength(50);
                entity.Property(e => e.ResultPayload).HasColumnType("nvarchar(max)");
                entity.Property(e => e.OverallConfidence).HasPrecision(5, 2);

                entity.HasOne(e => e.VideoSession)
                    .WithMany(v => v.AIAnalysisResults)
                    .HasForeignKey(e => e.SessionID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ClusterRun configuration
            modelBuilder.Entity<ClusterRun>(entity =>
            {
                entity.HasKey(e => e.RunID);

                entity.Property(e => e.ReportPath).HasMaxLength(500);

                entity.HasOne(e => e.TriggeredByUser)
                    .WithMany(u => u.ClusterRuns)
                    .HasForeignKey(e => e.TriggeredByUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ClusterGroups)
                    .WithOne(c => c.ClusterRun)
                    .HasForeignKey(c => c.RunID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ClusterMembers)
                    .WithOne(c => c.ClusterRun)
                    .HasForeignKey(c => c.RunID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ClusterGroup configuration
            modelBuilder.Entity<ClusterGroup>(entity =>
            {
                entity.HasKey(e => e.GroupID);

                entity.Property(e => e.GroupSummary).HasMaxLength(1000);

                entity.HasOne(e => e.ClusterRun)
                    .WithMany(c => c.ClusterGroups)
                    .HasForeignKey(e => e.RunID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ClusterMembers)
                    .WithOne(c => c.ClusterGroup)
                    .HasForeignKey(c => c.GroupID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ClusterMember configuration
            modelBuilder.Entity<ClusterMember>(entity =>
            {
                entity.HasKey(e => e.MemberID);
                entity.HasIndex(e => new { e.RunID, e.StudentID });

                entity.Property(e => e.Features).HasColumnType("nvarchar(max)");

                entity.HasOne(e => e.ClusterRun)
                    .WithMany(c => c.ClusterMembers)
                    .HasForeignKey(e => e.RunID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.ClusterGroup)
                    .WithMany(c => c.ClusterMembers)
                    .HasForeignKey(e => e.GroupID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.ClusterMembers)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SupportTicket configuration
           
            // HelpContent configuration
            modelBuilder.Entity<HelpContent>(entity =>
            {
                entity.HasKey(e => e.ContentID);

                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(255);
                entity.Property(e => e.Body).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Tags).HasMaxLength(500);
            });

            // AuditLog configuration
          
        }
    }
}