-- Descri��o:       Realiza a exclus�o dos dados de situa��o financeira patrimonial de um cliente
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28
CREATE PROCEDURE cliente_situacaofinanceirapatrimonial_del_sp
                 @id_sfp bigint
AS
    DELETE
    FROM   [dbo].[tb_cliente_situacaofinanceirapatrimonial]
    WHERE  [dbo].[tb_cliente_situacaofinanceirapatrimonial].[id_sfp] = @id_sfp