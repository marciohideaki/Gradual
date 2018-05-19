CREATE PROCEDURE prc_upd_ConfirmaPresenca
( @id_cursoPalestra int
, @id_cliente       int
, @status           char(1)
)
AS
   UPDATE [dbo].[tb_cliente_curso_palestra]
   SET    [tb_cliente_curso_palestra].[st_presenca]      = @status
   WHERE  [tb_cliente_curso_palestra].[id_cursopalestra] = @id_cursoPalestra
   AND    [tb_cliente_curso_palestra].[id_cliente]       = @id_cliente