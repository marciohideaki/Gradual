-- Descrição:       Realiza a inclusão dos dados de cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_ins_sp
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
               , @dt_nascimento              datetime
               , @cd_orgaoemissordocumento   varchar(4)
               , @cd_udemissaodocumento      varchar(4)
               , @cd_profissao               numeric(3, 0)
               , @cd_cargo                   varchar(3)
               , @ds_empresa                 varchar(60)
               , @st_ppe                     bit
               , @st_carteirapropria         bit
               , @st_cvm387                  bit
               , @st_emancipado              bit
               , @id_assessorinicial         numeric(5, 0)
               , @cd_escolaridade            numeric(1, 0)
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
            ,    dt_nascimento
            ,    cd_orgaoemissordocumento
            ,    cd_udemissaodocumento
            ,    cd_profissao
            ,    cd_cargo
            ,    ds_empresa
            ,    st_ppe
            ,    st_carteirapropria
            ,    st_cvm387
            ,    st_emancipado
            ,    id_assessorinicial
            ,    cd_escolaridade)
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
            ,    @dt_nascimento
            ,    @cd_orgaoemissordocumento
            ,    @cd_udemissaodocumento
            ,    @cd_profissao
            ,    @cd_cargo
            ,    @ds_empresa
            ,    @st_ppe
            ,    @st_carteirapropria
            ,    @st_cvm387
            ,    @st_emancipado
            ,    @id_assessorinicial
            ,    @cd_escolaridade)
    SELECT SCOPE_IDENTITY() --> Selecinando o id do último registro inserido.
     
