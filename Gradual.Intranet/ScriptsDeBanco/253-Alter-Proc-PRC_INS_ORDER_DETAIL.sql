ALTER PROC [dbo].[PRC_INS_ORDER_DETAIL]
( @TransactID       VARCHAR(200) = NULL
, @ClOrdID          VARCHAR(200)
, @OrderQty         INT = NULL
, @OrdQtyRemaining  INT = NULL
, @CumQty           INT = NULL
, @TradeQty         INT = NULL
, @Price            NUMERIC(18,12) = NULL
, @OrderStatusID    INT
, @Description	    VARCHAR(2000) = NULL)
AS
  DECLARE @OrderID INT
  SELECT  @OrderID = OrderID FROM tbOrder WHERE ClOrdID = @ClOrdID	

    INSERT INTO [TbOrderDetail]
           (    [TransactID]
           ,    [OrderID]
           ,    [OrderQty]
           ,    [OrdQtyRemaining]
           ,    [Price]
           ,    [OrderStatusID]
           ,    [TransactTime]
           ,    [Description]
           ,    [TradeQty]
           ,    [CumQty])
    VALUES (   @TransactID
           ,   @OrderID
           ,   @OrderQty
           ,   @OrdQtyRemaining
           ,   @Price
           ,   @OrderStatusID
           ,   GETDATE()
           ,   @Description
           ,   @TradeQty
           ,   @CumQty)

    select SCOPE_IDENTITY()