--Descrição: Lista os dados da tabela tb_pessoa_vinculada de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Alteração nome do campo de @ds_cpf para @ds_cpfcnpj

CREATE PROCEDURE pessoa_vinculada_sel_sp
	@id_pessoa_vinculada bigint
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
	[id_pessoa_vinculada] = @id_pessoa_vinculada
ORDER BY 
	[id_pessoa_vinculada] ASC