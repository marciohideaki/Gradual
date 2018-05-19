CREATE PROCEDURE prc_tb_palestrante_sel (@id_palestrante INT)
AS
   SELECT [tb_palestrante].[id_palestrante]
   ,      [tb_palestrante].[nm_palestrante]
   ,      [tb_palestrante].[ds_curriculo]
   FROM   [dbo].[tb_palestrante]
   WHERE  [tb_palestrante].[id_palestrante] = @id_palestrante