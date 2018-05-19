-- Descri��o:       Realiza a atualiza��o dos dados de tipo de telefone
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28
CREATE PROCEDURE tipo_telefone_ins_sp
                 @ds_telefone varchar(20)
AS
     INSERT INTO [dbo].[tb_tipo_telefone]
            (    [ds_telefone])
     VALUES (    @ds_telefone)

     SELECT SCOPE_IDENTITY()