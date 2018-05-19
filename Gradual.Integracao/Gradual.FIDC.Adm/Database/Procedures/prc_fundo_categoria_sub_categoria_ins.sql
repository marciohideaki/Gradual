USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_categoria_sub_categoria_ins') 
	DROP PROCEDURE prc_fundo_categoria_sub_categoria_ins
GO

CREATE PROCEDURE prc_fundo_categoria_sub_categoria_ins
	@IdFundoCadastro int,
	@IdFundoCategoria int,
	@IdFundoSubCategoria int
AS
BEGIN
	--Verifica existência do registro antes de realizar a inserção
	IF NOT EXISTS (
		SELECT 1 FROM TbFundoCategoriaSubCategoria
		WHERE IdFundoCadastro = @IdFundoCadastro AND IdFundoCategoria = @IdFundoCategoria and IdFundoSubCategoria = @IdFundoSubCategoria
	)
	BEGIN
		INSERT INTO TbFundoCategoriaSubCategoria
		VALUES (@IdFundoCadastro, @IdFundoCategoria, @IdFundoSubCategoria)

		SELECT SCOPE_IDENTITY()
	END
END
GO
