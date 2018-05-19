CREATE PROCEDURE prc_tb_curso_situacao_upd
( @id_cursopalestra int
, @st_situacao      int)
AS
   UPDATE [dbo].[tb_curso_palestra]
   SET    [tb_curso_palestra].[st_situacao]      = @st_situacao
   WHERE  [tb_curso_palestra].[id_cursopalestra] = @id_cursopalestra