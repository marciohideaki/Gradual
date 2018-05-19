USE GradualFundosAdm
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_cadastro_categoria_sub_categoria_sel') 
	DROP PROCEDURE prc_fundos_cadastro_categoria_sub_categoria_sel
GO

CREATE PROCEDURE prc_fundos_cadastro_categoria_sub_categoria_sel
	@IdFundoCategoria INT = NULL
AS
BEGIN
	SELECT DISTINCT A.idFundoCadastro, A.nomeFundo
	FROM tbFundoCadastro A
	JOIN TbFundoCategoriaSubCategoria B ON B.IdFundoCadastro = a.idFundoCadastro
	where @IdFundoCategoria is null or B.IdFundoCategoria = @IdFundoCategoria
END
GO
