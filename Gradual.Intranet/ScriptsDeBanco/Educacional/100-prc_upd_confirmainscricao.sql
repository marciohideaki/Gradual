CREATE PROCEDURE prc_upd_confirmainscricao
( @id_cursopalestra int
, @id_cliente       int
, @status           char)
AS
   UPDATE [dbo].[tb_cliente_curso_palestra]
   SET    [tb_cliente_curso_palestra].[st_confirmainscricao] = @status
   WHERE  [tb_cliente_curso_palestra].[id_cursopalestra]     = @id_cursopalestra
   AND    [tb_cliente_curso_palestra].[id_cliente]           = @id_cliente