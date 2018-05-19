USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_upd') 
	DROP PROCEDURE prc_cotista_fidc_upd
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_upd
(
	@IdCotistaFidc int,
	@NomeCotista varchar(255),
	@CpfCnpj varchar(14),
	@Email varchar(100),
	@DataNascFundacao datetime2,
	@IsAtivo bit,
	@QtdCotas int = null,
	@DsClasseCotas varchar(100) = null,
	@DtVencimentoCadastro datetime2 = null
)
AS
BEGIN
	
	UPDATE dbo.TbCotistaFidc
	SET
		NomeCotista = @NomeCotista,
		CpfCnpj = @CpfCnpj,
		Email = @Email,
		DataNascFundacao = @DataNascFundacao,
		IsAtivo = @IsAtivo,
		QtdCotas = @QtdCotas,
		DsClasseCotas = @DsClasseCotas,
		DtVencimentoCadastro = @DtVencimentoCadastro
	WHERE IdCotistaFidc = @IdCotistaFidc

END
GO
