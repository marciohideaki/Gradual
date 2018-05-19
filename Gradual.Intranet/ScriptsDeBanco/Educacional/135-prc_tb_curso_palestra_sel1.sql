CREATE PROCEDURE prc_tb_curso_palestra_sel1
( @id_nivel      int = null
, @id_localidade int = null)
AS
   SELECT     [tcp].[id_cursopalestra]
   ,          [tcp].[id_localidade]
   ,          [tcp].[id_tema]
   ,          [tma].[ds_titulo]
   ,          [tcp].[ds_texto]
   ,          [tcp].[ds_municipio]
   ,          [loc].[ds_localidade]
   ,          [nvl].[ds_nivel]
   ,          [tcp].[dt_datahoracurso]
   ,          [tcp].[nr_vagalimite]
   ,          [tcp].[ds_cep]
   ,          [tcp].[ds_endereco]
   ,          [tcp].[st_situacao]
   ,          [tcp].[dt_datahoralimite]
   ,          [tcp].[nr_vagainscritos]
   ,          [tcp].[nr_vagalimite] - [tcp].[nr_vagainscritos] AS [nr_vagarestante]
   ,          [tcp].[st_realizado]
   ,          [tcp].[st_situacao]
   ,          [tcp].[st_tipoevento]
   ,          [tcp].[valor]
   FROM       [dbo].[tb_curso_palestra] AS [tcp]
   INNER JOIN [dbo].[tb_tema]           AS [tma] ON [tma].[id_tema]       = [tcp].[id_tema]
   INNER JOIN [dbo].[tb_nivel]          AS [nvl] ON [nvl].[id_nivel]      = [tma].[id_nivel]
   INNER JOIN [dbo].[tb_localidade]     AS [loc] ON [loc].[id_localidade] = [tcp].[id_localidade]
   WHERE      [tcp].[id_localidade]     = @id_localidade
   AND        [tma].[id_nivel]          = @id_nivel