CREATE PROCEDURE prc_tb_usuario_sel
( @ds_email varchar(200)
, @ds_senha varchar(50))
AS
   SELECT     [usr].[id_usuario]
   ,          [usr].[id_perfil]
   ,          [per].[ds_perfil]
   ,          [usr].[ds_nome]
   ,          [usr].[ds_email]
   ,          [usr].[st_usuario]
   ,          [usr].[id_localidade]
   ,          [loc].[ds_localidade]
   ,          [usr].[id_assessor]
   FROM       [dbo].[tb_usuario]    AS [usr]
   INNER JOIN [dbo].[tb_perfil]     AS [per] ON [usr].[id_perfil] = [per].[id_perfil]
   INNER JOIN [dbo].[tb_localidade] AS [loc] ON [usr].[id_localidade] = [log].[id_localidade]
   WHERE      [usr].[ds_email] = @ds_email
   AND        [usr].[ds_senha] = @ds_senha