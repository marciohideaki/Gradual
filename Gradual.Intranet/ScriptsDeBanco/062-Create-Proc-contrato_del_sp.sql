CREATE PROCEDURE contrato_del_sp
	@id_contrato bigint
AS
--Descri��o: Exclui registro da tabela tb_contrato.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 05/05/2010
SET NOCOUNT ON
DELETE FROM tb_contrato
WHERE
	id_contrato = @id_contrato
GO