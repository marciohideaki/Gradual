--Descrição: Lista os dados da tabela tb_cliente_banco de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 05/05/2010
CREATE PROCEDURE cliente_assessor_lst_sp
	@id_assessor bigint
AS
SELECT 
	* 
FROM 
	tb_cliente 
WHERE 
	[id_assessorinicial] = @id_assessor
UNION
SELECT 
	cliente.* 
from 
	tb_cliente as cliente, 
	tb_cliente_conta as conta 
WHERE 
	cliente.st_passo = 4
	AND cliente.id_cliente = conta.id_cliente 
	AND conta.cd_assessor = @id_assessor
