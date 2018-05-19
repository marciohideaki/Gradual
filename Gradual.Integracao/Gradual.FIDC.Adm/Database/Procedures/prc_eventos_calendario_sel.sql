USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_eventos_calendario_sel') 
	DROP PROCEDURE prc_eventos_calendario_sel
GO


CREATE PROCEDURE dbo.prc_eventos_calendario_sel
(
	@idCalendarioEvento int = null,
	@idFundoCadastro int = null,
	@nomeFundo varchar(100) = null,
	@dtEvento date = null,
	@dtEventoEnd date = null
)
AS
BEGIN	
	SELECT 
		 ec.idCalendarioEvento
		,ec.idFundoCadastro
		,f.nomeFundo
		,ec.dtEvento
		,ec.descEvento
		,ec.emailEvento
		,ec.enviarNotificacaoDia
		,ec.mostrarHome
	FROM dbo.tbEventoCalendario ec
		INNER JOIN tbFundoCadastro f On (ec.idFundoCadastro = f.idFundoCadastro)
	WHERE (@idCalendarioEvento is null or idCalendarioEvento = @idCalendarioEvento)
		AND (@idFundoCadastro is null or ec.idFundoCadastro = @idFundoCadastro)
		AND (@nomeFundo is null or f.nomeFundo = @nomeFundo)
		AND (@dtEvento is null or ec.dtEvento >= @dtEvento)
		AND (@dtEventoEnd is null or ec.dtEvento < @dtEventoEnd)
END
GO
