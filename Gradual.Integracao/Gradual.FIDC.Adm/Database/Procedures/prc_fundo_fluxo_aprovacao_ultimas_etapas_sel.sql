USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_aprovacao_ultimas_etapas_sel') 
	DROP PROCEDURE prc_fundo_fluxo_aprovacao_ultimas_etapas_sel
GO

CREATE PROCEDURE prc_fundo_fluxo_aprovacao_ultimas_etapas_sel
	@IdFundoCadastro INT
AS
BEGIN

	SELECT 
		B.IdFundoFluxoAprovacao,
		B.IdFundoCadastro,
		B.IdFundoFluxoGrupoEtapa,
		B.IdFundoFluxoStatus,
		B.DtInicio,
		B.DtConclusao,
		B.UsuarioResponsavel
	FROM
	(
	SELECT
		MAX(IdFundoFluxoAprovacao) AS IdFundoFluxoAprovacao,
		IdFundoFluxoGrupoEtapa
	FROM TbFundoFluxoAprovacao
	WHERE IdFundoCadastro = @IdFundoCadastro
	GROUP BY IdFundoCadastro, IdFundoFluxoGrupoEtapa
	)A  (IdFundoFluxoAprovacao, IdFundoCadastro)
	JOIN TbFundoFluxoAprovacao B ON B.IdFundoFluxoAprovacao = A.IdFundoFluxoAprovacao
END
GO
