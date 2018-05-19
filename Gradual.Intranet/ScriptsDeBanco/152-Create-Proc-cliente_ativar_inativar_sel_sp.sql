-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 23/06/2010
-- Description:	Atualiza o status do cliente - Ativo/Inativo
-- =============================================
CREATE PROCEDURE [dbo].[cliente_ativar_inativar_sel_sp]
	@id_cliente int
AS
BEGIN
	SELECT 
		id_cliente,
		ds_nome,
		st_ativo,
		dt_ativacaoinativacao
	FROM 
		tb_cliente 
	WHERE 
		id_cliente = @id_cliente
END



