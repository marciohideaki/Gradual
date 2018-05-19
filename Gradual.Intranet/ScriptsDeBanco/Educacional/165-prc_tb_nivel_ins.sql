set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_nivel_ins]
( @id_nivel INT OUTPUT
, @ds_nivel VARCHAR(50)
, @nr_order INT = null)
AS
   INSERT INTO [dbo].[tb_nivel]
          (    [ds_nivel]
          ,    [nr_order])
   VALUES (    @ds_nivel
          ,    @nr_order)

   SELECT @id_nivel = SCOPE_IDENTITY()

