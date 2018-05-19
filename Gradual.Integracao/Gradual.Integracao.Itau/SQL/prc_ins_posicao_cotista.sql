USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_INS_POSICAO_COTISTA]    Script Date: 10/08/2012 20:21:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_INS_POSICAO_COTISTA]
(
	@idProcessamento		INTEGER,
	@dsCpfCnpj				VARCHAR(20) = '',
	@agencia				VARCHAR(4),
	@conta					VARCHAR(9),
	@idCotista				INTEGER = 0,
	@idFundo				INTEGER,
	@quantidadeCotas		DECIMAL(20,8),
	@valorCota				DECIMAL(20,8),
	@valorBruto				DECIMAL(20,8),
	@valorIR				DECIMAL(20,8),
	@valorIOF				DECIMAL(20,8),	
	@valorLiquido			DECIMAL(20,8),
	@dtReferencia			DATETIME,
	@dtProcessamento		DATETIME,
	@DigitoConta			CHAR(1) = ' ',
	@Subconta				VARCHAR(3) = ' ',
	@Banco					VARCHAR(6) = ' '
)
AS

INSERT INTO [GRADUAL_CLUBES_FUNDOS].[dbo].[tbClientePosicao]
	(
		[idProcessamento],
		[dsCpfCnpj],
		[agencia],
		[conta],
		[idCotista],
		[idFundo],
		[quantidadeCotas],
		[valorCota],
		[valorBruto],
		[valorIR],
		[valorIOF],
		[valorLiquido],
		[dtReferencia],
		[dtProcessamento],
		[DigitoConta],
		[Subconta],
		[Banco]
	)
VALUES
	(
		@idProcessamento,
		@dsCpfCnpj,
		@agencia,
		@conta,
		@idCotista,
		@idFundo,
		@quantidadeCotas,
		@valorCota,
		@valorBruto,
		@valorIR,
		@valorIOF,
		@valorLiquido,
		@dtReferencia,
		@dtProcessamento,
		@DigitoConta,
		@Subconta,
		@Banco
	)


GO

