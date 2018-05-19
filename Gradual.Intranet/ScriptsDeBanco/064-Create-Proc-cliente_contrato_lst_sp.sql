CREATE PROCEDURE cliente_contrato_lst_sp
	@id_cliente bigint
AS
--Descri��o: Lista registros de um cliente da tabela tb_cliente_contrato.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 05/05/2010
SET NOCOUNT ON
SELECT
	id_cliente,
	id_contrato,
	dt_assinatura
FROM tb_cliente_contrato
WHERE
	id_cliente = @id_cliente
ORDER BY 
	id_cliente ASC
	, id_contrato ASC
GO