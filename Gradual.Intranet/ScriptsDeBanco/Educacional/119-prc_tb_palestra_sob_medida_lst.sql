CREATE PROCEDURE prc_tb_palestra_sob_medida_lst
( @id_tema        int     = null
, @id_estado      int     = null
, @id_palestrante int     = null
, @tp_solicitante char(1) = null)
AS
   SELECT * FROM [dbo].[tb_curso_palestra_sob_medida] AS [psm]
   WHERE  [psm].[id_tema]        = ISNULL(@id_tema       , [psm].[id_tema])
   AND    [psm].[id_estado]      = ISNULL(@id_estado     , [psm].[id_estado])
   AND    [psm].[id_palestrante] = ISNULL(@id_palestrante, [psm].[id_palestrante])
   AND    [psm].[tp_solicitante] = ISNULL(@tp_solicitante, [psm].[tp_solicitante])