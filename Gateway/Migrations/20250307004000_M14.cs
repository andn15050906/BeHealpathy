using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roadmaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    IntroText = table.Column<string>(type: "NVARCHAR(3000)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roadmaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoadmapPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(3000)", nullable: false),
                    TimeSpan = table.Column<int>(type: "int", nullable: false),
                    RoadmapId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadmapPhases_Roadmaps_RoadmapId",
                        column: x => x.RoadmapId,
                        principalTable: "Roadmaps",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoadmapMilestones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    EventName = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    RepeatTimesRequired = table.Column<int>(type: "int", nullable: false),
                    TimeSpentRequired = table.Column<int>(type: "int", nullable: false),
                    RoadmapPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapMilestones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadmapMilestones_RoadmapPhases_RoadmapPhaseId",
                        column: x => x.RoadmapPhaseId,
                        principalTable: "RoadmapPhases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoadmapRecommendations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    MilestoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Trait = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    TraitDescription = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadmapRecommendations_RoadmapMilestones_MilestoneId",
                        column: x => x.MilestoneId,
                        principalTable: "RoadmapMilestones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapMilestones_RoadmapPhaseId",
                table: "RoadmapMilestones",
                column: "RoadmapPhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapPhases_RoadmapId",
                table: "RoadmapPhases",
                column: "RoadmapId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapRecommendations_MilestoneId",
                table: "RoadmapRecommendations",
                column: "MilestoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoadmapRecommendations");

            migrationBuilder.DropTable(
                name: "RoadmapMilestones");

            migrationBuilder.DropTable(
                name: "RoadmapPhases");

            migrationBuilder.DropTable(
                name: "Roadmaps");
        }
    }
}
