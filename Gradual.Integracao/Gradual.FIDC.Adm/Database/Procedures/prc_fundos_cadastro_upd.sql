use GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_cadastro_upd') 
	DROP PROCEDURE prc_fundos_cadastro_upd
GO


CREATE PROCEDURE dbo.prc_fundos_cadastro_upd
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
	@TxGestao decimal(5,2) = null,
	@TxCustodia decimal(5,2) = null,
	@TxConsultoria decimal(5,2) = null,
	@isAtivo bit
)
AS
BEGIN
	
	UPDATE dbo.tbFundoCadastro
	SET
		nomeFundo = @nomeFundo,
		cnpjFundo = @cnpjFundo,
		nomeAdministrador = @nomeAdministrador,
		cnpjAdministrador = @cnpjAdministrador,
		nomeCustodiante = @nomeCustodiante,
		cnpjCustodiante = @cnpjCustodiante,
		nomeGestor = @nomeGestor,
		cnpjGestor = @cnpjGestor,
		TxGestao = @TxGestao,
		TxCustodia = @TxCustodia,
		TxConsultoria = @TxConsultoria,
		isAtivo = @isAtivo
	WHERE idFundoCadastro = @idFundoCadastro

END
GO
