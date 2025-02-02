using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MessageReactions",
                type: "NVARCHAR(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(45)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LectureReactions",
                type: "NVARCHAR(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ArticleReactions",
                type: "NVARCHAR(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "MessageReactions",
                type: "VARCHAR(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(45)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "LectureReactions",
                type: "NVARCHAR(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(45)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "ArticleReactions",
                type: "NVARCHAR(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(45)");
        }
    }
}
