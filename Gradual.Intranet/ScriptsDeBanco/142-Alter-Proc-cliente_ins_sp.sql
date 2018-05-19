set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO


-- Descri��o:       Realiza a inclus�o dos dados de cliente
-- Autor:           Ant�nio Rodrigues
-- Data de cria��o: 2010 04 28

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 30/04/2010
-- Motivo                   : Inclus�o dos campos 
							--ds_nomefantasia varchar(60);
							--cd_nire numeric(15,0);
							--cd_ramoatividade numeric(5,0);
							--ds_formaconstituicao varchar(60);
							--Altera��o do campo dt_nascimento para dt_nascimentofundacao

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 07/05/2010
-- Motivo                   : Inclus�o do campo: @ds_autorizadooperar

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 10/05/2010
-- Motivo                   : Inclus�o dos campos:  
							-- @st_interdito
							-- @st_situacaolegaloutros

-- Autor Ultima Altera��o   : Ant�nio Rodrigues
-- Data da ultima altera��o : 11/05/2010
-- Motivo                   : Altera��o do tipo de retorno
                           -- @id_cliente int output


-- Autor Ultima Altera��o   : Gustavo Malta Guimar�es
-- Data da ultima altera��o : 25/05/2010
-- Motivo                   : Inclus�o dos campos: 
--                            Ds_NomePai //Varchar(60)
--                            Ds_NomeMae //Varchar(60)
--                            Dt_EmissaoDocumento //Date
--                            Ds_Naturalidade //Varchar(20)
--                            Ds_Cargo  //Antigo Cd_Cargo Varchar(3) Agora Ds_Cargo Varchar(40)
	

-- Autor Ultima Altera��o   : Andr� Cristino Miguel
-- Data da ultima altera��o : 29/05/2010
-- Motivo                   : Inclus�o dos campos: 
--							cd_AtividadePrincipal
--						Na tela nova de cadastro PJ existe 2 campos, 1 - Ramo de Atividade (cd_profissaoatividade), 2 - Atividade Principal da empresa (cd_AtividadePrincipal).
--						N�o apagar esta coluna.

-- Autor Ultima Altera��o   : Gustavo Malta Guimar�es
-- Data da ultima altera��o : 31/05/2010
-- Motivo                   : Inclus�o do campo: 
--                            nr_inscricaoestadual VARCHAR2(12) referente ao cvm220.NR_INSCRICAO
--                            Altera��o do Campo:
--							  @ds_formaconstituicao para 	VARCHAR2(15) para ficar igual ao campo cvm220.DS_FORMACAO evitando problemas de integra��o

-- Autor Ultima Altera��o   : Bruno Varandas Ribeiro
-- Data da ultima altera��o : 11/06/2010
-- Motivo                   : Inclus�o da vari�vel: 
--                            @ds_numerodocumento VARCHAR2(16) 
--                            @ds_emailcomercial VARCHAR2(80) 

 
ALTER PROCEDURE [dbo].[cliente_ins_sp]
                 @ds_nome                    varchar(60)
               , @id_login                   int
               , @dt_ultimaatualizacao       datetime
               , @ds_cpfcnpj                 varchar(15)
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
               , @cd_ufemissaodocumento      varchar(4)
               , @cd_profissaoatividade               numeric(3, 0)
               , @ds_cargo                   varchar(40)
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
               , @ds_formaconstituicao       varchar(15)
               , @st_interdito				 bit
               , @st_situacaolegaloutros	 bit
			   , @id_cliente                 int output
			   , @Ds_NomePai					Varchar(60)
	           , @Ds_NomeMae					Varchar(60)
	           , @Dt_EmissaoDocumento			Datetime
	           , @Ds_Naturalidade				Varchar(20)
	           , @cd_atividadePrincipal			int = null
			   , @nr_inscricaoestadual			VARCHAR(12)
			   , @ds_numerodocumento			varchar(16)
			   , @ds_emailcomercial			 varchar(80)
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
            ,    cd_ufemissaodocumento
            ,    cd_profissaoatividade
            ,    ds_cargo
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
            ,	 ds_formaconstituicao
			,	 st_interdito
			,	 st_situacaolegaloutros	
			,	 Ds_NomePai 
			,	 Ds_NomeMae 
			,	 Dt_EmissaoDocumento 
			,	 Ds_Naturalidade 		
			,	cd_atividadePrincipal
			,   nr_inscricaoestadual
			,	ds_numerodocumento
			,	ds_emailcomercial
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
            ,    @cd_ufemissaodocumento
            ,    @cd_profissaoatividade
            ,    @ds_cargo
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
            ,	 @ds_formaconstituicao
            ,	 @st_interdito
            ,	 @st_situacaolegaloutros
			,	@Ds_NomePai 
			,	@Ds_NomeMae 
			,	@Dt_EmissaoDocumento 
			,	@Ds_Naturalidade 
			,	@cd_atividadePrincipal
			,	@nr_inscricaoestadual
			,	@ds_numerodocumento
			,	@ds_emailcomercial)
    SELECT @id_cliente = SCOPE_IDENTITY()