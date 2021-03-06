ALTER PROC [dbo].[prc_buscar_ordens_stop_start] 
( @Account           INT          = null
, @DataDe            DATETIME     = null
, @DataAte           DATETIME     = null
, @Symbol            VARCHAR(100) = null
, @StopStartStatusID INT          = null
, @CodigoAssessor    INT          = null)

AS

IF @DataDe IS NOT NULL AND @DataAte IS NULL BEGIN

    Set @DataAte = getdate()

END

SELECT [ord].[StopStartID]
,      [ord].[OrdTypeID]
,      [ord].[StopStartStatusID]
,      [ord].[Symbol]
,      [ord].[OrderQty]
,      [ord].[Account]
,      [ord].[RegisterTime]
,      [ord].[ExpireDate]
,      [ord].[ExecutionTime]
,      [ord].[ReferencePrice]
,      [ord].[StartPriceValue]
,      [ord].[SendStartPrice]
,      [ord].[StopGainValuePrice]
,      [ord].[SendStopGainPrice]
,      [ord].[StopLossValuePrice]
,      [ord].[SendStopLossValuePrice]
,      [ord].[InitialMovelPrice]
,      [ord].[AdjustmentMovelPrice]
,      [ord].[id_bolsa]
,      [ord].[StopStartTipoEnum]
,      [detalhe].[RegisterTime] as RegisterTimeDetail
,      [detalhe].[OrderStatusID]
--,    RTRIM(LTRIM([status].[OrderStatusDescription]+ ' ' + [detalhe].[Critica])) as OrderStatusDescription
,      RTRIM(LTRIM( ISNULL ( [detalhe].[Critica] , ''))) as OrderStatusDescription

FROM TBStopStartOrder       AS ord
LEFT JOIN tbStopStartDetail AS detalhe ON detalhe.StopStartID = ord.StopStartID
LEFT JOIN tbStopStartStatus AS  status ON status.StopStartStatusID = detalhe.OrderStatusID
LEFT JOIN DirectTradeCadastro.dbo.tb_cliente_conta AS conta ON conta.cd_assessor = ISNULL(@CodigoAssessor, conta.cd_assessor) AND [ord].Account = conta.cd_codigo and conta.st_principal = 1

WHERE [ord].Account          = ISNULL(@Account,           [ord].Account)
AND [ord].Symbol             = ISNULL(@Symbol,            [ord].Symbol)
AND [ord].StopStartStatusID  = ISNULL(@StopStartStatusID, [ord].StopStartStatusID)
AND [ord].RegisterTime BETWEEN ISNULL(@DataDe,  [ord].RegisterTime) AND ISNULL(@DataAte, [ord].RegisterTime)
ORDER BY [ord].[RegisterTime] DESC
,        [ord].[StopStartID] ASC
,        [detalhe].[StopStartDetailID] ASC