CREATE PROCEDURE prc_tb_palestra_sob_medida_upd
( @id_curso_palestra_sob_medida INT
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
   UPDATE [dbo].[tb_palestra_sob_medida]
   SET    [tb_palestra_sob_medida].[id_palestrante]               = @id_palestrante
   ,      [tb_palestra_sob_medida].[id_tema]                      = @id_tema
   ,      [tb_palestra_sob_medida].[id_estado]                    = @id_estado
   ,      [tb_palestra_sob_medida].[ds_municipio]                 = @ds_municipio
   ,      [tb_palestra_sob_medida].[ds_endereco]                  = @ds_endereco
   ,      [tb_palestra_sob_medida].[ds_cep]                       = @ds_cep
   ,      [tb_palestra_sob_medida].[ds_local]                     = @ds_local
   ,      [tb_palestra_sob_medida].[tp_local]                     = @tp_local
   ,      [tb_palestra_sob_medida].[tp_solicitante]               = @tp_solicitante
   ,      [tb_palestra_sob_medida].[dt_datahora_inicio]           = @dt_datahora_inicio
   ,      [tb_palestra_sob_medida].[dt_datahora_fim]              = @dt_datahora_fim
   ,      [tb_palestra_sob_medida].[ds_publico_alvo]              = @ds_publico_alvo
   ,      [tb_palestra_sob_medida].[qt_pessoas]                   = @qt_pessoas
   ,      [tb_palestra_sob_medida].[st_situacao]                  = @st_situacao
   WHERE  [tb_palestra_sob_medida].[id_curso_palestra_sob_medida] = @id_curso_palestra_sob_medida