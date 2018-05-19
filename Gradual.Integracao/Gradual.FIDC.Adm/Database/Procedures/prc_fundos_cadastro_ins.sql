USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_cadastro_ins') 
	DROP PROCEDURE prc_fundos_cadastro_ins
GO

CREATE PROCEDURE dbo.prc_fundos_cadastro_ins
(
	@nomeFundo varchar(100),
	@cnpjFundo char(14),
	@nomeAdministrador varchar(100),
	@cnpjAdministrador char(14),
	@nomeCustodiante varchar(100),
	@cnpjCustodiante char(14),
	@nomeGestor	varchar(100),
	@cnpjGestor	char(14),
	@TxGestao decimal(5,2) = null,
	@TxCustodia decimal(5,2) = null,
	@TxConsultoria decimal(5,2) = null,
	@isAtivo bit
)
AS
BEGIN
	
	INSERT INTO dbo.tbFundoCadastro
	(
		nomeFundo,
		cnpjFundo,
		nomeAdministrador,
		cnpjAdministrador,
		nomeCustodiante,
		cnpjCustodiante,
		nomeGestor,
		cnpjGestor, 
		TxGestao,
		TxCustodia,
		TxConsultoria,
		isAtivo
	)
	VALUES
	(
		@nomeFundo,
		@cnpjFundo,
		@nomeAdministrador,
		@cnpjAdministrador,
		@nomeCustodiante,
		@cnpjCustodiante,
		@nomeGestor,
		@cnpjGestor,
		@TxGestao,
		@TxCustodia,
		@TxConsultoria,
		@isAtivo
	)

	SELECT SCOPE_IDENTITY()

END
GO
