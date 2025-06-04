using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;

namespace ProjectPipeline.Infrastructure.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<BusinessUnit> BusinessUnits { get; set; }
        public DbSet<ProfileSubmission> ProfileSubmissions { get; set; }
        public DbSet<ProfileStatusHistory> ProfileStatusHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Department).HasMaxLength(200);
                entity.Property(e => e.Designation).HasMaxLength(200);

                entity.HasOne(u => u.BusinessUnit)
                      .WithMany(bu => bu.Users)
                      .HasForeignKey(u => u.BusinessUnitId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure BusinessUnit entity
            modelBuilder.Entity<BusinessUnit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.HeadOfUnit).HasMaxLength(100);

                // Configure nullable audit relationships
                ConfigureNullableAuditProperties<BusinessUnit>(entity);
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Technology).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProjectType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EstimatedValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualValue).HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.BusinessUnit)
                      .WithMany(bu => bu.Projects)
                      .HasForeignKey(p => p.BusinessUnitId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure nullable audit relationships
                ConfigureNullableAuditProperties<Project>(entity);
            });

            // Configure ProfileSubmission entity
            modelBuilder.Entity<ProfileSubmission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CandidateName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CandidateEmail).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Technology).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ExpectedSalary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OfferedSalary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.InterviewScore).HasColumnType("decimal(3,2)");
                entity.Property(e => e.TechnicalScore).HasColumnType("decimal(3,2)");
                entity.Property(e => e.SubmittedBy).IsRequired().HasMaxLength(450);
                entity.Property(e => e.LastUpdatedBy).HasMaxLength(450);

                entity.HasOne(ps => ps.Project)
                      .WithMany(p => p.ProfileSubmissions)
                      .HasForeignKey(ps => ps.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ps => ps.SubmittedByUser)
                      .WithMany()
                      .HasForeignKey(ps => ps.SubmittedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ps => ps.LastUpdatedByUser)
                      .WithMany()
                      .HasForeignKey(ps => ps.LastUpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure nullable audit relationships
                ConfigureNullableAuditProperties<ProfileSubmission>(entity);

                entity.HasIndex(e => e.CandidateEmail);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.SubmissionDate);
            });

            // Configure ProfileStatusHistory entity
            modelBuilder.Entity<ProfileStatusHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Comments).HasMaxLength(1000);
                entity.Property(e => e.Reason).HasMaxLength(200);
                entity.Property(e => e.ChangedBy).IsRequired().HasMaxLength(450);

                entity.HasOne(h => h.ProfileSubmission)
                      .WithMany(ps => ps.StatusHistory)
                      .HasForeignKey(h => h.ProfileSubmissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.ChangedByUser)
                      .WithMany()
                      .HasForeignKey(h => h.ChangedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure nullable audit relationships
                ConfigureNullableAuditProperties<ProfileStatusHistory>(entity);

                entity.HasIndex(e => e.ChangedDate);
            });
        }

        private void ConfigureNullableAuditProperties<T>(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> entity) where T : BaseEntity
        {
            // Configure CreatedBy relationship - nullable
            entity.HasOne(e => e.CreatedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false); // Allow null values

            // Configure UpdatedBy relationship - nullable
            entity.HasOne(e => e.UpdatedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.UpdatedBy)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false); // Allow null values

            // Configure DeletedBy relationship - nullable
            entity.HasOne(e => e.DeletedByUser)
                  .WithMany()
                  .HasForeignKey(e => e.DeletedBy)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false); // Allow null values
        }
    }
}
