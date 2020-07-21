using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
    public partial class GetMonthlyReads_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROC GetMonthlyReads
	            @year	AS INT,
	            @month	AS INT
                AS
                SET NOCOUNT ON;
                
                BEGIN
                	SELECT		b.Title, br.ReadDate
                	FROM		Books b
                	JOIN		BooksReadDate br ON BookId = b.Id
                	WHERE		YEAR(br.ReadDate) = @year
                	AND			MONTH(br.ReadDate) = @month
                END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROC IF EXISTS GetMonthlyReads");
        }
    }
}
