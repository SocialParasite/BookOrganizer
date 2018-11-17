using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class AuthorAndNationalityLinked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NationalityId",
                table: "Authors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_NationalityId",
                table: "Authors",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Nationalities_NationalityId",
                table: "Authors",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Nationalities_NationalityId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_NationalityId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "Authors");
        }
    }
}
