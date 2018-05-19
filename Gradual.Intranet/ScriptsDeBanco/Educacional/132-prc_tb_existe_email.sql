CREATE PROCEDURE prc_tb_existe_email (@ds_email VARCHAR(200))
AS
   SELECT [tb_usuario].[ds_email]
   FROM   [dbo].[tb_usuario]
   WHERE  [tb_usuario].[ds_email] = @ds_email