USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_INS_FUNDO]    Script Date: 10/08/2012 20:22:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_INS_FUNDO]
(
	@idDistribuidor			INTEGER = 1,
	@dsRazaoSocial			varchar(500) ='',
	@dsNomeFantasia			varchar(500) ='',
	@dsCnpj					varchar(20) ='',
	@stAtivo				char(1) ='',
	@dtAtualizacao			datetime = null,
	@PatrimonioLiquido		decimal(18,12) = 0,
	@NumeroCotas			decimal(18,12) = 0,
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

INSERT INTO [GRADUAL_CLUBES_FUNDOS].[dbo].[tbCadastroFundo]
           ([idDistribuidor]
           ,[dsRazaoSocial]
           ,[dsNomeFantasia]
           ,[dsCnpj]
           ,[stAtivo]
           ,[dtAtualizacao]
           ,[PatrimonioLiquido]
           ,[NumeroCotas]
           ,[dsGestor]
           ,[dsCodFundo]
           ,[dsGestorCli]
           ,[Agencia]
           ,[Conta]
           ,[DigitoConta]
           ,[SubConta]
           ,[CodFundoFC])
     VALUES
           ( @idDistribuidor,
           @dsRazaoSocial,
           @dsNomeFantasia,
           @dsCnpj,
           @stAtivo,
           @dtAtualizacao,
           @PatrimonioLiquido,
           @NumeroCotas,
           @dsGestor,
           @dsCodFundo,
           @dsGestorCli,
           @Agencia,
           @Conta, 
           @DigitoConta,
           @SubConta,
           @CodFundoFC)

GO

