--Descri��o: Efetua a migra��o de TODOS os clientes entre assessores
--Autor: Bruno Varandas Ribeiro
--Data de cria��o: 05/05/2010
CREATE PROCEDURE cliente_assessor_migracao_todos_sp
	@id_assessor_origem bigint,
	@id_assessor_destino bigint
AS
UPDATE 
	tb_cliente
SET 
	id_assessorinicial = @id_assessor_destino
WHERE
	id_assessorinicial = @id_assessor_origem
	and st_passo < 4
	
	
UPDATE 
	tb_cliente_conta 
SET 
	tb_cliente_conta.cd_assessor = @id_assessor_destino
FROM 
	tb_cliente as cliente
WHERE
	tb_cliente_conta.id_cliente = cliente.id_cliente
	and tb_cliente_conta.cd_assessor = @id_assessor_origem
	and cliente.st_passo = 4
	
	
	
	