set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_usuario_ins]
( @id_usuario    INT OUTPUT
, @id_perfil     INT
, @id_localidade INT
, @ds_nome       VARCHAR(200)
, @ds_email      VARCHAR(200)
, @ds_senha      VARCHAR(50)
, @id_assessor   INT
, @st_usuario    CHAR(1))
AS
   INSERT INTO [dbo].[tb_usuario]
          (    [id_perfil]
          ,    [id_localidade]
          ,    [id_assessor]
          ,    [ds_nome]
          ,    [ds_email]
          ,    [ds_senha]
          ,    [st_usuario])
   VALUES (    @id_perfil
          ,    @id_localidade
          ,    @id_assessor
          ,    @ds_nome
          ,    @ds_email
          ,    @ds_senha
          ,    @st_usuario)

   SELECT @id_usuario = SCOPE_IDENTITY()

