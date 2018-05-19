use gradualfundosadm
go

IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE name = 'TbFundoCadastroAnexo')
BEGIN
	CREATE TABLE TbFundoCadastroAnexo
	(
		IdFundoCadastroAnexo INT PRIMARY KEY IDENTITY,
		IdFundoCadastro INT NOT NULL FOREIGN KEY REFERENCES TbFundoCadastro(IdFundoCadastro),
		CaminhoAnexo VARCHAR(200) NOT NULL,
		TipoAnexo VARCHAR(50),
		DtInclusao Datetime2 NOT NULL
	)
END