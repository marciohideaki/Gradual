ALTER Procedure [dbo].[prc_tb_fundos_sel] (@id_cliente INT) 
AS
  DECLARE @id_cliente_conta INT

  SELECT @id_cliente_conta = [id_cliente]
  FROM  [DirectTradeCadastro].[dbo].[tb_cliente_conta]
  WHERE [cd_codigo] = @id_cliente

  SELECT   [tb_fundos].[ds_nome_fundo]
  ,        [tb_fundos].[vl_cota]
  ,        [tb_fundos].[vl_quantidade]
  ,        [tb_fundos].[vl_bruto]
  ,        [tb_fundos].[vl_ir]
  ,        [tb_fundos].[vl_iof]
  ,        [tb_fundos].[vl_liquido]
  ,        [tb_fundos].[dt_atualizacao]
  ,        [tb_fundos].[id_cliente]
  FROM     [dbo].[tb_fundos]
  WHERE    [tb_fundos].[id_cliente] IN (SELECT [cd_codigo]
                                        FROM   [DirectTradeCadastro].[dbo].[tb_cliente_conta]
                                        WHERE  [id_cliente] = @id_cliente_conta)
  ORDER BY [tb_fundos].[ds_nome_fundo]