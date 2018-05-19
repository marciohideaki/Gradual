-- Descrição:       Realiza a atualização dos dados de tipo de telefone
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE tipo_telefone_upd_sp
                 @id_tipo_telefone bigint
               , @ds_telefone      varchar(20)
AS
     UPDATE    [dbo].[tb_tipo_telefone]
     SET       [ds_telefone]      = @ds_telefone
     WHERE     [id_tipo_telefone] = @id_tipo_telefone