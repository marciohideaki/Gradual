set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_curso_palestra_online_upd]
( @id_cursopalestraonline    INT
, @id_nivel                  INT
, @ds_curso                  VARCHAR(500)
, @ds_url                    VARCHAR(200)
, @ds_texto                  VARCHAR(MAX))
AS
   UPDATE  [dbo].[tb_curso_palestra_online]
   SET     [id_nivel] = @id_nivel
   ,       [ds_curso] = @ds_curso
   ,       [ds_url]   = @ds_url
   ,       [ds_texto] = @ds_texto
   WHERE   [id_cursopalestraonline] = @id_cursopalestraonline
