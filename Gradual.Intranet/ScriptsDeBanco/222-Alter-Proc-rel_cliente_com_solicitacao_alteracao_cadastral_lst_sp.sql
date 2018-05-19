set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Gustavo
-- Create date: 04/10/2010
-- Description:	Relatório de Clientes Com Solicitação de Alteração Cadastral
-- =============================================

-- =============================================
-- Author:		Bruno Varandas 
-- Create date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- =============================================
ALTER PROCEDURE [dbo].[rel_cliente_com_solicitacao_alteracao_cadastral_lst_sp] 
	@DtDe						datetime,
	@DtAte						datetime,
	@CodigoAssessor				int,
   	@DsCpfCnpj	                varchar(14),
    @StResolvida       bit ,
	@TipoPessoa varchar (1)
AS
BEGIN
	SELECT  
		  [cliente].[id_cliente]
		, [ds_nome]
		, [ds_cpfcnpj]
		, CASE WHEN [st_passo] = 4 THEN 1 ELSE 0 END AS blnExportado
		, CASE WHEN [st_passo] = 4 OR (cliente.id_assessorinicial IS NULL) THEN  conta.cd_assessor ELSE cliente.id_assessorinicial END AS cd_assessor
		, [alteracao].Dt_Solicitacao  AS Dt_Solicitacao
		, [cliente].[tp_pessoa]       AS [tp_pessoa]
		, [alteracao].[dt_realizacao]
		, [conta].[cd_codigo]
		, [alteracao].cd_tipo
		, [alteracao].Ds_Informacao
	FROM 
		tb_cliente	AS [cliente]
		LEFT JOIN  tb_cliente_conta AS [conta] ON [conta].[id_cliente] = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		INNER JOIN tb_Alteracao  AS [alteracao] ON [alteracao].[id_cliente] = [cliente].[id_cliente]
	WHERE
		[alteracao].[dt_solicitacao] BETWEEN @dtDe  AND  @dtAte 
		AND ((@CodigoAssessor IS NULL) OR (([conta].[cd_assessor] =  @CodigoAssessor) OR ([cliente].[id_assessorinicial] = @CodigoAssessor)))
		AND ((@DsCpfCnpj IS NULL) OR (([cliente].[Ds_CpfCnpj] like '%'+@DsCpfCnpj+'%' )))
        AND ((@StResolvida        IS NULL) OR ((@StResolvida = '1' AND NOT [alteracao].[dt_realizacao] IS NULL) OR (@StResolvida = '0' AND [alteracao].[dt_realizacao] IS NULL)))
		AND ((@TipoPessoa = '') OR (cliente.tp_pessoa = @TipoPessoa))
ORDER BY Dt_Solicitacao DESC, [alteracao].cd_tipo, [ds_nome]
END










