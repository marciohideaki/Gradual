CREATE PROCEDURE prc_tb_perfil_lst
AS
   SELECT   [tb_perfil].[id_perfil]
   ,        [tb_perfil].[ds_perfil]
   FROM     [dbo].[tb_perfil]
   ORDER BY [tb_perfil].[ds_perfil]