USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_eventos_calendario_del') 
	DROP PROCEDURE prc_eventos_calendario_del
GO

CREATE PROCEDURE dbo.prc_eventos_calendario_del
(
	@IdCalendarioEvento int
)
AS
BEGIN
	
	DELETE FROM dbo.TbEventoCalendario
	WHERE IdCalendarioEvento = @IdCalendarioEvento
	
END
GO
