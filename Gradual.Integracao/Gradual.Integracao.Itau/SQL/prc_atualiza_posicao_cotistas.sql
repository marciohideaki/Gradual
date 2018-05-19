USE [GRADUAL_CLUBES_FUNDOS]
GO

/****** Object:  StoredProcedure [dbo].[PRC_ATUALIZA_POSICAO_COTISTA]    Script Date: 10/08/2012 20:22:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[PRC_ATUALIZA_POSICAO_COTISTA]
(
	@idMovimento			INTEGER,
	@idProcessamento		INTEGER,
	@dsCpfCnpj				VARCHAR(20) = '',
	@agencia				VARCHAR(4),
	@conta					VARCHAR(9),
	@idCotista				INTEGER,
	@idFundo				INTEGER,
	@quantidadeCotas		DECIMAL(20,8),
	@valorCota				DECIMAL(20,8),
	@valorBruto				DECIMAL(20,8),
	@valorIR				DECIMAL(20,8),
	@valorIOF				DECIMAL(20,8),	
	@valorLiquido			DECIMAL(20,8),
	@dtReferencia			DATETIME,
	@dtProcessamento		DATETIME,
	@DigitoConta			CHAR(1),
	@Subconta				VARCHAR(3) =' ',
	@Banco					VARCHAR(6)

)
AS

UPDATE [GRADUAL_CLUBES_FUNDOS].[dbo].[tbClientePosicao]
SET
		[idProcessamento] = @idProcessamento,
		[dsCpfCnpj] = @dsCpfCnpj,
		[agencia] = @agencia,
		[conta] = @conta,
		[idCotista] = @idCotista,
		[idFundo] = @idFundo,
		[quantidadeCotas] = @quantidadeCotas,
		[valorCota] = @valorCota,
		[valorBruto] = @valorBruto,
		[valorIR] = @valorIR,
		[valorIOF] = @valorIOF,
		[valorLiquido] = @valorLiquido,
		[dtReferencia] = @dtReferencia,
		[dtProcessamento] = @dtProcessamento,
		[DigitoConta] = @DigitoConta,
		[Subconta] = @Subconta,
		[Banco] = @Banco
WHERE [idMovimento] = @idMovimento

GO

