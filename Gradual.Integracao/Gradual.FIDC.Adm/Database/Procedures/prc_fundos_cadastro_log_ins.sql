USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_cadastro_log_ins') 
	DROP PROCEDURE prc_fundos_cadastro_log_ins
GO

CREATE PROCEDURE dbo.prc_fundos_cadastro_log_ins
(
	@idFundoCadastro int,
	@nomeFundo varchar(100),
	@cnpjFundo char(14),
	@nomeAdministrador varchar(100),
	@cnpjAdministrador char(14),
	@nomeCustodiante varchar(100),
	@cnpjCustodiante char(14),
	@nomeGestor	varchar(100),
	@cnpjGestor	char(14),
	@isAtivo bit,
	@tipoTransacao varchar(10),
	@usuarioLogado varchar(100)
)
AS
BEGIN
	
	INSERT INTO dbo.tbFundoCadastroLog
	(
		idFundoCadastro,
		nomeFundo,
		cnpjFundo,
		nomeAdministrador,
		cnpjAdministrador,
		nomeCustodiante,
		cnpjCustodiante,
		nomeGestor,
		cnpjGestor, 
		isAtivo,
		tipoTransacao,
		usuarioLogado,
		dtAlteracao
	)
	VALUES
	(
		@idFundoCadastro,
		@nomeFundo,
		@cnpjFundo,
		@nomeAdministrador,
		@cnpjAdministrador,
		@nomeCustodiante,
		@cnpjCustodiante,
		@nomeGestor,
		@cnpjGestor,
		@isAtivo,
		@tipoTransacao,
		@usuarioLogado,
		getdate()
	)
	
END
GO
