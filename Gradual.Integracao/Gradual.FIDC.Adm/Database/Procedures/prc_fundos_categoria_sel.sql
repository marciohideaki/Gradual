USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundos_categoria_sel') 
	DROP PROCEDURE prc_fundos_categoria_sel
GO

CREATE PROCEDURE prc_fundos_categoria_sel
(
	@IdFundoCategoria int = null,
	@DsFundoCategoria varchar(50) = null
)
AS
BEGIN
	SELECT IdFundoCategoria, DsFundoCategoria
	FROM TbFundoCategoria
	WHERE @IdFundoCategoria is null or IdFundoCategoria = @IdFundoCategoria
	AND @DsFundoCategoria is null or DsFundoCategoria = @DsFundoCategoria
	ORDER BY IdFundoCategoria
END
GO
