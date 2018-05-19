USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_ATUALIZA_FUNDO]    Script Date: 10/08/2012 20:23:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_ATUALIZA_FUNDO]
(
	@idDistribuidor			INTEGER = 1,
	@dsRazaoSocial			varchar(500) ='',
	@dsNomeFantasia			varchar(500) ='',
	@dsCnpj					varchar(20) ='',
	@stAtivo				char(1) ='',
	@dtAtualizacao			datetime = null,
	@PatrimonioLiquido		decimal(20,8) = 0,
	@NumeroCotas			decimal(20,8) = 0,
	@dsGestor				varchar(6) = '',
	@dsCodFundo				varchar(5) = '',
	@dsGestorCli			varchar(6) = '',
	@Agencia				varchar(4) = '',
	@Conta					varchar(9) = '',
	@DigitoConta			char(1) = '',
	@SubConta				varchar(3) = '',
	@CodFundoFC				varchar(50) = ''
)
AS

UPDATE [GRADUAL_CLUBES_FUNDOS].[dbo].[tbCadastroFundo]
SET
           [idDistribuidor] = @idDistribuidor,
           [stAtivo] = @stAtivo,
           [dtAtualizacao]= @dtAtualizacao,
           [PatrimonioLiquido] = @PatrimonioLiquido,
           [NumeroCotas] = @NumeroCotas,
           [CodFundoFC] = @CodFundoFC
           
WHERE [dsCnpj]=@dsCnpj

GO

