using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPipeline.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceAllocations");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "JoiningDate",
                table: "ProfileSubmissions",
                newName: "ShortlistDate");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "ProfileSubmissions",
                newName: "OfferLetterPath");

            migrationBuilder.AlterColumn<string>(
                name: "StatusReason",
                table: "Projects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Projects",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ProfileSubmissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualJoiningDate",
                table: "ProfileSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedJoiningDate",
                table: "ProfileSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldReason",
                table: "ProfileSubmissions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewFeedback",
                table: "ProfileSubmissions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InterviewScore",
                table: "ProfileSubmissions",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InterviewerName",
                table: "ProfileSubmissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionDate",
                table: "ProfileSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "ProfileSubmissions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScreeningDate",
                table: "ProfileSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SelectionDate",
                table: "ProfileSubmissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusComments",
                table: "ProfileSubmissions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalFeedback",
                table: "ProfileSubmissions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TechnicalScore",
                table: "ProfileSubmissions",
                type: "decimal(3,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "BusinessUnits",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "BusinessUnits",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "BusinessUnits",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileSubmissionId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: false),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileStatusHistory_AspNetUsers_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileStatusHistory_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileStatusHistory_AspNetUsers_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileStatusHistory_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileStatusHistory_ProfileSubmissions_ProfileSubmissionId",
                        column: x => x.ProfileSubmissionId,
                        principalTable: "ProfileSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DeletedBy",
                table: "Projects",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_CandidateEmail",
                table: "ProfileSubmissions",
                column: "CandidateEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_CreatedBy",
                table: "ProfileSubmissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_DeletedBy",
                table: "ProfileSubmissions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_LastUpdatedBy",
                table: "ProfileSubmissions",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_Status",
                table: "ProfileSubmissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_SubmissionDate",
                table: "ProfileSubmissions",
                column: "SubmissionDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_SubmittedBy",
                table: "ProfileSubmissions",
                column: "SubmittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubmissions_UpdatedBy",
                table: "ProfileSubmissions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_CreatedBy",
                table: "BusinessUnits",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_DeletedBy",
                table: "BusinessUnits",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_UpdatedBy",
                table: "BusinessUnits",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_ChangedBy",
                table: "ProfileStatusHistory",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_ChangedDate",
                table: "ProfileStatusHistory",
                column: "ChangedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_CreatedBy",
                table: "ProfileStatusHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_DeletedBy",
                table: "ProfileStatusHistory",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_ProfileSubmissionId",
                table: "ProfileStatusHistory",
                column: "ProfileSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileStatusHistory_UpdatedBy",
                table: "ProfileStatusHistory",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_CreatedBy",
                table: "BusinessUnits",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_DeletedBy",
                table: "BusinessUnits",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_UpdatedBy",
                table: "BusinessUnits",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_CreatedBy",
                table: "ProfileSubmissions",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_DeletedBy",
                table: "ProfileSubmissions",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_LastUpdatedBy",
                table: "ProfileSubmissions",
                column: "LastUpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_SubmittedBy",
                table: "ProfileSubmissions",
                column: "SubmittedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_UpdatedBy",
                table: "ProfileSubmissions",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_DeletedBy",
                table: "Projects",
                column: "DeletedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_CreatedBy",
                table: "BusinessUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_DeletedBy",
                table: "BusinessUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessUnits_AspNetUsers_UpdatedBy",
                table: "BusinessUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_CreatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_DeletedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_LastUpdatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_SubmittedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileSubmissions_AspNetUsers_UpdatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_DeletedBy",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "ProfileStatusHistory");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DeletedBy",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_CandidateEmail",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_CreatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_DeletedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_LastUpdatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_Status",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_SubmissionDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_SubmittedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProfileSubmissions_UpdatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUnits_CreatedBy",
                table: "BusinessUnits");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUnits_DeletedBy",
                table: "BusinessUnits");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUnits_UpdatedBy",
                table: "BusinessUnits");

            migrationBuilder.DropColumn(
                name: "ActualJoiningDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "ExpectedJoiningDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "HoldReason",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "InterviewFeedback",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "InterviewScore",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "InterviewerName",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "RejectionDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "ScreeningDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "SelectionDate",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "StatusComments",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "TechnicalFeedback",
                table: "ProfileSubmissions");

            migrationBuilder.DropColumn(
                name: "TechnicalScore",
                table: "ProfileSubmissions");

            migrationBuilder.RenameColumn(
                name: "ShortlistDate",
                table: "ProfileSubmissions",
                newName: "JoiningDate");

            migrationBuilder.RenameColumn(
                name: "OfferLetterPath",
                table: "ProfileSubmissions",
                newName: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "StatusReason",
                table: "Projects",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProfileSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "BusinessUnits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "BusinessUnits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "BusinessUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResourceAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AllocationEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllocationPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AllocationStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AllocationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BillingRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ReasonForChange = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReplacedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResourceEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ResourceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Technology = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceAllocations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceAllocations_ProjectId",
                table: "ResourceAllocations",
                column: "ProjectId");
        }
    }
}
