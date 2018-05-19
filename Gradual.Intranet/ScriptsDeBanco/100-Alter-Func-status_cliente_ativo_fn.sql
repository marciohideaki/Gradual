CREATE FUNCTION [dbo].[status_cliente_ativo_fn] (@id_cliente int)
RETURNS BIT
AS
BEGIN

DECLARE @retorno bit

SELECT  @retorno = CONVERT(bit, COUNT(*))
FROM    [dbo].[tb_cliente_conta] AS [cco]
WHERE   [cco].[id_cliente] = @id_cliente
AND     [cco].[st_ativa]   = 1

RETURN  @retorno

END