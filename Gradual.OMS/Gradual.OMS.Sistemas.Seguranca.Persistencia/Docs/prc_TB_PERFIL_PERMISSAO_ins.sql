set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_PERFIL_PERMISSAO_ins]
	@ID_PERFIL int = null,
	@CD_PERFIL varchar(200) = null,
	@ID_PERMISSAO int = null,
	@CD_PERMISSAO varchar(200) = null,
	@LI_PERMISSAO_STATUS smallint = null,
	@MN_PERMISSAO_STATUS varchar(150) = null
AS
/*
DESCRIÇÃO:
	Insere registro na tabela TB_PERFIL_PERMISSAO.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON
IF(@ID_PERFIL IS NULL)
	select @ID_PERFIL =ID_PERFIL from tb_perfil where cd_perfil = @cd_perfil

IF(@ID_PERMISSAO IS NULL)
	select @ID_PERMISSAO = ID_PERMISSAO from tb_permissao where cd_permissao = @cd_permissao

IF @LI_PERMISSAO_STATUS IS NULL
	SELECT @LI_PERMISSAO_STATUS = dbo.fnListaItem('PermissaoAssociadaStatusEnum', @MN_PERMISSAO_STATUS)

INSERT [dbo].[TB_PERFIL_PERMISSAO]
(
	[ID_PERFIL],
	[ID_PERMISSAO],
	[LI_PERMISSAO_STATUS]
)
VALUES
(
	@ID_PERFIL,
	@ID_PERMISSAO,
	@LI_PERMISSAO_STATUS
)

