ALTER PROCEDURE [dbo].[prc_limites_cliente_sel] (@id_cliente INT)
AS
BEGIN
  SELECT   a.id_cliente
  ,        a.id_cliente_parametro
  ,        a.id_parametro
  ,        a.vl_parametro as valor_total
  ,        c.ds_parametro
  ,        a.dt_movimento
  ,       (ISNULL(vl_parametro,0) - ISNULL(vl_alocado,0)) as valor
  FROM     tb_cliente_parametro       a
  INNER JOIN tb_parametro_risco       c on a.id_parametro = c.id_parametro
  WHERE      id_cliente = @id_cliente
  AND        a.id_parametro in (12,13)
  AND        st_ativo = 'S'
END