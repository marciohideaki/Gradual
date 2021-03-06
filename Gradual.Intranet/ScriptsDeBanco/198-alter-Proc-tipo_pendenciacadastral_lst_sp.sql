set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--Descrição: Lista os dados da tabela tb_tipo_pendenciacadastral de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Adicionando informação se é automatica.
--Autor: Gustavo Malta Guimarães
--Data de criação: 14/10/2010

ALTER PROCEDURE [dbo].[tipo_pendenciacadastral_lst_sp]
AS
SELECT
	[id_tipo_pendencia],
	[ds_pendencia],
	[st_automatica]
FROM [dbo].[tb_tipo_pendenciacadastral]
ORDER BY 
	[id_tipo_pendencia] ASC
