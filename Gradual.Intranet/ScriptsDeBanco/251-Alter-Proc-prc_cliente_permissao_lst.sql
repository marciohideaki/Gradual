ALTER PROCEDURE [dbo].[prc_cliente_permissao_lst] (@id_cliente INT = NULL)
AS
BEGIN
	SELECT     a.id_cliente_permissao
    ,          a.id_cliente
    ,          a.id_grupo
    ,          a.id_permissao
    ,          b.ds_permissao as dscr_permissao
    ,          b.url_namespace
    ,          b.nm_metodo as nome_metodo
    ,          c.ds_grupo as dscr_grupo
    FROM       tb_cliente_permissao a
    INNER JOIN tb_permissao b ON a.id_permissao = b.id_permissao
    LEFT  JOIN tb_grupo     c ON a.id_grupo = c.id_grupo
	WHERE      id_cliente = isnull(@id_cliente, id_cliente)
	ORDER BY   b.ds_permissao
END