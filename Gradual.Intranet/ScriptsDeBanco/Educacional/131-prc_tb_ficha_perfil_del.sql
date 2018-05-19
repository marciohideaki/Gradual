CREATE PROCEDURE prc_tb_ficha_perfil_del (@id_ficha_pefil INT)
AS
   DELETE FROM [dbo].[tb_ficha_perfil] WHERE [tb_ficha_perfil].[id_ficha_perfil] = @id_ficha_pefil