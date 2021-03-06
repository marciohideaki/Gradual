set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



-- Descrição:       Realiza a inclusão dos dados de cliente
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
-- Motivo                   : Inclusão do campo: @ds_autorizadooperar

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 10/05/2010
-- Motivo                   : Inclusão dos campos:  
							-- @st_interdito
							-- @st_situacaolegaloutros

-- Autor Ultima Alteração   : Antônio Rodrigues
-- Data da ultima alteração : 11/05/2010
-- Motivo                   : Alteração do tipo de retorno
                           -- @id_cliente int output
	
 
ALTER PROCEDURE [dbo].[cliente_ins_sp]
                 @ds_nome                    varchar(60)
               , @id_login                   bigint
               , @dt_ultimaatualizacao       datetime
               , @ds_cpfcnpj                 numeric(15, 0)
               , @dt_passo1                  datetime
               , @dt_passo2                  datetime
               , @dt_passo3                  datetime
               , @dt_primeiraexporacao       datetime
               , @dt_ultimaexportacao        datetime
               , @ds_origemcadastro          varchar(20)
               , @tp_pessoa                  varchar(1)
               , @tp_cliente                 numeric(2, 0)
               , @st_passo                   numeric(1, 0)
               , @cd_sexo                    varchar(1)
               , @cd_nacionalidade           numeric(1, 0)
               , @cd_paisnascimento          varchar(3)
               , @cd_ufnascimento            varchar(4)
               , @ds_ufnascimentoestrangeuro varchar(20)
               , @cd_estadocivil             numeric(1, 0)
               , @ds_conjugue                varchar(60)
               , @tp_documento               varchar(2)
               , @dt_nascimentofundacao      datetime
               , @cd_orgaoemissordocumento   varchar(4)
               , @cd_udemissaodocumento      varchar(4)
               , @cd_profissao               numeric(3, 0)
               , @cd_cargo                   varchar(3)
               , @ds_empresa                 varchar(60)
               , @st_ppe                     bit
               , @st_carteirapropria         bit
               , @ds_autorizadooperar		 varchar(200)
               , @st_cvm387                  bit
               , @st_emancipado              bit
               , @st_cadastroportal          bit
               , @id_assessorinicial         numeric(5, 0)
               , @cd_escolaridade            numeric(1, 0)
               , @ds_nomefantasia            varchar(60)
               , @cd_nire                    numeric(15,0)
               , @cd_ramoatividade			 numeric(5,0)
               , @ds_formaconstituicao       varchar(60)
               , @st_interdito				 bit
               , @st_situacaolegaloutros	 bit
               , @id_cliente                 bigint output
AS
    INSERT INTO tb_cliente
            (    ds_nome
            ,    id_login
            ,    dt_ultimaatualizacao
            ,    ds_cpfcnpj
            ,    dt_passo1
            ,    dt_passo2
            ,    dt_passo3
            ,    dt_primeiraexporacao
            ,    dt_ultimaexportacao
            ,    ds_origemcadastro
            ,    tp_pessoa
            ,    tp_cliente
            ,    st_passo
            ,    cd_sexo
            ,    cd_nacionalidade
            ,    cd_paisnascimento
            ,    cd_ufnascimento
            ,    ds_ufnascimentoestrangeuro
            ,    cd_estadocivil
            ,    ds_conjugue
            ,    tp_documento
            ,    dt_nascimentofundacao
            ,    cd_orgaoemissordocumento
            ,    cd_udemissaodocumento
            ,    cd_profissao
            ,    cd_cargo
            ,    ds_empresa
            ,    st_ppe
            ,    st_carteirapropria
            ,	 ds_autorizadooperar
            ,    st_cvm387
            ,    st_emancipado
            ,    id_assessorinicial
            ,    cd_escolaridade
            ,    st_cadastroportal
            ,	 ds_nomefantasia
            ,    cd_nire
            ,	 cd_ramoatividade
            ,	 ds_formaconstituicao
			,	 st_interdito
			,	 st_situacaolegaloutros				
            )
     VALUES (    @ds_nome
            ,    @id_login
            ,    @dt_ultimaatualizacao
            ,    @ds_cpfcnpj
            ,    @dt_passo1
            ,    @dt_passo2
            ,    @dt_passo3
            ,    @dt_primeiraexporacao
            ,    @dt_ultimaexportacao
            ,    @ds_origemcadastro
            ,    @tp_pessoa
            ,    @tp_cliente
            ,    @st_passo
            ,    @cd_sexo
            ,    @cd_nacionalidade
            ,    @cd_paisnascimento
            ,    @cd_ufnascimento
            ,    @ds_ufnascimentoestrangeuro
            ,    @cd_estadocivil
            ,    @ds_conjugue
            ,    @tp_documento
            ,    @dt_nascimentofundacao
            ,    @cd_orgaoemissordocumento
            ,    @cd_udemissaodocumento
            ,    @cd_profissao
            ,    @cd_cargo
            ,    @ds_empresa
            ,    @st_ppe
            ,    @st_carteirapropria
            ,	 @ds_autorizadooperar
            ,    @st_cvm387
            ,    @st_emancipado
            ,    @id_assessorinicial
            ,    @cd_escolaridade
            ,    @st_cadastroportal
            ,	 @ds_nomefantasia
            ,    @cd_nire
            ,	 @cd_ramoatividade
            ,	 @ds_formaconstituicao
            ,	 @st_interdito
            ,	 @st_situacaolegaloutros)
    SELECT @id_cliente = SCOPE_IDENTITY()
     



