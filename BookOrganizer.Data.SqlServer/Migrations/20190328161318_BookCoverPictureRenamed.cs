using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class BookCoverPictureRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksReadDate_Books_BookId",
                table: "BooksReadDate");

            migrationBuilder.RenameColumn(
                name: "BookCoverPicture",
                table: "Books",
                newName: "BookCoverPicturePath");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "BooksReadDate",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BooksReadDate_Books_BookId",
                table: "BooksReadDate",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksReadDate_Books_BookId",
                table: "BooksReadDate");

            migrationBuilder.RenameColumn(
                name: "BookCoverPicturePath",
                table: "Books",
                newName: "BookCoverPicture");

            migrationBuilder.AlterColumn<Guid>(
                name: "BookId",
                table: "BooksReadDate",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "PublisherId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "LanguageId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Languages_LanguageId",
                table: "Books",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BooksReadDate_Books_BookId",
                table: "BooksReadDate",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
