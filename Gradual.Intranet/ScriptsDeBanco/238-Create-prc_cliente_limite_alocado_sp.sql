CREATE PROCEDURE [dbo].[prc_cliente_limite_alocado_sp](@id_cliente INT)
AS
SELECT     [pri].[id_parametro]
,          [pri].[ds_parametro]
,          ISNULL([cpr].[vl_parametro], 0)                     AS [vl_parametro]
,          ISNULL([cpv].[vl_alocado], 0)                       AS [vl_alocado]
,          ISNULL([cpv].[vl_disponivel], [cpr].[vl_parametro]) AS [vl_disponivel]
FROM       [dbo].[tb_cliente_parametro]       AS [cpr]
INNER JOIN [dbo].[tb_parametro_risco]         AS [pri] ON [cpr].[id_parametro] = [pri].[id_parametro]
LEFT JOIN  [dbo].[tb_cliente_parametro_valor] AS [cpv] 
                                              ON [cpv].[id_cliente_parametro] = [cpr].[id_cliente_parametro]
                                             AND [cpv].[dt_movimento] = (
                                                                         SELECT MAX([cp1].[dt_movimento]) 
                                                                         FROM       [dbo].[tb_cliente_parametro_valor] AS [cp1] 
                                                                         WHERE      [cp1].[id_cliente_parametro]        = [cpv].[id_cliente_parametro])
WHERE [cpr].[st_ativo] = 'S'
AND   [cpr].[id_cliente] = @id_cliente