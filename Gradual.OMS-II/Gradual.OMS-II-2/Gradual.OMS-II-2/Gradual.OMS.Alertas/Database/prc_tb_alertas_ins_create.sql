USE [OMS]
GO
/****** Object:  StoredProcedure [dbo].[prc_TB_ALERTAS_ins]    Script Date: 08/23/2011 10:51:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[prc_TB_ALERTAS_ins]
	@IdAlerta char(20),
	@IdCliente int,
	@Instrumento varchar(50),
	@TipoOperando int,
	@TipoOperador int,
	@Valor decimal(23,9),
	@Atingido char(1) = null,
	@Exibido char(1) = null ,
	@DataCadastro datetime = null,
	@DataAtingimento datetime = null
AS
/*
DESCRIÇÃO:
	Insere registro na tabela TB_ALERTAS.
CRIAÇÃO:
	Desenvolvedor: Equipe de Sistemas Gradual
	Data: 23/11/2009
*/
SET NOCOUNT ON
INSERT [dbo].[TB_ALERTAS]
(
	[IdAlerta],
	[IdCliente],
	[Instrumento],
	[TipoOperando],
	[TipoOperador],
	[Valor],
	[Atingido],
	[Exibido],
	[DataCadastro],
	[DataAtingimento]
)
VALUES
(
	@IdAlerta,
	@IdCliente,
	@Instrumento,
	@TipoOperando,
	@TipoOperador,
	@Valor,
	@Atingido,
	@Exibido,
	@DataCadastro,
	@DataAtingimento
)





