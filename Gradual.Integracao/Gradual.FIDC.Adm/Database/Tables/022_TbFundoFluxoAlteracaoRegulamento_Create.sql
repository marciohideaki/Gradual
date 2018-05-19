USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoAlteracaoRegulamento')
BEGIN
	CREATE TABLE TbFundoFluxoAlteracaoRegulamento
	(
		IdFundoFluxoAlteracaoRegulamento INT PRIMARY KEY IDENTITY,
		IdFundoCadastro INT FOREIGN KEY REFERENCES TbFundoCadastro (IdFundoCadastro),
		IdFluxoAlteracaoRegulamentoGrupoEtapa INT FOREIGN KEY REFERENCES TbFluxoAlteracaoRegulamentoGrupoEtapa (IdFluxoAlteracaoRegulamentoGrupoEtapa),
		IdFluxoAlteracaoRegulamentoStatus INT FOREIGN KEY REFERENCES TbFluxoAlteracaoRegulamentoStatus (IdFluxoAlteracaoRegulamentoStatus),
		DtInicio DATETIME NOT NULL,
		DtConclusao DATETIME,
		UsuarioResponsavel VARCHAR(100) NOT NULL
	)
END
GO
