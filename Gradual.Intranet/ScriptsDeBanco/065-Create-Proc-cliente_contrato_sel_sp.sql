CREATE PROCEDURE cliente_contrato_sel_sp
	@id_cliente bigint,
	@id_contrato bigint
AS
--Descrição: Seleciona registro da tabela tb_cliente_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
SELECT
	id_cliente,
	id_contrato,
	dt_assinatura
FROM tb_cliente_contrato
WHERE
	id_cliente = @id_cliente
	and id_contrato = @id_contrato
ORDER BY 
	id_cliente ASC
	, id_contrato ASC
GO