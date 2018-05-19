use GradualFundosAdm
go

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbCotistaFidcFundo')
BEGIN
CREATE TABLE dbo.TbCotistaFidcFundo
(
	IdCotistaFidcFundo int primary key identity,
	IdCotistaFidc int not null foreign key references TbCotistaFidc(IdCotistaFidc),
	IdFundoCadastro int not null foreign key references TbFundoCadastro(IdFundoCadastro),
	DtInclusao datetime2 not null default getdate()
)
END
GO

ALTER TABLE dbo.TbCotistaFidcFundo ADD CONSTRAINT ucTbCotistaFidcFundo_IdCotistaFidcIdFundoCadastro UNIQUE (IdCotistaFidc, IdFundoCadastro)
GO
