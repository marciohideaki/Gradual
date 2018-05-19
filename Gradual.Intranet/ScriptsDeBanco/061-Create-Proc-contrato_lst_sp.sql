CREATE PROCEDURE contrato_lst_sp
AS
--Descrição: Lista registros da tabela tb_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
SELECT
	id_contrato,
	ds_contrato,
	ds_path,
	st_obrigatorio
FROM tb_contrato
ORDER BY 
	id_contrato ASC
GO
