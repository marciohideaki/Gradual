USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoAlteracaoRegulamentoAnexo')
BEGIN
	CREATE TABLE TbFundoFluxoAlteracaoRegulamentoAnexo
	(
		IdFundoFluxoAlteracaoRegulamentoAnexo INT PRIMARY KEY IDENTITY,
		IdFundoFluxoAlteracaoRegulamento INT NOT NULL FOREIGN KEY REFERENCES TbFundoFluxoAlteracaoRegulamento(IdFundoFluxoAlteracaoRegulamento),
		CaminhoAnexo VARCHAR(200) NOT NULL
	)
	;

	CREATE NONCLUSTERED INDEX IX_TbFundoFluxoAlteracaoRegulamentoAnexo_IdFundoFluxoAlteracaoRegulamento ON TbFundoFluxoAlteracaoRegulamento(IdFundoFluxoAlteracaoRegulamento)
	;
END
GO
