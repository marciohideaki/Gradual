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
	SELECT 1, 'Conclu�do'
	UNION SELECT 2, 'N�o se Aplica'
	UNION SELECT 3, 'A Definir'
	UNION SELECT 4, 'Em An�lise'
	;
END
GO
