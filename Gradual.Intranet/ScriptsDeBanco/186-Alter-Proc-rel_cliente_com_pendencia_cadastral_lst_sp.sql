set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 21/05/2010
-- Description:	Retorna um lista de clientes como pendências cadastrais
-- =============================================

-- =============================================
-- Author:		Gustavo
-- Modifield date: 04/10/2010
-- Description:	Não existe filtro por Bolsa
-- =============================================

ALTER PROCEDURE [dbo].[rel_cliente_com_pendencia_cadastral_lst_sp] 
	@DtDe						datetime,
	@DtAte						datetime,
	@CodigoAssessor				int,
   	@IdTipoPendenciaCadastral	int,
    @StPendenciaResolvida       bit 
AS
BEGIN
	SELECT  
		  [cliente].[id_cliente]
		, [ds_nome]
		, [ds_cpfcnpj]
		, CASE WHEN [st_passo] = 4 THEN 1 ELSE 0 END AS blnExportado
		, CASE WHEN [st_passo] = 4 OR (cliente.id_assessorinicial IS NULL) THEN  conta.cd_assessor ELSE cliente.id_assessorinicial END AS cd_assessor
		, [dt_passo1] AS dtcadastro
		, [dt_ultimaatualizacao]
		, [pendencia].[dt_cadastropendencia]      AS [dtpendencia]
		, [tp_pendencia].[ds_pendencia]           AS [ds_tipopendencia]
		, [pendencia].[id_tipo_pendencia]         AS [id_tipopendenciacadastral]
		, [pendencia].[ds_pendencia]              AS [ds_pendenciacadastral]
		, [cliente].[tp_pessoa]                   AS [tp_pessoa]
		, [pendencia].[dt_resolucao]
		, [conta].[cd_codigo]
	FROM 
		tb_cliente								  AS [cliente]
		LEFT JOIN  tb_cliente_conta               AS [conta]        ON [conta].[id_cliente]     = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		INNER JOIN tb_cliente_pendenciacadastral  AS [pendencia]    ON [pendencia].[id_cliente] = [cliente].[id_cliente]
		INNER JOIN tb_tipo_pendenciacadastral     AS [tp_pendencia] ON [tp_pendencia].[id_tipo_pendencia] = [pendencia].[id_tipo_pendencia]
	WHERE
		[pendencia].[dt_cadastropendencia] BETWEEN @dtDe  AND  @dtAte 
		AND ((@CodigoAssessor              IS NULL)       OR (([conta].[cd_assessor] =  @CodigoAssessor) OR ([cliente].[id_assessorinicial] = @CodigoAssessor)))
		AND ((@IdTipoPendenciaCadastral    IS NULL)       OR (([pendencia].[id_tipo_pendencia] = @IdTipoPendenciaCadastral )))
        AND ((@StPendenciaResolvida        IS NULL)       OR ((@StPendenciaResolvida = '1' AND NOT [pendencia].[dt_resolucao] IS NULL) OR (@StPendenciaResolvida = '0' AND [pendencia].[dt_resolucao] IS NULL)))
order by [dtpendencia]
END







