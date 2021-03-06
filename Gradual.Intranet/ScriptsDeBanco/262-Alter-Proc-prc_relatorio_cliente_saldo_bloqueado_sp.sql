ALTER PROCEDURE [dbo].[prc_relatorio_cliente_saldo_bloqueado_sp]
( @id_cliente         INT          = NULL
, @ds_nome            VARCHAR(60)  = NULL
, @ds_cpfcnpj         NVARCHAR(15) = NULL
, @ds_ativo           VARCHAR(100) = NULL
, @tp_ordem           CHAR(1)      = NULL
, @dt_transacao_de    DATETIME     = NULL
, @dt_transacao_ate   DATETIME     = NULL
, @id_canal_bovespa   INT) -- 4274 -- [Intranet Produção] 
AS

DECLARE @side INT

IF (@tp_ordem = 'C') SET @side = 1 ELSE SET @side = 2

SELECT CASE [Side] WHEN 1 THEN 'Compra' ELSE 'Venda' END [Side]
,           [con].[cd_codigo]
,           [cli].[ds_nome]
,           [cli].[ds_cpfcnpj]
,           [ord].[OrderQty]
,           [ord].[Price]
,           [ord].[symbol]
,           [sta].[OrderStatusDescription]
,           [ord].[transactTime]
,          ([ord].[OrderQty] * [ord].[Price])     AS [BloqueioOperacaoTotal]
FROM        [dbo].[tborder]                       AS [ord]
INNER JOIN  [dbo].[tborderstatus]                 AS [sta] ON ([ord].[OrdStatusId] = [sta].[OrderStatusID])
INNER JOIN  [DirectTradeCadastro].[dbo].[tb_cliente_conta] AS [con] ON [con].[cd_codigo]    = [ord].[account]
INNER JOIN  [DirectTradeCadastro].[dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente]   = [con].[id_cliente]
WHERE       [account] IN
		    (
                SELECT     [co1].[cd_codigo]
                FROM       [DirectTradeCadastro].[dbo].[tb_cliente_conta] AS [co1]
                INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente]       AS [cl1] ON [cl1].[id_cliente] = [co1].[id_cliente]
                WHERE      (@ds_nome    IS NULL OR LOWER([cl1].[ds_nome]) LIKE '%' + LOWER(@ds_nome) + '%')
                AND        (@ds_cpfcnpj IS NULL OR [cl1].[ds_cpfcnpj]     LIKE @ds_cpfcnpj + '%')
	        )
AND         [ord].[OrdStatusId] IN (0, 1, 8, 100, 101)
AND         [channelId]         = @id_canal_bovespa
AND         (@id_cliente        IS NULL OR [account] = @id_cliente)
AND         (@ds_ativo	        IS NULL OR [symbol]  = @ds_ativo)
AND         (@tp_ordem          IS NULL OR [Side]    = @side)
AND         (@dt_transacao_de   IS NULL OR @dt_transacao_ate IS NULL OR ([transactTime] BETWEEN @dt_transacao_de AND @dt_transacao_ate))
AND         [ExpireDate] >= getDate()
ORDER BY    [transactTime] DESC