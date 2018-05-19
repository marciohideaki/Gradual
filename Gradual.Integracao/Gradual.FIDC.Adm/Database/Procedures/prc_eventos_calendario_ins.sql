USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_eventos_calendario_ins') 
	DROP PROCEDURE prc_eventos_calendario_ins
GO

CREATE PROCEDURE dbo.prc_eventos_calendario_ins
(
	@idFundoCadastro int,
	@dtEvento datetime,
	@descEvento varchar(300),
	@emailEvento varchar(100),
	@enviarNotificacaoDia bit,
	@mostrarHome bit
)
AS
BEGIN
	
	INSERT INTO dbo.tbEventoCalendario
	(
		idFundoCadastro,
		dtEvento,
		descEvento,
		emailEvento,
		enviarNotificacaoDia,
		mostrarHome 
	)
	VALUES
	(
		@idFundoCadastro,
		@dtEvento,
		@descEvento,
		@emailEvento,
		@enviarNotificacaoDia,
		@mostrarHome
	)

	SELECT SCOPE_IDENTITY()

END
GO
