set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 17/05/2010
-- Description:	Relatorio de clientes cadastrados por período
-- =============================================

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- =============================================

ALTER procedure [dbo].[rel_cliente_cadastrado_periodo_lst_sp]
	@dtde datetime,
	@dtate datetime,
	@codigoassessor int = null,
	@TipoPessoa varchar
as
begin
    -- insert statements for procedure here
	select  
		[cliente].[id_cliente]
		,[ds_nome]
		,[ds_cpfcnpj]
		,case when [st_passo] <> 4 then 0 else 1 end as blnexportado
		,case when [st_passo] = 4 then  conta.cd_assessor else cliente.id_assessorinicial end as cd_assessor
		,[tp_pessoa]
		,[dt_passo1] as dtcadastro
		,[tel].[ds_numero] as ds_telefone
		,[ds_ramal]
		,[ds_ddd]
		,[dt_ultimaatualizacao]
		,case when lower([conta].[cd_sistema]) = 'bol' then cd_codigo else null end as cd_bovespa
		,case when lower([conta].[cd_sistema]) = 'bmf' then cd_codigo else null end as cd_bmf
	from 
		tb_cliente as cliente
		left join tb_cliente_conta as conta on conta.id_cliente = cliente.id_cliente and conta.cd_sistema = 'BOL'
		and conta.st_principal = 1
		left join tb_cliente_telefone as tel on tel.id_cliente = cliente.id_cliente and tel.st_principal = 1
	where
		dt_passo1 between  @dtde and @dtate
		and (conta.cd_assessor = isnull(@codigoassessor, conta.cd_assessor))
		and ((@TipoPessoa = '') or (cliente.tp_pessoa  = @TipoPessoa))
    order by [ds_nome]
end








