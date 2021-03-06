set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_USUARIO_GRUPO_lst]
	@ID_GRUPO INT=null,
	@CD_GRUPO VARCHAR(200) = NULL
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela TB_USUARIO_GRUPO de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON

IF( @ID_GRUPO IS NULL) 
	SELECT @ID_GRUPO= ID_GRUPO FROM TB_GRUPO WHERE CD_GRUPO = @CD_GRUPO

SELECT
	A.ID_USUARIO,
	A.ID_GRUPO,
	B.DS_GRUPO
FROM 
	[dbo].[TB_USUARIO_GRUPO] A
	INNER JOIN [dbo].[TB_GRUPO] B ON A.id_grupo = b.id_grupo
WHERE 
	A.ID_GRUPO = ISNULL(@ID_GRUPO,A.ID_GRUPO)


