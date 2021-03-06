ALTER PROCEDURE [dbo].[prc_cliente_parametro_lst] (@id_cliente INT = null)
AS
  SELECT     a.id_cliente_parametro
  ,          a.id_cliente
  ,          a.id_grupo
  ,          a.id_parametro
  ,          b.ds_parametro as dscr_parametro
  ,          a.vl_parametro
  ,          a.dt_validade
  ,          b.id_bolsa
  ,          a.st_ativo
  ,          c.ds_grupo as dscr_grupo
  FROM       tb_cliente_parametro a
  INNER JOIN tb_parametro_risco b ON a.id_parametro = b.id_parametro
  LEFT  JOIN tb_grupo c ON a.id_grupo = c.id_grupo
  WHERE      id_cliente = isnull(@id_cliente, id_cliente)
  AND        a.st_ativo = 'S'