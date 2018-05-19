-- Descri��o:       Exclui fisicamente os dados de um cliente na base
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28
CREATE PROCEDURE cliente_del_sp
                 @id_cliente  bigint
AS
    DELETE
    FROM   tb_cliente
    WHERE  tb_cliente.id_cliente = @id_cliente