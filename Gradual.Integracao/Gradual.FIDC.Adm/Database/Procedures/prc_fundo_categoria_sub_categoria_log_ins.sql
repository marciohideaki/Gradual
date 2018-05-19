USE GradualFundosADM
GO

IF EXISTS (SELECT * FROM SYS.procedures WHERE name = 'prc_fundo_categoria_sub_categoria_log_ins') 
	DROP PROCEDURE prc_fundo_categoria_sub_categoria_log_ins
GO

CREATE PROCEDURE prc_fundo_categoria_sub_categoria_log_ins
	@IdFundoCadastro INT,
	@IdFundoCategoria INT,
	@IdFundoSubCategoria INT,
	@UsuarioLogado varchar(100),
	@DtAlteracao datetime,
	@TipoTransacao varchar(10)
AS
BEGIN

	DECLARE @DsFundoCategoria AS VARCHAR(50)
	DECLARE @DsFundoSubCategoria AS VARCHAR(50)
	DECLARE @NomeFundo AS VARCHAR(100)

	SELECT @NomeFundo = nomeFundo FROM tbFundoCadastro WHERE IdFundoCadastro = @IdFundoCadastro
	SELECT @DsFundoCategoria = DsFundoCategoria FROM TbFundoCategoria WHERE IdFundoCategoria = @IdFundoCategoria
	SELECT @DsFundoSubCategoria = DsFundoSubCategoria FROM TbFundoSubCategoria WHERE IdFundoSubCategoria = @IdFundoSubCategoria
	
	INSERT INTO TbFundoCategoriaSubCategoriaManutencaoLog
	VALUES (@NomeFundo, @DsFundoCategoria, @DsFundoSubCategoria, @UsuarioLogado, @DtAlteracao, @TipoTransacao)

	SELECT NomeFundo, DsFundoCategoria, DsFundoSubCategoria
	FROM TbFundoCategoriaSubCategoriaManutencaoLog
	WHERE IdFundoCategoriaSubCategoriaManutencaoLog = SCOPE_IDENTITY()
END
GO
