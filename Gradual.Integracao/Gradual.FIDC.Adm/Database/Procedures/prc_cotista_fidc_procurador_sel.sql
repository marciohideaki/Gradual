USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_sel') 
	DROP PROCEDURE prc_cotista_fidc_procurador_sel
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_sel
(
	@IdCotistaFidcProcurador int = null,
	@IdCotistaFidc int = null
)
AS
BEGIN
	
	SELECT 
		IdCotistaFidcProcurador,
		IdCotistaFidc,
		NomeProcurador,
		CPF
	FROM dbo.TbCotistaFidcProcurador
	WHERE (@IdCotistaFidcProcurador is null or IdCotistaFidcProcurador = @IdCotistaFidcProcurador) 
	  AND (@IdCotistaFidc is null or IdCotistaFidc = @IdCotistaFidc) 
	
END
GO
