
ALTER TRIGGER [dbo].[TG_AlterarBancoPrincipal]  
   ON [dbo].[tb_cliente_banco]
   FOR INSERT,UPDATE
AS 
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: <Create Date,,>
-- Description:	Manter apenas um registro principal para cada cliente
-- =============================================
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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




Create TRIGGER [dbo].[TG_AlterarTelefonePrincipal]  
   ON [dbo].[tb_cliente_telefone]
   FOR INSERT,UPDATE
AS 
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 19/05/2010
-- Description:	Manter apenas um registro principal para cada cliente
-- =============================================
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	Declare @id_cliente int
	Declare @id_telefone int
	Declare @st_principal bit
	
	Select @id_cliente = id_cliente,
		@id_telefone = id_telefone,
		@st_principal = st_principal
	from inserted

      IF @st_principal = 1
		Begin
            update tb_cliente_telefone
            set st_principal = 0 
            where id_cliente = @id_cliente and id_telefone <> @id_telefone
		End 
END


--------------------------------------------------------------


Create TRIGGER [dbo].[TG_AlterarEnderecoPrincipal]  
   ON [dbo].[tb_cliente_endereco]
   FOR INSERT,UPDATE
AS 
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 19/05/2010
-- Description:	Manter apenas um registro principal para cada cliente
-- =============================================
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	Declare @id_cliente int
	Declare @id_endereco int
	Declare @st_principal bit
	
	Select @id_cliente = id_cliente,
		@id_endereco = id_endereco,
		@st_principal = st_principal
	from inserted

      IF @st_principal = 1
		Begin
            update tb_cliente_endereco
            set st_principal = 0 
            where id_cliente = @id_cliente and id_endereco <> @id_endereco
		End 
END


------------------------------------------------------



Create TRIGGER [dbo].[TG_AlterarCodigoPrincipal]  
   ON [dbo].[tb_cliente_conta]
   FOR INSERT,UPDATE
AS 
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 19/05/2010
-- Description:	Manter apenas um registro principal para cada cliente
-- =============================================
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON
	Declare @id_cliente int
	Declare @id_cliente_conta int
	Declare @st_principal bit
	
	Select @id_cliente = id_cliente,
		@id_cliente_conta = id_cliente_conta,
		@st_principal = st_principal
	from inserted

      IF @st_principal = 1
		Begin
            update tb_cliente_conta
            set st_principal = 0 
            where id_cliente = @id_cliente and id_cliente_conta <> @id_cliente_conta
		End 
END