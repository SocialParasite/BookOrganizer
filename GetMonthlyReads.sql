DROP PROC IF EXISTS GetMonthlyReads
GO

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
END