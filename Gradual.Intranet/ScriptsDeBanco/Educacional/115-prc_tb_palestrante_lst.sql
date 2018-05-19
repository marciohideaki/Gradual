CREATE PROCEDURE prc_tb_palestrante_lst
AS
   SELECT   [tb_palestrante].[id_palestrante]
   ,        [tb_palestrante].[nm_palestrante]
   ,        [tb_palestrante].[ds_curriculo]
   FROM     [dbo].[tb_palestrante]
   ORDER BY [tb_palestrante].[nm_palestrante]