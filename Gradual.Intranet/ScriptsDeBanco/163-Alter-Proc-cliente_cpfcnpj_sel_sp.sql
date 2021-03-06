set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- Descrição:       Realiza a seleção dos dados de cliente com base no cpf
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 05 13

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/06/2010
-- Motivo                   : Inclusão da variável: 
--                            @ds_numerodocumento VARCHAR2(16) 
--                            @ds_emailcomercial VARCHAR2(80) 

-- Autor Ultima Alteração   : Antônio Rodrigues
-- Data da ultima alteração : 30/06/2010
-- Motivo                   : Inclusão do campo telefone e ddd

ALTER PROCEDURE [dbo].[cliente_cpfcnpj_sel_sp]
                 @ds_cpfcnpj  varchar(15)
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
    ,         [cli].[ds_nomefantasia]
    ,	      [cli].[cd_nire]
    ,	      [cli].[ds_formaconstituicao]
    ,	      [cli].[st_interdito]
    ,	      [cli].[st_situacaolegaloutros]
	,	      [cli].[cd_atividadeprincipal]
	,	      [cli].[ds_numerodocumento]
	,	      [cli].[ds_emailcomercial]
    ,         [tel].[ds_ddd]              AS [ds_telefone_ddd]
    ,         [tel].[ds_numero]           AS [ds_telefone_numero]
    FROM      [dbo].[tb_cliente]          AS [cli]
    LEFT JOIN [dbo].[tb_cliente_telefone] AS [tel] ON [tel].[id_cliente] = [cli].[id_cliente] AND [tel].[st_principal] = 1
    WHERE     [cli].[ds_cpfcnpj] LIKE '%' + @ds_cpfcnpj + '%'


