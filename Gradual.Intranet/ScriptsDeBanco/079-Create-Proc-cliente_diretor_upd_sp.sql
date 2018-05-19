--Descrição: Atualiza registro(s) na tabela tb_cliente_diretor.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_diretor_upd_sp
	@id_cliente_diretor bigint,
	@id_cliente bigint,
	@ds_identidade varchar(16),
	@ds_cpfcnpj numeric(15, 0),
	@ds_nome varchar(30)
AS
UPDATE tb_cliente_diretor
SET 
	[id_cliente] = @id_cliente,
	[ds_identidade] = @ds_identidade,
	[ds_cpfcnpj] = @ds_cpfcnpj,
	[ds_nome] = @ds_nome
WHERE
	[id_cliente_diretor] = @id_cliente_diretor