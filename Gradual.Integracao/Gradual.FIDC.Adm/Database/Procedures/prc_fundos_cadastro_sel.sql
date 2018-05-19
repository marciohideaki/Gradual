USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_cadastro_sel') 
	DROP PROCEDURE prc_fundos_cadastro_sel
GO


CREATE PROCEDURE dbo.prc_fundos_cadastro_sel
(
	@idFundoCadastro int = null,
	@nomeFundo varchar(100) = null
)
AS
BEGIN	
	SELECT 
		idFundoCadastro,
		nomeFundo,
		cnpjFundo,
		nomeAdministrador,
		cnpjAdministrador,
		nomeCustodiante,
		cnpjCustodiante,
		nomeGestor,
		cnpjGestor,
		TxGestao,
		TxConsultoria,
		TxCustodia,
		isAtivo
	FROM dbo.tbFundoCadastro
	WHERE (@idFundoCadastro is null or idFundoCadastro = @idFundoCadastro)
	AND (@nomeFundo is null or nomeFundo = @nomeFundo)
END
GO
