CREATE PROCEDURE prc_tb_nivel_del (@id_nivel INT)
AS
   DELETE FROM [dbo].[tb_nivel] WHERE [tb_nivel].[id_nivel] = @id_nivel