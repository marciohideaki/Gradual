CREATE PROCEDURE cliente_contrato_del_sp
	@id_cliente bigint,
	@id_contrato bigint
AS
--Descrição: Exclui registro da tabela tb_cliente_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
DELETE FROM tb_cliente_contrato
WHERE
	id_cliente = @id_cliente
	AND id_contrato = @id_contrato
GO
