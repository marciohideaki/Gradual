-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 04/11/2010
-- Description:	Lista os códigos dos clientes BOVESPA e BMF para serem listados no 
-- monitoramento para uso de filtro de ordens por assessor
-- =============================================
ALTER PROCEDURE [dbo].[prc_cliente_assessor_monitoramento_lst] (@cd_assessor INT)
AS
BEGIN
	SELECT [cd_bovespa] = CASE WHEN [cd_sistema] = 'BOL' THEN [cd_codigo] ELSE 0 END
    ,      [cd_bmf]     = CASE WHEN [cd_sistema] = 'BMF' THEN [cd_codigo] ELSE 0 END
	FROM   [DirectTradeCadastro].[dbo].[tb_cliente_conta]
	WHERE  [st_ativa] = 1
    AND    [cd_assessor] = @cd_assessor
    AND    [cd_sistema]  IN ('BOL','BMF')
END