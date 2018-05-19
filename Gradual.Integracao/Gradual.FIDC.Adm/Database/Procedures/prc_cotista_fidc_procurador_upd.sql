USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_upd') 
	DROP PROCEDURE prc_cotista_fidc_procurador_upd
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_upd
(
	@IdCotistaFidcProcurador int,
	@IdCotistaFidc int,
	@NomeProcurador varchar(200),
	@CPF char(11)
)
AS
BEGIN
	UPDATE dbo.TbCotistaFidcProcurador			
	SET IdCotistaFidc = @IdCotistaFidc,
		NomeProcurador = @NomeProcurador,
		CPF = @CPF
	WHERE IdCotistaFidcProcurador = @IdCotistaFidcProcurador

	SELECT SCOPE_IDENTITY()
END
GO
