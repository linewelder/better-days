using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterDays.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoodAndProductivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Mood",
                table: "DailyNotes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Productivity",
                table: "DailyNotes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mood",
                table: "DailyNotes");

            migrationBuilder.DropColumn(
                name: "Productivity",
                table: "DailyNotes");
        }
    }
}
