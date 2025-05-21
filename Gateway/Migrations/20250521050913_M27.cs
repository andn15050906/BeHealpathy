using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roadmaps_Advisors_AdvisorId",
                table: "Roadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Roadmaps_AdvisorId",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "AdvisorId",
                table: "Roadmaps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdvisorId",
                table: "Roadmaps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Roadmaps_AdvisorId",
                table: "Roadmaps",
                column: "AdvisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roadmaps_Advisors_AdvisorId",
                table: "Roadmaps",
                column: "AdvisorId",
                principalTable: "Advisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
