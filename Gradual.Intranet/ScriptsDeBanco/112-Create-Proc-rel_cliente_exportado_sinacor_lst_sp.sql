set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 18/05/2010
-- Description:	Relatorio de Clientes exportados para o sinacor
-- =============================================
ALTER PROCEDURE [dbo].[rel_cliente_exportado_sinacor_lst_sp]
	@dtDe datetime,
	@dtAte datetime,
	@CdAssessor int,
	@DsCpfCnpj varchar(15)
AS
BEGIN
	SELECT 
		[cliente].[id_cliente],
		[ds_nome],
		[ds_cpfcnpj],
		case when [st_passo] <> 4 then 0 else 1 end as blnExportado,
		[conta].cd_assessor,
		[tp_pessoa],
		[dt_passo1] as dtcadastro, 
		[tel].[ds_numero] as ds_telefone,
		[ds_ramal],
		[ds_ddd],
		[dt_ultimaatualizacao],
		case when lower([conta].[cd_sistema]) = 'bol' then cd_codigo else null end as cd_bovespa,
		case when lower([conta].[cd_sistema]) = 'bmf' then cd_codigo else null end as cd_bmf,
		[conta].[cd_codigo] as cd_codigo
	FROM
		tb_cliente as cliente
		LEFT JOIN tb_cliente_conta as conta on [conta].[id_cliente] = [cliente].[id_cliente] and [conta].[st_principal] = 1
		LEFT JOIN tb_cliente_telefone as tel on [tel].[id_cliente] = [cliente].[id_cliente] and [tel].[st_principal] = 1
	WHERE
		st_passo = 4 -- Clientes exportados para o sinacor
		AND dt_passo1 between @dtDe and @dtAte
		AND [conta].[cd_assessor]  = ISNULL(@CdAssessor,[conta].[cd_assessor])
		AND ((@DsCpfCnpj  = '') OR  ([cliente].[ds_cpfcnpj] = @DsCpfCnpj))
		--AND ((@CdBolsa	  =  '')   OR  ([conta].[cd_sistema]    = @CdBolsa))
END





