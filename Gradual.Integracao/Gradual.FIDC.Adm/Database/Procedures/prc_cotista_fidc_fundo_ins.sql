USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_fundo_ins') 
	DROP PROCEDURE prc_cotista_fidc_fundo_ins
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_fundo_ins
(
	@IdCotistaFidc int,
	@IdFundoCadastro int,
	@DtInclusao datetime2
)
AS
BEGIN
	
	INSERT INTO dbo.TbCotistaFidcFundo
	(
		IdCotistaFidc,
		IdFundoCadastro,
		DtInclusao
	)
	VALUES
	(
		@IdCotistaFidc,
		@IdFundoCadastro,
		@DtInclusao
	)

	SELECT SCOPE_IDENTITY()

END
GO
