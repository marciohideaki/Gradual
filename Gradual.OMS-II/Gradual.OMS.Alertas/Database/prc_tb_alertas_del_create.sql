USE [OMS]
GO
/****** Object:  StoredProcedure [dbo].[prc_TB_ALERTAS_del]    Script Date: 08/23/2011 10:52:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[prc_TB_ALERTAS_del]
	@id_alerta varchar(20)
AS
/*
DESCRIÇÃO:
	Exclui registro da tabela TB_ALERTAS, conforme IdAlerta informado.
CRIAÇÃO:
	Desenvolvedor: Equipe Sistemas - Gradual
	Data: 21/06/2011
*/
SET NOCOUNT ON
-- DELETE an existing row from the table.
DELETE FROM [dbo].[TB_ALERTAS]
WHERE
	[IdAlerta] = @id_alerta

