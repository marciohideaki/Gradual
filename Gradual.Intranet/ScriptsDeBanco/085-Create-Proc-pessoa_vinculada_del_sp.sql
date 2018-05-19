--Descrição: Exclui registro da tabela tb_pessoa_vinculada.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010
CREATE PROCEDURE pessoa_vinculada_del_sp
	@id_pessoa_vinculada bigint
AS
DELETE FROM tb_pessoa_vinculada
WHERE
	[id_pessoa_vinculada] = @id_pessoa_vinculada