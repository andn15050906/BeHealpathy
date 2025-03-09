using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoadmapId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "RoadmapPhases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RoadmapProgress",
                columns: table => new
                {
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoadmapPhaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MilestonesCompleted = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapProgress", x => new { x.CreatorId, x.RoadmapPhaseId });
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "RoadmapId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapProgress_RoadmapPhaseId",
                table: "RoadmapProgress",
                column: "RoadmapPhaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoadmapProgress");

            migrationBuilder.DropColumn(
                name: "RoadmapId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "RoadmapPhases");
        }
    }
}
