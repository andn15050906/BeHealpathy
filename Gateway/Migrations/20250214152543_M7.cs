using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "AdvisorId", "AvatarUrl", "Balance", "Bio", "DateOfBirth", "Email", "EnrollmentCount", "FullName", "IsApproved", "IsBanned", "IsDeleted", "IsVerified", "MetaFullName", "Password", "Phone", "RefreshToken", "Role", "Token", "UnbanDate", "UserName" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), (byte)0, null, "", 0L, "", null, "", 0, "", false, false, false, false, "", "", null, "", "Member", "", null, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
