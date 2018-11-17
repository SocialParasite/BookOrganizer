using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class Format : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formats",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    Abbreveation = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formats");
        }
    }
}
