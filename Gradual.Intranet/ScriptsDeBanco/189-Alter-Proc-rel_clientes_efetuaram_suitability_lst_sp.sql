set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 10/06/2010
-- Description:	Retorna uma lista dos clientes efetuaram o suitability em um certo periodo
-- =============================================

-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 04/10/2010
-- Description:	Ver clientes que realizaram e que não realizaram o suitability e pegar vários campos que estavam Faltando
-- =============================================


ALTER PROCEDURE [dbo].[rel_clientes_efetuaram_suitability_lst_sp]
	@DtDe			 datetime,
	@DtAte			 datetime,
	@StRealizado	 bit,
	@CodigoAssessor  int,
	@CpfCnpj         varchar(14)
AS
BEGIN

	if @StRealizado = 1
	BEGIN
		SELECT 
			 [cliente].[id_cliente]
			,[cliente].[ds_nome]
			,[cliente].[ds_cpfcnpj]
			,[conta].[cd_codigo]
			,[conta].[cd_assessor]
			,[suit].[ds_status]
			,[suit].[ds_perfil]
			,[suit].[ds_fonte]
			,[suit].[dt_realizacao]
			,[suit].[dt_realizacao]
			,[suit].[dt_realizacao]
			,[suit].ds_loginrealizado
			,[suit].st_preenchidopelocliente
		FROM 
			[tb_cliente] as cliente
			INNER JOIN [tb_cliente_suitability]	as suit ON [cliente].[id_cliente] = [suit].[id_cliente]
			LEFT JOIN  tb_cliente_conta AS [conta] ON [conta].[id_cliente] = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		WHERE --Ver se buscar por não realizado e ignorar data (usar também IN) - Caso não realizado (usar também Not in)
			 ([suit].[dt_realizacao] between @DtDe and @DtAte)
			AND ([conta].[cd_assessor] = ISNULL(@CodigoAssessor, [conta].[cd_assessor])) 
			AND ((@CpfCnpj IS NULL) OR  (@CpfCnpj = '') OR(([cliente].[Ds_CpfCnpj] like '%'+@CpfCnpj+'%' )))
			AND cliente.tp_pessoa = 'F' AND cliente.st_passo=4 
		order by cliente.ds_nome;
	END
	ELSE
	BEGIN
		SELECT 
			 [cliente].[id_cliente]
			,[cliente].[ds_nome]
			,[cliente].[ds_cpfcnpj]
			,[conta].[cd_codigo]
			,[conta].[cd_assessor]
			,null [ds_status]
			,null [ds_perfil]
			,null [ds_fonte]
			,null [dt_realizacao]
			,null [dt_realizacao]
			,null [dt_realizacao]
			, null ds_loginrealizado
			,null st_preenchidopelocliente
		FROM 
			[tb_cliente] as cliente
			LEFT JOIN  tb_cliente_conta AS [conta] ON [conta].[id_cliente] = [cliente].[id_cliente] AND conta.st_principal = 1 AND CD_SISTEMA = 'BOL'
		WHERE --Ver se buscar por não realizado e ignorar data (usar também IN) - Caso não realizado (usar também Not in)
			 ([dt_passo1] between @DtDe and @DtAte)
			AND ([conta].[cd_assessor] = ISNULL(@CodigoAssessor, [conta].[cd_assessor])) 
			AND ((@CpfCnpj IS NULL) OR  (@CpfCnpj = '') OR (([cliente].[Ds_CpfCnpj] like '%'+@CpfCnpj+'%' )))
			AND cliente.tp_pessoa = 'F' AND cliente.st_passo=4 
			AND cliente.id_cliente not in (select id_cliente from tb_cliente_suitability)
		order by cliente.ds_nome;	
	END 
	

	
END







