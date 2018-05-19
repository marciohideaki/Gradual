set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_SISTEMA_del]
	@ID_SISTEMA int
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela TB_SISTEMA.
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 04/05/2010
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM [dbo].[TB_SISTEMA]
WHERE
	[ID_SISTEMA] = @ID_SISTEMA

