CREATE PROCEDURE contrato_ins_sp
	@ds_contrato varchar(150),
	@ds_path varchar(500),
	@st_obrigatorio bit,
	@id_contrato bigint OUTPUT
AS
--Descrição: Inclui registro na tabela tb_contrato.
--Autor: Gustavo Malta Guimarães
--Data de criação: 05/05/2010
SET NOCOUNT ON
INSERT tb_contrato
(
	ds_contrato,
	ds_path,
	st_obrigatorio
)
VALUES
(
	@ds_contrato,
	@ds_path,
	@st_obrigatorio
)
-- Get the IDENTITY value for the row just inserted.
SELECT @id_contrato=SCOPE_IDENTITY()
GO