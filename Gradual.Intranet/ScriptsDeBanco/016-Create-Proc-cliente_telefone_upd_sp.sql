-- Descrição:       Realiza a atualização dos dados de telefone do cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_telefone_upd_sp
                 @id_telefone      bigint
               , @id_cliente       bigint
               , @id_tipo_telefone bigint
               , @st_principal     bit
               , @ds_ddd           numeric(7, 0)
               , @ds_ramal         numeric(5, 0)
               , @ds_numero        numeric(10, 0)
AS
     UPDATE    [dbo].[tb_cliente_telefone]
     SET       [id_cliente]       = @id_cliente
     ,         [id_tipo_telefone] = @id_tipo_telefone
     ,         [st_principal]     = @st_principal
     ,         [ds_ddd]           = @ds_ddd
     ,         [ds_ramal]         = @ds_ramal
     ,         [ds_numero]        = @ds_numero
     WHERE     [id_telefone]      = @id_telefone