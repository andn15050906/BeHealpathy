using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "Submissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Routines",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SurveyId",
                table: "Submissions",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Surveys_SurveyId",
                table: "Submissions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Surveys_SurveyId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_SurveyId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Routines");
        }
    }
}
