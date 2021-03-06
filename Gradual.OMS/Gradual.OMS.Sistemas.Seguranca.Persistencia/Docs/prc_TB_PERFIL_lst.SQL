set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_PERFIL_lst]
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela TB_PERFIL de acordo com o filtro especificado.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON
SELECT
	[ID_PERFIL],
	[CD_PERFIL],
	[NM_PERFIL]
FROM [dbo].[TB_PERFIL]
ORDER BY 
	[ID_PERFIL] ASC

