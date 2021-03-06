ALTER PROC [dbo].[arquivo_contrato_lst_sp]
( @id_arquivocontrato INT = NULL
, @id_contrato        INT = NULL)
AS
BEGIN
  SELECT [aqc].[id_arquivocontrato]
  ,      [aqc].[id_contrato]
  ,      ''
  ,      [aqc].[extensao]
  ,      [aqc].[mime_type]
  ,      [aqc].[tamanho]
  ,      [aqc].[nome]
  FROM   [dbo].[tb_arquivo_contrato] AS [aqc]
  WHERE  [aqc].[id_arquivocontrato] = ISNULL(@id_arquivocontrato, [aqc].[id_arquivocontrato])
  AND    [aqc].[id_contrato] = ISNULL(@id_contrato, [aqc].[id_contrato])
END