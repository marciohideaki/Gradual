set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- Descrição:       Realiza a seleção dos dados de cliente com base no cpf
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 05 13

CREATE PROCEDURE [dbo].[cliente_cpfcnpj_sel_sp]
                 @ds_cpfcnpj  varchar(1000)
AS
    SELECT [tb_cliente].[id_cliente]
	,      [tb_cliente].[ds_nome]
    ,      [tb_cliente].[id_login]
    ,      [tb_cliente].[dt_ultimaatualizacao]
    ,      [tb_cliente].[ds_cpfcnpj]
    ,      [tb_cliente].[dt_passo1]
    ,      [tb_cliente].[dt_passo2]
    ,      [tb_cliente].[dt_passo3]
    ,      [tb_cliente].[dt_primeiraexporacao]
    ,      [tb_cliente].[dt_ultimaexportacao]
    ,      [tb_cliente].[ds_origemcadastro]
    ,      [tb_cliente].[tp_pessoa]
    ,      [tb_cliente].[tp_cliente]
    ,      [tb_cliente].[st_passo]
    ,      [tb_cliente].[cd_sexo]
    ,      [tb_cliente].[cd_nacionalidade]
    ,      [tb_cliente].[cd_paisnascimento]
    ,      [tb_cliente].[cd_ufnascimento]
    ,      [tb_cliente].[ds_ufnascimentoestrangeuro]
    ,      [tb_cliente].[cd_estadocivil]
    ,      [tb_cliente].[ds_conjugue]
    ,      [tb_cliente].[tp_documento]
    ,      [tb_cliente].[dt_nascimentofundacao]
    ,      [tb_cliente].[cd_orgaoemissordocumento]
    ,      [tb_cliente].[cd_ufemissaodocumento]
    ,      [tb_cliente].[cd_profissaoatividade]
    ,      [tb_cliente].[cd_cargo]
    ,      [tb_cliente].[ds_empresa]
    ,      [tb_cliente].[st_ppe]
    ,      [tb_cliente].[st_carteirapropria]
    ,	   [tb_cliente].[ds_autorizadooperar]
    ,      [tb_cliente].[st_cvm387]
    ,      [tb_cliente].[st_emancipado]
    ,      [tb_cliente].[id_assessorinicial]
    ,      [tb_cliente].[cd_escolaridade]
    ,      [tb_cliente].[id_cliente]
    ,      [tb_cliente].[st_cadastroportal]
    ,	   [tb_cliente].[ds_nomefantasia]
    ,	   [tb_cliente].[cd_nire]
    ,	   [tb_cliente].[ds_formaconstituicao]
    ,	   [tb_cliente].[st_interdito]
    ,	   [tb_cliente].[st_situacaolegaloutros]
    FROM   [dbo].[tb_cliente]
    WHERE  [tb_cliente].[ds_cpfcnpj] IN (@ds_cpfcnpj)