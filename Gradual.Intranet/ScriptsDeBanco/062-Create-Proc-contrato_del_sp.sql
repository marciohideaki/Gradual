CREATE PROCEDURE contrato_del_sp
	@id_contrato bigint
AS
--Descrição: Exclui registro da tabela tb_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
DELETE FROM tb_contrato
WHERE
	id_contrato = @id_contrato
GO