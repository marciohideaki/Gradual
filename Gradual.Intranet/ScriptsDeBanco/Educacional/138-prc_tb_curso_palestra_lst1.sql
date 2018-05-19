CREATE PROCEDURE prc_tb_curso_palestra_lst1
( @id_nivel      INT = NULL
, @id_localidade INT = NULL
, @id_palestra   INT = NULL)
AS
   SELECT     [tcp].[id_cursopalestra]
   ,          [tcp].[id_localidade]
   ,          [tcp].[id_tema]
   ,          [tma].[ds_titulo]
   ,          [tcp].[ds_texto]
   ,          [tcp].[ds_municipio]
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
   ,          [tcp].[fl_home]
   FROM       [dbo].[tb_curso_palestra] AS [tcp]
   INNER JOIN [dbo].[tb_tema]           AS [tma] ON [tma].[id_tema]       = [tcp].[id_tema]
   INNER JOIN [dbo].[tb_nivel]          AS [nvl] ON [tma].[id_nivel]      = [nvl].[id_nivel]
   INNER JOIN [dbo].[tb_localidade]     AS [loc] ON [tcp].[id_localidade] = [loc].[id_localidade]
   WHERE      [tcp].[id_localidade]     = ISNULL(@id_localidade, [tcp].[id_localidade])
   AND        [tma].[id_nivel]          = ISNULL(@id_nivel     , [tma].[id_nivel])
   AND        [tcp].[id_cursopalestra]  = ISNULL(@id_palestra  , [tcp].[id_cursopalestra])
   AND        [tcp].[st_situacao]       = 2
   ORDER BY   [tcp].[dt_datahoracurso]  ASC
   ,          [tcp].[st_tipoevento]     DESC