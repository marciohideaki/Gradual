CREATE PROCEDURE prc_tb_tema_del (@id_tema int)
AS
   DELETE FROM [dbo].[tb_tema]
   WHERE  [tb_tema].[id_tema] = @id_tema