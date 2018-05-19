CREATE FUNCTION [pendencia_cadastral_cliente_fn] (@id_cliente int)
RETURNS BIT
AS
BEGIN

DECLARE @retorno bit

SELECT @retorno = CONVERT(bit, ISNULL([cpe].[id_cliente], 0))
FROM   [dbo].[tb_cliente_pendenciacadastral] AS [cpe]
WHERE  [cpe].[dt_resolucao] IS NULL
AND    [cpe].[id_cliente] = @id_cliente

RETURN @retorno

END

