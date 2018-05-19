CREATE PROCEDURE prc_tb_usuario_upd
( @id_usuario    INT
, @id_perfil     INT
, @id_assessor   INT
, @id_localidade INT
, @ds_nome       VARCHAR(200)
, @ds_email      VARCHAR(200)
, @ds_senha      VARCHAR(50)
, @st_usuario    CHAR(1))
AS
  UPDATE [dbo].[tb_usuario]
  SET    [tb_usuario].[id_perfil]     = @id_perfil
  ,      [tb_usuario].[ds_nome]       = @ds_nome
  ,      [tb_usuario].[ds_email]      = @ds_email
  ,      [tb_usuario].[ds_senha]      = @ds_senha
  ,      [tb_usuario].[st_usuario]    = @st_usuario
  ,      [tb_usuario].[id_localidade] = @id_localidade
  ,      [tb_usuario].[idassessor]    = @id_assessor
  WHERE  [tb_usuario].[id_usuario]    = @id_usuario