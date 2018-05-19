set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =======================================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 19/05/2010
-- Description:	Retorna uma lista dos clientes suspeitos
-- ======================================================

-- =======================================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- ======================================================

ALTER PROCEDURE [dbo].[rel_cliente_suspeito_lst_sp]
	@DtDe			 datetime,
	@DtAte			 datetime,
	@CodigoBolsa	 int,
	@CodigoPais		 varchar(3),
	@CodigoAtividade varchar(3),
	@TipoPessoa      varchar(1) = ''
AS
BEGIN
    SELECT DISTINCT
               ISNULL([con].[cd_assessor], [cli].[id_assessorinicial]) AS [cd_assessor]
             , [con].[cd_codigo]
             , [cli].[id_cliente]
             , [cli].[ds_nome]
             , [cli].[ds_cpfcnpj]
             , [cli].[tp_pessoa]
             , [cli].[dt_passo1] AS [dt_cadastro]
             , CASE WHEN  [cli].[st_passo] = 4 THEN 1 ELSE 0 END AS bl_exportado
             , [atv].[cd_atividade]
             , [bkl].[cd_pais]
             , null                                    AS [st_principal] 
             , null                                    AS [id_endereco]
	FROM       [dbo].[tb_cliente]                      AS [cli]
    LEFT JOIN  [dbo].[tb_atividades_ilicitas]          AS [atv] ON [atv].[cd_atividade]  = [cli].[cd_profissaoatividade]
    LEFT JOIN  [dbo].[tb_paises_blacklist]             AS [bkl] ON [bkl].[cd_pais]       = [cli].[cd_paisnascimento]
	LEFT JOIN  [dbo].[tb_cliente_conta]                AS [con] ON [cli].[id_cliente]    = [con].[id_cliente] AND UPPER([con].[cd_sistema]) LIKE 'BOL'
	WHERE     ([atv].[cd_atividade] IS NOT NULL OR [bkl].[cd_pais] IS NOT NULL)
    AND        [cli].[dt_passo1] BETWEEN @DtDe AND @DtAte
	AND ((@CodigoBolsa = 0 )                           OR ([cli].[id_assessorinicial]    = @CodigoBolsa OR [con].[cd_codigo] = @CodigoBolsa))
	AND ((@CodigoPais  = '' )                          OR ([cli].[cd_paisnascimento]     = @CodigoPais))
	AND ((@CodigoAtividade = 0)                        OR ([cli].[cd_profissaoatividade] = @CodigoAtividade))
	AND (([bkl].[cd_pais] = [cli].[cd_paisnascimento]) OR ([atv].[cd_atividade]          = [cli].[cd_profissaoatividade]))
	AND ((@TipoPessoa = '')							   OR ([cli].tp_pessoa				 = @TipoPessoa))
	ORDER BY  [cli].[ds_nome]
END


