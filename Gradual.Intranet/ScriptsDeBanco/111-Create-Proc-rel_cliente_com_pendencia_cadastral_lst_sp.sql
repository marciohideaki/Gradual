set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 21/05/2010
-- Description:	Retorna um lista de clientes como pendências cadastrais
-- =============================================
CREATE PROCEDURE [dbo].[rel_cliente_com_pendencia_cadastral_lst_sp] 
	@DtDe						datetime,
	@DtAte						datetime,
	@CodigoAssessor				int,
    @CodigoBolsa				int,
	@IdTipoPendenciaCadastral	int
AS
BEGIN
	SELECT  
		[cliente].[id_cliente]
		,[ds_nome]
		,[ds_cpfcnpj]
		,case when [st_passo] <> 4 then 0 else 1 end as blnExportado
		,case when [st_passo] = 4 then  conta.cd_assessor else cliente.id_assessorinicial end as cd_assessor
		,[dt_passo1] as dtcadastro
		,[dt_ultimaatualizacao]
		,case when lower([conta].[cd_sistema]) = 'bol' then cd_codigo else null end as cd_bovespa
		,case when lower([conta].[cd_sistema]) = 'bmf' then cd_codigo else null end as cd_bmf
		,[pendencia].[dt_cadastropendencia] as dtpendencia
		,[tp_pendencia].[ds_pendencia]		as ds_tipopendencia
		,[pendencia].[id_tipo_pendencia]	as id_tipopendenciacadastral
		,[pendencia].[ds_pendencia]         as ds_pendenciacadastral
		,[cliente].[tp_pessoa]              as tp_pessoa
	FROM 
		tb_cliente								  as cliente
		LEFT JOIN tb_cliente_conta				  as conta		ON [conta].[id_cliente] = [cliente].[id_cliente] and conta.st_principal = 1
		INNER JOIN tb_cliente_pendenciacadastral  as pendencia	ON [pendencia].[id_cliente] = [cliente].[id_cliente]
		INNER JOIN tb_tipo_pendenciacadastral     as tp_pendencia ON [tp_pendencia].[id_tipo_pendencia] = [pendencia].[id_tipo_pendencia]
	WHERE
		[pendencia].[dt_cadastropendencia] between  @dtDe and @dtAte 
		AND ((@CodigoAssessor IS NULL)				OR (([conta].[cd_assessor] =  @CodigoAssessor) OR ([cliente].[id_assessorinicial] = @CodigoAssessor)))
		AND ((@IdTipoPendenciaCadastral IS NULL)	OR ([pendencia].[id_tipo_pendencia] = @IdTipoPendenciaCadastral ))
		AND ((@CdSistema = '')				    OR ([conta].[cd_sistema] = @CdSistema))
END



