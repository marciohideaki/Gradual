USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_sub_categoria_sel') 
	DROP PROCEDURE prc_fundos_sub_categoria_sel
GO

CREATE PROCEDURE prc_fundos_sub_categoria_sel
(
	@IdFundoSubCategoria int = null,
	@DsFundoSubCategoria varchar(50) = null
)
AS
BEGIN
	SELECT IdFundoSubCategoria, DsFundoSubCategoria
	FROM TbFundoSubCategoria
	WHERE @IdFundoSubCategoria is null or IdFundoSubCategoria = @IdFundoSubCategoria
	AND @DsFundoSubCategoria is null or DsFundoSubCategoria = @DsFundoSubCategoria
	ORDER BY IdFundoSubCategoria
END
GO
