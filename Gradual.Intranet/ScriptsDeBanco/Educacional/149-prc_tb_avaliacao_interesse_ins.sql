set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER  PROCEDURE  [dbo].[prc_tb_avaliacao_interesse_ins]
( @id_avaliacaoInteresse int
, @ds_avaliacaoInteresse VARCHAR(max))
AS
 INSERT INTO [EDUCACIONAL].[dbo].[TB_AVALIACAO_INTERESSE]
 VALUES (    @id_avaliacaoInteresse
        ,    @ds_avaliacaoInteresse)
