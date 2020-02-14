DROP PROC IF EXISTS GetBookStatisticsByYear
GO

CREATE PROC GetBookStatisticsByYear
	@year	AS INT
AS
SET NOCOUNT ON;

SELECT		DATENAME(month, br.ReadDate)						AS 'MonthName',
			COUNT(*)											AS 'TotalNumberOfBooksRead',
			MIN(b.PageCount)									AS 'ShortestBookRead',
			MAX(b.PageCount)									AS 'LongestBookRead',
			SUM(b.PageCount)									AS 'TotalPagesRead', 
			IIF((MONTH(br.ReadDate) = MONTH(GETDATE()) AND (YEAR(br.ReadDate) = YEAR(GETDATE()))),
				SUM(b.PageCount)/DAY(GETDATE()),
				SUM(b.PageCount)/DAY(EOMONTH(br.ReadDate)))		AS 'AveragePagesReadDaily',
			AVG(b.PageCount)									AS 'AverageBookLength'
FROM		Books b
JOIN		BooksReadDate br ON BookId = b.Id
WHERE		YEAR(br.ReadDate) = @year
GROUP BY	MONTH(br.ReadDate), EOMONTH(br.ReadDate), YEAR(br.ReadDate), DATENAME(month, br.ReadDate)
UNION ALL
	SELECT 'Total', 
			COUNT(*), 
			MIN(b.PageCount)									AS 'Shortest book',
			MAX(b.PageCount)									AS 'Longest book',
			SUM(b.PageCount)									AS 'Total pages', 
			SUM(b.PageCount) / 365								AS 'Daily average',
			AVG(b.PageCount)									AS 'Average book length'
FROM		Books b
JOIN		BooksReadDate br ON BookId = b.Id
WHERE		YEAR(br.ReadDate) = @year