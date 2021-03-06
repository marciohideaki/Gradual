
--Descrição: Insere registro na tabela tb_alteracao.
--Autor: Gustavo Malta Guimarães
--Data de criação: 12/07/2010
CREATE PROCEDURE [dbo].[alteracao_ins_sp]
	@id_cliente	int,
	@cd_tipo char(1),
	@ds_informacao	varchar(30),
	@ds_descricao varchar(4000),
	@id_alteracao int OUTPUT
AS
INSERT tb_alteracao
(
	id_cliente	,
	cd_tipo,
	ds_informacao	,
	ds_descricao,
	dt_solicitacao
)
VALUES
(
	@id_cliente	,
	@cd_tipo,
	@ds_informacao	,
	@ds_descricao,
	getdate()
);
-- Get the IDENTITY value for the row just inserted.
SELECT @id_alteracao=SCOPE_IDENTITY()



