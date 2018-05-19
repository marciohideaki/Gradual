USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoSubCategoria')
BEGIN

	CREATE TABLE TbFundoSubCategoria
	(
		IdFundoSubCategoria int primary key identity,
		DsFundoSubCategoria varchar(50) unique
	);

	INSERT INTO TbFundoSubCategoria
	SELECT 'FIDC'
	UNION SELECT 'FIP'
	UNION SELECT 'FII'
	UNION SELECT '555'
	
END
GO
