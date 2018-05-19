--Descrição: Atualiza registro(s) na tabela tb_cliente_emitente.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 28/04/2010
CREATE PROCEDURE cliente_emitente_upd_sp
	@id_pessoaautorizada bigint,
	@id_cliente bigint,
	@ds_nome varchar(30),
	@ds_cpfcnpj numeric(15, 0),
	@dt_nascimento datetime,
	@ds_numerodocumento varchar(16),
	@cd_sistema varchar(4),
	@st_principal bit,
	@ds_email varchar(80),
	@ds_data datetime,
	@dt_nascimento datetime
AS
UPDATE tb_cliente_emitente
SET 
	[id_cliente] = @id_cliente,
	[ds_nome] = @ds_nome,
	[ds_cpfcnpj] = @ds_cpfcnpj,
	[dt_nascimento] = @dt_nascimento,
	[ds_numerodocumento] = @ds_numerodocumento,
	[cd_sistema] = @cd_sistema,
	[st_principal] = @st_principal,
	[ds_email] = @ds_email,
	[ds_data] = @ds_data,
	[dt_nascimento] = @dt_nascimento
WHERE
	[id_pessoaautorizada] = @id_pessoaautorizada