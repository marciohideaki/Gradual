set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Insere registro na tabela tb_alteracao.
--Autor: Gustavo Malta Guimarães
--Data de criação: 12/07/2010

--Descrição: Adicionando Campos id_login_solicitante e id_login.
--Autor: Gustavo Malta Guimarães
--Data: 06/10/2010

ALTER PROCEDURE [dbo].[alteracao_ins_sp]
	@id_cliente	int,
	@id_login_solicitante int,
	@id_login_resolucao int,
	@cd_tipo char(1),
	@ds_informacao	varchar(30),
	@ds_descricao varchar(4000),
	@id_alteracao int OUTPUT
AS
	if (@id_login_solicitante = 0 )
	Begin
		set @id_login_solicitante = null;
	End
	if (@id_login_resolucao = 0 )
	Begin
		set @id_login_resolucao = null;
	End
INSERT tb_alteracao
(
	id_cliente	,
	cd_tipo,
	ds_informacao	,
	ds_descricao,
	dt_solicitacao,
	id_login_solicitante,
	id_login
)
VALUES
(
	@id_cliente	,
	@cd_tipo,
	@ds_informacao	,
	@ds_descricao,
	getdate(),
	@id_login_solicitante,
	@id_login_resolucao
);
-- Get the IDENTITY value for the row just inserted.
SELECT @id_alteracao=SCOPE_IDENTITY()




