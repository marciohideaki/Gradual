set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- Descrição:       Realiza a consulta dos dados de cliente por nome/cpf
-- Autor:           Gustavo Malta Guimarães
-- Data de criação: 27/07/2010


ALTER PROCEDURE [dbo].[cliente_educacional_lst_sp]
	@ds_nome varchar(60),
	@ds_cpfcnpj varchar (15)
AS
    SELECT    [cli].[id_cliente]
	,         [cli].[ds_nome]
    ,         [cli].[id_login]
    ,         [cli].[dt_ultimaatualizacao]
    ,         [cli].[ds_cpfcnpj]
    ,         [cli].[dt_passo1]
    ,         [cli].[dt_passo2]   
    ,         [cli].[dt_passo3]
    ,         [cli].[dt_primeiraexporacao]
    ,         [cli].[dt_ultimaexportacao]
    ,         [cli].[ds_origemcadastro]
    ,         [cli].[tp_pessoa]
    ,         [cli].[tp_cliente]
    ,         [cli].[st_passo]
    ,         [cli].[cd_sexo]
    ,         [cli].[cd_nacionalidade]
    ,         [cli].[cd_paisnascimento]
    ,         [cli].[cd_ufnascimento]
    ,         [cli].[ds_ufnascimentoestrangeuro]
    ,         [cli].[cd_estadocivil]
    ,         [cli].[ds_conjugue]
    ,         [cli].[tp_documento]
    ,         [cli].[dt_nascimentofundacao]
    ,         [cli].[cd_orgaoemissordocumento]
    ,         [cli].[cd_ufemissaodocumento]
    ,         [cli].[cd_profissaoatividade]
    ,         [cli].[ds_cargo]
    ,         [cli].[ds_empresa]
    ,         [cli].[st_ppe]
    ,         [cli].[st_carteirapropria]
    ,	      [cli].[ds_autorizadooperar]
    ,         [cli].[st_cvm387]
    ,         [cli].[st_emancipado]
    ,         [cli].[id_assessorinicial]
    ,         [cli].[cd_escolaridade]
    ,         [cli].[id_cliente]
    ,         [cli].[st_cadastroportal]
    ,	      [cli].[ds_nomefantasia]
    ,	      [cli].[cd_nire]
    ,	      [cli].[ds_formaconstituicao]
    ,	      [cli].[st_interdito]
    ,	      [cli].[st_situacaolegaloutros]
   	,	      [cli].[Ds_NomePai]
	,	      [cli].[Ds_NomeMae]
	,	      [cli].[Dt_EmissaoDocumento]
	,	      [cli].[Ds_Naturalidade]
   	,	      [cli].[cd_atividadePrincipal]
	,	      [cli].[nr_inscricaoestadual]
	,	      [cli].[ds_numerodocumento]
	,	      [cli].[ds_emailcomercial]
	,	      [cli].[st_pessoavinculada]
    ,         [log].[ds_email]
    FROM      [dbo].[tb_cliente]  AS [cli]
    LEFT JOIN [dbo].[tb_login]    AS [log] ON [log].[id_login] = [cli].[id_login]
    WHERE  lower ([cli].[ds_nome]) like  '%' + lower(@ds_nome)+ '%'
			and [cli].[ds_cpfcnpj] like '%' + @ds_cpfcnpj + '%'
















