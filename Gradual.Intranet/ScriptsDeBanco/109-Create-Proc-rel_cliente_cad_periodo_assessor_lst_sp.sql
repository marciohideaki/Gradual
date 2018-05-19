set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 01/06/2010
-- Description:	Relatorio de clientes cadastrados por período com filtro de assessor
-- =============================================
CREATE PROCEDURE [dbo].[rel_cliente_cad_periodo_assessor_lst_sp]
	@dtDe datetime,
	@dtAte datetime,
	@CdAssessor int
AS
BEGIN
    -- Insert statements for procedure here
	SELECT  
		[cliente].[id_cliente]
		,[ds_nome]
		,[ds_cpfcnpj]
		,case when [st_passo] <> 4 then 0 else 1 end as blnExportado
		,case when [st_passo] = 4 then  conta.cd_assessor else cliente.id_assessorinicial end as cd_assessor
		,[tp_pessoa]
		,[dt_passo1] as dtcadastro
		,[tel].[ds_numero] as ds_telefone
		,[ds_ramal]
		,[ds_ddd]
		,[dt_ultimaatualizacao]
		,case when lower([conta].[cd_sistema]) = 'bol' then cd_codigo else null end as cd_bovespa
		,case when lower([conta].[cd_sistema]) = 'bmf' then cd_codigo else null end as cd_bmf
	FROM 
		tb_cliente as cliente
		LEFT JOIN tb_cliente_conta as conta on conta.id_cliente = cliente.id_cliente and conta.st_principal = 1
		LEFT JOIN tb_cliente_telefone as tel on tel.id_cliente = cliente.id_cliente and tel.st_principal = 1
	WHERE
		dt_passo1 between  @dtDe and @dtAte
		AND ((@CdAssessor is null) OR  ([conta].[cd_assessor]  = @CdAssessor))
END




