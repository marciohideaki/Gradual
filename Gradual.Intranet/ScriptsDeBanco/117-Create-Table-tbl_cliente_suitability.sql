
/****** Object:  Table [dbo].[tb_cliente_suitability]    Script Date: 06/07/2010 15:24:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_cliente_suitability](
	[id_cliente_suitability] [int] IDENTITY(1,1) NOT NULL,
	[id_cliente] [int] NOT NULL,
	
	[ds_perfil] [varchar](50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[ds_status] [varchar](50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[dt_realizacao] [datetime] NOT NULL,
	[st_preenchidopelocliente] [bit] NOT NULL,
	[ds_loginrealizado] [varchar](200) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[ds_fonte] [varchar](200) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[ds_respostas] [varchar](500) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL
	
 CONSTRAINT [PK_tb_cliente_suitability] PRIMARY KEY CLUSTERED 
(
	[id_cliente_suitability] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
