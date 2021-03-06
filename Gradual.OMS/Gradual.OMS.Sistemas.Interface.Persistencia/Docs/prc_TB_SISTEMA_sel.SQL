set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


CREATE PROCEDURE [dbo].[prc_TB_SISTEMA_sel]
	@ID_SISTEMA	INT = NULL
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela TB_SISTEMA de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON
SELECT
	[ID_SISTEMA],
	[NM_SISTEMA]
FROM [dbo].[TB_SISTEMA]
WHERE
	ID_SISTEMA = ISNULL(@ID_SISTEMA,ID_SISTEMA)


