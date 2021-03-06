set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER  PROCEDURE    [dbo].[prc_curso_palestra_online_lst] 
AS
BEGIN 
     SELECT     [cpo].[id_cursopalestraonline]
         ,      [cpo].[id_nivel]
         ,      [tnl].[ds_nivel]
         ,      [cpo].[ds_curso]
         ,      [cpo].[ds_url]
         ,      [cpo].[ds_texto]
     FROM       [educacional].[dbo].[tb_curso_palestra_online] AS [cpo]
     INNER JOIN [educacional].[dbo].[tb_nivel] AS [tnl] ON ([tnl].[id_nivel] = [cpo].[id_nivel])
     ORDER BY   [cpo].[id_cursopalestraonline] ASC;
 END;
