--Descrição: Exclui registro da tabela tb_cliente_endereco.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 06/05/2010
CREATE PROCEDURE cliente_endereco_del_sp 
@id_endereco bigint 
AS
DELETE FROM 
	tb_cliente_endereco 
WHERE 
	[id_endereco] = @id_endereco