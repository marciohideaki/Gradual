set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[prc_cancela_ordem_stop] ( 
									  @StopStartID int,
									  @StopStartStatusID int
									 )
AS

BEGIN TRANSACTION

	insert into dbo.TbStopStartDetail ( StopStartID,OrderStatusID, RegisterTime ) values ( @StopStartID, @StopStartStatusID, getdate())

	update 
		TbStopStartOrder 
	set 
		StopStartStatusID = @StopStartStatusID,
		ExecutionTime = getdate()
	where 
		StopStartID = @StopStartID

if @@error > 0
begin
	rollback tran
end
else
begin
	commit tran
end


