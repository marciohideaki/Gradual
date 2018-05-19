USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoAprovacao')
BEGIN
	CREATE TABLE TbFundoFluxoAprovacao
	(
		IdFundoFluxoAprovacao INT PRIMARY KEY IDENTITY,
		IdFundoCadastro INT FOREIGN KEY REFERENCES TbFundoCadastro (IdFundoCadastro),
		IdFundoFluxoGrupoEtapa INT FOREIGN KEY REFERENCES TbFundoFluxoGrupoEtapa (IdFundoFluxoGrupoEtapa),
		IdFundoFluxoStatus INT FOREIGN KEY REFERENCES TbFundoFluxoStatus (IdFundoFluxoStatus),
		DtInicio DATETIME NOT NULL,
		DtConclusao DATETIME,
		UsuarioResponsavel VARCHAR(100) NOT NULL
	)
END
GO
