USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbCotistaFidcProcurador')
BEGIN
	CREATE TABLE TbCotistaFidcProcurador
	(
		IdCotistaFidcProcurador INT PRIMARY KEY IDENTITY,
		IdCotistaFidc INT FOREIGN KEY REFERENCES TbCotistaFidc(IdCotistaFidc),
		NomeProcurador varchar(200),
		CPF char(11) not null,
		DtInsercao datetime2 not null
	)
END
GO
