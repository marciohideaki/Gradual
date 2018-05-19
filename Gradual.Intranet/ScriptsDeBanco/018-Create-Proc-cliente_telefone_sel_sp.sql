-- Descrição:       Seleciona dados de telefone de um cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_telefone_sel_sp
                 @id_telefone      bigint
AS
     SELECT [tb_cliente_telefone].[id_telefone]
     ,      [tb_cliente_telefone].[id_cliente]
     ,      [tb_cliente_telefone].[id_tipo_telefone]
     ,      [tb_cliente_telefone].[st_principal]
     ,      [tb_cliente_telefone].[ds_ddd]
     ,      [tb_cliente_telefone].[ds_ramal]
     ,      [tb_cliente_telefone].[ds_numero]
     FROM   [tb_cliente_telefone]
     WHERE  [tb_cliente_telefone].[id_telefone] = @id_telefone