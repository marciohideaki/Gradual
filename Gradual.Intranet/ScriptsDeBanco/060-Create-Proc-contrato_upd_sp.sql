CREATE PROCEDURE contrato_upd_sp
	@id_contrato bigint,
	@ds_contrato varchar(150),
	@ds_path varchar(500),
	@st_obrigatorio bit
AS
--Descrição: Altera registro da tabela tb_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
UPDATE tb_contrato
SET 
	ds_contrato = @ds_contrato,
	ds_path = @ds_path,
	st_obrigatorio = @st_obrigatorio
WHERE
	id_contrato = @id_contrato
GO