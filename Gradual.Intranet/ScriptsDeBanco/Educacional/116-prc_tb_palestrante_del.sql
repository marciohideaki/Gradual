CREATE PROCEDURE prc_tb_palestrante_del (@id_palestrante int)
AS
   DELETE FROM [dbo].[tb_palestrante] WHERE [tb_palestrante].[id_palestrante] = @id_palestrante