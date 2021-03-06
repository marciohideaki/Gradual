set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- Descrição:       Exclui o cliente com todos os seus dados de todas as tabelas
-- Autor:           Gustavo Malta Guimarães
-- Data de criação: 14/05/2010

-- Modificação:     Excluir os contratos do cliente
-- Autor:           Gustavo Malta Guimarães
-- Data:			07/06/2010
ALTER PROCEDURE [dbo].[clientecompleto_del_sp]
                 @id_cliente  int
AS

	Begin Transaction trans;
    Declare @id_Login bigint;
	select @id_login = id_login from tb_cliente where id_cliente = @id_cliente;
	DELETE FROM tb_cliente_contrato WHERE id_cliente = @id_cliente;
	DELETE FROM tb_pessoa_vinculada WHERE id_cliente = @id_cliente;
	DELETE FROM tb_cliente_endereco WHERE id_cliente = @id_cliente;
	DELETE FROM tb_cliente_controladora WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_investidor_naoresidente WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_diretor WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_telefone WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_emitente WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_pendenciacadastral WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_banco WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_situacaofinanceirapatrimonial WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_conta WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente_procuradorrepresentante WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_cliente WHERE  id_cliente = @id_cliente;
	DELETE FROM tb_login WHERE  id_login = @id_login;

	if (@@error > 0)
	begin
		rollback tran	
	end
	else
	begin
		commit tran
	end

