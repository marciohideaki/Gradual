set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- Descrição:       Realiza a seleção dos dados de cliente com base no cpf
-- Autor:           Gustavo Malta Guimarães
-- Data de criação: 05/11/2010

-- Descrição:       Inclusão do filtro no campo tipopessoa
-- Autor:           Bruno Varandas
-- Data de criação: 30/11/2010            

ALTER PROCEDURE [dbo].[cliente_cpfcnpj_lst_sp]
                 @ds_cpfcnpj  varchar(max),
				 @TipoPessoa  varchar(1)
AS
    SELECT    [cli].[id_cliente]
	,         [cli].[ds_nome]
    ,         [cli].[ds_cpfcnpj]
    ,         [cli].[dt_passo1]
    ,         [cli].[tp_pessoa]
    ,         [cli].[tp_cliente]
    ,         [con].[cd_codigo]
    ,         ISNULL([con].[cd_assessor], [cli].[id_assessorinicial]) AS [cd_assessor]
    ,         [tel].[ds_ddd]              AS [ds_telefone_ddd]
    ,         [tel].[ds_numero]           AS [ds_telefone_numero]
    FROM      [dbo].[tb_cliente]          AS [cli]
    LEFT JOIN [dbo].[tb_cliente_telefone] AS [tel] ON [tel].[id_cliente] = [cli].[id_cliente] AND [tel].[st_principal] = 1
    LEFT JOIN [dbo].[tb_cliente_conta]    AS [con] ON [con].[id_cliente] = [cli].[id_cliente] AND UPPER([con].[cd_sistema]) = 'BOL' and [con].[st_principal] = 1
    WHERE  CHARINDEX (convert(varchar, convert( bigint , [cli].[ds_cpfcnpj] )), @ds_cpfcnpj ) > 0 
		   AND ((@TipoPessoa = '') or (cli.tp_pessoa = @TipoPessoa))
--select convert( bigint , [ds_cpfcnpj] ) from tb_cliente


