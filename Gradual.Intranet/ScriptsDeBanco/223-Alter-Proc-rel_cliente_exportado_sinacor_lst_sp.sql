set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 18/05/2010
-- Description:	Relatorio de Clientes exportados para o sinacor
-- =============================================

-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Modified date: 04/10/2010
-- Description:	Arrumando a Conta Principal, as datas de exportação
-- =============================================

-- =============================================
-- Author:		Bruno Varandas
-- Modified date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- =============================================

ALTER PROCEDURE [dbo].[rel_cliente_exportado_sinacor_lst_sp]
	@dtDe datetime,
	@dtAte datetime,
	@CdAssessor int,
	@DsCpfCnpj varchar(15),
	@TipoPessoa varchar(1)
AS
BEGIN
	SELECT 
		[cliente].[id_cliente],
		[ds_nome],
		[ds_cpfcnpj],
		[conta].cd_assessor,
		[tp_pessoa],
		[dt_passo1] as dtcadastro, 
		[tel].[ds_numero] as ds_telefone,
		[ds_ramal],
		[ds_ddd],
		[dt_ultimaatualizacao],
		[conta].cd_codigo  as cd_bovespa,	
		[conta].[cd_codigo] as cd_codigo,
		cliente.dt_primeiraexportacao,
		cliente.dt_ultimaexportacao
	FROM
		tb_cliente as cliente
				LEFT JOIN  tb_cliente_conta AS [conta] ON [conta].[id_cliente] = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		LEFT JOIN tb_cliente_telefone as tel on [tel].[id_cliente] = [cliente].[id_cliente] and [tel].[st_principal] = 1
	WHERE
		st_passo = 4 -- Clientes exportados para o sinacor
		AND dt_ultimaexportacao between @dtDe and @dtAte
		AND [conta].[cd_assessor]  = ISNULL(@CdAssessor,[conta].[cd_assessor])
		AND ((@DsCpfCnpj  = '') OR (@DsCpfCnpj IS NULL) OR  ([cliente].[ds_cpfcnpj] like '%'+@DsCpfCnpj+'%' ))
		AND ((@TipoPessoa = '') OR (cliente.tp_pessoa=@TipoPessoa))
order by ds_nome
END










