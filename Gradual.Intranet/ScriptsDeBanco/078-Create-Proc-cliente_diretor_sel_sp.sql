--Descrição: Seleciona os dados da tabela tb_cliente_diretor de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_diretor_sel_sp
	@id_cliente_diretor bigint
AS
SELECT
	[id_cliente_diretor],
	[id_cliente],
	[ds_identidade],
	[ds_cpfcnpj],
	[ds_nome]
FROM tb_cliente_diretor
WHERE
	[id_cliente_diretor] = @id_cliente_diretor
ORDER BY 
	[id_cliente_diretor] ASC