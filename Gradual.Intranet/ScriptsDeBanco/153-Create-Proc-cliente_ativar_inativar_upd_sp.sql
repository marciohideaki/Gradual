-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 23/06/2010
-- Description:	Atualiza o status do cliente - Ativo/Inativo
-- =============================================
CREATE PROCEDURE [dbo].[cliente_ativar_inativar_upd_sp]
	@id_cliente int,
	@st_ativo bit,
	@dt_ativacaoinativacao datetime
AS
BEGIN
	UPDATE 
		tb_cliente 
	SET 
		st_ativo = @st_ativo, 
		dt_ativacaoinativacao = @dt_ativacaoinativacao
	WHERE 
		id_cliente = @id_cliente
END



