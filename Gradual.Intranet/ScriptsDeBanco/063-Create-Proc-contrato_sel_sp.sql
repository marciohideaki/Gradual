CREATE PROCEDURE contrato_sel_sp
	@id_contrato bigint
AS
--Descri��o: Seleciona registro da tabela tb_contrato.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 05/05/2010
SET NOCOUNT ON
SELECT
	id_contrato,
	ds_contrato,
	ds_path,
	st_obrigatorio
FROM tb_contrato
where id_contrato = @id_contrato 
ORDER BY 
	id_contrato ASC
GO