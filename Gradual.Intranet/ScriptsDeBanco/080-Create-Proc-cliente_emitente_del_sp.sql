--Descrição: Exclui registro da tabela tb_cliente_emitente.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010
CREATE PROCEDURE cliente_emitente_del_sp
	@id_pessoaautorizada bigint
AS
DELETE FROM tb_cliente_emitente WHERE [id_pessoaautorizada] = @id_pessoaautorizada