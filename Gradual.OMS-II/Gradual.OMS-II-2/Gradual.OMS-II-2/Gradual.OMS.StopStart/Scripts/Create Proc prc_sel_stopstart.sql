set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[prc_sel_stopstart](
								@StopStartID int
							 )

AS

SELECT 
		StopStartID,
		OrdTypeID,
		StopStartStatusID,
		Symbol,
		OrderQty,
		Account,
		RegisterTime,
		ExpireDate,
		ExecutionTime,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,ReferencePrice),0),'.',',') as ReferencePrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,StartPriceValue),0),'.',',') as StartPriceValue,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,SendStartPrice),0),'.',',') as SendStartPrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,StopGainValuePrice),0),'.',',') as StopGainValuePrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,SendStopGainPrice),0),'.',',') as SendStopGainPrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,StopLossValuePrice),0),'.',',') as StopLossValuePrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,SendStopLossValuePrice),0),'.',',') as SendStopLossValuePrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,InitialMovelPrice),0),'.',',') as InitialMovelPrice,
		REPLACE(CONVERT(VARCHAR,CONVERT(MONEY,AdjustmentMovelPrice),0),'.',',') as AdjustmentMovelPrice,
		StopStartTipoEnum,
		id_Bolsa
FROM 
	TbStopStartOrder 
WHERE 
	StopStartID = @StopStartID

