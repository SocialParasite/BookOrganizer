using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    Title = table.Column<string>(maxLength: 256, nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false),
                    PageCount = table.Column<int>(nullable: false),
                    ISBN = table.Column<string>(maxLength: 13, nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    BookCoverPicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
