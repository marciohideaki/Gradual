--Descrição: Atualiza registro(s) na tabela tb_cliente_controladora.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_controladora_upd_sp
	@id_cliente_controlada bigint,
	@id_cliente bigint,
	@ds_nomerazaosocial varchar(60),
	@ds_cpfcnpj numeric(15, 0)
AS

UPDATE tb_cliente_controladora
SET 
	[id_cliente] = @id_cliente,
	[ds_nomerazaosocial] = @ds_nomerazaosocial,
	[ds_cpfcnpj] = @ds_cpfcnpj
WHERE
	[id_cliente_controlada] = @id_cliente_controlada