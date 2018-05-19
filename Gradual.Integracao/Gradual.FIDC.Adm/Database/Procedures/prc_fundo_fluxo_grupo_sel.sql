use GradualFundosAdm
go

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_grupo_sel') 
	DROP PROCEDURE prc_fundo_fluxo_grupo_sel
GO

CREATE PROCEDURE prc_fundo_fluxo_grupo_sel
AS
BEGIN

	SELECT IdFundoFluxoGrupo, DsFundoFluxoGrupo
	FROM TbFundoFluxoGrupo

END
GO
