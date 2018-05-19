USE GradualFundosADM
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbCotistaFidc')
BEGIN
CREATE TABLE dbo.TbCotistaFidc
(
	IdCotistaFidc int primary key identity,
	NomeCotista varchar(255) not null,
	CpfCnpj varchar(14) not null,
	Email varchar(100) not null,
	DataNascFundacao DateTime2 not null,
	IsAtivo bit not null default 1,
	DtInclusao datetime2 not null default getdate(),
	QtdCotas int,
	DsClasseCotas varchar(100),
	DtVencimentoCadastro datetime2
)
END
GO
