USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_categoria_sub_categoria_del') 
	DROP PROCEDURE prc_fundo_categoria_sub_categoria_del
GO

CREATE PROCEDURE prc_fundo_categoria_sub_categoria_del
	@IdFundoCadastro int,
	@IdFundoCategoria int,
	@IdFundoSubCategoria int
AS
BEGIN
	DELETE TbFundoCategoriaSubCategoria
	WHERE IdFundoCadastro = @IdFundoCadastro 
	AND IdFundoCategoria = @IdFundoCategoria 
	AND IdFundoSubCategoria = @IdFundoSubCategoria
END
GO
