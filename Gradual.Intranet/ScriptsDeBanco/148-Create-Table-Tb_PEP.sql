
/****** Object:  Table [dbo].[tb_pep]    Script Date: 06/07/2010 15:24:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tb_pep](
	[id_pep] [int] IDENTITY(1,1) NOT NULL,
		
	[ds_documento] [varchar](20) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[ds_identificacao] [varchar](20) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[ds_nome] [varchar](200) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	[dt_importacao] [datetime] NOT NULL,
	[ds_linha] [varchar](2000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL
	
CONSTRAINT [PK_tb_pep] PRIMARY KEY CLUSTERED 
(
	[id_pep] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
