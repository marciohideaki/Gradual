use GradualFundosAdm
go

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_cotista_fidc_fundo_sel') 
	DROP PROCEDURE prc_cotista_fidc_fundo_sel
GO

CREATE PROCEDURE dbo.prc_cotista_fidc_fundo_sel
(
	@IdCotistaFidcFundo int = null,
	@IdCotistaFidc  int = null,
	@IdFundoCadastro  int = null
)
AS
BEGIN
	
	SELECT A.IdCotistaFidcFundo, b.NomeCotista, c.nomeFundo, B.Email EmailCotista
	FROM dbo.TbCotistaFidcFundo A
	JOIN dbo.TbCotistaFidc B on B.IdCotistaFidc = A.IdCotistaFidc
	JOIN dbo.tbFundoCadastro C on C.idFundoCadastro = A.IdFundoCadastro
	WHERE (@IdCotistaFidcFundo is null or IdCotistaFidcFundo = @IdCotistaFidcFundo)
		AND (@IdCotistaFidc is null or A.IdCotistaFidc = @IdCotistaFidc)
		AND (@IdFundoCadastro is null or A.IdFundoCadastro = @IdFundoCadastro)
END
GO
