--Descrição: Atualiza registro(s) na tabela tb_pessoa_vinculada.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Alteração nome do parametro de @ds_cpf para @ds_cpfcnpj

CREATE PROCEDURE pessoa_vinculada_upd_sp
	@id_pessoa_vinculada			bigint,
	@ds_nome						varchar(60),
	@st_ativo						bit,
	@ds_cpfcnpj						numeric(15, 0),
	@id_pessoavinculadaresponsavel	bigint,
	@id_cliente						bigint
AS
UPDATE tb_pessoa_vinculada
SET 
	[ds_nome]						= @ds_nome,
	[st_ativo]						= @st_ativo,
	[ds_cpfcnpj]					= @ds_cpfcnpj,
	[id_pessoavinculadaresponsavel] = @id_pessoavinculadaresponsavel,
	[id_cliente]					= @id_cliente
WHERE
	[id_pessoa_vinculada] = @id_pessoa_vinculada