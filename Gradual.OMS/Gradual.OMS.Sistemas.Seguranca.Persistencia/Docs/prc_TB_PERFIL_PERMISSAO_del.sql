set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_PERFIL_PERMISSAO_del]
	@ID_PERFIL int
AS
/*
DESCRIÇÃO:
	Exclui todas as permissões vinculadas a este perfil.
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 04/05/2010
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM [dbo].[TB_PERFIL_PERMISSAO]
WHERE
	[ID_PERFIL] = @ID_PERFIL