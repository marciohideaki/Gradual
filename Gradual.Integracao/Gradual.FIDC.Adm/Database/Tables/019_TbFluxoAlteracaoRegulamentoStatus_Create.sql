USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFluxoAlteracaoRegulamentoStatus')
BEGIN
	CREATE TABLE TbFluxoAlteracaoRegulamentoStatus
	(
		IdFluxoAlteracaoRegulamentoStatus INT PRIMARY KEY,
		DsFluxoAlteracaoRegulamentoStatus VARCHAR(100)
	)
	;

	INSERT INTO TbFluxoAlteracaoRegulamentoStatus
	SELECT 1, 'Concluído'
	UNION SELECT 2, 'Não se Aplica'
	UNION SELECT 3, 'A Definir'
	UNION SELECT 4, 'Em Análise'
	;
END
GO
