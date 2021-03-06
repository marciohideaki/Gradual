/*
* Autor: André Cristino Miguel
* Data: 02/06/2010
*/
ALTER PROCEDURE [dbo].[arquivo_contrato_upd_sp]
( @id_arquivocontrato INT
, @id_contrato        INT
, @arquivo            IMAGE
, @extensao           NVARCHAR(10)
, @mime_type          NVARCHAR(256)
, @tamanho            INT
, @nome               NVARCHAR(256))
AS
BEGIN
    UPDATE [dbo].[tb_arquivo_contrato]
    SET    [id_contrato]        = @id_contrato
    ,      [arquivo]            = @arquivo
    ,      [extensao]           = @extensao
    ,      [mime_type]          = @mime_type
    ,      [tamanho]            = @tamanho
    ,      [nome]               = @nome
    WHERE  [id_arquivocontrato] = @id_arquivocontrato
END