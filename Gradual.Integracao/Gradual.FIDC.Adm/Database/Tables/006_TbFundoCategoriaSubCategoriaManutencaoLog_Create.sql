USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoCategoriaSubCategoriaManutencaoLog')
BEGIN
	CREATE TABLE dbo.TbFundoCategoriaSubCategoriaManutencaoLog
	(
		IdFundoCategoriaSubCategoriaManutencaoLog INT PRIMARY KEY IDENTITY,
		NomeFundo VARCHAR(100) NOT NULL,
		DsFundoCategoria VARCHAR(50) NOT NULL,
		DsFundoSubCategoria varchar(50) NOT NULL,
		UsuarioLogado varchar(100) NOT NULL,
		DtAlteracao datetime NOT NULL,
		TipoTransacao varchar(10) NOT NULL
	)
END
GO
