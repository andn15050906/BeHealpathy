using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Frequency",
                table: "Routines",
                newName: "Repeater");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Routines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Routines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Routines",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RepeaterSequenceId",
                table: "Routines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Routines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Tag",
                table: "Routines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "RepeaterSequenceId",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Routines");

            migrationBuilder.RenameColumn(
                name: "Repeater",
                table: "Routines",
                newName: "Frequency");
        }
    }
}
