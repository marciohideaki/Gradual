set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO




-- Descrição:       Realiza a atualização dos dados de cliente
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
-- Motivo                   : Inclusão dos campos: @ds_autorizadooperar
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
--                            Alteração do Campo:
--							  @ds_formaconstituicao para 	VARCHAR2(15) para ficar igual ao campo cvm220.DS_FORMACAO evitando problemas de integração
 
-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/06/2010
-- Motivo                   : Inclusão da variável: 
--                            @ds_numerodocumento VARCHAR2(16) 
--                            @ds_emailcomercial VARCHAR2(80) 
								
					
ALTER PROCEDURE [dbo].[cliente_upd_sp]
	  @id_cliente                 int
	, @ds_nome                    varchar(60)
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
	, @ds_nomefantasia			  varchar(60)
	, @cd_nire				  	  numeric(15,0)
	, @ds_formaconstituicao       varchar(15)
	, @st_interdito				  bit
	, @st_situacaolegaloutros	  bit
	, @Ds_NomePai					Varchar(60)
	, @Ds_NomeMae					Varchar(60)
	, @Dt_EmissaoDocumento			Datetime
	, @Ds_Naturalidade				Varchar(20)
	, @cd_atividadePrincipal		int
	, @nr_inscricaoestadual			VARCHAR(12)
	, @ds_numerodocumento			varchar(16)
	, @ds_emailcomercial			varchar(80)
AS
    UPDATE tb_cliente
    SET    tb_cliente.ds_nome                    = @ds_nome
    ,      tb_cliente.id_login                   = @id_login
    ,      tb_cliente.dt_ultimaatualizacao       = @dt_ultimaatualizacao
    ,      tb_cliente.ds_cpfcnpj                 = @ds_cpfcnpj
    ,      tb_cliente.dt_passo1                  = @dt_passo1
    ,      tb_cliente.dt_passo2                  = @dt_passo2
    ,      tb_cliente.dt_passo3                  = @dt_passo3
    ,      tb_cliente.dt_primeiraexporacao       = @dt_primeiraexporacao
    ,      tb_cliente.dt_ultimaexportacao        = @dt_ultimaexportacao
    ,      tb_cliente.ds_origemcadastro          = @ds_origemcadastro
    ,      tb_cliente.tp_pessoa                  = @tp_pessoa
    ,      tb_cliente.tp_cliente                 = @tp_cliente
    ,      tb_cliente.st_passo                   = @st_passo
    ,      tb_cliente.cd_sexo                    = @cd_sexo
    ,      tb_cliente.cd_nacionalidade           = @cd_nacionalidade
    ,      tb_cliente.cd_paisnascimento          = @cd_paisnascimento
    ,      tb_cliente.cd_ufnascimento            = @cd_ufnascimento
    ,      tb_cliente.ds_ufnascimentoestrangeuro = @ds_ufnascimentoestrangeuro
    ,      tb_cliente.cd_estadocivil             = @cd_estadocivil
    ,      tb_cliente.ds_conjugue                = @ds_conjugue
    ,      tb_cliente.tp_documento               = @tp_documento
    ,      tb_cliente.dt_nascimentofundacao      = @dt_nascimentofundacao
    ,      tb_cliente.cd_orgaoemissordocumento   = @cd_orgaoemissordocumento
    ,      tb_cliente.cd_ufemissaodocumento      = @cd_ufemissaodocumento
    ,      tb_cliente.cd_profissaoatividade      = @cd_profissaoatividade
    ,      tb_cliente.ds_cargo                   = @ds_cargo
    ,      tb_cliente.ds_empresa                 = @ds_empresa
    ,      tb_cliente.st_ppe                     = @st_ppe
    ,      tb_cliente.st_carteirapropria         = @st_carteirapropria
    ,	   tb_cliente.ds_autorizadooperar        = @ds_autorizadooperar
    ,      tb_cliente.st_cvm387                  = @st_cvm387
    ,      tb_cliente.st_emancipado              = @st_emancipado
    ,      tb_cliente.id_assessorinicial         = @id_assessorinicial
    ,      tb_cliente.cd_escolaridade            = @cd_escolaridade
    ,      tb_cliente.st_cadastroportal          = @st_cadastroportal
    ,	   tb_cliente.ds_nomefantasia            = @ds_nomefantasia
    ,	   tb_cliente.cd_nire					 = @cd_nire
    ,	   tb_cliente.ds_formaconstituicao       = @ds_formaconstituicao
    ,	   tb_cliente.st_interdito			     = @st_interdito
    ,	   tb_cliente.st_situacaolegaloutros     = @st_situacaolegaloutros
	,	   tb_cliente.Ds_NomePai				 = @Ds_NomePai
	,	   tb_cliente.Ds_NomeMae				 = @Ds_NomeMae
	,	   tb_cliente.Dt_EmissaoDocumento		 = @Dt_EmissaoDocumento
	,	   tb_cliente.Ds_Naturalidade			 = @Ds_Naturalidade
	,	   tb_cliente.cd_atividadePrincipal		 = @cd_atividadePrincipal
    ,	   tb_cliente.nr_inscricaoestadual		 = @nr_inscricaoestadual
	,	   tb_cliente.ds_numerodocumento		 = @ds_numerodocumento
	,	   tb_cliente.ds_emailcomercial		     = @ds_emailcomercial
    WHERE  tb_cliente.id_cliente                 = @id_cliente










