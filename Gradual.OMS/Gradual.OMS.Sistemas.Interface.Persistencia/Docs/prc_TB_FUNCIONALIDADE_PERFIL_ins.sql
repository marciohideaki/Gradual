set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_FUNCIONALIDADE_PERFIL_ins]
	@ID_FUNCIONALIDADE int,
	@ID_PERFIL int
AS
/*
DESCRIÇÃO:
	Insere registro na tabela TB_FUNCIONALIDADE_PERFIL.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON
INSERT [dbo].[TB_FUNCIONALIDADE_PERFIL]
(
	[ID_FUNCIONALIDADE],
	[ID_PERFIL]
)
VALUES
(
	@ID_FUNCIONALIDADE,
	@ID_PERFIL
)

