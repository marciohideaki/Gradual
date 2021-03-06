set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_lista_presenca_lst] (@id_cursopalestra INT)
AS
   SELECT   '' AS [telefone]
   ,         [cur].[id_cursopalestra]
   ,         [cli].[id_cliente]
   ,         [cli].[ds_nome]
   ,         [cli].[ds_cpfcnpj]
   ,         CASE [cur].[st_listaespera]       WHEN 'N' THEN 'Nao' ELSE 'Sim' END AS [st_listaespera]
   ,         CASE [cur].[st_presenca]          WHEN 'N' THEN 'Nao' ELSE 'Sim' END AS [st_presenca]
   ,         CASE [cur].[st_confirmainscricao] WHEN 'N' THEN 'Nao' ELSE 'Sim' END AS [st_confirmainscricao]
   FROM      [DirectTradeCadastro].[dbo].[tb_cliente]                   AS [cli]
   LEFT JOIN [DirectTradeCadastro].[dbo].[tb_login]                     AS [log] ON ([cli].[id_login]   = [log].[id_login])
   LEFT JOIN [educacional].[dbo].[tb_cliente_curso_palestra] AS [cur] ON ([cur].[id_cliente] = [cli].[id_cliente])
   WHERE     [cur].[id_cursopalestra] = @id_cursopalestra;

