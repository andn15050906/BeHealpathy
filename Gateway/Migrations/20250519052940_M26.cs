using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gateway.Migrations
{
    /// <inheritdoc />
    public partial class M26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "McqQuestions");

            migrationBuilder.RenameColumn(
                name: "TraitDescription",
                table: "RoadmapRecommendations",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Trait",
                table: "RoadmapRecommendations",
                newName: "Source");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Roadmaps",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Coupons",
                table: "Roadmaps",
                type: "NVARCHAR(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roadmaps",
                type: "NVARCHAR(3000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Roadmaps",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscountExpiry",
                table: "Roadmaps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Roadmaps",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbUrl",
                table: "Roadmaps",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetEntityId",
                table: "RoadmapRecommendations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "EntityType",
                table: "RoadmapRecommendations",
                type: "VARCHAR(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "RoadmapRecommendations",
                type: "NVARCHAR(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoadmapRecommendations",
                type: "NVARCHAR(1000)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "RoadmapRecommendations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAction",
                table: "RoadmapRecommendations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneralTip",
                table: "RoadmapRecommendations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MoodTags",
                table: "RoadmapRecommendations",
                type: "NVARCHAR(1000)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionsToAdvance",
                table: "RoadmapPhases",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoadmapPhases",
                type: "NVARCHAR(3000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(3000)");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "RoadmapPhases",
                type: "NVARCHAR(3000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "RoadmapPhases",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "RoadmapMilestones",
                type: "NVARCHAR(3000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RoadmapMilestones",
                type: "NVARCHAR(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Precondition",
                table: "McqQuestions",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionValue",
                table: "McqAnswers",
                type: "NVARCHAR(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Coupons",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "DiscountExpiry",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "ThumbUrl",
                table: "Roadmaps");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "IsAction",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "IsGeneralTip",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "MoodTags",
                table: "RoadmapRecommendations");

            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "RoadmapPhases");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "RoadmapPhases");

            migrationBuilder.DropColumn(
                name: "Precondition",
                table: "McqQuestions");

            migrationBuilder.DropColumn(
                name: "OptionValue",
                table: "McqAnswers");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "RoadmapRecommendations",
                newName: "TraitDescription");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "RoadmapRecommendations",
                newName: "Trait");

            migrationBuilder.AlterColumn<Guid>(
                name: "TargetEntityId",
                table: "RoadmapRecommendations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityType",
                table: "RoadmapRecommendations",
                type: "VARCHAR(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuestionsToAdvance",
                table: "RoadmapPhases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RoadmapPhases",
                type: "NVARCHAR(3000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(3000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "RoadmapMilestones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(3000)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "RoadmapMilestones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)");

            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "McqQuestions",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");
        }
    }
}
