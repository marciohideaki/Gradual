ALTER PROCEDURE [dbo].[prc_relatorio_cliente_limite_movimento_sp]
( @ds_nome      VARCHAR(60)  = NULL
, @ds_cpfcnpj   NVARCHAR(15) = NULL
, @cd_bovespa   INT          = NULL)
AS
   SELECT     [cpv].[id_cliente_parametro]
   ,          [cpv].[vl_movimento]
   ,          [cpv].[vl_alocado]
   ,          [cpv].[vl_disponivel]
   ,          [cpv].[dt_movimento]
   ,          [cpv].[ds_historico]
   ,          [cpv].[id_cliente_parametro_valor]
   FROM       [dbo].[tb_cliente_parametro_valor] AS [cpv]
   LEFT  JOIN [dbo].[tb_cliente_parametro]       AS [cpr]    ON [cpr].[id_cliente_parametro] = [cpv].[id_cliente_parametro]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente_conta] AS [con] ON [con].[cd_codigo]   = [cpr].[id_cliente]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente]  = [con].[id_cliente]
   WHERE      [cpr].[id_cliente] IN
      (
			SELECT     [co1].[cd_codigo]
			FROM       [cadastro].[dbo].[tb_cliente_conta]   AS [co1]
			INNER JOIN [cadastro].[dbo].[tb_cliente]         AS [cl1] ON [cl1].[id_cliente] = [co1].[id_cliente]
			WHERE      (@ds_nome    IS NULL OR LOWER([cl1].[ds_nome]) LIKE '%' + LOWER(@ds_nome) + '%')
			AND        (@ds_cpfcnpj IS NULL OR [cl1].[ds_cpfcnpj]     LIKE @ds_cpfcnpj + '%')
       )
   AND        [cpr].[id_cliente]   =  ISNULL(@cd_bovespa,   [cpr].[id_cliente])