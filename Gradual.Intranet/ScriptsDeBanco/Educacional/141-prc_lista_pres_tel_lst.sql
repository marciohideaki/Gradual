set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_lista_pres_tel_lst] ( @id_cliente INT)
AS
   SELECT [tel].[ds_ddd] + '-' + [tel].[ds_numero] AS [telefone]
   FROM   [cadastro].[dbo].[tb_cliente_telefone]   AS [tel]
   WHERE  [tel].[id_cliente] = @id_cliente;