using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class booksSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Series_BookSeriesId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookSeriesId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookSeriesId",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BooksSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    BookId = table.Column<Guid>(nullable: false),
                    SeriesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksSeries_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BooksSeries_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BooksSeries_BookId",
                table: "BooksSeries",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksSeries_SeriesId",
                table: "BooksSeries",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BooksSeries");

            migrationBuilder.AddColumn<Guid>(
                name: "BookSeriesId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookSeriesId",
                table: "Books",
                column: "BookSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Series_BookSeriesId",
                table: "Books",
                column: "BookSeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
