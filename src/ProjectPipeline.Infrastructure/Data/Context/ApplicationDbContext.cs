using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectPipeline.Core.Entities;

namespace ProjectPipeline.Infrastructure.Data.Context;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets for entities
    public DbSet<Project> Projects { get; set; }
    public DbSet<BusinessUnit> BusinessUnits { get; set; }
    public DbSet<ProfileSubmission> ProfileSubmissions { get; set; }
    public DbSet<ResourceAllocation> ResourceAllocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints
        ConfigureProjectEntity(modelBuilder);
        ConfigureUserEntity(modelBuilder);
        ConfigureBusinessUnitEntity(modelBuilder);
        ConfigureProfileSubmissionEntity(modelBuilder);
        ConfigureResourceAllocationEntity(modelBuilder);
    }

    private void ConfigureProjectEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Technology).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ProjectType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EstimatedValue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ActualValue).HasColumnType("decimal(18,2)");

            // Configure relationships
            entity.HasOne(p => p.BusinessUnit)
                  .WithMany(bu => bu.Projects)
                  .HasForeignKey(p => p.BusinessUnitId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.CreatedByUser)
                  .WithMany(u => u.CreatedProjects)
                  .HasForeignKey(p => p.CreatedBy)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.UpdatedByUser)
                  .WithMany(u => u.UpdatedProjects)
                  .HasForeignKey(p => p.UpdatedBy)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Department).HasMaxLength(200);
            entity.Property(e => e.Designation).HasMaxLength(200);

            // Configure relationship with BusinessUnit
            entity.HasOne(u => u.BusinessUnit)
                  .WithMany(bu => bu.Users)
                  .HasForeignKey(u => u.BusinessUnitId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }

    private void ConfigureBusinessUnitEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BusinessUnit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.HeadOfUnit).HasMaxLength(100);
        });
    }

    private void ConfigureProfileSubmissionEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileSubmission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CandidateName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CandidateEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Technology).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ExpectedSalary).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OfferedSalary).HasColumnType("decimal(18,2)");

            // Configure relationship with Project
            entity.HasOne(ps => ps.Project)
                  .WithMany(p => p.ProfileSubmissions)
                  .HasForeignKey(ps => ps.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureResourceAllocationEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResourceAllocation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ResourceName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ResourceEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Technology).IsRequired().HasMaxLength(100);
            entity.Property(e => e.AllocationPercentage).HasColumnType("decimal(5,2)");
            entity.Property(e => e.BillingRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CostRate).HasColumnType("decimal(18,2)");

            // Configure relationship with Project
            entity.HasOne(ra => ra.Project)
                  .WithMany(p => p.ResourceAllocations)
                  .HasForeignKey(ra => ra.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
