SELECT		DATENAME(month, br.ReadDate)						AS 'Month',
			COUNT(*)											AS 'Books read',
			MIN(b.PageCount)									AS 'Shortest book',
			MAX(b.PageCount)									AS 'Longest book',
			SUM(b.PageCount)									AS 'Total pages', 
			IIF((MONTH(br.ReadDate) = MONTH(GETDATE()) AND (YEAR(br.ReadDate) = YEAR(GETDATE()))),
				SUM(b.PageCount)/DAY(GETDATE()),
				SUM(b.PageCount)/DAY(EOMONTH(br.ReadDate)))		AS 'Daily average',
			AVG(b.PageCount)									AS 'Average book length'
FROM		Books b
JOIN		BooksReadDate br ON BookId = b.Id
WHERE		YEAR(br.ReadDate) = 2019
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
WHERE		YEAR(br.ReadDate) = 2019