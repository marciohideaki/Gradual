USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFluxoAlteracaoRegulamentoGrupo')
BEGIN
	CREATE TABLE TbFluxoAlteracaoRegulamentoGrupo
	(
		IdFluxoAlteracaoRegulamentoGrupo INT PRIMARY KEY,
		DsFluxoAlteracaoRegulamentoGrupo VARCHAR(100)
	)
	;

	insert TbFluxoAlteracaoRegulamentoGrupo
	values (1, 'Alteração de Regulamentos')
	;
END
GO
