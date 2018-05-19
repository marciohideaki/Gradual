--Descrição: Efetua a migração de um cliente específico entre assessores
--Autor: Bruno Varandas Ribeiro
--Data de criação: 05/05/2010
CREATE PROCEDURE cliente_assessor_migracao_sp
	@id_assessor_origem bigint,
	@id_assessor_destino bigint,
	@id_cliente bigint
AS
UPDATE 
	tb_cliente
SET 
	id_assessorinicial = @id_assessor_destino
WHERE
	id_assessorinicial = @id_assessor_origem
	and id_cliente     = @id_cliente
	and st_passo       < 4
	
	
UPDATE 
	tb_cliente_conta 
SET 
	tb_cliente_conta.cd_assessor     = @id_assessor_destino
FROM 
	tb_cliente as cliente
WHERE
	tb_cliente_conta.id_cliente      = cliente.id_cliente
	and tb_cliente_conta.cd_assessor = @id_assessor_origem
	and cliente.id_cliente           = @id_cliente
	and cliente.st_passo             = 4

