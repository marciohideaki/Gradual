set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Lista os dados da tabela tb_pessoa_vinculada de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 09/06/2010

CREATE PROCEDURE [dbo].[pessoa_vinculada_resp_sel_sp]
	@id_pessoa_vinculada int
AS
SELECT
	[id_pessoa_vinculada],
	[ds_nome],
	[st_ativo],
	[ds_cpfcnpj],
	[id_pessoavinculadaresponsavel],
	[id_cliente]
FROM [tb_pessoa_vinculada]
WHERE 
	[id_pessoavinculadaresponsavel] = @id_pessoa_vinculada
ORDER BY 
	[id_pessoa_vinculada] 

