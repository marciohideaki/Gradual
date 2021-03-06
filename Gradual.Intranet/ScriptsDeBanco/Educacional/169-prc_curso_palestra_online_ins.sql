set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_curso_palestra_online_ins]
( @id_cursopalestraonline INT OUTPUT
, @id_nivel               INT
, @ds_curso               VARCHAR(500)
, @ds_url                 VARCHAR(200)
, @ds_texto               VARCHAR(MAX))
AS
   INSERT INTO [dbo].[tb_curso_palestra_online]
          (    [id_nivel]
          ,    [ds_curso]
          ,    [ds_url]
          ,    [ds_texto])
   VALUES (    @id_nivel
          ,    @ds_curso
          ,    @ds_url
          ,    @ds_texto)

   SELECT @id_cursopalestraonline = SCOPE_IDENTITY()
