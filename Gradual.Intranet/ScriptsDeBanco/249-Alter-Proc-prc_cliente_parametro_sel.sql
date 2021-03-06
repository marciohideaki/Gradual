ALTER PROCEDURE [dbo].[prc_cliente_parametro_sel] (@id_cliente_parametro INT)
AS
SELECT     a.id_cliente_parametro
,          a.id_cliente
,          a.id_grupo
,          a.id_parametro
,          b.ds_parametro AS dscr_parametro
,          a.vl_parametro
,          a.dt_validade
,          c.ds_grupo     AS dscr_grupo
,          b.id_bolsa
FROM       tb_cliente_parametro a
INNER JOIN tb_parametro_risco   b ON a.id_parametro = b.id_parametro
LEFT  JOIN tb_grupo             c ON a.id_grupo = c.id_grupo
WHERE      id_cliente_parametro = @id_cliente_parametro