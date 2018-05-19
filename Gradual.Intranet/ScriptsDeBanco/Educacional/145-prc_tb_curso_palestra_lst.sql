CREATE PROCEDURE prc_tb_curso_palestra_lst
( @id_tema       INT          = NULL
, @id_localidade INT          = NULL
, @ds_texto      VARCHAR(MAX) = NULL
, @st_tipoevento CHAR(1)      = NULL
, @st_situacao   INT          = NULL
, @id_assessor   INT          = NULL)
AS
   SELECT    [tcp].[id_cursopalestra]
   ,         [tcp].[id_tema]
   ,         [tcp].[ds_municipio]
   ,         [tcp].[ds_endereco]
   ,         [tcp].[ds_cep]
   ,         [tcp].[ds_texto]
   ,         [tcp].[dt_criacao]
   ,         [tcp].[nr_vagalimite]
   ,         [tcp].[nr_vagainscritos]
   ,         [tcp].[st_situacao]
   ,         [tcp].[st_realizado]
   ,         [tcp].[st_tipoevento]
   ,         [tcp].[valor]
   ,         [tcp].[dt_datahoralimite]
   ,         [tcp].[dt_datahoracurso]
   ,         [loc].[id_localidade]
   ,         [tcp].[fl_home]
   ,         [tcp].[id_assessor]
   ,         [loc].[ds_localidade] + ' - ' + [cli].[ds_nome]
   FROM      [educacional].[dbo].[tb_curso_palestra] AS [tcp]
   LEFT JOIN [educacional].[dbo].[tb_localidade]     AS [loc] ON [tcp].[id_localidade] = [loc].[id_localidade]
   LEFT JOIN [cadastro].[dbo].[tb_cliente]           AS [cli] ON [tcp].[id_assessor]   = [cli].[id_assessorinicial]
   WHERE     [tcp].[id_tema]       = ISNULL(@id_tema, [tcp].[id_tema])
   AND       [tcp].[id_localidade] = ISNULL(@id_localidade, [tcp].[id_localidade])
   AND       [tcp].[ds_texto]      = ISNULL(@ds_texto, [tcp].[ds_texto])
   AND       [tcp].[st_tipoevento] = ISNULL(@st_tipoevento, [tcp].[st_tipoevento])
   AND       [tcp].[st_situacao]   = ISNULL(@st_situacao, [tcp].[st_situacao])
   AND       [tcp].[id_assessor]   = ISNULL(@id_assessor, [tcp].[id_assessor])