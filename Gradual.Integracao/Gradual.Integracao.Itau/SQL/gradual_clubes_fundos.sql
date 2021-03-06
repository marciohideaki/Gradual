USE [GRADUAL_CLUBES_FUNDOS]
GO
/****** Object:  StoredProcedure [dbo].[prc_sel_fundos]    Script Date: 10/23/2012 17:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_sel_fundos] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM tbCadastroFundo
END
GO
/****** Object:  Table [dbo].[tbDistribuidor]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbDistribuidor](
	[idDistribuidor] [int] IDENTITY(1,1) NOT NULL,
	[dsDistribuidor] [varchar](500) NOT NULL,
 CONSTRAINT [PK_tbDistribuidor] PRIMARY KEY CLUSTERED 
(
	[idDistribuidor] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCadastroCotistas]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCadastroCotistas](
	[idCotista] [int] IDENTITY(1,1) NOT NULL,
	[idCotistaItau] [varchar](14) NOT NULL,
	[dsNome] [varchar](500) NOT NULL,
	[stAtivo] [char](1) NOT NULL,
	[dtImportacao] [datetime] NOT NULL,
	[Banco] [varchar](6) NOT NULL,
	[Agencia] [varchar](4) NOT NULL,
	[Conta] [varchar](9) NOT NULL,
	[DigitoConta] [char](1) NOT NULL,
	[SubConta] [varchar](3) NOT NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[tbCadastroCotistas] ADD [dsCpfCnpj] [varchar](20) NOT NULL
ALTER TABLE [dbo].[tbCadastroCotistas] ADD  CONSTRAINT [PK_tbCadastroCotistas] PRIMARY KEY CLUSTERED 
(
	[idCotista] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbStatusArquivoFundo]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbStatusArquivoFundo](
	[idStatusProcessamento] [int] NOT NULL,
	[dsStatus] [varchar](200) NOT NULL,
 CONSTRAINT [PK_tbStatusArquivoFundo] PRIMARY KEY CLUSTERED 
(
	[idStatusProcessamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbCadastroFundo]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbCadastroFundo](
	[idFundo] [int] IDENTITY(1,1) NOT NULL,
	[idDistribuidor] [int] NOT NULL,
	[dsRazaoSocial] [varchar](500) NOT NULL,
	[dsNomeFantasia] [varchar](500) NOT NULL,
	[dsCnpj] [varchar](20) NOT NULL,
	[stAtivo] [char](1) NOT NULL,
	[dtAtualizacao] [datetime] NOT NULL,
	[PatrimonioLiquido] [decimal](20, 8) NULL,
	[NumeroCotas] [decimal](20, 8) NULL,
	[dsGestor] [varchar](6) NULL,
	[dsCodFundo] [varchar](5) NULL,
	[dsGestorCli] [varchar](6) NULL,
	[Agencia] [varchar](4) NULL,
	[Conta] [varchar](9) NULL,
	[DigitoConta] [char](1) NULL,
	[SubConta] [varchar](3) NULL,
	[CodFundoFC] [varchar](50) NULL,
 CONSTRAINT [PK_tbCadastroFundo] PRIMARY KEY CLUSTERED 
(
	[idFundo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbClienteResumo]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbClienteResumo](
	[dsCpfCnpj] [varchar](200) NULL,
	[agencia] [varchar](50) NOT NULL,
	[conta] [varchar](50) NOT NULL,
	[idFundo] [int] NOT NULL,
	[idCotista] [int] NOT NULL,
	[idMovimento] [int] NOT NULL,
	[dtUltimoProcessamento] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbClientePosicaoHistorico]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbClientePosicaoHistorico](
	[idMovimentoHistorico] [int] NOT NULL,
	[idProcessamento] [int] NOT NULL,
	[idMovimento] [int] NOT NULL,
	[dsCpfCnpj] [varchar](20) NOT NULL,
	[idCotista] [varchar](200) NOT NULL,
	[idFundo] [int] NOT NULL,
	[quantidadeCotas] [int] NOT NULL,
	[valorCota] [decimal](18, 12) NOT NULL,
	[valorBruto] [decimal](18, 12) NOT NULL,
	[valorIR] [decimal](18, 12) NOT NULL,
	[valorIOF] [decimal](18, 12) NOT NULL,
	[valorLiquido] [decimal](18, 12) NOT NULL,
	[dtReferencia] [datetime] NOT NULL,
	[dtProcessamento] [datetime] NOT NULL,
 CONSTRAINT [PK_tbClientePosicaoHistorico] PRIMARY KEY CLUSTERED 
(
	[idMovimentoHistorico] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbClientePosicao]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbClientePosicao](
	[idMovimento] [int] IDENTITY(1,1) NOT NULL,
	[idProcessamento] [int] NOT NULL,
	[dsCpfCnpj] [varchar](20) NULL,
	[banco] [varchar](50) NULL,
	[agencia] [varchar](50) NOT NULL,
	[conta] [varchar](50) NOT NULL,
	[digitoConta] [varchar](50) NULL,
	[subconta] [varchar](50) NULL,
	[idCotista] [varchar](200) NOT NULL,
	[idFundo] [int] NOT NULL,
	[quantidadeCotas] [decimal](20, 8) NULL,
	[valorCota] [decimal](18, 4) NULL,
	[valorBruto] [decimal](18, 4) NULL,
	[valorIR] [decimal](18, 4) NULL,
	[valorIOF] [decimal](18, 4) NULL,
	[valorLiquido] [decimal](18, 4) NULL,
	[dtReferencia] [datetime] NOT NULL,
	[dtProcessamento] [datetime] NULL,
 CONSTRAINT [PK_tbClientePosicao] PRIMARY KEY CLUSTERED 
(
	[idMovimento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tbProcessamentoPosicao]    Script Date: 10/23/2012 17:44:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tbProcessamentoPosicao](
	[idProcessamento] [int] NOT NULL,
	[idStatusProcessamento] [int] NOT NULL,
	[dsArquivo] [varchar](200) NOT NULL,
	[dtInicioProcessamento] [datetime] NOT NULL,
	[dtFimProcessamento] [datetime] NOT NULL,
	[dsObservacao] [varchar](2000) NOT NULL,
 CONSTRAINT [PK_tbArquivoPosicao] PRIMARY KEY CLUSTERED 
(
	[idProcessamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[prc_sel_posicao_cotista]    Script Date: 10/23/2012 17:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_sel_posicao_cotista] 
(
	@dsCpfCnpj VARCHAR(20)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		P.idMovimento,
		P.idProcessamento,
		P.dsCpfCnpj,
		P.banco,
		P.agencia,
		P.conta,
		P.digitoConta,
		P.subconta,
		P.idCotista,
		P.idFundo,
		F.dsRazaoSocial,
		P.quantidadeCotas,
		P.valorCota,
		P.valorBruto,
		P.valorIR,
		P.valorIOF,
		P.valorLiquido,
		P.dtReferencia,
		P.dtProcessamento
    FROM tbClientePosicao P, tbCadastroFundo F
    WHERE [dsCpfCnpj] = @dsCpfCnpj
    AND F.idFundo = P.idFundo
END
GO
/****** Object:  StoredProcedure [dbo].[PRC_INS_FUNDO]    Script Date: 10/23/2012 17:44:23 ******/
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
/****** Object:  StoredProcedure [dbo].[PRC_ATUALIZA_FUNDO]    Script Date: 10/23/2012 17:44:23 ******/
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
/****** Object:  StoredProcedure [dbo].[prc_sel_posicao_cotistas]    Script Date: 10/23/2012 17:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_sel_posicao_cotistas] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM tbClientePosicao
END
GO
/****** Object:  StoredProcedure [dbo].[PRC_ATUALIZA_POSICAO_COTISTA]    Script Date: 10/23/2012 17:44:23 ******/
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
/****** Object:  StoredProcedure [dbo].[PRC_INS_POSICAO_COTISTA]    Script Date: 10/23/2012 17:44:23 ******/
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
/****** Object:  StoredProcedure [dbo].[PRC_INS_COTISTA]    Script Date: 10/23/2012 17:44:23 ******/
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
/****** Object:  StoredProcedure [dbo].[prc_sel_cotistas]    Script Date: 10/23/2012 17:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_sel_cotistas] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM tbCadastroCotistas
END
GO
/****** Object:  ForeignKey [FK_tbCadastroFundo_tbDistribuidor]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbCadastroFundo]  WITH CHECK ADD  CONSTRAINT [FK_tbCadastroFundo_tbDistribuidor] FOREIGN KEY([idDistribuidor])
REFERENCES [dbo].[tbDistribuidor] ([idDistribuidor])
GO
ALTER TABLE [dbo].[tbCadastroFundo] CHECK CONSTRAINT [FK_tbCadastroFundo_tbDistribuidor]
GO
/****** Object:  ForeignKey [FK_tbClientePosicao_tbProcessamentoPosicao]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbClientePosicao]  WITH CHECK ADD  CONSTRAINT [FK_tbClientePosicao_tbProcessamentoPosicao] FOREIGN KEY([idProcessamento])
REFERENCES [dbo].[tbProcessamentoPosicao] ([idProcessamento])
GO
ALTER TABLE [dbo].[tbClientePosicao] CHECK CONSTRAINT [FK_tbClientePosicao_tbProcessamentoPosicao]
GO
/****** Object:  ForeignKey [FK_tbClientePosicaoHistorico_tbClientePosicao]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbClientePosicaoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_tbClientePosicaoHistorico_tbClientePosicao] FOREIGN KEY([idMovimento])
REFERENCES [dbo].[tbClientePosicao] ([idMovimento])
GO
ALTER TABLE [dbo].[tbClientePosicaoHistorico] CHECK CONSTRAINT [FK_tbClientePosicaoHistorico_tbClientePosicao]
GO
/****** Object:  ForeignKey [FK_tbClientePosicaoHistorico_tbProcessamentoPosicao]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbClientePosicaoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_tbClientePosicaoHistorico_tbProcessamentoPosicao] FOREIGN KEY([idProcessamento])
REFERENCES [dbo].[tbProcessamentoPosicao] ([idProcessamento])
GO
ALTER TABLE [dbo].[tbClientePosicaoHistorico] CHECK CONSTRAINT [FK_tbClientePosicaoHistorico_tbProcessamentoPosicao]
GO
/****** Object:  ForeignKey [FK_tbClienteResumo_tbCadastroFundo]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbClienteResumo]  WITH CHECK ADD  CONSTRAINT [FK_tbClienteResumo_tbCadastroFundo] FOREIGN KEY([idFundo])
REFERENCES [dbo].[tbCadastroFundo] ([idFundo])
GO
ALTER TABLE [dbo].[tbClienteResumo] CHECK CONSTRAINT [FK_tbClienteResumo_tbCadastroFundo]
GO
/****** Object:  ForeignKey [FK_tbClienteResumo_tbClientePosicao]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbClienteResumo]  WITH CHECK ADD  CONSTRAINT [FK_tbClienteResumo_tbClientePosicao] FOREIGN KEY([idMovimento])
REFERENCES [dbo].[tbClientePosicao] ([idMovimento])
GO
ALTER TABLE [dbo].[tbClienteResumo] CHECK CONSTRAINT [FK_tbClienteResumo_tbClientePosicao]
GO
/****** Object:  ForeignKey [FK_tbArquivoPosicao_tbStatusArquivoFundo]    Script Date: 10/23/2012 17:44:24 ******/
ALTER TABLE [dbo].[tbProcessamentoPosicao]  WITH CHECK ADD  CONSTRAINT [FK_tbArquivoPosicao_tbStatusArquivoFundo] FOREIGN KEY([idStatusProcessamento])
REFERENCES [dbo].[tbStatusArquivoFundo] ([idStatusProcessamento])
GO
ALTER TABLE [dbo].[tbProcessamentoPosicao] CHECK CONSTRAINT [FK_tbArquivoPosicao_tbStatusArquivoFundo]
GO
