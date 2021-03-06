set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Lista os dados da tabela tb_cliente_conta de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Distigue conta depósito de investimento.
--Autor: Gustavo Malta Guimarães
--Data de criação: 06/12/2010

ALTER PROCEDURE [dbo].[cliente_conta_lst_sp]
	@id_cliente int
AS

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
	[dbo].[tb_cliente_conta]
WHERE
	[id_cliente] = @id_cliente
ORDER BY 
	[st_principal] desc,
	[st_ativa] asc,
	[cd_codigo] asc,
	[cd_sistema] asc
	




