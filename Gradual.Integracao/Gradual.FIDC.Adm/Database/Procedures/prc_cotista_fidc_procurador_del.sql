USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_del') 
	DROP PROCEDURE prc_cotista_fidc_procurador_del
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_del
(
	@IdCotistaFidcProcurador int
)
AS
BEGIN
	
	DELETE FROM dbo.TbCotistaFidcProcurador
	WHERE IdCotistaFidcProcurador = @IdCotistaFidcProcurador

END
GO
