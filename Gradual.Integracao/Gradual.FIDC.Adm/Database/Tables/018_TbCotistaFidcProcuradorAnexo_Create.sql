USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbCotistaFidcProcuradorAnexo')
BEGIN
	CREATE TABLE TbCotistaFidcProcuradorAnexo
	(
		IdCotistaFidcProcuradorAnexo INT PRIMARY KEY IDENTITY,
		IdCotistaFidcProcurador INT FOREIGN KEY REFERENCES TbCotistaFidcProcurador(IdCotistaFidcProcurador),
		CaminhoAnexo VARCHAR(200) NOT NULL,
		TipoAnexo VARCHAR(50),
		DtInclusao Datetime2 NOT NULL
	)
END
GO
