set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Seleciona os dados da tabela tb_cliente_pendenciacadastral de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Consultando o id_login e o nome de quem resolveu.
--Autor: Gustavo Malta Guimaraes
--Data de criação: 20/10/2010

ALTER PROCEDURE [dbo].[cliente_pendenciacadastral_sel_sp]
	@id_pendencia_cadastral int
AS
SELECT
			[cpc].[id_pendencia_cadastral]
,          [cpc].[id_tipo_pendencia]
,          [cpc].[id_cliente]
,          [cpc].[dt_cadastropendencia]
,          [cpc].[dt_resolucao]
,          [cpc].[ds_resolucao]
,          [tpc].[ds_pendencia]
,          [lg].id_login as id_login
,		   [lg].ds_nome	as ds_login	
FROM tb_cliente_pendenciacadastral as [cpc]
left join  [dbo].[tb_login] AS [lg] ON [lg].[id_login] = [cpc].[id_login]
INNER JOIN [dbo].[tb_tipo_pendenciacadastral]    AS [tpc] ON [cpc].[id_tipo_pendencia] = [tpc].[id_tipo_pendencia]
WHERE id_pendencia_cadastral = @id_pendencia_cadastral
ORDER BY 
	[id_pendencia_cadastral] ASC




