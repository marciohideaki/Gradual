set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[cliente_conta_pri_sp]
	@id_cliente int
AS
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 20/05/2010
-- Description:	Pegar o registro principal de cada cliente

--Descrição: Distigue conta depósito de investimento.
--Autor: Gustavo Malta Guimarães
--Data de criação: 06/12/2010

-- =============================================



SELECT
	[id_cliente_conta],
	[id_cliente],
	[cd_assessor],
	[cd_codigo],
	[cd_sistema],
	[st_containvestimento],
	[st_ativa],
	[st_principal]
FROM 
	[tb_cliente_conta]
WHERE id_cliente = @id_cliente
	and st_principal = 1;



