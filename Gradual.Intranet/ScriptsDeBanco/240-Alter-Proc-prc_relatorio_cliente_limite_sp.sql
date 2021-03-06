USE [DirectTradeRisco]
GO
/****** Object:  StoredProcedure [dbo].[prc_relatorio_cliente_limite_sp]    Script Date: 01/05/2011 10:58:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[prc_relatorio_cliente_limite_sp]
( @ds_nome      VARCHAR(60)  = NULL
, @ds_cpfcnpj   NVARCHAR(15) = NULL
, @cd_bovespa   INT          = NULL
, @id_parametro INT          = NULL)
AS

SELECT DISTINCT [cli].[ds_nome], [cpr].[id_cliente_parametro]
   ,            [con].[cd_codigo]
   ,            [cli].[ds_cpfcnpj]
   ,		    [pri].[id_parametro]
   ,            [pri].[ds_parametro]
   ,            [cpr].[vl_parametro]
   ,            [cpv].[vl_alocado]
   ,            [cpr].[id_cliente_parametro]
   ,            [cpr].[dt_validade]
   FROM         [dbo].[tb_cliente_parametro]        AS [cpr]
   INNER JOIN   [dbo].[tb_cliente_parametro_valor]  AS [cpv] 
	                                                ON [cpr].[id_cliente_parametro] = [cpv].[id_cliente_parametro] 
	                                               AND [cpv].[dt_movimento] =
                                                                             (
		                                                                      SELECT MAX([cp1].[dt_movimento])
			                                                                  FROM       [dbo].[tb_cliente_parametro_valor] AS [cp1] 
			                                                                  WHERE      [cp1].[id_cliente_parametro] = [cpv].[id_cliente_parametro]
                                                                             )
   INNER JOIN  [dbo].[tb_parametro_risco]          AS [pri] ON [cpr].[id_parametro] = [pri].[id_parametro]
   INNER JOIN  [cadastro].[dbo].[tb_cliente_conta] AS [con] ON [con].[cd_codigo]    = [cpr].[id_cliente]
   LEFT  JOIN  [cadastro].[dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente]   = [con].[id_cliente]
   WHERE [cpr].[id_cliente] IN
         (
			SELECT     [co1].[cd_codigo]
			FROM       [cadastro].[dbo].[tb_cliente_conta] AS [co1]
			INNER JOIN [cadastro].[dbo].[tb_cliente]       AS [cl1] ON [cl1].[id_cliente] = [co1].[id_cliente]
			WHERE      (@ds_nome    IS NULL OR LOWER([cl1].[ds_nome]) LIKE '%' + LOWER(@ds_nome) + '%')
			AND        (@ds_cpfcnpj IS NULL OR [cl1].[ds_cpfcnpj] LIKE @ds_cpfcnpj + '%')
         )
   AND        [cpr].[id_cliente]   =  ISNULL(@cd_bovespa,   [cpr].[id_cliente])
   AND        [cpr].[id_parametro] =  ISNULL(@id_parametro, [cpr].[id_parametro])
   AND        [cpr].[st_ativo]        = 'S'
   ORDER BY   [cli].[ds_nome]
   ,          [pri].[ds_parametro] 





-- exec [prc_relatorio_cliente_limite_sp] null, null, 31940
-- exec prc_relatorio_cliente_limite_movimento_sp
-- select * from [tb_cliente_parametro] where id_cliente = 31940 and st_ativo = 'S'
