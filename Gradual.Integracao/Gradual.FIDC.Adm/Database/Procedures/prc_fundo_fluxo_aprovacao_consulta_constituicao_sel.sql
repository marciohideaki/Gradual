USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_aprovacao_consulta_constituicao_sel') 
	DROP PROCEDURE prc_fundo_fluxo_aprovacao_consulta_constituicao_sel
GO

CREATE PROCEDURE prc_fundo_fluxo_aprovacao_consulta_constituicao_sel
	@SelecionarConcluidos BIT,
	@SelecionarPendentes BIT,
	@DataDe DATETIME = NULL,
	@DataAte DATETIME = NULL,
	@IdFundoCadastro INT = NULL,
	@IdFundoFluxoGrupo INT = NULL
AS
BEGIN

	--agrupa os fundos por grupos e a última etapa de cada grupo
	select max(A.IdFundoFluxoGrupoEtapa) IdFundoFluxoGrupoEtapa, cad.IdFundoCadastro, C.IdFundoFluxoGrupo
	into #tempEtapas
	from tbFundoCadastro cad
	left join TbFundoFluxoAprovacao A on a.IdFundoCadastro = cad.idFundoCadastro
	left join TbFundoFluxoGrupoEtapa B on B.IdFundoFluxoGrupoEtapa = A.IdFundoFluxoGrupoEtapa
	left join TbFundoFluxoGrupo C on C.IdFundoFluxoGrupo = B.IdFundoFluxoGrupo
	JOIN TbFundoCategoriaSubCategoria D on D.IdFundoCadastro = cad.IdFundoCadastro
	JOIN TbFundoCategoria E on E.IdFundoCategoria = D.IdFundoCategoria and (E.IdFundoCategoria = 4) --todo apenas 4
	where @IdFundoCadastro is null or cad.idFundoCadastro = @IdFundoCadastro
	group by cad.IdFundoCadastro, C.IdFundoFluxoGrupo

	--obtém os ids do fluxo de aprovação e agrupa por fundo e por grupo
	select max(A.IdFundoFluxoAprovacao) IdFundoFluxoAprovacao, c.IdFundoFluxoGrupo, cad.idFundoCadastro
	into #temp
	from tbFundoCadastro cad
	join TbFundoFluxoAprovacao A on a.IdFundoCadastro = cad.idFundoCadastro
	join #tempEtapas B on cad.idFundoCadastro = B.IdFundoCadastro and b.IdFundoFluxoGrupoEtapa = a.IdFundoFluxoGrupoEtapa
	join TbFundoFluxoGrupo C on C.IdFundoFluxoGrupo = B.IdFundoFluxoGrupo
	where @IdFundoCadastro is null or cad.idFundoCadastro = @IdFundoCadastro	
	group by cad.IdFundoCadastro, C.IdFundoFluxoGrupo

	DECLARE @QTDTOTETAPAS INT --ARMAZENA O TOTAL DE ETAPAS PARAMETRIZADAS NO BANCO DE DADOS
	SELECT @QTDTOTETAPAS = COUNT(*) FROM TbFundoFluxoGrupoEtapa

	select nomeFundo, DsFundoFluxoGrupo, DsFundoFluxoGrupoEtapa, c.idFundoCadastro, d.IdFundoFluxoGrupoEtapa, 
	StatusGeral, 
	DsFundoFluxoStatus, B.DtInicio, B.DtConclusao, D.IdFundoFluxoGrupo, A.IdFundoFluxoAprovacao
	into #TEMP2
	from #temp a
	join TbFundoFluxoAprovacao b on b.IdFundoFluxoAprovacao = a.IdFundoFluxoAprovacao
	join tbFundoCadastro c on c.idFundoCadastro = b.IdFundoCadastro
	join TbFundoFluxoGrupoEtapa d on d.IdFundoFluxoGrupoEtapa = b.IdFundoFluxoGrupoEtapa
	join TbFundoFluxoGrupo e on e.IdFundoFluxoGrupo = d.IdFundoFluxoGrupo
	join TbFundoFluxoStatus f on f.IdFundoFluxoStatus = b.IdFundoFluxoStatus
	join (
		SELECT 
			IdFundoCadastro, 
			CASE WHEN COUNT(DISTINCT IdFundoFluxoGrupoEtapa) = @QTDTOTETAPAS THEN 1 ELSE 0 END AS StatusGeral --VERIFICA SE O FUNDO EM QUESTÃO JÁ PASSOU POR TODAS AS ETAPAS
		FROM TbFundoFluxoAprovacao
		WHERE IdFundoFluxoStatus = 1 OR IdFundoFluxoStatus = 2 --APENAS ETAPAS COM STATUS CONCLUIDO OU NAO SE APLICA
		GROUP BY IdFundoCadastro)G(IdFundoCadastro, StatusGeral) ON G.IdFundoCadastro = a.idFundoCadastro
	order by idFundoCadastro, e.IdFundoFluxoGrupo

	select * from
	(
	select *
	from #temp2
	WHERE ((@SelecionarConcluidos = 1 AND StatusGeral = 1) OR (@SelecionarPendentes = 1 AND StatusGeral = 0))
		AND (@DataDe IS NULL OR DtInicio >= @DataDe) AND (@DataAte IS NULL OR DtConclusao IS NULL OR DtConclusao <= @DataAte)	
		and (@IdFundoFluxoGrupo is null or IdFundoFluxoGrupo = @IdFundoFluxoGrupo)	
	--order by idFundoCadastro, IdFundoFluxoGrupo, IdFundoFluxoGrupoEtapa
	UNION ALL
	SELECT 
		fun.nomeFundo, 'N/A' DsFundoFluxoGrupo, 'N/A' DsFundoFluxoGrupoEtapa, fun.idFundoCadastro, 0 IdFundoFluxoGrupoEtapa,
		0 StatusGeral, 'N/A' DsFundoFluxoStatus, '' DtInicio, '' DtConclusao, 0 IdFundoFluxoGrupo, 0 IdFundoFluxoAprovacao
	FROM tbFundoCadastro fun
	join TbFundoCategoriaSubCategoria cs on cs.IdFundoCadastro = fun.idFundoCadastro and (cs.IdFundoCategoria = 4)
	left join TbFundoFluxoAprovacao ap on ap.IdFundoCadastro = fun.idFundoCadastro
	where ap.IdFundoFluxoAprovacao is null AND (@IdFundoCadastro IS NULL OR fun.idFundoCadastro = @IdFundoCadastro)
	)xx(nomeFundo, DsFundoFluxoGrupo, DsFundoFluxoGrupoEtapa, idFundoCadastro, IdFundoFluxoGrupoEtapa, StatusGeral, DsFundoFluxoStatus,
		DtInicio, DtConclusao, IdFundoFluxoGrupo, IdFundoFluxoAprovacao
	)
	where @SelecionarPendentes = 1
	order by idFundoCadastro, IdFundoFluxoGrupo, IdFundoFluxoGrupoEtapa asc
	
END
GO
