USE GRADUALFUNDOSADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFluxoAlteracaoRegulamentoGrupoEtapa')
BEGIN
	CREATE TABLE TbFluxoAlteracaoRegulamentoGrupoEtapa
	(
		IdFluxoAlteracaoRegulamentoGrupoEtapa INT PRIMARY KEY,
		IdFluxoAlteracaoRegulamentoGrupo INT FOREIGN KEY REFERENCES TbFundoFluxoAlteracaoRegulamentoGrupo(IdFundoFluxoAlteracaoRegulamentoGrupo) NOT NULL,
		DsFluxoAlteracaoRegulamentoGrupoEtapa VARCHAR(100) NOT NULL,
	)
	;

	CREATE NONCLUSTERED INDEX IX_TbFundoFluxoAlteracaoRegulamentoGrupoEtapa_IdFundoFluxoAlteracaoRegulamentoGrupo ON TbFluxoAlteracaoRegulamentoGrupoEtapa(IdFluxoAlteracaoRegulamentoGrupo)

	/*
	--CARGA INICIAL
	INSERT INTO TbFundoFluxoGrupoEtapa
	SELECT		 1, 1, 'Elaboração e revisão pela gestora'
	UNION SELECT 2, 1, 'Revisão da administradora'
	UNION SELECT 3, 1, 'Revisão do custodiante'
	UNION SELECT 4, 1, 'Registro dos documentos'
	*/
	
END
GO
