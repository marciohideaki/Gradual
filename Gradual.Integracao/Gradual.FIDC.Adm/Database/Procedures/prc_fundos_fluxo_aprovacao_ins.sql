USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_fluxo_aprovacao_ins') 
	DROP PROCEDURE prc_fundos_fluxo_aprovacao_ins
GO

CREATE PROCEDURE prc_fundos_fluxo_aprovacao_ins
	@IdFundoCadastro INT,
	@IdFundoFluxoGrupoEtapa INT,
	@IdFundoFluxoStatus INT,
	@DtInicio DATETIME,
	@DtConclusao DATETIME = NULL,
	@UsuarioResponsavel VARCHAR(100)
AS
BEGIN
	INSERT INTO TbFundoFluxoAprovacao
	VALUES(@IdFundoCadastro, @IdFundoFluxoGrupoEtapa, @IdFundoFluxoStatus, @DtInicio, @DtConclusao, @UsuarioResponsavel)
	
	SELECT SCOPE_IDENTITY()	
END
GO
