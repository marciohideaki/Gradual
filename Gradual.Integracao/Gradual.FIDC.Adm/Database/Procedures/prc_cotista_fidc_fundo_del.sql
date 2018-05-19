USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_fundo_del') 
	DROP PROCEDURE prc_cotista_fidc_fundo_del
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_fundo_del
(
	@IdCotistaFidcFundo int
)
AS
BEGIN
	
	DELETE FROM dbo.TbCotistaFidcFundo
	WHERE IdCotistaFidcFundo = @IdCotistaFidcFundo
	
END
GO
