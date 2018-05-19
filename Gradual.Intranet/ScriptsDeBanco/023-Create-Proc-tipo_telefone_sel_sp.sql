-- Descri��o:       Realiza a sele��o dos dados de tipo de telefone
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28
CREATE PROCEDURE tipo_telefone_sel_sp
                 @id_tipo_telefone bigint
AS
     SELECT [tb_tipo_telefone].[id_tipo_telefone]
     ,      [tb_tipo_telefone].[ds_telefone]
     FROM   [dbo].[tb_tipo_telefone]
     WHERE  [tb_tipo_telefone].[id_tipo_telefone] = @id_tipo_telefone