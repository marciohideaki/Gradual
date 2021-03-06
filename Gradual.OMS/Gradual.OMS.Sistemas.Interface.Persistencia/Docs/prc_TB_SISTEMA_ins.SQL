set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[prc_TB_SISTEMA_ins]
	@NM_SISTEMA varchar(150),
	@ID_SISTEMA int OUTPUT
AS
/*
DESCRIÇÃO:
	Insere registro na tabela TB_SISTEMA.
CRIAÇÃO:
	Desenvolvedor: Alex Kubo
	Data: 04/05/2010
*/
SET NOCOUNT ON

INSERT [dbo].[TB_SISTEMA]
(
	[NM_SISTEMA]
)
VALUES
(
	@NM_SISTEMA
)
-- Get the IDENTITY value for the row just inserted.
SELECT @ID_SISTEMA=SCOPE_IDENTITY()

