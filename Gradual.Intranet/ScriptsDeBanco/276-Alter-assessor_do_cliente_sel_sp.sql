-- =============================================
-- Author:		Gustavo Malta Guimaraes
-- Create date: 22/11/2010
-- Description:	Receber os dados do assessor do cliente
-- =============================================
ALTER PROCEDURE [dbo].[assessor_do_cliente_sel_sp] 
	@id_cliente int
AS
BEGIN
	declare @st_passo int;
	declare @id_assessor int;
	
	--Pegar Passo e Assessor inicial
	select 
		@st_passo = st_passo,
		@id_assessor = id_assessorinicial
	from tb_cliente
	where id_cliente = @id_cliente;

	--pegar assessor correto se passo = 4
	if (@st_passo = 4) 
	Begin
		select @id_assessor = cd_assessor 
		from tb_cliente_conta
		where id_cliente = @id_cliente and st_principal = 1; 
	End;

	--Pegando os dados do assessor
	select cd_assessor,ds_email
	from tb_login
	where tp_acesso = 2 and cd_assessor = @id_assessor;

END