--Descrição: Insere registro na tabela tb_cliente_diretor.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 29/04/2010
CREATE PROCEDURE cliente_diretor_ins_sp
	@id_cliente bigint,
	@ds_identidade varchar(16),
	@ds_cpfcnpj numeric(15, 0),
	@ds_nome varchar(30),
	@id_cliente_diretor bigint OUTPUT
AS
INSERT tb_cliente_diretor
(
	[id_cliente],
	[ds_identidade],
	[ds_cpfcnpj],
	[ds_nome]
)
VALUES
(
	@id_cliente,
	@ds_identidade,
	@ds_cpfcnpj,
	@ds_nome
)
-- Get the IDENTITY value for the row just inserted.
SELECT @id_cliente_diretor=SCOPE_IDENTITY()