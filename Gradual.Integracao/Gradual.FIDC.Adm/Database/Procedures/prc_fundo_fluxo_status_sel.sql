USE GradualFundosAdm
GO

ALTER PROCEDURE prc_fundo_fluxo_status_sel
	@IdFundoFluxoStatus INT = NULL
AS
BEGIN
	SELECT IdFundoFluxoStatus, DsFundoFluxoStatus
	FROM TbFundoFluxoStatus
	WHERE @IdFundoFluxoStatus IS NULL OR IdFundoFluxoStatus = @IdFundoFluxoStatus
END
GO
