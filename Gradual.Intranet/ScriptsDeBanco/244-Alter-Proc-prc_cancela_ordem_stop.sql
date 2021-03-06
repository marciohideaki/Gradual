ALTER proc [dbo].[prc_cancela_ordem_stop]
( @StopStartID int
, @StopStartStatusID int)
AS

BEGIN TRANSACTION

	INSERT INTO dbo.TbStopStartDetail ( StopStartID,OrderStatusID, RegisterTime ) VALUES ( @StopStartID, @StopStartStatusID, getdate())

	UPDATE TbStopStartOrder 
	SET    StopStartStatusID = @StopStartStatusID
    ,      ExecutionTime = GETDATE()
	WHERE  StopStartID = @StopStartID

IF @@ERROR > 0
BEGIN
	ROLLBACK TRAN
END
ELSE
BEGIN
	COMMIT TRAN
END