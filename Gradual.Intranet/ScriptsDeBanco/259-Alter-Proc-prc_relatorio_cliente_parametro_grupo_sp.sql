ALTER PROCEDURE [dbo].[prc_relatorio_cliente_parametro_grupo_sp]
( @ds_cpfcnpj   VARCHAR(15) = null
, @ds_nome      VARCHAR(60) = null
, @cd_assessor  INT         = null
, @id_parametro INT         = null
, @id_grupo     INT         = null)
AS
    SELECT     [con].[cd_codigo]
    ,          [cli].[ds_nome]
    ,          [cli].[ds_cpfcnpj]
    ,          [con].[cd_assessor]
    ,          [pri].[ds_parametro]
    ,          [gru].[ds_grupo]
    FROM       [dbo].[tb_cliente_parametro_grupo]             AS [cpg]
    INNER JOIN [dbo].[tb_parametro_risco]                     AS [pri] ON [cpg].[id_parametro] = [pri].[id_parametro]
    INNER JOIN [dbo].[DirectTradeCadastro]                    AS [gru] ON [cpg].[id_grupo]     = [gru].[id_grupo]
    INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente_conta] AS [con] ON [cpg].[id_cliente]   = [con].[cd_codigo]
    INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente]   = [con].[id_cliente]
    WHERE      (@ds_cpfcnpj   IS NULL OR [cli].[ds_cpfcnpj]     LIKE '%' + @ds_cpfcnpj + '%')
    AND        (@ds_nome      IS NULL OR UPPER([cli].[ds_nome]) LIKE '%' + UPPER(@ds_nome) + '%')
    AND        (@cd_assessor  IS NULL OR [con].[cd_assessor]  = @cd_assessor)
    AND        (@id_parametro IS NULL OR [pri].[id_parametro] = @id_parametro)
    AND        (@id_grupo     IS NULL OR [gru].[id_grupo]     = @id_grupo)