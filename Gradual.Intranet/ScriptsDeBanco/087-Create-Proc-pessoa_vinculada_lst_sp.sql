--Descri��o: Lista os dados da tabela tb_pessoa_vinculada de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de cria��o: 28/04/2010

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Altera��o nome do parametro de @ds_cpf para @ds_cpfcnpj
CREATE PROCEDURE pessoa_vinculada_lst_sp
	@id_cliente bigint
AS
SELECT
	[id_pessoa_vinculada],
	[ds_nome],
	[st_ativo],
	[ds_cpfcnpj],
	[id_pessoavinculadaresponsavel],
	[id_cliente]
FROM [tb_pessoa_vinculada]
WHERE [id_cliente] =  @id_cliente
ORDER BY 
	[id_pessoa_vinculada] ASC
	
--Poss�vel listagem a ser implementada futuramente
/*
SELECT     b.id_pessoa_vinculada, b.ds_nome, b.st_ativo, b.ds_cpf, b.id_cliente, CASE WHEN b.id_pessoavinculadaresponsavel IS NOT NULL 
                      THEN 'vinculada' ELSE NULL END AS status
FROM         tb_pessoa_vinculada AS a LEFT OUTER JOIN
                      tb_pessoa_vinculada AS b ON a.id_pessoa_vinculada = b.id_pessoavinculadaresponsavel OR a.id_pessoavinculadaresponsavel IS NULL
WHERE     (a.id_cliente = 6) AND (a.id_cliente = b.id_cliente)
*/