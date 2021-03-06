ALTER PROCEDURE [dbo].[prc_relatorio_cliente_bloqueio_instrumento]
( @ds_cpfcnpj   VARCHAR(15)  = null
, @ds_nome      VARCHAR(60)  = null
, @cd_assessor  INT          = null
, @cd_ativo     VARCHAR(200) = null)
AS
    SELECT     [con].[cd_codigo]
    ,          [cli].[ds_nome]
    ,          [cli].[ds_cpfcnpj]
    ,          [con].[cd_assessor]
    ,          [cbi].[cd_ativo]
    ,          [cbi].[direcao] AS [ds_direcao]
    FROM       [dbo].[tb_cliente_bloqueio_instrumento] AS [cbi]
    INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente_conta]     AS [con] ON [cbi].[id_cliente] = [con].[cd_codigo]
    INNER JOIN [DirectTradeCadastro].[dbo].[tb_cliente]           AS [cli] ON [cli].[id_cliente] = [con].[id_cliente]
    WHERE      (@ds_cpfcnpj   IS NULL OR [cli].[ds_cpfcnpj]      LIKE '%' + @ds_cpfcnpj + '%')
    AND        (@ds_nome      IS NULL OR UPPER([cli].[ds_nome])  LIKE '%' + UPPER(@ds_nome)  + '%')
    AND        (@cd_ativo     IS NULL OR UPPER([cbi].[cd_ativo]) LIKE '%' + UPPER(@cd_ativo) + '%')
    AND        (@cd_assessor  IS NULL OR [con].[cd_assessor]     = @cd_assessor)