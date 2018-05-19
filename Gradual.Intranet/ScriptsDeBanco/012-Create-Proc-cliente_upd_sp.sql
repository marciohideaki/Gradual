-- Descrição:       Realiza a atualização dos dados de cliente
-- Autor:           Antônio Rodrigues
-- Data de criação: 2010 04 28
CREATE PROCEDURE cliente_upd_sp
                 @id_cliente                 bigint
               , @ds_nome                    varchar(60)
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
    ,      tb_cliente.dt_nascimento              = @dt_nascimento
    ,      tb_cliente.cd_orgaoemissordocumento   = @cd_orgaoemissordocumento
    ,      tb_cliente.cd_udemissaodocumento      = @cd_udemissaodocumento
    ,      tb_cliente.cd_profissao               = @cd_profissao
    ,      tb_cliente.cd_cargo                   = @cd_cargo
    ,      tb_cliente.ds_empresa                 = @ds_empresa
    ,      tb_cliente.st_ppe                     = @st_ppe
    ,      tb_cliente.st_carteirapropria         = @st_carteirapropria
    ,      tb_cliente.st_cvm387                  = @st_cvm387
    ,      tb_cliente.st_emancipado              = @st_emancipado
    ,      tb_cliente.id_assessorinicial         = @id_assessorinicial
    ,      tb_cliente.cd_escolaridade            = @cd_escolaridade
    WHERE  tb_cliente.id_cliente                 = @id_cliente