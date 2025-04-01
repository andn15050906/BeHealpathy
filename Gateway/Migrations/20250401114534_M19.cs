using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YogaPoses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    EmbeddedUrl = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    VideoUrl = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Level = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    EquipmentRequirement = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    ThumpUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YogaPoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YogaPoses_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_YogaPoses_CreatorId",
                table: "YogaPoses",
                column: "CreatorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YogaPoses");
        }
    }
}
