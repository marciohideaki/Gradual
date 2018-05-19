set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[prc_ins_ordem_stop] (
											@StopStartID          int output,
											@OrdTypeID            char(2) ,
											@StopStartStatusID    int = null,
											@Symbol               varchar(200),
											@OrderQty             int = null,
											@Account              int  = null ,       
											@RegisterTime         datetime = null , 
											@ExpireDate			  datetime = null,
											@ExecutionTime        datetime = null,
											@ReferencePrice       money = null,
											@StartPriceValue      money = null,
											@SendStartPrice       money = null,
											@StopGainValuePrice   money = null,
											@SendStopGainPrice    money = null,
											@StopLossValuePrice   money = null,
											@SendStopLossValuePrice   money = null,
											@InitialMovelPrice    money = null,
											@AdjustmentMovelPrice money = null,
											@StopStartTipoEnum int
									   ) 
AS
BEGIN TRAN

INSERT INTO TbStopStartOrder(
							OrdTypeID,
							StopStartStatusID,
							Symbol,
							OrderQty,
							Account,
							RegisterTime,
							ExpireDate,
							--ExecutionTime,
							ReferencePrice,
							StartPriceValue,
							SendStartPrice,
							StopGainValuePrice,
							SendStopGainPrice,
							StopLossValuePrice,
							SendStopLossValuePrice,
							InitialMovelPrice,
							AdjustmentMovelPrice,
							StopStartTipoEnum
						   )
					values (
							@OrdTypeID,          
							@StopStartStatusID,  
							@Symbol,             
							@OrderQty,           
							@Account,            
							getdate(),
							@ExpireDate,         
							--@ExecutionTime,      
							@ReferencePrice,     
							@StartPriceValue,    
							@SendStartPrice,     
							@StopGainValuePrice,
							@SendStopGainPrice, 
							@StopLossValuePrice,     
							@SendStopLossValuePrice, 
							@InitialMovelPrice,  
							@AdjustmentMovelPrice,
							@StopStartTipoEnum
					       )

SELECT @StopStartID = SCOPE_IDENTITY() 

	INSERT INTO [dbo].[TbStopStartDetail]
	(
		[StopStartID],
		[OrderStatusID],
		[RegisterTime]
	)
	VALUES
	(
		@StopStartID,
		1,
		getdate()
	)

	if @@error > 0		
	begin
		rollback tran
	end
	else
	begin
		commit tran
	end




