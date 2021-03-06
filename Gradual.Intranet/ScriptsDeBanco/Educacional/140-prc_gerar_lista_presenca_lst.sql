set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_gerar_lista_presenca_lst] (@id_cursopalestra  INT)
AS
   SELECT     [cli].[ds_nome]
   ,          [cli].[ds_cpfcnpj]
   ,          '                                                              ' AS Assinatura
   FROM       [Cadastro].[dbo].[tb_cliente]                   AS [cli]
   INNER JOIN [Educacional].[dbo].[tb_cliente_curso_palestra] AS [cur]  ON [cur].[id_cliente] = [cli].[id_cliente]
   WHERE      [cur].[id_cursopalestra] = @id_cursopalestra;


