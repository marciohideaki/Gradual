USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_categoria_sub_categoria_sel') 
	DROP PROCEDURE prc_fundo_categoria_sub_categoria_sel
GO

CREATE PROCEDURE prc_fundo_categoria_sub_categoria_sel
	@IdFundoCategoria INT,
	@IdFundoSubCategoria INT
AS
BEGIN 
	SELECT A.idFundoCadastro, A.nomeFundo
	FROM tbFundoCadastro A
	JOIN TbFundoCategoriaSubCategoria B ON B.IdFundoCadastro = A.idFundoCadastro
	WHERE B.IdFundoCategoria = 	@IdFundoCategoria
	AND B.IdFundoSubCategoria = @IdFundoSubCategoria
END
GO
