--Descrição: Insere registro na tabela tb_cliente_emitente.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010
CREATE PROCEDURE cliente_emitente_ins_sp
	@id_cliente bigint,
	@ds_nome varchar(30),
	@ds_cpfcnpj numeric(15, 0),
	@dt_nascimento datetime,
	@ds_numerodocumento varchar(16),
	@cd_sistema varchar(4),
	@st_principal bit,
	@ds_email varchar(80),
	@ds_data datetime,
	@dt_nascimento datetime,
	@id_pessoaautorizada bigint OUTPUT
AS
INSERT tb_cliente_emitente
(
	[id_cliente],
	[ds_nome],
	[ds_cpfcnpj],
	[dt_nascimento],
	[ds_numerodocumento],
	[cd_sistema],
	[st_principal],
	[ds_email],
	[ds_data],
	[dt_nascimento]
)
VALUES
(
	@id_cliente,
	@ds_nome,
	@ds_cpfcnpj,
	@dt_nascimento,
	@ds_numerodocumento,
	@cd_sistema,
	@st_principal,
	@ds_email,
	@ds_data,
	@dt_nascimento
)

SELECT @id_pessoaautorizada=SCOPE_IDENTITY()