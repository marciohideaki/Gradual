USE [Risco]
GO
/****** Object:  Table [dbo].[TbStopStartStatus]    Script Date: 09/20/2010 15:31:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TbStopStartStatus](
	[StopStartStatusID] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatusDescription] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_TbStopStartStatus] PRIMARY KEY CLUSTERED 
(
	[StopStartStatusID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF