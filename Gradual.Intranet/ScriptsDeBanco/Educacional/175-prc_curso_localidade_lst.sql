set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_curso_localidade_lst] (@id_assessor INT)
AS
	SELECT     [tcp].[id_cursopalestra]
	,          [tma].[ds_titulo]
	,          [nvl].[ds_nivel]
	,          [tcp].[dt_datahoralimite]
	,          [tcp].[dt_datahoracurso]
	,          [tcp].[nr_vagalimite]
	,          [tcp].[nr_vagainscritos]
	,         ([tcp].[nr_vagalimite] - [tcp].[nr_vagainscritos]) AS [vagasrestantes]
	,          [tcp].[valor]
	,          [tcp].[id_localidade]
	,          CASE [tcp].[st_tipoevento] WHEN 'C' THEN  'Curso' ELSE 'Palestra' END AS [st_tipoevento]
	,          [tcp].[fl_home] 
	FROM       [dbo].[tb_curso_palestra] AS [tcp]
	INNER JOIN [dbo].[tb_tema]           AS [tma] ON ([tcp].[id_tema]  = [tma].[id_tema])
	INNER JOIN [dbo].[tb_nivel]          AS [nvl] ON ([nvl].[id_nivel] = [tma].[id_nivel])
	WHERE      [tcp].[id_assessor] = @id_assessor
	ORDER BY   [tcp].[dt_datahoracurso] DESC
    ,          [tma].[ds_titulo]


