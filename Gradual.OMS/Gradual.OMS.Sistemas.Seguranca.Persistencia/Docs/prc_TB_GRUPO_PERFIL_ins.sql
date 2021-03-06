set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_GRUPO_PERFIL_ins]
	@ID_GRUPO smallint,
	@ID_PERFIL int = null,
	@cd_perfil VARCHAR(200) = null
AS
/*
DESCRIÇÃO:
	Insere registro na tabela TB_GRUPO_PERFIL.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON
IF(@ID_PERFIL IS NULL) 
	Select @ID_PERFIL = ID_PERFIL from tb_perfil where cd_perfil = @id_perfil

INSERT [dbo].[TB_GRUPO_PERFIL]
(
	[ID_GRUPO],
	[ID_PERFIL]
)
VALUES
(
	@ID_GRUPO,
	@ID_PERFIL
)

