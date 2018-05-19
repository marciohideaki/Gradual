CREATE PROCEDURE prc_tb_palestra_sob_medida_del (@id_curso_palestra_sob_medida INT)
AS
   DELETE FROM [dbo].[tb_palestra_sob_medida] WHERE [id_curso_palestra_sob_medida] = @id_curso_palestra_sob_medida