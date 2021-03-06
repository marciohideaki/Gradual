ALTER proc [dbo].[prc_sel_regras_cliente] (@id_cliente int)
AS
	SELECT     'Permissao' AS [Especie]
    ,          [id_cliente_permissao] AS 'IdClienteParametroPermissao'
    ,          [id_grupo]
    ,          [id_bolsa]
    ,          [id_cliente]
    ,          [ds_permissao] AS 'ParametroPermissao'
    ,          [tb_cliente_permissao].[id_permissao] AS IdParametroPermissao
    ,          NULL AS [valor]
    ,          NULL AS [dt_validade]
    ,          NULL AS [dt_movimento]
    FROM       [dbo].[tb_cliente_permissao]
    INNER JOIN [dbo].[tb_permissao] ON ([tb_cliente_permissao].[id_permissao] = [tb_permissao].[id_permissao]) 
    WHERE      [id_cliente] = @id_cliente
    UNION ALL
    SELECT     'Parametro' AS [Especie]
    ,          [tb_cliente_parametro].[id_cliente_parametro] AS 'IdClienteParametroPermissao'
    ,          [id_grupo]
    ,          [id_bolsa]
    ,          [id_cliente]
    ,          [ds_parametro] AS 'ParametroPermissao'
    ,          [tb_cliente_parametro].[id_parametro] AS [IdParametroPermissao]
    ,          (ISNULL([vl_parametro], 0) - ISNULL([vl_alocado], 0)) AS [valor]
    ,          [dt_validade]
    ,          [dt_movimento]
    FROM       [dbo].[tb_cliente_parametro]
    INNER JOIN [dbo].[tb_parametro_risco] ON ([tb_cliente_parametro].[id_parametro] = [tb_parametro_risco].[id_parametro])
    WHERE      [id_cliente] = @id_cliente
    AND        [st_ativo] = 'S'
    AND        [dt_validade] >= GETDATE() -- Data de validade 
    ORDER BY   [idParametroPermissao] ASC 
    ,          [dt_movimento] DESC