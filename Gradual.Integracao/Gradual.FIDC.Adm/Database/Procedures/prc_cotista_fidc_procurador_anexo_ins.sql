USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_procurador_anexo_ins') 
	DROP PROCEDURE prc_cotista_fidc_procurador_anexo_ins
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_procurador_anexo_ins
(
	@IdCotistaFidcProcurador int,
	@CaminhoAnexo varchar(200),
	@TipoAnexo varchar(50)
)
AS
BEGIN
	
	INSERT INTO dbo.TbCotistaFidcProcuradorAnexo
	(		
		IdCotistaFidcProcurador,
		CaminhoAnexo,
		TipoAnexo,
		DtInclusao
	)
	VALUES
	(		
		@IdCotistaFidcProcurador,
		@CaminhoAnexo,
		@TipoAnexo,
		GETDATE()
	)

	SELECT SCOPE_IDENTITY()

END
GO
