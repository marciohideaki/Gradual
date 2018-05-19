USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_anexo_removerPorProcurador') 
	DROP PROCEDURE prc_cotista_fidc_procurador_anexo_removerPorProcurador
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_anexo_removerPorProcurador
(
	@IdCotistaFidcProcurador int
)
AS
BEGIN
	
	DELETE FROM dbo.TbCotistaFidcProcuradorAnexo
	WHERE IdCotistaFidcProcurador = @IdCotistaFidcProcurador

END
GO
