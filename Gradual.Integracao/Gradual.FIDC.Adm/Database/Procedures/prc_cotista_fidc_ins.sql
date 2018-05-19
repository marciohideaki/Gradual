USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_ins') 
	DROP PROCEDURE prc_cotista_fidc_ins
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_ins
(
	@NomeCotista varchar(255),
	@CpfCnpj varchar(14),
	@Email varchar(100),
	@DataNascFundacao datetime2,
	@IsAtivo bit,
	@DtInclusao datetime2,
	@QtdCotas int = null,
	@DsClasseCotas varchar(100) = null,
	@DtVencimentoCadastro datetime2 = null
)
AS
BEGIN
	
	INSERT INTO dbo.TbCotistaFidc
	(
		NomeCotista,
		CpfCnpj,
		Email,
		DataNascFundacao,
		IsAtivo,
		DtInclusao,
		QtdCotas,
		DsClasseCotas,
		DtVencimentoCadastro
	)
	VALUES
	(
		@NomeCotista,
		@CpfCnpj,
		@Email,
		@DataNascFundacao,
		@IsAtivo,
		@DtInclusao,
		@QtdCotas,
		@DsClasseCotas,
		@DtVencimentoCadastro
	)

	SELECT SCOPE_IDENTITY()

END
GO
