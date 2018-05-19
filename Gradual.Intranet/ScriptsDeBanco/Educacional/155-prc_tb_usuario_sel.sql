CREATE PROCEDURE prc_tb_usuario_sel
( @ds_email VARCHAR(200)
, @ds_senha VARCHAR(50))
AS
   SELECT     [usr].[id_usuario]
   ,          [per].[id_perfil]
   ,          [per].[ds_perfil]
   ,          [usr].[ds_nome]
   ,          [usr].[ds_email]
   ,          [usr].[ds_senha]
   ,          [usr].[st_usuario]
   ,          [loc].[id_localidade]
   ,          [loc].[ds_localidade]
   ,          [usr].[id_assessor]
   FROM       [dbo].[tb_usuario]    AS [usr]
   INNER JOIN [dbo].[tb_perfil]     AS [per] ON [usr].[id_perfil]     = [per].[id_perfil]
   INNER JOIN [dbo].[tb_localidade] AS [loc] ON [usr].[id_localidade] = [loc].[id_localidade]
   WHERE      [usr].[ds_email] = @ds_email
   AND        [usr].[ds_senha] = @ds_senha