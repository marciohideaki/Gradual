USE GradualFundosAdm
GO

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoCategoriaSubCategoria')
BEGIN
	CREATE TABLE TbFundoCategoriaSubCategoria
	(
		IdFundoCategoriaSubCategoria int primary key identity,
		IdFundoCadastro int not null FOREIGN KEY REFERENCES tbFundoCadastro (idFundoCadastro),
		IdFundoCategoria int not null FOREIGN KEY REFERENCES TbFundoCategoria (IdFundoCategoria),
		IdFundoSubCategoria int not null FOREIGN KEY REFERENCES TbFundoSubCategoria (IdFundoSubCategoria)
	)
	;

	ALTER TABLE TbFundoCategoriaSubCategoria 
		ADD constraint ucTbFundoCategoriaSubCategoria01 unique (IdFundoCadastro, IdFundoCategoria, IdFundoSubCategoria)
	;
END
GO