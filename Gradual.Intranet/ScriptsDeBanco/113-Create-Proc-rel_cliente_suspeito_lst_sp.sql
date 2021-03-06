set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 19/05/2010
-- Description:	Retorna uma lista dos clientes suspeitos
-- =============================================
CREATE PROCEDURE [dbo].[rel_cliente_suspeito_lst_sp]
	@DtDe			 datetime,
	@DtAte			 datetime,
	@CodigoBolsa	 int,
	@CodigoPais		 varchar(3),
	@CodigoAtividade varchar(3)
AS
BEGIN
	SELECT 
		[cliente].[id_cliente]
		,[cliente].[ds_nome]
		,[cliente].[ds_cpfcnpj]
		,[cliente].[tp_pessoa]
		,[cliente].[dt_passo1] as dt_cadastro
		,case when  [cliente].[st_passo] = 4 then 1 else 0  end as bl_exportado
		,[atividade].[cd_atividade]
		,[endereco].[cd_pais]
		,[endereco].[st_principal] 
		,[endereco].[id_endereco]
	FROM 
		[tb_cliente] as cliente
		LEFT JOIN [tb_cliente_endereco]		as endereco  ON [endereco].[id_cliente]		= [cliente].[id_cliente] 
		--AND  [endereco].[cd_pais] IN (SELECT DISTINCT cd_pais FROM [tb_paises_blacklist] where  cd_pais = [endereco].[cd_pais] )
		LEFT JOIN [tb_atividades_ilicitas]	as atividade ON [atividade].[cd_atividade]	= [cliente].[cd_profissaoatividade]	
		LEFT JOIN [tb_cliente_conta]		as conta ON [cliente].[id_cliente] = [conta].[id_cliente] 
	WHERE
		[cliente].[dt_passo1] between @DtDe and @DtAte AND
		((@CodigoBolsa = 0 )		OR ([cliente].	[id_assessorinicial]	= @CodigoBolsa OR [conta].[cd_codigo] = @CodigoBolsa)) AND
		((@CodigoPais ='' )			OR ([endereco].	[cd_pais]				= @CodigoPais)) AND
		((@CodigoAtividade IS NULL)		OR ([cliente].	[cd_profissaoatividade] = @CodigoAtividade)) 
END




