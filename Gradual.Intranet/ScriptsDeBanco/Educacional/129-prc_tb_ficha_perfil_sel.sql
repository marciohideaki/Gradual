CREATE PROCEDURE prc_tb_ficha_perfil_sel (@id_ficha_perfil int)
AS
   SELECT * FROM [dbo].[tb_ficha_perfil]
   WHERE  [tb_ficha_perfil].[id_ficha_perfil] = @id_ficha_perfil