USE [OMS]
GO
/****** Object:  StoredProcedure [dbo].[prc_TB_ALERTAS_lst]    Script Date: 08/23/2011 10:51:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prc_TB_ALERTAS_lst]
(
	 @id_alerta varchar(20) = null
)
AS
/*
DESCRIÇÃO:
	Seleciona os dados da tabela TB_ALERTAS de acordo com o ID especificado (caso exista).
CRIAÇÃO:
	Desenvolvedor: Equipe de Sistemas Gradual
	Data: 21/06/2011
*/

-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

SELECT
	[IdAlerta],
	[IdCliente],
	[Instrumento],
	[TipoOperando],
	[TipoOperador],
	[Valor],
	[Atingido],
	[Exibido],
	[DataCadastro],
	[DataAtingimento],
	[Cotacao]
FROM [dbo].[TB_ALERTAS]
WHERE 
	IdAlerta = isnull(@id_alerta, IdAlerta)


