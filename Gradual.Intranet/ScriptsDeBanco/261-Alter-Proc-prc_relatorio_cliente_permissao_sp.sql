ALTER PROCEDURE [dbo].[prc_relatorio_cliente_permissao_sp]
( @cd_bovespa INT          = NULL
, @ds_cpfcnpj NVARCHAR(15) = NULL
, @ds_nome    VARCHAR(60)  = NULL
, @id_bolsa   INT          = NULL
, @id_grupo   INT          = NULL)
AS
SELECT     
   DISTINCT   [cli].[ds_nome], [cpm].[id_cliente]
   ,          [cli].[ds_cpfcnpj]
   ,          [pms].[ds_permissao]
   ,          [bol].[ds_bolsa]
   ,          [gru].[ds_grupo]
   FROM       [risco].[dbo].[tb_cliente_permissao]   AS [cpm]
   LEFT  JOIN [risco].[dbo].[tb_grupo]               AS [gru] ON [cpm].[id_grupo]     = [gru].[id_grupo]
   INNER JOIN [risco].[dbo].[tb_permissao]           AS [pms] ON [cpm].[id_permissao] = [pms].[id_permissao]
   LEFT  JOIN [risco].[dbo].[tb_bolsa]               AS [bol] ON [pms].[id_bolsa]     = [bol].[id_bolsa]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente_conta]  AS [con] ON [con].[cd_codigo]    = [cpm].[id_cliente]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente]        AS [cli] ON [cli].[id_cliente]   = [con].[id_cliente]
   WHERE [cpm].[id_cliente] IN
      (
			SELECT     [co1].[cd_codigo]
			FROM       [DirectTradeCadastro].[dbo].[tb_cliente_conta]  AS [co1]
			INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente]        AS [cl1] ON [cl1].[id_cliente] = [co1].[id_cliente]
			WHERE      (@ds_nome    IS NULL OR LOWER([cl1].[ds_nome]) LIKE '%' + LOWER(@ds_nome) + '%')
			AND        (@ds_cpfcnpj IS NULL OR [cl1].[ds_cpfcnpj] LIKE @ds_cpfcnpj + '%')
       )
   AND (@cd_bovespa IS NULL OR[cpm].[id_cliente] = @cd_bovespa)
   AND (@id_bolsa   IS NULL OR [bol].[id_bolsa] = @id_bolsa)
   AND (@id_grupo   IS NULL OR [cpm].[id_grupo] = @id_grupo)
   ORDER BY [cli].[ds_nome], [pms].[ds_permissao]