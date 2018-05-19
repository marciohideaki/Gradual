set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_FUNCIONALIDADE_PERFIL_del]
	@ID_FUNCIONALIDADE int
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela TB_FUNCIONALIDADE_PERFIL.
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 04/05/2010
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM [dbo].[TB_FUNCIONALIDADE_PERFIL]
WHERE
	[ID_FUNCIONALIDADE] = @ID_FUNCIONALIDADE

