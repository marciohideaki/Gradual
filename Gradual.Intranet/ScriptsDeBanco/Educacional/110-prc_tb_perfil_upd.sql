CREATE PROCEDURE prc_tb_perfil_upd
( @id_perfil int
, @ds_perfil varchar(50))
AS
   UPDATE [dbo].[tb_perfil]
   SET    [tb_perfil].[ds_perfil] = @ds_perfil
   WHERE  [tb_perfil].[id_perfil] = @id_perfil