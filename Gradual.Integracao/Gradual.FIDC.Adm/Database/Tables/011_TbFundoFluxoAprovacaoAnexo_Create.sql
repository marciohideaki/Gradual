USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoAprovacaoAnexo')
BEGIN
	CREATE TABLE TbFundoFluxoAprovacaoAnexo
	(
		IdFundoFluxoAprovacaoAnexo INT PRIMARY KEY IDENTITY,
		IdFundoFluxoAprovacao INT NOT NULL FOREIGN KEY REFERENCES TbFundoFluxoAprovacao(IdFundoFluxoAprovacao),
		CaminhoAnexo VARCHAR(200) NOT NULL
	)
	;

	CREATE NONCLUSTERED INDEX IX_TbFundoFluxoAprovacaoAnexo_IdFundoFluxoAprovacao ON TbFundoFluxoAprovacaoAnexo(IdFundoFluxoAprovacao)
	;
END
GO
