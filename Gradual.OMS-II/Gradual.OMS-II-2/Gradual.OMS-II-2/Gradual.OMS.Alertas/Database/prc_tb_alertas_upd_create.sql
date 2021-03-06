USE [OMS]
GO
/****** Object:  StoredProcedure [dbo].[prc_TB_ALERTAS_upd]    Script Date: 08/23/2011 10:53:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prc_TB_ALERTAS_upd]
	@IdAlerta char(20),
	@Atingido char(1) = null,
	@Exibido char(1) = null ,
	@DataAtingimento datetime = null,
	@Cotacao decimal(23,9) = null
AS
/*
DESCRIÇÃO:
	Atualiza registro na tabela TB_ALERTAS.
CRIAÇÃO:
	Desenvolvedor: Equipe de Sistemas Gradual
	Data: 23/11/2009
*/
SET NOCOUNT ON
UPDATE [dbo].[TB_ALERTAS]
SET 
	[Atingido] = isnull(@Atingido, Atingido), 
	[Exibido] = isnull(@Exibido, Exibido), 
	[DataAtingimento] = isnull(@DataAtingimento, DataAtingimento),
	[Cotacao] = isnull(@Cotacao, Cotacao)
WHERE 
	[IdAlerta] = @IdAlerta





