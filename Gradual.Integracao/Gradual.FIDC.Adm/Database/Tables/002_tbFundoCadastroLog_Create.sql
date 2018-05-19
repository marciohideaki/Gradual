USE GradualFundosADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'tbFundoCadastroLog')
BEGIN

	CREATE TABLE dbo.tbFundoCadastroLog
	(
		idFundoCadastroLog int primary key identity,
		idFundoCadastro int foreign key references tbFundoCadastro (idFundoCadastro),
		nomeFundo varchar(100) not null,
		cnpjFundo char(14) not null,
		nomeAdministrador varchar(100) not null,
		cnpjAdministrador char(14) not null,
		nomeCustodiante varchar(100) not null,
		cnpjCustodiante char(14) not null,
		nomeGestor varchar(100) not null,
		cnpjGestor char(14) not null,
		isAtivo bit not null default 1,
		tipoTransacao varchar(10) not null,
		usuarioLogado varchar(100) not null,
		dtAlteracao datetime not null
	)

	create nonclustered index ixtbFundoCadastroLogtbFundoCadastro on tbFundoCadastroLog (idFundoCadastro)

END
GO