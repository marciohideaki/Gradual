set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE [dbo].[prc_tb_palestrante_ins]
( @nm_palestrante VARCHAR(100)
, @ds_curriculo   VARCHAR(MAX))
AS
   INSERT INTO [dbo].[tb_palestrante]
          (    [tb_palestrante].[nm_palestrante]
          ,    [tb_palestrante].[ds_curriculo])
   VALUES (    @nm_palestrante
          ,    @ds_curriculo)

   SELECT SCOPE_IDENTITY()
