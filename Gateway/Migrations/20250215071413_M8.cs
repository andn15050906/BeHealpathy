using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ChatMessages",
                type: "NVARCHAR(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ChatMessages",
                type: "NVARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");
        }
    }
}
