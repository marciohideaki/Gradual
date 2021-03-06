
CREATE TRIGGER [dbo].[TG_AlterarEmitentePrincipal]  
   ON [dbo].[tb_cliente_emitente]
   FOR INSERT,UPDATE
AS 
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 01/06/2010
-- Description:	Manter apenas um registro principal para sistema para cada cliente
-- =============================================
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	Declare @id_cliente int
	Declare @id_pessoaautorizada int
	Declare @st_principal bit
	Declare @cd_sistema varchar(4)
	
	Select @id_cliente = id_cliente,
		@id_pessoaautorizada = id_pessoaautorizada,
		@st_principal = st_principal,
		@cd_sistema = cd_sistema
	from inserted

      IF @st_principal = 1
		Begin
            update tb_cliente_emitente
            set st_principal = 0 
            where id_cliente = @id_cliente 
			and cd_sistema = @cd_sistema
			and id_pessoaautorizada <> @id_pessoaautorizada
		End 
END

