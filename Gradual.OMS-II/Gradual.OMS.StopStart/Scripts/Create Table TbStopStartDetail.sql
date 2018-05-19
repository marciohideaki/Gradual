USE [Risco]
GO
/****** Object:  Table [dbo].[TbStopStartDetail]    Script Date: 09/20/2010 15:26:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TbStopStartDetail](
	[StopStartDetailID] [int] IDENTITY(1,1) NOT NULL,
	[StopStartID] [int] NOT NULL,
	[OrderStatusID] [int] NOT NULL,
	[RegisterTime] [datetime] NOT NULL,
 CONSTRAINT [PK_TbStopStartExec] PRIMARY KEY CLUSTERED 
(
	[StopStartDetailID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TbStopStartDetail]  WITH CHECK ADD  CONSTRAINT [FK_TbStopStartExec_TbStopStartOrder] FOREIGN KEY([StopStartID])
REFERENCES [dbo].[TbStopStartOrder] ([StopStartID])
GO
ALTER TABLE [dbo].[TbStopStartDetail] CHECK CONSTRAINT [FK_TbStopStartExec_TbStopStartOrder]