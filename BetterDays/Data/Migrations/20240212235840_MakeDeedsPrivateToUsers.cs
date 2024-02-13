using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetterDays.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeDeedsPrivateToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (!migrationBuilder.IsSqlite())
            {
                throw new InvalidOperationException("Only SQLite is supported");
            }

            migrationBuilder.CreateTable(
                name: "temp_Deeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deeds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "temp_DoneDeeds",
                columns: table => new
                {
                    DailyNoteId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeedId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneDeeds", x => new { x.DailyNoteId, x.DeedId });
                    table.ForeignKey(
                        name: "FK_DoneDeeds_DailyNotes_DailyNoteId",
                        column: x => x.DailyNoteId,
                        principalTable: "DailyNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoneDeeds_Deeds_DeedId",
                        column: x => x.DeedId,
                        principalTable: "Deeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                @"INSERT INTO temp_Deeds(UserId, Name)
SELECT DISTINCT DailyNotes.UserId, Deeds.Name
FROM DoneDeeds
JOIN Deeds on DoneDeeds.DeedId = Deeds.Id
JOIN DailyNotes on DoneDeeds.DailyNoteId = DailyNotes.Id;");

            migrationBuilder.Sql(
                @"INSERT INTO temp_DoneDeeds(DailyNoteId, DeedId)
SELECT DailyNoteId, temp_Deeds.Id
FROM DoneDeeds
JOIN Deeds ON DoneDeeds.DeedId = Deeds.Id
JOIN temp_Deeds ON temp_Deeds.Name = Deeds.Name");

            migrationBuilder.Sql("PRAGMA foreign_keys = 0;", suppressTransaction: true);

            migrationBuilder.DropTable("Deeds");
            migrationBuilder.RenameTable("temp_Deeds", null, "Deeds");

            migrationBuilder.DropTable("DoneDeeds");
            migrationBuilder.RenameTable("temp_DoneDeeds", null, "DoneDeeds");

            migrationBuilder.Sql("PRAGMA foreign_keys = 1;", suppressTransaction: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new InvalidOperationException();
        }
    }
}
