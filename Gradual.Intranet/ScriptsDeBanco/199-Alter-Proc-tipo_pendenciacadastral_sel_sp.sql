set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Seleciona os dados da tabela tb_tipo_pendenciacadastral de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Adicionando informação se é automatica.
--Autor: Gustavo Malta Guimarães
--Data de criação: 14/10/2010

ALTER PROCEDURE [dbo].[tipo_pendenciacadastral_sel_sp]
	@id_tipo_pendencia int
AS
SELECT
	[id_tipo_pendencia],
	[ds_pendencia],
	[st_automatica]
FROM tb_tipo_pendenciacadastral
WHERE 
[id_tipo_pendencia] = @id_tipo_pendencia
ORDER BY 
	[id_tipo_pendencia] ASC

