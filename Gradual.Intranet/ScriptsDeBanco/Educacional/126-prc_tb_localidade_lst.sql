CREATE PROCEDURE prc_tb_localidade_lst
AS
   SELECT   *
   FROM     [dbo].[tb_localidade]
   WHERE    [tb_localidade].[bit_portal] = 1
   ORDER BY [tb_localidade].[ds_localidade]