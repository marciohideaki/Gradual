ALTER PROC [dbo].[prc_buscar_ordens_stop_start] 
(						
    @Account           int          = null,
    @DataDe            datetime     = null,
    @DataAte           datetime     = null,
    @Symbol            varchar(100) = null,
    @StopStartStatusID int          = null,
	@CodigoAssessor    int          = null
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
	--RTRIM(LTRIM([status].[OrderStatusDescription]+ ' ' + [detalhe].[Critica])) as OrderStatusDescription
	RTRIM(LTRIM( ISNULL ( [detalhe].[Critica] , ''))) as OrderStatusDescription

FROM 

    TBStopStartOrder as ord

	LEFT JOIN tbStopStartDetail as detalhe ON
		detalhe.StopStartID      = ord.StopStartID

	LEFT JOIN tbStopStartStatus as  status ON 
		status.StopStartStatusID = detalhe.OrderStatusID
	
	LEFT JOIN DirectTradeCadastro.dbo.tb_cliente_conta as conta ON
		conta.cd_assessor = ISNULL(@CodigoAssessor, conta.cd_assessor) AND
		[ord].Account = conta.cd_codigo and conta.st_principal = 1

WHERE
		[ord].Account            = ISNULL(@Account,           [ord].Account)
    And [ord].Symbol             = ISNULL(@Symbol,            [ord].Symbol)
    And [ord].StopStartStatusID  = ISNULL(@StopStartStatusID, [ord].StopStartStatusID)
    And [ord].RegisterTime 
                BetWeen ISNULL(@DataDe,  [ord].RegisterTime) 
                    And ISNULL(@DataAte, [ord].RegisterTime)
	ORDER BY [ord].[RegisterTime] DESC, [ord].[StopStartID] ASC, [detalhe].[StopStartDetailID] ASC