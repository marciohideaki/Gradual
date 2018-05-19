-- Descrição:       Exclui os dados de telefone de um cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_telefone_del_sp
                 @id_telefone      bigint
AS
     DELETE
     FROM   [tb_cliente_telefone]
     WHERE  [tb_cliente_telefone].[id_telefone] = @id_telefone