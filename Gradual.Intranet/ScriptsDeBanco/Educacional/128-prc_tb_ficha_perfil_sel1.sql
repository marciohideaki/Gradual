CREATE PROCEDURE prc_tb_ficha_perfil_sel1 (@id_cliente INT)
AS
   SELECT * FROM [dbo].[tb_ficha_perfil]
   WHERE  [tb_ficha_perfil].[id_cliente] = @id_cliente