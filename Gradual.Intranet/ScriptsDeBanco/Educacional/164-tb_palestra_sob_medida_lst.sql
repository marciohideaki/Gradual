set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_palestra_sob_medida_lst]
( @id_tema        INT     = null
, @id_estado      INT     = null
, @id_palestrante INT     = null
, @tp_solicitante CHAR(1) = null)
AS
   SELECT * FROM [dbo].[tb_palestra_sob_medida] AS [psm]
   WHERE         [psm].[id_tema]        = ISNULL(@id_tema       , [psm].[id_tema])
   AND           [psm].[id_estado]      = ISNULL(@id_estado     , [psm].[id_estado])
   AND           [psm].[id_palestrante] = ISNULL(@id_palestrante, [psm].[id_palestrante])
   AND           [psm].[tp_solicitante] = ISNULL(@tp_solicitante, [psm].[tp_solicitante])

