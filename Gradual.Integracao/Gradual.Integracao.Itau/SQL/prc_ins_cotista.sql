USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_INS_COTISTA]    Script Date: 10/08/2012 20:22:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_INS_COTISTA]
(
	@idCotistaItau			varchar(14),
	@dsNome					varchar(500) ='',
	@dsCPfCnpj				varchar(20) ='',
	@stAtivo				char(1) ='',
	@dtImportacao			datetime = null,
	@Banco					varchar(6) = '',
	@Agencia				varchar(4) = '',
	@Conta					varchar(9) = '',
	@DigitoConta			char(1) = '',
	@SubConta				varchar(3) = ''
)
AS

INSERT INTO [GRADUAL_CLUBES_FUNDOS].[dbo].[tbCadastroCotistas]
           (
			[idCotistaItau],
			[dsNome],
			[dsCPfCnpj],
			[stAtivo],
			[dtImportacao],
			[Banco],
			[Agencia],
			[Conta],
			[DigitoConta],
			[SubConta]			
		)
     VALUES
           (
			@idCotistaItau,
			@dsNome,
			@dsCPfCnpj,
			@stAtivo,
			@dtImportacao,
			@Banco,
			@Agencia,
			@Conta,
			@DigitoConta,
			@SubConta
		)

GO

