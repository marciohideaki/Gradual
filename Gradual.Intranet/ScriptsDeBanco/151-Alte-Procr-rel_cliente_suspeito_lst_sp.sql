set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 19/05/2010
-- Description:	Retorna uma lista dos clientes suspeitos
-- =============================================
ALTER PROCEDURE [dbo].[rel_cliente_suspeito_lst_sp]
	@DtDe			 datetime,
	@DtAte			 datetime,
	@CodigoBolsa	 int,
	@CodigoPais		 varchar(3),
	@CodigoAtividade varchar(3)
AS
BEGIN
	SELECT DISTINCT
               [con].[cd_assessor]
			 , [cli].[id_cliente]
			 , [cli].[ds_nome]
			 , [cli].[ds_cpfcnpj]
			 , [cli].[tp_pessoa]
			 , [cli].[dt_passo1] AS [dt_cadastro]
			 , CASE WHEN  [cli].[st_passo] = 4 THEN 1 ELSE 0 END AS bl_exportado
			 , [cli].[cd_profissaoatividade]   AS [cd_atividade]
			 , [end].[cd_pais]
			 , [end].[st_principal] 
			 , [end].[id_endereco]
	FROM       [dbo].[tb_cliente]              AS [cli]
	FULL  JOIN [dbo].[tb_paises_blacklist]     AS [pbl] ON [pbl].[id_pais_blacklist]   = [pbl].[id_pais_blacklist]
	FULL  JOIN [dbo].[tb_atividades_ilicitas]  AS [atv] ON [atv].[id_atividadeilicita] = [atv].[id_atividadeilicita]
	INNER JOIN [dbo].[tb_cliente_endereco]     AS [end] ON [end].[id_cliente]          = [cli].[id_cliente] -- AND [end].[st_principal] = 1
	LEFT  JOIN [dbo].[tb_cliente_conta]        AS [con] ON [cli].[id_cliente]          = [con].[id_cliente]
	WHERE      [cli].[dt_passo1] BETWEEN @DtDe AND @DtAte
	AND ((@CodigoBolsa = 0 )                   OR ([cli].[id_assessorinicial]           = @CodigoBolsa OR [con].[cd_codigo] = @CodigoBolsa))
	AND ((@CodigoPais  = '' )                  OR ([end].[cd_pais]                      = @CodigoPais))
	AND ((@CodigoAtividade = 0)                OR ([cli].[cd_profissaoatividade]        = @CodigoAtividade))
	AND (([pbl].[cd_pais] = [end].[cd_pais] )  OR ([atv].[cd_atividade]                 = [cli].[cd_profissaoatividade]))
	ORDER BY  [cli].[ds_nome]
END