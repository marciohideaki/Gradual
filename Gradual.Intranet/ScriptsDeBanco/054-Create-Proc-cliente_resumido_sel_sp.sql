CREATE PROCEDURE [dbo].[cliente_resumido_sel_sp]
       @cd_bovespa bigint
     , @ds_cpfcnpj numeric(15, 0)
     , @ds_nome    varchar(60)
AS

--Descrição: Lista os clientes com base em critérios de seleção
--Autor: Antônio Rodrigues
--Data de criação: 03/05/2010

IF '' = @ds_nome
BEGIN --> Resolvendo o problema da string vazia
    SET @ds_nome = NULL
END

SELECT    [cli].[id_cliente]
,         [cli].[ds_nome]
,         [cli].[ds_cpfcnpj]
,         [bov].[cd_codigo]        AS [cd_bovespa]
,         [cli].[dt_passo1]        AS [dt_cadastro]
,         [cli].[st_passo]
FROM      [dbo].[tb_cliente]       AS [cli]
LEFT JOIN [dbo].[tb_cliente_conta] AS [bov]
     ON   [cli].[id_cliente] = [bov].[id_cliente]
     AND  lower([bov].[cd_sistema]) = 'bol'
WHERE     lower([cli].[ds_nome]) like '%' + lower(@ds_nome) + '%'
OR        [cli].[ds_cpfcnpj] = @ds_cpfcnpj
OR        EXISTS
          (
				SELECT [cl2].[id_cliente]
				FROM   [dbo].[tb_cliente]           AS [cl2]
				INNER JOIN [dbo].[tb_cliente_conta] AS [clc]
					  ON lower([clc].[cd_sistema]) = 'bol'
					  AND [cl2].[id_cliente] = [cli].[id_cliente]
					  AND [clc].[cd_codigo]  = @cd_bovespa
          )
