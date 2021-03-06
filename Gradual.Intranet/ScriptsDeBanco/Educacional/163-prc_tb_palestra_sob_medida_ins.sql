set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_tb_palestra_sob_medida_ins]
( @id_curso_palestra_sob_medida INT OUTPUT
, @id_palestrante               INT
, @id_tema                      INT
, @id_estado                    INT
, @ds_municipio                 VARCHAR(500)
, @ds_endereco                  VARCHAR(500)
, @ds_cep                       CHAR(10)
, @ds_local                     VARCHAR(500)
, @tp_local                     VARCHAR(500)
, @tp_solicitante               CHAR(1)
, @dt_datahora_inicio           DATETIME
, @dt_datahora_fim              DATETIME
, @ds_publico_alvo              VARCHAR(200)
, @qt_pessoas                   INT
, @st_situacao                  CHAR(1))
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

   SELECT @id_curso_palestra_sob_medida = SCOPE_IDENTITY()

