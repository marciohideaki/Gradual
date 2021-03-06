ALTER PROCEDURE [dbo].[prc_relatorio_cliente_parametro_sp]
( @ds_nome      VARCHAR(60)  = NULL
, @ds_cpfcnpj   NVARCHAR(15) = NULL
, @cd_bovespa   INT          = NULL
, @id_bolsa     INT          = NULL
, @id_grupo     INT          = NULL
, @id_parametro INT          = NULL
, @ds_valido    VARCHAR(5)   = NULL)
AS
   SELECT     [cli].[ds_nome]
   ,          [cli].[ds_cpfcnpj]
   ,          [pri].[ds_parametro]
   ,          [bol].[ds_bolsa]
   ,          [gru].[ds_grupo]
   FROM       [dbo].[tb_cliente_parametro]          AS [cpm]
   INNER JOIN [dbo].[tb_parametro_risco]            AS [pri] ON [cpm].[id_parametro]          = [pri].[id_parametro]
   LEFT  JOIN [dbo].[tb_bolsa]                      AS [bol] ON [bol].[id_bolsa]              = [pri].[id_bolsa]
   LEFT  JOIN [dbo].[tb_grupo]                      AS [gru] ON [cpm].[id_grupo]              = [gru].[id_grupo]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente_conta] AS [con] ON [con].[cd_codigo]    = [cpm].[id_cliente]
   LEFT  JOIN [DirectTradeCadastro].[dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente]   = [con].[id_cliente]
   WHERE [cpm].[id_cliente] IN
      (
			SELECT     [co1].[cd_codigo]
			FROM       [DirectTradeCadastro].[dbo].[tb_cliente_conta]  AS [co1]
			INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente]        AS [cl1] ON [cl1].[id_cliente] = [co1].[id_cliente]
			WHERE      (@ds_nome    IS NULL OR LOWER([cl1].[ds_nome])  LIKE '%' + LOWER(@ds_nome) + '%')
			AND        (@ds_cpfcnpj IS NULL OR [cl1].[ds_cpfcnpj]      LIKE @ds_cpfcnpj + '%')
       )
   AND      (@cd_bovespa IS NULL OR [cpm].[id_cliente] = @cd_bovespa)
   AND      ([bol].[id_bolsa]     = ISNULL(@id_bolsa    , [bol].[id_bolsa]) OR ([bol].[id_bolsa] IS NULL AND @id_bolsa IS NULL))
   AND      ([cpm].[id_grupo]     = ISNULL(@id_grupo    , [cpm].[id_grupo]) OR ([cpm].[id_grupo] IS NULL AND @id_grupo IS NULL))
   AND       [cpm].[id_parametro] = ISNULL(@id_parametro, [cpm].[id_parametro])
   AND      (@ds_valido IS NULL OR ('true' = LOWER(@ds_valido) AND [cpm].[dt_validade] >= GETDATE()) OR ('false' = LOWER(@ds_valido) AND [cpm].[dt_validade] < GETDATE()))
   AND       [cpm].[st_ativo] = 'S'
   ORDER BY [cli].[ds_nome], [pri].[ds_parametro], [bol].[ds_bolsa]