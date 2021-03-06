set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Lista os dados da tabela tb_cliente_pendenciacadastral de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Consultando o id_login e o nome de quem resolveu.
--Autor: Gustavo Malta Guimaraes
--Data de criação: 20/10/2010

--Descrição: Arrumando a descrição da pendencia e descrição do tipo de pendencia.
--Autor: Gustavo Malta Guimaraes
--Data de criação: 03/11/2010

ALTER PROCEDURE [dbo].[cliente_pendenciacadastral_lst_sp] 
	@id_cliente int = null
AS
SELECT     [cpc].[id_pendencia_cadastral]
,          [cpc].[id_tipo_pendencia]
,          [cpc].[id_cliente]
,          [cpc].[dt_cadastropendencia]
,          [cpc].[dt_resolucao]
,          [cpc].[ds_resolucao]
,          [cpc].[ds_pendencia]
,          [tpc].[ds_pendencia] as ds_tipo_pendencia
,          [lg].id_login as id_login
,		   [lg].ds_nome	as ds_login	
FROM       [dbo].[tb_cliente_pendenciacadastral] AS [cpc]
left join  [dbo].[tb_login] AS [lg] ON [lg].[id_login] = [cpc].[id_login]  
INNER JOIN [dbo].[tb_tipo_pendenciacadastral]    AS [tpc] ON [cpc].[id_tipo_pendencia] = [tpc].[id_tipo_pendencia]
WHERE      [id_cliente] = ISNULL(@id_cliente,id_cliente)
ORDER BY   [id_pendencia_cadastral] ASC





