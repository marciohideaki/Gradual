set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE [dbo].[prc_tb_palestra_sob_medida_ins]
( @id_palestrante     int
, @id_tema            int
, @id_estado          int
, @ds_municipio       varchar(500)
, @ds_endereco        varchar(500)
, @ds_cep             char(10)
, @ds_local           varchar(500)
, @tp_local           varchar(500)
, @tp_solicitante     char(1)
, @dt_datahora_inicio datetime
, @dt_datahora_fim    datetime
, @ds_publico_alvo    varchar(200)
, @qt_pessoas         int
, @st_situacao        char(1))
AS
   INSERT INTO [dbo].[tb_palestra_sob_medida]
          (    [id_palestrante]
          ,    [id_tema]
          ,    [id_estado]
          ,    [ds_municipio]
          ,    [ds_endereco]
          ,    [ds_cep]
          ,    [ds_local]
          ,    [tp_local]
          ,    [tp_solicitante]
          ,    [dt_criacao]
          ,    [dt_datahora_inicio]
          ,    [dt_datahora_fim]
          ,    [ds_publico_alvo]
          ,    [qt_pessoas]
          ,    [st_situacao])
   VALUES (    @id_palestrante
          ,    @id_tema
          ,    @id_estado
          ,    @ds_municipio
          ,    @ds_endereco
          ,    @ds_cep
          ,    @ds_local
          ,    @tp_local
          ,    @tp_solicitante
          ,    GETDATE()
          ,    @dt_datahora_inicio
          ,    @dt_datahora_fim
          ,    @ds_publico_alvo
          ,    @qt_pessoas
          ,    @st_situacao)

   SELECT SCOPE_IDENTITY()
