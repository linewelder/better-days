using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterDays.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyNotes",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNotes", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "Deeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoneDeeds",
                columns: table => new
                {
                    DailyNoteDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DeedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneDeeds", x => new { x.DailyNoteDate, x.DeedId });
                    table.ForeignKey(
                        name: "FK_DoneDeeds_DailyNotes_DailyNoteDate",
                        column: x => x.DailyNoteDate,
                        principalTable: "DailyNotes",
                        principalColumn: "Date",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoneDeeds_Deeds_DeedId",
                        column: x => x.DeedId,
                        principalTable: "Deeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoneDeeds_DeedId",
                table: "DoneDeeds",
                column: "DeedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoneDeeds");

            migrationBuilder.DropTable(
                name: "DailyNotes");

            migrationBuilder.DropTable(
                name: "Deeds");
        }
    }
}
