using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class PublisherDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Publishers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Publishers");
        }
    }
}
