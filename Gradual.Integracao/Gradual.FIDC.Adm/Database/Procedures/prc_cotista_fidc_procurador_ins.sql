USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_ins') 
	DROP PROCEDURE prc_cotista_fidc_procurador_ins
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_ins
(
	@NomeProcurador varchar(200),
	@IdCotistaFidc int,
	@CPF char(11)
)
AS
BEGIN
	
	INSERT INTO dbo.TbCotistaFidcProcurador
	(		
		NomeProcurador,
		IdCotistaFidc,
		CPF,
		DtInsercao
	)
	VALUES
	(		
		@NomeProcurador,
		@IdCotistaFidc,
		@CPF,
		getdate()
	)

	SELECT SCOPE_IDENTITY()

END
GO
