CREATE PROCEDURE prc_tb_palestra_sob_medida_sel 
( @id_curso_palestra_sob_medida int)
AS
   SELECT * FROM [dbo].[tb_palestra_sob_medida]
   WHERE  [tb_palestra_sob_medida].[id_curso_palestra_sob_medida] = @id_curso_palestra_sob_medida