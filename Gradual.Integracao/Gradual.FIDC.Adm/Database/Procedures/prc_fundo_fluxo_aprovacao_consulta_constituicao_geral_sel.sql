USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_aprovacao_consulta_constituicao_geral_sel') 
	DROP PROCEDURE prc_fundo_fluxo_aprovacao_consulta_constituicao_geral_sel
GO

CREATE PROCEDURE prc_fundo_fluxo_aprovacao_consulta_constituicao_geral_sel
	@IdFundoCadastro INT
AS
BEGIN

	CREATE TABLE #TempIdsFluxos (IdFundoFluxoAprovacao INT PRIMARY KEY)
		
	INSERT INTO #TempIdsFluxos
	SELECT MAX(A.IdFundoFluxoAprovacao) IdFundoFluxoAprovacao
	FROM TbFundoFluxoAprovacao A
	JOIN (SELECT IdFundoCadastro, MAX(IdFundoFluxoGrupoEtapa) IdFundoFluxoGrupoEtapa FROM TbFundoFluxoAprovacao GROUP BY IdFundoCadastro) B
		ON B.IdFundoCadastro = A.IdFundoCadastro AND B.IdFundoFluxoGrupoEtapa = A.IdFundoFluxoGrupoEtapa
	JOIN TbFundoCategoriaSubCategoria C ON C.IdFundoCadastro = A.IdFundoCadastro
	JOIN TbFundoCategoria D on D.IdFundoCategoria = C.IdFundoCategoria
	GROUP BY A.IdFundoCadastro, A.IdFundoFluxoGrupoEtapa

	DECLARE @QTDTOTETAPAS INT --ARMAZENA O TOTAL DE ETAPAS PARAMETRIZADAS NO BANCO DE DADOS
	SELECT @QTDTOTETAPAS = COUNT(*) FROM TbFundoFluxoGrupoEtapa

	SELECT nomeFundo, DsFundoFluxoGrupo, DsFundoFluxoGrupoEtapa, E.idFundoCadastro, C.IdFundoFluxoGrupoEtapa, StatusGeral, D.DsFundoFluxoStatus,
		B.DtInicio, B.DtConclusao
	INTO #TEMPSemGrupo
	FROM #TempIdsFluxos A
	JOIN TbFundoFluxoAprovacao B ON a.IdFundoFluxoAprovacao = B.IdFundoFluxoAprovacao
	JOIN TbFundoFluxoGrupoEtapa C ON C.IdFundoFluxoGrupoEtapa = B.IdFundoFluxoGrupoEtapa
	JOIN TbFundoFluxoStatus D ON D.IdFundoFluxoStatus = B.IdFundoFluxoStatus
	JOIN tbFundoCadastro E ON E.idFundoCadastro = b.IdFundoCadastro
	JOIN TbFundoFluxoGrupo F ON F.IdFundoFluxoGrupo = C.IdFundoFluxoGrupo
	JOIN (
		SELECT 
			IdFundoCadastro, 
			CASE WHEN COUNT(DISTINCT IdFundoFluxoGrupoEtapa) = @QTDTOTETAPAS THEN 1 ELSE 0 END AS StatusGeral --VERIFICA SE O FUNDO EM QUESTÃO JÁ PASSOU POR TODAS AS ETAPAS
		FROM TbFundoFluxoAprovacao
		WHERE IdFundoFluxoStatus = 1 OR IdFundoFluxoStatus = 2 --APENAS ETAPAS COM STATUS CONCLUIDO OU NAO SE APLICA
		GROUP BY IdFundoCadastro)G(IdFundoCadastro, StatusGeral) ON G.IdFundoCadastro = E.idFundoCadastro
	AND (@IdFundoCadastro IS NULL OR E.IdFundoCadastro = @IdFundoCadastro)

	SELECT * 
	FROM #TEMPSemGrupo	

	DROP TABLE #TEMPSemGrupo
	
	DROP TABLE #TempIdsFluxos	
END
GO
