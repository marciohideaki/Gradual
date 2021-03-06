set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO




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


-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 25/05/2010
-- Motivo                   : Inclusão dos campos: 
--                            Ds_NomePai //Varchar(60)
--                            Ds_NomeMae //Varchar(60)
--                            Dt_EmissaoDocumento //Date
--                            Ds_Naturalidade //Varchar(20)
--                            Ds_Cargo  //Antigo Cd_Cargo Varchar(3) Agora Ds_Cargo Varchar(40)
	

-- Autor Ultima Alteração   : André Cristino Miguel
-- Data da ultima alteração : 29/05/2010
-- Motivo                   : Inclusão dos campos: 
--							cd_AtividadePrincipal
--						Na tela nova de cadastro PJ existe 2 campos, 1 - Ramo de Atividade (cd_profissaoatividade), 2 - Atividade Principal da empresa (cd_AtividadePrincipal).
--						Não apagar esta coluna.

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

-- Autor Ultima Alteração   : Antônio Rodrigues
-- Data da ultima alteração : 15/06/2010
-- Motivo                   : Inclusão do campo 'st_pessoavinculada'


-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 23/09/2010
-- Motivo                   : Ao Inserir um cliente (Exceto Importação - st_passo=4) cadastrar todas as pendências para ele.

 
ALTER PROCEDURE [dbo].[cliente_ins_sp]
             @ds_nome                    varchar(60)
           , @id_login                   int
           , @dt_ultimaatualizacao       datetime
           , @ds_cpfcnpj                 varchar(15)
           , @dt_passo1                  datetime 
           , @dt_passo2                  datetime
           , @dt_passo3                  datetime
           , @dt_primeiraexportacao       datetime
           , @dt_ultimaexportacao        datetime
           , @ds_origemcadastro          varchar(40)
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
           , @cd_profissaoatividade      numeric(3, 0)
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
		   , @Ds_NomePai                 Varchar(60)
           , @Ds_NomeMae                 Varchar(60)
           , @Dt_EmissaoDocumento        Datetime
           , @Ds_Naturalidade            Varchar(20)
           , @cd_atividadePrincipal      int = null
		   , @nr_inscricaoestadual       VARCHAR(12)
		   , @ds_numerodocumento         varchar(16)
		   , @ds_emailcomercial          varchar(80)
		   , @st_pessoavinculada         bit
           , @st_ativo                   bit = 1
AS
    INSERT INTO  [dbo].[tb_cliente]
            (    [ds_nome]
            ,    [id_login]
            ,    [dt_ultimaatualizacao]
            ,    [ds_cpfcnpj]
            ,    [dt_passo1]
            ,    [dt_passo2]
            ,    [dt_passo3]
            ,    [dt_primeiraexportacao]
            ,    [dt_ultimaexportacao]
            ,    [ds_origemcadastro]
            ,    [tp_pessoa]
            ,    [tp_cliente]
            ,    [st_passo]
            ,    [cd_sexo]
            ,    [cd_nacionalidade]
            ,    [cd_paisnascimento]
            ,    [cd_ufnascimento]
            ,    [ds_ufnascimentoestrangeuro]
            ,    [cd_estadocivil]
            ,    [ds_conjugue]
            ,    [tp_documento]
            ,    [dt_nascimentofundacao]
            ,    [cd_orgaoemissordocumento]
            ,    [cd_ufemissaodocumento]
            ,    [cd_profissaoatividade]
            ,    [ds_cargo]
            ,    [ds_empresa]
            ,    [st_ppe]
            ,    [st_carteirapropria]
            ,	 [ds_autorizadooperar]
            ,    [st_cvm387]
            ,    [st_emancipado]
            ,    [id_assessorinicial]
            ,    [cd_escolaridade]
            ,    [st_cadastroportal]
            ,	 [ds_nomefantasia]
            ,    [cd_nire]
            ,	 [ds_formaconstituicao]
			,	 [st_interdito]
			,	 [st_situacaolegaloutros]
			,	 [Ds_NomePai]
			,	 [Ds_NomeMae]
			,	 [Dt_EmissaoDocumento]
			,	 [Ds_Naturalidade]
			,	 [cd_atividadePrincipal]
			,    [nr_inscricaoestadual]
			,	 [ds_numerodocumento]
			,	 [ds_emailcomercial]
			,    [st_pessoavinculada]
            ,    [st_ativo])
     VALUES (    @ds_nome
            ,    @id_login
            ,    @dt_ultimaatualizacao
            ,    @ds_cpfcnpj
            ,    isnull(@dt_passo1,getdate())
            ,    @dt_passo2
            ,    @dt_passo3
            ,    @dt_primeiraexportacao
            ,    @dt_ultimaexportacao
            ,    @ds_origemcadastro
            ,    @tp_pessoa
            ,    @tp_cliente
            ,    isnull(@st_passo,1)
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
			,	 @Ds_NomePai 
			,	 @Ds_NomeMae 
			,	 @Dt_EmissaoDocumento 
			,	 @Ds_Naturalidade 
			,	 @cd_atividadePrincipal
			,	 @nr_inscricaoestadual
			,	 @ds_numerodocumento
			,	 @ds_emailcomercial
			,    @st_pessoavinculada
            ,    @st_ativo)

    SELECT @id_cliente = SCOPE_IDENTITY();

--Procedimento para Inserir todas as pendências para o novo cliente

--Não inserir pendências para clientes importados

if (@st_passo<>4)
BEGIN

	--Declarando o Cursor
	DECLARE curPendencias
	Cursor For
	Select id_tipo_pendencia from tb_tipo_pendenciacadastral
	--Declarando uma variavel para as linhas do cursor
	Declare @id_tipo_pendencia int;
	--Abrindo o cursor
	OPEN curPendencias
	--Pegando a primeira linha
	FETCH NEXT FROM curPendencias INTO @id_tipo_pendencia
	--Pegando cada linha
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--Efetuando o procedimento: Inserindo cada uma das pendências para o novo cliente
		INSERT INTO tb_cliente_pendenciacadastral
           (id_tipo_pendencia
           ,id_cliente
           ,ds_pendencia
           ,dt_cadastropendencia
           ,dt_resolucao
           ,ds_resolucao)
		VALUES
           (@id_tipo_pendencia
           ,@id_cliente
           ,'Pendencia cadastrada autometicamente para novo cliente'
           ,getdate()
           ,null
           ,null)
		--Próxima linha do cursor
		FETCH NEXT FROM curPendencias INTO @id_tipo_pendencia
	END
	--Fechando e desalocando cursor
	CLOSE curPendencias
	DEALLOCATE curPendencias
     

END






















