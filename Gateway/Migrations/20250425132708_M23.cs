using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoadmapProgress",
                table: "RoadmapProgress");

            migrationBuilder.DropColumn(
                name: "MilestonesCompleted",
                table: "RoadmapProgress");

            migrationBuilder.RenameColumn(
                name: "TimeSpentRequired",
                table: "RoadmapMilestones",
                newName: "Index");

            migrationBuilder.AddColumn<Guid>(
                name: "Milestone",
                table: "RoadmapProgress",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "RoadmapMilestones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoadmapProgress",
                table: "RoadmapProgress",
                columns: new[] { "CreatorId", "Milestone" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoadmapProgress",
                table: "RoadmapProgress");

            migrationBuilder.DropColumn(
                name: "Milestone",
                table: "RoadmapProgress");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "RoadmapMilestones");

            migrationBuilder.RenameColumn(
                name: "Index",
                table: "RoadmapMilestones",
                newName: "TimeSpentRequired");

            migrationBuilder.AddColumn<string>(
                name: "MilestonesCompleted",
                table: "RoadmapProgress",
                type: "VARCHAR(MAX)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoadmapProgress",
                table: "RoadmapProgress",
                columns: new[] { "CreatorId", "RoadmapPhaseId" });
        }
    }
}
