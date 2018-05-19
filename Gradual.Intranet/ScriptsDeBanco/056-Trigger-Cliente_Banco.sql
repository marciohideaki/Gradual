set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 04/05/2010
-- Description:	Trigger para manter apenas uma Conta Bancária Principal por Cliente
-- =============================================
ALTER TRIGGER [dbo].[TG_AlterarBancoPrincipal]  
   ON [dbo].[tb_cliente_banco]
   FOR INSERT,UPDATE
AS 
BEGIN
	SET NOCOUNT ON
	Declare @id_cliente int
	Declare @id_banco int
	Declare @st_principal bit
	
	Select @id_cliente = id_cliente,
		@id_banco = id_banco,
		@st_principal = st_principal
	from inserted

      IF @st_principal = 1
		Begin
            update tb_cliente_banco
            set st_principal = 0 
            where id_cliente = @id_cliente and id_banco <> @id_banco
		End
END

