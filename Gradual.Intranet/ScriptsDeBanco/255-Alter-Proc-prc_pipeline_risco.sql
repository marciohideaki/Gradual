ALTER proc [dbo].[prc_pipeline_risco] (@id_acao INT)
AS
  SELECT    b.id_acao
  ,         ds_acao
  ,         CASE WHEN c.id_permissao IS NULL THEN 'PARAMETRO' ELSE 'PERMISSAO' END AS Especie
  ,         c.id_permissao
  ,         ds_permissao
--  ,         ISNULL(c.url_namespace, d.url_namespace) AS url_namespace
--  ,         ISNULL(c.nm_metodo, d.nm_metodo)         AS nome_metodo
  ,         '' AS url_namespace
  ,         ''         AS nome_metodo
  ,         d.id_parametro
  ,         ds_parametro
  ,         ordem_disparo   		
  FROM      tb_pipeline_acao   a
  JOIN      tb_acao            b ON (a.id_acao = b.id_acao)		
  LEFT JOIN tb_permissao       c ON (a.id_permissao = c.id_permissao)
  LEFT JOIN tb_parametro_risco d ON (a.id_parametro = d.id_parametro)
  WHERE     b.id_acao = @id_acao
  AND       st_ativo  = 'S'
  ORDER BY	ordem_disparo  ASC