using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterDays.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToDailyNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_DailyNotes_AspNetUsers_UserId",
                table: "DailyNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyNotes_AspNetUsers_UserId",
                table: "DailyNotes");
        }
    }
}
