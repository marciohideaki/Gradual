ALTER PROCEDURE [dbo].[prc_cliente_parametro_del] (@id_cliente_parametro INT)
AS
  DELETE FROM [dbo].[tb_cliente_parametro]
  WHERE [id_cliente_parametro] = @id_cliente_parametro