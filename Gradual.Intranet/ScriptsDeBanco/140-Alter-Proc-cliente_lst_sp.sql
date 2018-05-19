set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


-- Descrição:       Lista todos os dados de cliente da base
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 30/04/2010
-- Motivo                   : Inclusão dos campos 
							--ds_nomefantasia varchar(60);
							--cd_nire numeric(15,0);
							--cd_ramoatividade numeric(5,0);
							--ds_formaconstituicao varchar(60);
							--Alteração do campo dt_nascimento para dt_nascimentofundacao
							
-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 07/05/2010
-- Motivo                   : Inclusão do campo: ds_autorizadooperar

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 10/05/2010
-- Motivo                   : Inclusão dos campos: 
							--st_interdito
							--st_situacaolegaloutros

-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 25/05/2010
-- Motivo                   : Inclusão dos campos: 
--                            Ds_NomePai //Varchar(60)
--                            Ds_NomeMae //Varchar(60)
--                            Dt_EmissaoDocumento //Date
--                            Ds_Naturalidade //Varchar(20)
--                            Ds_Cargo  //Antigo Cd_Cargo Varchar(3) Agora Ds_Cargo Varchar(40)

-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 31/05/2010
-- Motivo                   : Inclusão do campo: 
--                            nr_inscricaoestadual VARCHAR2(12) referente ao cvm220.NR_INSCRICAO

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/06/2010
-- Motivo                   : Inclusão da variável: 
--                            @ds_numerodocumento VARCHAR2(16) 
--                            @ds_emailcomercial VARCHAR2(80) 
							
ALTER PROCEDURE [dbo].[cliente_lst_sp]
AS
    SELECT tb_cliente.id_cliente
	,      tb_cliente.ds_nome
    ,      tb_cliente.id_login
    ,      tb_cliente.dt_ultimaatualizacao
    ,      tb_cliente.ds_cpfcnpj
    ,      tb_cliente.dt_passo1
    ,      tb_cliente.dt_passo2
    ,      tb_cliente.dt_passo3
    ,      tb_cliente.dt_primeiraexporacao
    ,      tb_cliente.dt_ultimaexportacao
    ,      tb_cliente.ds_origemcadastro
    ,      tb_cliente.tp_pessoa
    ,      tb_cliente.tp_cliente
    ,      tb_cliente.st_passo
    ,      tb_cliente.cd_sexo
    ,      tb_cliente.cd_nacionalidade
    ,      tb_cliente.cd_paisnascimento
    ,      tb_cliente.cd_ufnascimento
    ,      tb_cliente.ds_ufnascimentoestrangeuro
    ,      tb_cliente.cd_estadocivil
    ,      tb_cliente.ds_conjugue
    ,      tb_cliente.tp_documento
    ,      tb_cliente.dt_nascimentofundacao
    ,      tb_cliente.cd_orgaoemissordocumento
    ,      tb_cliente.cd_ufemissaodocumento
    ,      tb_cliente.cd_profissaoatividade
    ,      tb_cliente.ds_cargo
    ,      tb_cliente.ds_empresa
    ,      tb_cliente.st_ppe
    ,      tb_cliente.st_carteirapropria
    ,	   tb_cliente.ds_autorizadooperar
    ,      tb_cliente.st_cvm387
    ,      tb_cliente.st_emancipado
    ,      tb_cliente.id_assessorinicial
    ,      tb_cliente.cd_escolaridade
    ,      tb_cliente.id_cliente
    ,      tb_cliente.st_cadastroportal
    ,	   tb_cliente.ds_nomefantasia     
    ,	   tb_cliente.cd_nire					 
    ,	   tb_cliente.ds_formaconstituicao
    ,	   tb_cliente.st_interdito
    ,	   tb_cliente.st_situacaolegaloutros
	,	   tb_cliente.Ds_NomePai 
	,	   tb_cliente.Ds_NomeMae 
	,	   tb_cliente.Dt_EmissaoDocumento 
	,	   tb_cliente.Ds_Naturalidade 
	,		tb_cliente.cd_atividadePrincipal
	,		tb_cliente.nr_inscricaoestadual
	,		tb_cliente.ds_numerodocumento
	,		tb_cliente.ds_emailcomercial
    FROM   tb_cliente