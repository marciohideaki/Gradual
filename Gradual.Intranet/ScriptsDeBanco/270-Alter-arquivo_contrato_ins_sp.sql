ALTER PROCEDURE [dbo].[arquivo_contrato_ins_sp]
(
	  @id_arquivocontrato int output,
      @id_contrato int,
      @arquivo image,
      @extensao nvarchar(10),
      @mime_type nvarchar(256),
      @tamanho int,
	  @nome nvarchar(256)
)
AS 
BEGIN

INSERT INTO [dbo].[tb_arquivo_contrato]
           ([id_contrato]
           ,[arquivo]
           ,[extensao]
           ,[mime_type]
           ,[tamanho]
		   ,[nome] )
     VALUES
           (@id_contrato
           ,@arquivo
           ,@extensao
           ,@mime_type
           ,@tamanho
		   ,@nome)

SELECT @id_arquivocontrato = SCOPE_IDENTITY()

END