CREATE PROCEDURE prc_tb_perfil_del (@id_perfil INT)
AS
    DELETE FROM [dbo].[tb_perfil] WHERE [tb_perfil].[id_perfil] = @id_perfil