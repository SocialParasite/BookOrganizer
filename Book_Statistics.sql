SELECT		YEAR(br.ReadDate)									AS 'Year',
			COUNT(*)											AS 'Books read',
			MIN(b.PageCount)									AS 'Shortest book',
			MAX(b.PageCount)									AS 'Longest book',
			SUM(b.PageCount)									AS 'Total pages', 
			IIF(YEAR(br.ReadDate) = YEAR(GETDATE()), 
				SUM(b.PageCount)/(SELECT MONTH(GETDATE())), 
				SUM(b.PageCount)/12)							AS 'Monthly average',
			AVG(b.PageCount)									AS 'Average book length'
FROM		Books b
JOIN		BooksReadDate br ON BookId = b.Id
GROUP BY	YEAR(br.ReadDate)