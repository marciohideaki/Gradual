set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_GRUPO_PERFIL_del]
	@ID_GRUPO smallint
AS
/*
DESCRIÇÃO:
	Exclui todos os perfis existentes para um determinado grupo 
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 04/05/2010
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM [dbo].[TB_GRUPO_PERFIL]
WHERE
	[ID_GRUPO] = @ID_GRUPO
