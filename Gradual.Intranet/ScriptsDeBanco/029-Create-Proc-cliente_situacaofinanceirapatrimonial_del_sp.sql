-- Descrição:       Realiza a exclusão dos dados de situação financeira patrimonial de um cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_situacaofinanceirapatrimonial_del_sp
                 @id_sfp bigint
AS
    DELETE
    FROM   [dbo].[tb_cliente_situacaofinanceirapatrimonial]
    WHERE  [dbo].[tb_cliente_situacaofinanceirapatrimonial].[id_sfp] = @id_sfp