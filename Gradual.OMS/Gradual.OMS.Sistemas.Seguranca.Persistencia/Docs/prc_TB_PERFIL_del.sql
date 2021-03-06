set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_PERFIL_del]
	@ID_PERFIL int = null
	@CD_PERFIL VARCHAR(200) = null 
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela TB_PERFIL.
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 04/05/2010
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
if(@ID_PERFIL is null)
	SELECT @ID_PERFIL = ID_PERFIL FROM TB_PERFIL WHERE CD_PERFIL = @CD_PERFIL

DELETE FROM [dbo].[TB_PERFIL]
WHERE
	[ID_PERFIL] = @ID_PERFIL

