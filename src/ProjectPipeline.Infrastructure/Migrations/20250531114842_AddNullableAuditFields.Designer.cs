﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectPipeline.Infrastructure.Data.Context;

#nullable disable

namespace ProjectPipeline.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250531114842_AddNullableAuditFields")]
    partial class AddNullableAuditFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.BusinessUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("HeadOfUnit")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeletedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("BusinessUnits");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.ProfileStatusHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ChangedBy")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ChangedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Comments")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("FromStatus")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ProfileSubmissionId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ToStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChangedBy");

                    b.HasIndex("ChangedDate");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeletedBy");

                    b.HasIndex("ProfileSubmissionId");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("ProfileStatusHistory");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.ProfileSubmission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ActualJoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CandidateEmail")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CandidateName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CandidatePhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ExpectedJoiningDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("ExpectedSalary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ExperienceYears")
                        .HasColumnType("int");

                    b.Property<string>("HoldReason")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("InterviewDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InterviewFeedback")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal?>("InterviewScore")
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("InterviewerName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OfferLetterPath")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal?>("OfferedSalary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RejectionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RejectionReason")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ResumePath")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("ScreeningDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SelectionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ShortlistDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusComments")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubmittedBy")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TechnicalFeedback")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal?>("TechnicalScore")
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("Technology")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CandidateEmail");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeletedBy");

                    b.HasIndex("LastUpdatedBy");

                    b.HasIndex("ProjectId");

                    b.HasIndex("Status");

                    b.HasIndex("SubmissionDate");

                    b.HasIndex("SubmittedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("ProfileSubmissions");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("ActualValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BusinessUnitId")
                        .HasColumnType("int");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EstimatedValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ExpectedClosureDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ProfilesSelected")
                        .HasColumnType("int");

                    b.Property<int>("ProfilesShortlisted")
                        .HasColumnType("int");

                    b.Property<int>("ProfilesSubmitted")
                        .HasColumnType("int");

                    b.Property<string>("ProjectType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("StatusReason")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Technology")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("DeletedBy");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("BusinessUnitId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Department")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Designation")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("BusinessUnitId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.BusinessUnit", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.ProfileStatusHistory", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", "ChangedByUser")
                        .WithMany()
                        .HasForeignKey("ChangedBy")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.ProfileSubmission", "ProfileSubmission")
                        .WithMany("StatusHistory")
                        .HasForeignKey("ProfileSubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ChangedByUser");

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("ProfileSubmission");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.ProfileSubmission", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "LastUpdatedByUser")
                        .WithMany()
                        .HasForeignKey("LastUpdatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.Project", "Project")
                        .WithMany("ProfileSubmissions")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", "SubmittedByUser")
                        .WithMany()
                        .HasForeignKey("SubmittedBy")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("LastUpdatedByUser");

                    b.Navigation("Project");

                    b.Navigation("SubmittedByUser");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.Project", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.BusinessUnit", "BusinessUnit")
                        .WithMany("Projects")
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectPipeline.Core.Entities.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "DeletedByUser")
                        .WithMany()
                        .HasForeignKey("DeletedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectPipeline.Core.Entities.User", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedBy")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("BusinessUnit");

                    b.Navigation("CreatedByUser");

                    b.Navigation("DeletedByUser");

                    b.Navigation("UpdatedByUser");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.User", b =>
                {
                    b.HasOne("ProjectPipeline.Core.Entities.BusinessUnit", "BusinessUnit")
                        .WithMany("Users")
                        .HasForeignKey("BusinessUnitId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("BusinessUnit");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.BusinessUnit", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.ProfileSubmission", b =>
                {
                    b.Navigation("StatusHistory");
                });

            modelBuilder.Entity("ProjectPipeline.Core.Entities.Project", b =>
                {
                    b.Navigation("ProfileSubmissions");
                });
#pragma warning restore 612, 618
        }
    }
}
