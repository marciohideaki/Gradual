CREATE PROC prc_tb_usuario_del (@id_usuario int)
AS
   DELETE FROM [dbo].[tb_usuario] WHERE [tb_usuario].[id_usuario] = @id_usuario