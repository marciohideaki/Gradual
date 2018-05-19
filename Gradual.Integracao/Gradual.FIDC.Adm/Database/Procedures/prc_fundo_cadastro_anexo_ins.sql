USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_cadastro_anexo_ins') 
	DROP PROCEDURE prc_fundo_cadastro_anexo_ins
GO

CREATE PROCEDURE prc_fundo_cadastro_anexo_ins
	@IdFundoCadastro INT,
	@CaminhoAnexo VARCHAR(200),
	@TipoAnexo VARCHAR(50)
AS
BEGIN
	INSERT INTO TbFundoCadastroAnexo
	VALUES (
		@IdFundoCadastro,
		@CaminhoAnexo,
		@TipoAnexo,
		GETDATE()
	)
END
GO
