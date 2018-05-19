set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 05/10/2010
-- Description:	Clientes que não realizaram o primeiro acesso
-- =============================================

-- =============================================
-- Author:		Bruno Varandas
-- Create date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- =============================================

ALTER PROCEDURE [dbo].[prc_sel_cliente_sem_login]
	@dtDe datetime,
	@dtAte datetime,
	@CdAssessor int,
	@DsCpfCnpj varchar(15),
	@TipoPessoa varchar(1)
AS
BEGIN
	SELECT 
		[cliente].[id_cliente],
		cliente.[ds_nome],
		[ds_cpfcnpj],
		[conta].cd_assessor,
		[tp_pessoa],
		[dt_passo1] as dtcadastro, 
		[dt_ultimaatualizacao],
		[conta].cd_codigo  as cd_bovespa,	
		cliente.dt_primeiraexportacao,
		cliente.dt_ultimaexportacao,
		tb_login.ds_email
	FROM
		tb_cliente as cliente
		LEFT JOIN  tb_cliente_conta AS [conta] ON [conta].[id_cliente] = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		INNER JOIN tb_login ON cliente.id_login = tb_login.id_login		
	WHERE
		st_passo = 4 -- Clientes importados do sinacor
		AND dt_passo1 between @dtDe and @dtAte
		AND [conta].[cd_assessor]  = ISNULL(@CdAssessor,[conta].[cd_assessor])
		AND ((@DsCpfCnpj  = '') OR  (@DsCpfCnpj IS NULL) OR  ([cliente].[ds_cpfcnpj] like '%'+@DsCpfCnpj+'%' ))
		AND tb_login.cd_senha = 'yxwasxza123456'
		AND ((@TipoPessoa='') OR (cliente.tp_pessoa=@TipoPessoa))
order by cliente.ds_nome
END










