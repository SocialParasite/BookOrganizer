using Microsoft.EntityFrameworkCore.Migrations;

namespace BookOrganizer.Data.SqlServer.Migrations
{
	public partial class GetAnnualBookStatisticsInRange_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
                CREATE PROC GetAnnualBookStatisticsInRange
					@start_year INT = NULL,
					@end_year	INT = NULL
				AS
				SET NOCOUNT ON;
				
				BEGIN
					IF @start_year IS NULL
						SET @start_year = YEAR(GetDate())-10
					IF @end_year IS NULL
						SET @end_year = YEAR(GetDate())
				
					SELECT		YEAR(br.ReadDate)									AS 'Year',
								COUNT(*)											AS 'TotalNumberOfBooksRead',
								MIN(b.PageCount)									AS 'ShortestBookRead',
								MAX(b.PageCount)									AS 'LongestBookRead',
								SUM(b.PageCount)									AS 'TotalPagesRead', 
								IIF(YEAR(br.ReadDate) = YEAR(GETDATE()), 
									CAST
									(
										SUM(b.PageCount)/
											(SELECT CAST(DAY(GETDATE()) AS DECIMAL)/
												CAST(DAY(EOMONTH(GETDATE())) AS DECIMAL)
												+MONTH(GETDATE())-1) 
										AS INT
									), 
									SUM(b.PageCount)/12)							AS 'AveragePagesReadMonthly',
								AVG(b.PageCount)									AS 'AverageBookLength'
					FROM		Books b
					JOIN		BooksReadDate br ON BookId = b.Id
					WHERE		YEAR(br.ReadDate) BETWEEN @start_year AND @end_year
					GROUP BY	YEAR(br.ReadDate)
					UNION ALL
						SELECT '', 
								COUNT(*), 
								MIN(b.PageCount)									AS 'Shortest book',
								MAX(b.PageCount)									AS 'Longest book',
								SUM(b.PageCount)									AS 'Total pages', 
								SUM(b.PageCount) / (12*(@end_year-@start_year+1))	AS 'Monthly average',
								AVG(b.PageCount)									AS 'Average book length'
					FROM		Books b
					JOIN		BooksReadDate br ON BookId = b.Id
					WHERE		YEAR(br.ReadDate) BETWEEN @start_year AND @end_year
				END"
			);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROC IF EXISTS GetAnnualBookStatisticsInRange");
        }
    }
}
