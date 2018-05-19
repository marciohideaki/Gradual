USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoFluxoGrupo')
BEGIN
	CREATE TABLE TbFundoFluxoGrupo
	(
		IdFundoFluxoGrupo INT PRIMARY KEY,
		DsFundoFluxoGrupo VARCHAR(100)
	)
	;

	INSERT INTO TbFundoFluxoGrupo
	SELECT		 1, 'Regulamento / Instrumento de constituição'
	UNION SELECT 2, 'Opinião Legal'
	UNION SELECT 3, 'CNPJ'
	UNION SELECT 4, 'Petição CVM'
	UNION SELECT 5, 'Monitoramento de rating'
	UNION SELECT 6, 'Contrato de custódia'
	UNION SELECT 7, 'Cadastro do fundo na custódia'
	UNION SELECT 8, 'Abertura de conta no banco cobrador'
	UNION SELECT 9, 'Abertura de conta no banco custodiante'
	UNION SELECT 10, 'Contrato de cessão / Termo de cessão'
	UNION SELECT 11, 'Contrato de consultoria'
	UNION SELECT 12, 'Cadastramento no portal do custodiante'
	UNION SELECT 13, 'Cadastramento dos cotistas'
	UNION SELECT 14, 'Conta SELIC'
	UNION SELECT 15, 'Conta CETIP'
	UNION SELECT 16, 'ISIN'
	;
END
GO
