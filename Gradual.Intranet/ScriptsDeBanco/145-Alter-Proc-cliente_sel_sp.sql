set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


--cliente_sel_sp 153
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
--                            ds_emailcomercial VARCHAR2(80)

-- Autor Ultima Alteração   : Bruno Varandas Ribeiro
-- Data da ultima alteração : 11/06/2010
-- Motivo                   : Inclusão da variável: 
--                            @ds_numerodocumento VARCHAR2(16) 
--                            @ds_emailcomercial VARCHAR2(80) 

-- Autor Ultima Alteração   : Antônio Rodrigues
-- Data da ultima alteração : 14/06/2010
-- Motivo                   : Inclusão do campo: ds_email

-- Autor Ultima Alteração   : Antônio Rodrigues
-- Data da ultima alteração : 16/06/2010
-- Motivo                   : Inclusão do campo: st_pessoavinculada


ALTER PROCEDURE [dbo].[cliente_sel_sp]
                 @id_cliente  int
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
    WHERE  [cli].[id_cliente] = (@id_cliente)













