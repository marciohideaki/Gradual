ALTER PROC [dbo].[arquivo_contrato_sel_sp] (@id_arquivocontrato INT = NULL)
AS
BEGIN
  SELECT [aqc].[id_arquivocontrato]
  ,      [aqc].[id_contrato]
  ,      [aqc].[arquivo]
  ,      [aqc].[extensao]
  ,      [aqc].[mime_type]
  ,      [aqc].[tamanho]
  ,      [aqc].[nome]
  FROM   [dbo].[tb_arquivo_contrato] AS [aqc]
  WHERE  [aqc].[id_arquivocontrato] = ISNULL(@id_arquivocontrato, [aqc].[id_arquivocontrato])
END