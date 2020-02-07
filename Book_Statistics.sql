SELECT		YEAR(br.ReadDate)									AS 'Year',
			COUNT(*)											AS 'Books read',
			MIN(b.PageCount)									AS 'Shortest book',
			MAX(b.PageCount)									AS 'Longest book',
			SUM(b.PageCount)									AS 'Total pages', 
			IIF(YEAR(br.ReadDate) = YEAR(GETDATE()), 
				CAST
				(
					SUM(b.PageCount)/
						(SELECT CAST(DAY(GETDATE()) AS DECIMAL)/
								CAST(DAY(EOMONTH(GETDATE())) AS DECIMAL)
								+ MONTH(GETDATE()) - 1) 
					AS INT
				), 
				SUM(b.PageCount)/12)							AS 'Monthly average',
			AVG(b.PageCount)									AS 'Average book length'
FROM		Books b
JOIN		BooksReadDate br ON BookId = b.Id
GROUP BY	YEAR(br.ReadDate)