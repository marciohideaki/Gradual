USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_sel') 
	DROP PROCEDURE prc_cotista_fidc_sel
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_sel
(
	@IdCotistaFidc int = null
)
AS
BEGIN	
	SELECT 
		IdCotistaFidc,
		NomeCotista,
		CpfCnpj,
		Email,
		DataNascFundacao,
		IsAtivo,
		DtInclusao,
		QtdCotas,
		DsClasseCotas,
		DtVencimentoCadastro
	FROM dbo.TbCotistaFidc
	WHERE (@IdCotistaFidc is null or IdCotistaFidc = @IdCotistaFidc)
END
GO
