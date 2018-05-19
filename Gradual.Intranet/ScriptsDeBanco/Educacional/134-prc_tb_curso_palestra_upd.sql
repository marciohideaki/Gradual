CREATE PROCEDURE prc_tb_curso_palestra_upd
( @id_curso_palestra int
, @id_tema           int
, @id_localidade     int
, @id_assessor       int
, @ds_municipio      varchar(500)
, @ds_endereco       varchar(500)
, @ds_cep            char(10)
, @ds_texto          varchar(max)
, @nr_vaga_limite    int
, @nr_vaga_inscritos int
, @st_situacao       int
, @st_realizado      char(1)
, @st_tipoevento     char(1)
, @valor             decimal(9, 2)
, @dt_datahoralimite datetime
, @dt_datahoracurso  datetime)
AS
   UPDATE [dbo].[tb_curso_palestra]
   SET    [id_tema]           = @id_tema
   ,      [id_localidade]     = @id_localidade
   ,      [id_assessor]       = @id_assessor
   ,      [ds_municipio]      = @ds_municipio
   ,      [ds_endereco]       = @ds_endereco
   ,      [ds_cep]            = @ds_cep
   ,      [ds_texto]          = @ds_texto
   ,      [nr_vagalimite]     = @nr_vaga_limite
   ,      [nr_vagainscritos]  = @nr_vaga_inscritos
   ,      [st_situacao]       = @st_situacao
   ,      [st_realizado]      = @st_realizado
   ,      [st_tipoevento]     = @st_tipoevento
   ,      [valor]             = @valor
   ,      [dt_datahoralimite] = @dt_datahoralimite
   ,      [dt_datahoracurso]  = @dt_datahoracurso
   WHERE  [id_cursopalestra]  = @id_curso_palestra