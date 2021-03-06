ALTER PROC [dbo].[prc_buscar_ordens_online] (@Account INT)
AS
    SELECT * 
    FROM   vwOrderDetails 
    WHERE  Account = @Account
    AND    (
               (DAY(RegisterTime) = DAY(GETDATE()) And MONTH(RegisterTime) = MONTH(GETDATE()) AND  YEAR(RegisterTime) = YEAR(GETDATE()))
            OR (expiredate >= GETDATE() And  OrdStatusId IN(0, 1))
           )
    ORDER BY TransactTime

/*
    parcialmente executada ou nova -> RegisterTime de todos os dias

    já executadas, rejeitadas ou canceladas -> RegisterTime de só de hoje
*/