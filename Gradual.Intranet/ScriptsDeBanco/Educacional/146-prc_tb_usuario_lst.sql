CREATE PROCEDURE prc_tb_usuario_lst
( @ds_nome       VARCHAR(200)
, @id_assessor   INT
, @id_localidade INT)
AS
   SELECT     [usu].[id_usuario]
   ,          [usu].[id_perfil]
   ,          [per].[ds_perfil]
   ,          [usu].[ds_nome]
   ,          [usu].[ds_email]
   ,          [usu].[ds_senha]
   ,          [usu].[st_usuario]
   ,          [usu].[id_assessor]
   ,          [loc].[ds_localidade] + ' - ' + [usu].[ds_nome] AS [ds_filialassessor]
   ,          [loc].[id_localidade]
   ,          [loc].[ds_localidade]
   FROM       [educacional].[dbo].[tb_usuario]    AS [usu]
   INNER JOIN [educacional].[dbo].[tb_perfil]     AS [per] ON [usu].[id_perfil]     = [per].[id_perfil]
   INNER JOIN [educacional].[dbo].[tb_localidade] AS [loc] ON [usu].[id_localidade] = [loc].[id_localidade]
   LEFT  JOIN [cadastro].[dbo].[tb_cliente]       AS [cli] ON [usu].[id_assessor]   = [cli].[id_assessorinicial]
   WHERE      @ds_nome       IS NULL OR UPPER([cli].[ds_nome]) LIKE '%' + UPPER(@ds_nome) + '%'
   AND        @id_assessor   IS NULL OR [usu].[id_assessor]   = @id_assessor
   AND        @id_localidade IS NULL OR [usu].[id_localidade] = @id_localidade
   ORDER BY   [usu].[ds_nome]