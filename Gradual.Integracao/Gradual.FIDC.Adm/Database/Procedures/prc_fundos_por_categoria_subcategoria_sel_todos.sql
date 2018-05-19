USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_por_categoria_subcategoria_sel_todos') 
	DROP PROCEDURE prc_fundos_por_categoria_subcategoria_sel_todos
GO

CREATE PROCEDURE prc_fundos_por_categoria_subcategoria_sel_todos (
	@IdFundoCategoria INT,
	@IdFundoSubCategoria INT
)
AS
BEGIN 
	SELECT *
	FROM 
	(
		SELECT A.idFundoCadastro, A.nomeFundo, 1 As Pertence
		FROM tbFundoCadastro A
		JOIN TbFundoCategoriaSubCategoria B ON B.IdFundoCadastro = A.idFundoCadastro
		WHERE B.IdFundoCategoria =	@IdFundoCategoria
		AND B.IdFundoSubCategoria = @IdFundoSubCategoria
		UNION
		SELECT A.idFundoCadastro, A.nomeFundo, 0 As Pertence
		FROM tbFundoCadastro A
		EXCEPT(
			SELECT A.idFundoCadastro, A.nomeFundo, 0 As Pertence
			FROM tbFundoCadastro A
			JOIN TbFundoCategoriaSubCategoria B ON B.IdFundoCadastro = A.idFundoCadastro
			WHERE B.IdFundoCategoria =	@IdFundoCategoria
			AND B.IdFundoSubCategoria = @IdFundoSubCategoria
		)
	)Temp(IdFundoCadastro, NomeFundo, Pertence)
	order by idFundoCadastro
END
GO
