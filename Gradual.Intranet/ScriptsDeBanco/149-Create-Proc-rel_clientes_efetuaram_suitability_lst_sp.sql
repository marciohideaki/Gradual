set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 10/06/2010
-- Description:	Retorna uma lista dos clientes efetuaram o suitability em um certo periodo
-- =============================================
ALTER PROCEDURE [dbo].[rel_clientes_efetuaram_suitability_lst_sp]
	@DtDe			 datetime,
	@DtAte			 datetime,
	@TipoBolsa		varchar(3),
	@CodigoAssessor int,
	@CpfCnpj        varchar(14)

AS
BEGIN
	SELECT 
		[cliente].[id_cliente]
		,[cliente].[ds_nome]
		,[cliente].[ds_cpfcnpj]
		,[cliente].[tp_pessoa]
		,[cliente].[dt_passo1] as dt_cadastro
		,case when  [cliente].[st_passo] = 4 then 1 else 0  end as st_exportado
		,[suit].[ds_status]
		,[suit].[ds_perfil]
		,[suit].[ds_fonte]
		,[conta].[cd_codigo]
		,[conta].[cd_assessor]
	FROM 
		[tb_cliente] as cliente
		LEFT JOIN [tb_cliente_suitability]	as suit ON [cliente].[id_cliente] = [suit].[id_cliente]
		LEFT JOIN [tb_cliente_conta] as conta on [cliente].[id_cliente] = [conta].[id_cliente]
	WHERE
		[suit].[dt_realizacao] between @DtDe and @DtAte
		AND [conta].[cd_assessor] = ISNULL(@CodigoAssessor, [conta].[cd_assessor]) 
		AND ((@CpfCnpj = '') OR ([cliente].[ds_cpfcnpj] = @CpfCnpj))
END






