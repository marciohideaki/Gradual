--Descrição: Insere registro na tabela tb_pessoa_vinculada.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Alteração nome do parametro de @ds_cpf para @ds_cpfcnpj

CREATE PROCEDURE pessoa_vinculada_ins_sp
	@ds_nome varchar(60),
	@st_ativo bit,
	@ds_cpfcnpj numeric(15, 0),
	@id_pessoavinculadaresponsavel bigint,
	@id_cliente bigint,
	@id_pessoa_vinculada bigint OUTPUT
AS
INSERT tb_pessoa_vinculada
(
	[ds_nome],
	[st_ativo],
	[ds_cpfcnpj],
	[id_pessoavinculadaresponsavel],
	[id_cliente]
)
VALUES
(
	@ds_nome,
	@st_ativo,
	@ds_cpfcnpj,
	@id_pessoavinculadaresponsavel,
	@id_cliente
)

SELECT @id_pessoa_vinculada = SCOPE_IDENTITY()