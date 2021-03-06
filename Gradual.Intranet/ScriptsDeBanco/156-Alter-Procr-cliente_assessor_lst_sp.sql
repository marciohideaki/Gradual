set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


--Descrição: Lista os dados da tabela tb_cliente_banco de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 05/05/2010

ALTER PROCEDURE [dbo].[cliente_assessor_lst_sp]
	@id_assessor bigint
AS
SELECT 
	  [tb_cliente].* 
	, ISNULL(dbo.status_cliente_ativo_fn([tb_cliente].[id_cliente]), 0) AS [st_status]
    , @id_assessor                                                      AS [cd_assessor]
FROM 
	[tb_cliente] 
WHERE 
	[id_assessorinicial] = @id_assessor
UNION
SELECT 
	  [cliente].* 
	, ISNULL(dbo.status_cliente_ativo_fn([cliente].[id_cliente]), 0) AS [st_status]
    , @id_assessor                                                   AS [cd_assessor]
FROM 
	tb_cliente as cliente, 
	tb_cliente_conta as conta 
WHERE 
	cliente.st_passo = 4
	AND cliente.id_cliente = conta.id_cliente 
	AND conta.cd_assessor = @id_assessor



