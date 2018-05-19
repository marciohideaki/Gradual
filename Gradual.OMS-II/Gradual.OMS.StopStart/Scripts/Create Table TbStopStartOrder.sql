USE [Risco]
GO
/****** Object:  Table [dbo].[TbStopStartOrder]    Script Date: 09/20/2010 15:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TbStopStartOrder](
	[StopStartID] [int] IDENTITY(1,1) NOT NULL,
	[OrdTypeID] [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StopStartStatusID] [int] NOT NULL,
	[Symbol] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OrderQty] [int] NOT NULL,
	[Account] [int] NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
	[ExpireDate] [datetime] NOT NULL,
	[ExecutionTime] [datetime] NULL,
	[ReferencePrice] [decimal](12, 8) NULL,
	[StartPriceValue] [decimal](12, 8) NULL,
	[SendStartPrice] [decimal](12, 8) NULL,
	[StopGainValuePrice] [decimal](12, 8) NULL,
	[SendStopGainPrice] [decimal](12, 8) NULL,
	[StopLossValuePrice] [decimal](12, 8) NULL,
	[SendStopLossValuePrice] [decimal](12, 8) NULL,
	[InitialMovelPrice] [decimal](12, 8) NULL,
	[AdjustmentMovelPrice] [decimal](12, 8) NULL,
	[id_bolsa] [smallint] NULL,
	[StopStartTipoEnum] [int] NULL,
 CONSTRAINT [PK_TbStopStartOrder] PRIMARY KEY CLUSTERED 
(
	[StopStartID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificador da Ordem de stopstart' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TbStopStartOrder', @level2type=N'COLUMN',@level2name=N'StopStartID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo da ordem stopstart ( stop , start, stop movel etc )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TbStopStartOrder', @level2type=N'COLUMN',@level2name=N'OrdTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código do cliente na Bolsa' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TbStopStartOrder', @level2type=N'COLUMN',@level2name=N'Account'
GO
ALTER TABLE [dbo].[TbStopStartOrder]  WITH CHECK ADD  CONSTRAINT [FK_TbStopStartOrder_TbOrderType] FOREIGN KEY([OrdTypeID])
REFERENCES [dbo].[TbOrderType] ([OrdTypeID])
GO
ALTER TABLE [dbo].[TbStopStartOrder] CHECK CONSTRAINT [FK_TbStopStartOrder_TbOrderType]
GO
ALTER TABLE [dbo].[TbStopStartOrder]  WITH CHECK ADD  CONSTRAINT [FK_TbStopStartOrder_TbStopStartStatus] FOREIGN KEY([StopStartStatusID])
REFERENCES [dbo].[TbStopStartStatus] ([StopStartStatusID])
GO
ALTER TABLE [dbo].[TbStopStartOrder] CHECK CONSTRAINT [FK_TbStopStartOrder_TbStopStartStatus]