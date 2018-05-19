USE GradualFundosADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'tbEventoCalendario')
BEGIN
CREATE TABLE dbo.tbEventoCalendario
(
	idCalendarioEvento int primary key identity,
	idFundoCadastro int not null,
	dtEvento datetime not null,
	descEvento varchar(300) not null,
	emailEvento char(100) not null,
	enviarNotificacaoDia bit not null default 1,
	mostrarHome bit not null default 1,
)
END
GO
