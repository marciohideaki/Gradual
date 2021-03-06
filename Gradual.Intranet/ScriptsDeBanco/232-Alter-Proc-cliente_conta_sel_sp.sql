set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Seleciona os dados da tabela tb_cliente_conta de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Distigue conta depósito de investimento.
--Autor: Gustavo Malta Guimarães
--Data de criação: 06/12/2010

ALTER PROCEDURE [dbo].[cliente_conta_sel_sp] 
	@id_cliente_conta int
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
FROM tb_cliente_conta
WHERE
	[id_cliente_conta] = @id_cliente_conta
ORDER BY 
	[id_cliente_conta] 


