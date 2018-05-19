USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_grupo_etapa_fluxo_aprovacao_sel') 
	DROP PROCEDURE prc_fundo_fluxo_grupo_etapa_fluxo_aprovacao_sel
GO

CREATE PROCEDURE prc_fundo_fluxo_grupo_etapa_fluxo_aprovacao_sel
	@IdFundoFluxoGrupo INT = NULL
AS
BEGIN
	
	SELECT 
		IdFundoFluxoGrupoEtapa,
		IdFundoFluxoGrupo,
		DsFundoFluxoGrupoEtapa
	FROM TbFundoFluxoGrupoEtapa
	WHERE @IdFundoFluxoGrupo IS NULL OR IdFundoFluxoGrupo = @IdFundoFluxoGrupo

END
GO
