set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[prc_atualiza_ordem_stop] (
												@StopStartID       int	,
												@StopStartStatusID int	,
												@ReferencePrice    decimal(12,8)=NULL								
										         )									

AS

BEGIN TRAN

	INSERT INTO tbStopStartDetail VALUES (@StopStartID,@StopStartStatusID,getdate())

	UPDATE 
		TbStopStartOrder 
	SET 
		RegisterTime        = getdate(),
		ExecutionTime       = getdate(),
		StopStartStatusID   = @StopStartStatusID,
		ReferencePrice      = @ReferencePrice
	WHERE
		StopStartID         = @StopStartID

	SELECT 
		Account, 
		OrderQty, 
		RegisterTime, 
		SendStartPrice, 
		SendStopLossValuePrice, 
		b.ds_bolsa,
		OrdTypeID 
	FROM
		TbStopStartOrder A inner join TB_BOLSA b
		on A.id_bolsa = B.id_bolsa
	WHERE 
		StopStartID = @StopStartID

if @@error > 0
begin
	rollback tran
end
else
begin
	commit tran
end
