USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoCategoria')
BEGIN

	CREATE TABLE TbFundoCategoria
	(
		IdFundoCategoria INT PRIMARY KEY,
		DsFundoCategoria VARCHAR(50) unique
	);


	insert into TbFundoCategoria
	select 1, 'Fundos Administrados'
	union select 2, 'Fundos Pré-operacionais'
	union select 3, 'Fundos de Prateleira'
	union select 4, 'Fundos em Constituição'

END
GO
