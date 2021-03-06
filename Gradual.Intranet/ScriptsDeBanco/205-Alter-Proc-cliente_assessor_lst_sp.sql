set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


--Descrição: Lista os dados da tabela tb_cliente_banco de acordo com o filtro especificado.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 05/05/2010

--Descrição: A proc foi desenvolvida novamente, pois estava demorando 3:40min para o assessor 22 e foi para menos de 1seg.
--Autor: Gustavo Malta Guimarães
--Data de criação: 25/10/2010

ALTER PROCEDURE [dbo].[cliente_assessor_lst_sp]
	@id_assessor int
AS

SELECT  
	@id_assessor  AS cd_assessor,
	dt_nascimentofundacao,
    dt_passo1,
    1 as st_status,
    tp_pessoa,
    ds_cpfcnpj,
    cd_sexo,
    id_cliente,
    st_passo,
    ds_nome,
    ds_cpfcnpj,
	1 as st_status -- TODO ver
from tb_cliente
	where st_passo < 4
	and id_assessorinicial = @id_assessor
union
SELECT  
	@id_assessor  AS cd_assessor,
	dt_nascimentofundacao,
    dt_passo1,
    tb_cliente_conta.st_ativa as st_status,
    tp_pessoa,
    ds_cpfcnpj,
    cd_sexo,
    tb_cliente.id_cliente,
    st_passo,
    ds_nome,
    ds_cpfcnpj,
	1 as st_status -- TODO ver
from tb_cliente
	inner join tb_cliente_conta on (tb_cliente.id_cliente = tb_cliente_conta.id_cliente and tb_cliente_conta.st_principal = 1 and tb_cliente_conta.cd_assessor = @id_assessor)
	where st_passo = 4


--SELECT 
--	  [tb_cliente].* 
--	, ISNULL(dbo.status_cliente_ativo_fn([tb_cliente].[id_cliente]), 0) AS [st_status]
--    , @id_assessor                                                      AS [cd_assessor]
--FROM 
--	[tb_cliente] 
--WHERE 
--	[id_assessorinicial] = @id_assessor
--UNION
--SELECT 
--	  [cliente].* 
--	, ISNULL(dbo.status_cliente_ativo_fn([cliente].[id_cliente]), 0) AS [st_status]
--    , @id_assessor                                                   AS [cd_assessor]
--FROM 
--	tb_cliente as cliente, 
--	tb_cliente_conta as conta 
--WHERE 
--	cliente.st_passo = 4
--	AND cliente.id_cliente = conta.id_cliente 
--	AND conta.cd_assessor = @id_assessor

