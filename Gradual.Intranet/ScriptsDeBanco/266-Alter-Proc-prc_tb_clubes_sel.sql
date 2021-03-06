ALTER Procedure [dbo].[prc_tb_clubes_sel] (@id_cliente INT)
AS
  DECLARE @id_cliente_conta INT

  SELECT @id_cliente_conta = [id_cliente]
  FROM  [DirectTradeCadastro].[dbo].[tb_cliente_conta]
  WHERE [cd_codigo] = @id_cliente

  SELECT   [tb_clubes].[ds_nome_clube]
  ,        [tb_clubes].[cd_cliente]
  ,        ''                             AS [vl_cota]
  ,        [tb_clubes].[vl_dcquantidade]  AS [vl_quantidade]
  ,        [tb_clubes].[vl_saldo_bruto]   AS [vl_bruto]
  ,        [tb_clubes].[vl_ir]
  ,        [tb_clubes].[vl_iof]
  ,        [tb_clubes].[vl_saldo_liquido] AS [vl_liquido]
  ,        [tb_clubes].[dt_atualizacao]
  FROM     [dbo].[tb_clubes]
  WHERE    [tb_clubes].[cd_cliente] IN ( SELECT [cd_codigo]
                                         FROM   [DirectTradeCadastro].[dbo].[tb_cliente_conta]
                                         WHERE  [id_cliente] = @id_cliente_conta )
  ORDER BY [tb_clubes].[ds_nome_clube]