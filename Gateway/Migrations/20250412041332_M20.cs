using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipants_Users_UserId",
                table: "MeetingParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants");

            migrationBuilder.DropIndex(
                name: "IX_MeetingParticipants_UserId",
                table: "MeetingParticipants");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MeetingParticipants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants",
                columns: new[] { "MeetingId", "CreatorId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingParticipants_CreatorId",
                table: "MeetingParticipants",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipants_Users_CreatorId",
                table: "MeetingParticipants",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingParticipants_Users_CreatorId",
                table: "MeetingParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants");

            migrationBuilder.DropIndex(
                name: "IX_MeetingParticipants_CreatorId",
                table: "MeetingParticipants");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "MeetingParticipants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetingParticipants",
                table: "MeetingParticipants",
                columns: new[] { "MeetingId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingParticipants_UserId",
                table: "MeetingParticipants",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingParticipants_Users_UserId",
                table: "MeetingParticipants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
