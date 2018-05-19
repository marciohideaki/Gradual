--Descrição: Exclui registro da tabela tb_cliente_controladora.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_controladora_del_sp
	@id_cliente_controlada bigint
AS
DELETE FROM tb_cliente_controladora
WHERE
	[id_cliente_controlada] = @id_cliente_controlada