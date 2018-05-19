--Descrição: Exclui registro da tabela tb_cliente_diretor.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_diretor_del_sp
	@id_cliente_diretor bigint
AS
DELETE FROM tb_cliente_diretor
WHERE
	[id_cliente_diretor] = @id_cliente_diretor