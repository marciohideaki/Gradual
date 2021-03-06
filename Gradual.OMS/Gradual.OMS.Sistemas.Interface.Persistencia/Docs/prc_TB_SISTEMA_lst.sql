set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_SISTEMA_lst]
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
ORDER BY 
	[ID_SISTEMA] ASC

