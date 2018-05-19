USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_fluxo_aprovacao_anexo_ins') 
	DROP PROCEDURE prc_fundo_fluxo_aprovacao_anexo_ins
GO

CREATE PROCEDURE prc_fundo_fluxo_aprovacao_anexo_ins
	@IdFundoFluxoAprovacao INT,
	@CaminhoAnexo VARCHAR(200)
AS
BEGIN
	INSERT INTO TbFundoFluxoAprovacaoAnexo
	VALUES (
		@IdFundoFluxoAprovacao,
		@CaminhoAnexo
	)
END
GO
