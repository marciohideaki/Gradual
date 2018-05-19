set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[prc_buscar_ordens_stop_start] 
(						
    @Account           int          = null,
    @DataDe            datetime     = null,
    @DataAte           datetime     = null,
    @Symbol            varchar(100) = null,
    @StopStartStatusID int          = null
)

as

IF @DataDe IS NOT NULL AND @DataAte IS NULL BEGIN

    Set @DataAte = getdate()

END


SELECT 
    [ord].[StopStartID],
	[ord].[OrdTypeID] ,
	[ord].[StopStartStatusID] ,
	[ord].[Symbol] ,
	[ord].[OrderQty] ,
	[ord].[Account] ,
	[ord].[RegisterTime] ,
	[ord].[ExpireDate] ,
	[ord].[ExecutionTime],
	[ord].[ReferencePrice] ,
	[ord].[StartPriceValue] ,
	[ord].[SendStartPrice] ,
	[ord].[StopGainValuePrice] ,
	[ord].[SendStopGainPrice] ,
	[ord].[StopLossValuePrice] ,
	[ord].[SendStopLossValuePrice] ,
	[ord].[InitialMovelPrice] ,
	[ord].[AdjustmentMovelPrice] ,
	[ord].[id_bolsa] ,
	[ord].[StopStartTipoEnum],
	[detalhe].[RegisterTime] as RegisterTimeDetail,
	[detalhe].[OrderStatusID] ,
	[status].[OrderStatusDescription]


FROM 

    TBStopStartOrder as ord

	LEFT JOIN tbStopStartDetail as detalhe ON
		detalhe.StopStartID      = ord.StopStartID

	LEFT JOIN tbStopStartStatus as  status ON 
		status.StopStartStatusID = detalhe.OrderStatusID

WHERE
		[ord].Account            = ISNULL(@Account,           [ord].Account)
    And [ord].Symbol             = ISNULL(@Symbol,            [ord].Symbol)
    And [ord].StopStartStatusID  = ISNULL(@StopStartStatusID, [ord].StopStartStatusID)
    And [ord].RegisterTime 
                BetWeen ISNULL(@DataDe,  [ord].RegisterTime) 
                    And ISNULL(@DataAte, [ord].RegisterTime)
	ORDER BY [ord].[RegisterTime] DESC, [ord].[StopStartID] DESC, [detalhe].[RegisterTime] DESC





