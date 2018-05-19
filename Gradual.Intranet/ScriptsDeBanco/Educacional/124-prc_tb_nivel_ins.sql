set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE [dbo].[prc_tb_nivel_ins]
( @ds_nivel varchar(50)
, @nr_order int)
AS
   INSERT INTO [dbo].[tb_nivel]
          (    [ds_nivel]
          ,    [nr_order])
   VALUES (    @ds_nivel
          ,    @nr_order)

   SELECT SCOPE_IDENTITY()
