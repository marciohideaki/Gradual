USE GradualFundosADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'tbFundoCadastro')
BEGIN
CREATE TABLE dbo.tbFundoCadastro
(
	idFundoCadastro int primary key identity,
	nomeFundo varchar(100) not null unique,
	cnpjFundo char(14) not null,
	nomeAdministrador varchar(100) not null,
	cnpjAdministrador char(14) not null,
	nomeCustodiante varchar(100) not null,
	cnpjCustodiante char(14) not null,
	nomeGestor varchar(100) not null,
	cnpjGestor char(14) not null,
	isAtivo bit not null default 1
)
END
GO
