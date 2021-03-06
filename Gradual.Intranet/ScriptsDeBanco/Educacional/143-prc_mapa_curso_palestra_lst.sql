set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER  PROCEDURE [dbo].[prc_mapa_curso_palestra_lst]
( @dt_inicio     DATETIME = NULL
, @dt_fim        DATETIME = NULL
, @status        CHAR     = NULL
, @id_tema       INT      = NULL
, @id_localidade INT      = NULL
, @id_assessor   INT      = NULL)
AS
    SELECT     [tcp].[id_cursopalestra]
    ,          [tma].[ds_titulo]
    ,          [nvl].[ds_nivel]
    ,          [tcp].[dt_datahoralimite]
    ,          [tcp].[dt_datahoracurso]
    ,          [tcp].[nr_vagalimite]
    ,          [tcp].[nr_vagainscritos]
    ,          [tcp].[nr_vagalimite] - [tcp].[nr_vagainscritos] AS [vagasrestantes]
    ,          [tcp].[valor]
    ,          [loc].[id_localidade]
    ,          [loc].[ds_localidade]
	,          CASE [tcp].[st_tipoevento] WHEN 'C' THEN 'Curso'     ELSE 'Palestra' END                                         AS [st_tipoevento]
	,          CASE [tcp].[st_situacao]   WHEN '1' THEN 'Cancelado' WHEN '2'        THEN 'A ser realizado' ELSE 'Realizado' END AS [st_situacao]
    ,          [tcp].[st_situacao]
    ,         (CONVERT(DATETIME, [tcp].[dt_datahoralimite], 103) - CONVERT(DATETIME, [tcp].[dt_datahoralimite], 103))           AS [diasrestantes]
    ,          [tcp].[fl_home]
    ,          [tcp].[st_situacao]
    ,          [tcp].[id_assessor]
    FROM       [educacional].[dbo].[tb_curso_palestra] AS [tcp]
    INNER JOIN [educacional].[dbo].[tb_tema]           AS [tma] ON [tcp].[id_tema]       = [tma].[id_tema]
    INNER JOIN [educacional].[dbo].[tb_nivel]          AS [nvl] ON [nvl].[id_nivel]      = [tma].[id_nivel]
    LEFT  JOIN [educacional].[dbo].[tb_localidade]     AS [loc] ON [tcp].[id_localidade] = [loc].[id_localidade]
    LEFT  JOIN [cadastro].[dbo].[tb_cliente]           AS [cli] ON [tcp].[id_assessor]   = [cli].[id_assessorinicial]
    LEFT  JOIN [cadastro].[dbo].[tb_login]             AS [log] ON [cli].[id_login]      = [log].[id_login]
    WHERE      [dt_datahoracurso]   >= @dt_inicio
    AND        [dt_datahoracurso]   <= @dt_fim
    AND        [tcp].[st_situacao]   = ISNULL(@status       , [tcp].[st_situacao])
    AND        [tma].[id_tema]       = ISNULL(@id_Tema      , [tma].[id_tema])
    AND        [tcp].[id_localidade] = ISNULL(@id_Localidade, [tcp].[id_localidade])
    AND        [tcp].[id_assessor]   = ISNULL(@id_Assessor  , [tcp].[id_assessor]);

