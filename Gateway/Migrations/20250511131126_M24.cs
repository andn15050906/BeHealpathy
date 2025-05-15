using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_McqChoices_McqQuestions_McqQuestionId",
                table: "McqChoices");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapMilestones_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapMilestones");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapPhases_Roadmaps_RoadmapId",
                table: "RoadmapPhases");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapRecommendations_RoadmapMilestones_MilestoneId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropTable(
                name: "RoadmapProgress");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropIndex(
                name: "IX_Users_Phone",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_RoadmapRecommendations_MilestoneId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_McqChoices",
                table: "McqChoices");

            migrationBuilder.DropIndex(
                name: "IX_McqChoices_McqQuestionId",
                table: "McqChoices");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MilestoneId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "RoadmapMilestones");

            migrationBuilder.DropColumn(
                name: "RepeatTimesRequired",
                table: "RoadmapMilestones");

            migrationBuilder.DropColumn(
                name: "McqQuestionId",
                table: "McqChoices");

            migrationBuilder.RenameColumn(
                name: "IsRequired",
                table: "RoadmapMilestones",
                newName: "IsSkipped");

            migrationBuilder.AddColumn<Guid>(
                name: "MentalProfileId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsScientific",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RoadmapPhaseId",
                table: "RoadmapRecommendations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RoadmapId",
                table: "RoadmapPhases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredToAdvance",
                table: "RoadmapPhases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QuestionsToAdvance",
                table: "RoadmapPhases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoadmapPhaseId",
                table: "RoadmapMilestones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "RoadmapMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "RoadmapMilestones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "RoadmapMilestones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "McqAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Lectures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LectureType",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaData",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrentIndex",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedCompletionDate",
                table: "Enrollments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Outcome",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbUrl",
                table: "Courses",
                type: "NVARCHAR(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Courses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Outcomes",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");

            migrationBuilder.AlterColumn<string>(
                name: "Intro",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountExpiry",
                table: "Courses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Courses",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "NVARCHAR(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1000)");

            migrationBuilder.AddColumn<string>(
                name: "AdvisorExpectedOutcome",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedCompletion",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_McqChoices",
                table: "McqChoices",
                columns: new[] { "SubmissionId", "McqAnswerId" });

            migrationBuilder.CreateTable(
                name: "MentalProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attribute = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentalProfile", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "MentalProfileId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AdvisorId",
                table: "Users",
                column: "AdvisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MentalProfileId",
                table: "Users",
                column: "MentalProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapRecommendations_RoadmapPhaseId",
                table: "RoadmapRecommendations",
                column: "RoadmapPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_McqChoices_McqAnswerId",
                table: "McqChoices",
                column: "McqAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_McqChoices_McqAnswers_McqAnswerId",
                table: "McqChoices",
                column: "McqAnswerId",
                principalTable: "McqAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapMilestones_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapMilestones",
                column: "RoadmapPhaseId",
                principalTable: "RoadmapPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapPhases_Roadmaps_RoadmapId",
                table: "RoadmapPhases",
                column: "RoadmapId",
                principalTable: "Roadmaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapRecommendations_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapRecommendations",
                column: "RoadmapPhaseId",
                principalTable: "RoadmapPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Advisors_AdvisorId",
                table: "Users",
                column: "AdvisorId",
                principalTable: "Advisors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_MentalProfile_MentalProfileId",
                table: "Users",
                column: "MentalProfileId",
                principalTable: "MentalProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_McqChoices_McqAnswers_McqAnswerId",
                table: "McqChoices");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapMilestones_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapMilestones");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapPhases_Roadmaps_RoadmapId",
                table: "RoadmapPhases");

            migrationBuilder.DropForeignKey(
                name: "FK_RoadmapRecommendations_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Advisors_AdvisorId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_MentalProfile_MentalProfileId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "MentalProfile");

            migrationBuilder.DropIndex(
                name: "IX_Users_AdvisorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MentalProfileId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_RoadmapRecommendations_RoadmapPhaseId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_McqChoices",
                table: "McqChoices");

            migrationBuilder.DropIndex(
                name: "IX_McqChoices_McqAnswerId",
                table: "McqChoices");

            migrationBuilder.DropColumn(
                name: "MentalProfileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsScientific",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "RoadmapPhaseId",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "IsRequiredToAdvance",
                table: "RoadmapPhases");

            migrationBuilder.DropColumn(
                name: "QuestionsToAdvance",
                table: "RoadmapPhases");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "RoadmapMilestones");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "RoadmapMilestones");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "RoadmapMilestones");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "McqAnswers");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "LectureType",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "MetaData",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "CurrentIndex",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "ExpectedCompletionDate",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "AdvisorExpectedOutcome",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ExpectedCompletion",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "IsSkipped",
                table: "RoadmapMilestones",
                newName: "IsRequired");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "VARCHAR(45)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MilestoneId",
                table: "RoadmapRecommendations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RoadmapId",
                table: "RoadmapPhases",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoadmapPhaseId",
                table: "RoadmapMilestones",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "RoadmapMilestones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RepeatTimesRequired",
                table: "RoadmapMilestones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "McqQuestionId",
                table: "McqChoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ThumbUrl",
                table: "Courses",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Requirements",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Outcomes",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Intro",
                table: "Courses",
                type: "NVARCHAR(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscountExpiry",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "NVARCHAR(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1000)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_McqChoices",
                table: "McqChoices",
                columns: new[] { "SubmissionId", "McqQuestionId" });

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoadmapProgress",
                columns: table => new
                {
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Milestone = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapProgress", x => new { x.CreatorId, x.Milestone });
                    table.ForeignKey(
                        name: "FK_RoadmapProgress_RoadmapPhases_RoadmapPhaseId",
                        column: x => x.RoadmapPhaseId,
                        principalTable: "RoadmapPhases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoadmapProgress_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Choice = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Phone",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapRecommendations_MilestoneId",
                table: "RoadmapRecommendations",
                column: "MilestoneId");

            migrationBuilder.CreateIndex(
                name: "IX_McqChoices_McqQuestionId",
                table: "McqChoices",
                column: "McqQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_CreatorId",
                table: "Preferences",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapProgress_RoadmapPhaseId",
                table: "RoadmapProgress",
                column: "RoadmapPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_CreatorId",
                table: "Settings",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_McqChoices_McqQuestions_McqQuestionId",
                table: "McqChoices",
                column: "McqQuestionId",
                principalTable: "McqQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapMilestones_RoadmapPhases_RoadmapPhaseId",
                table: "RoadmapMilestones",
                column: "RoadmapPhaseId",
                principalTable: "RoadmapPhases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapPhases_Roadmaps_RoadmapId",
                table: "RoadmapPhases",
                column: "RoadmapId",
                principalTable: "Roadmaps",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoadmapRecommendations_RoadmapMilestones_MilestoneId",
                table: "RoadmapRecommendations",
                column: "MilestoneId",
                principalTable: "RoadmapMilestones",
                principalColumn: "Id");
        }
    }
}
