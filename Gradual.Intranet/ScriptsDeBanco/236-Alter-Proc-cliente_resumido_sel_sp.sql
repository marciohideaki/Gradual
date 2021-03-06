set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO



/*
	cliente_resumido_sel_sp 
		 @cd_bovespa = 31940
		,@cd_assessor = 22
		,@st_compendenciacadastral = 0
		,@st_comsolicitacaoalteracao = 0
		,@st_visitante = 0
		,@st_cadastrado = 1
		,@st_exportadosinacor = 1
		,@st_ativo = 1
		,@st_inativo = 1

*/
-- Arquivo: 205-Alter-Proc-cliente_resumido_sel_sp.sql
ALTER PROCEDURE [dbo].[cliente_resumido_sel_sp]
       @cd_bovespa                 int          = null         
     , @ds_cpfcnpj                 nvarchar(15) = null
     , @ds_nome                    varchar(60)  = null
	 , @ds_email                  varchar(80)   = null
     , @st_visitante               bit          = 1
     , @st_cadastrado              bit          = 1
     , @st_exportadosinacor        bit          = 1
     , @tp_pessoa                  varchar(2)   = null
     , @st_ativo                   bit          = null
     , @st_inativo                 bit          = null
     , @st_compendenciacadastral   bit          = null
     , @st_comsolicitacaoalteracao bit          = null
	 , @cd_assessor                int          = null
AS

/* Descrição      : Lista os clientes com base em critérios de seleção
   Autor          : Antônio Rodrigues
   Data de criação: 03/05/2010
-- Alteração --
   Descrição: Inclusão de + parâmetros de busca
   Autor    : Antônio Rodrigues
   Data     : 12/05/2010
-- Alteração --
   Descrição: Alteração do tipo de campo CPF de Numeric(15,0) para Varchar(15)
   Autor    : André Miguel
   Data     : 18/05/2010
-- Alteração --
   Descrição: Inclusão do parametro 'tp_pessoa' para identificar PJ ou PF
   Autor    : Antonio Rodrigues.
   Data     : 17/06/2010
-- Alteração --
   Descrição: Manutenção para seleção através do código bovespa
   Autor    : Antonio Rodrigues.
   Data     : 18/06/2010
-- Alteração --
   Descrição: Manutenção para seleção através do passo do cadastro
   Autor    : Antonio Rodrigues.
   Data     : 22/06/2010
-- Alteração --
   Descrição: Inclusão da coluna cd_assessor na consulta
   Autor    : Antonio Rodrigues.
   Data     : 28/06/2010
-- Alteração --
   Descrição: Inclusão do filtro por assessor
   Autor    : Gustavo Malta Guimarães.
   Data     : 14/09/2010
-- Alteração --
   Descrição: Arrumando Pesquisa para não duplicar quando o cliente possuir mais de uma conta BMF
   Autor    : Gustavo Malta Guimarães.
   Data     : 13/10/2010
-- Alteração --
   Descrição: Pegar informação sobre ativação das contas
   Autor    : Gustavo Malta Guimarães.
   Data     : 21/10/2010
-- Alteração --
   Descrição: Busca por email
   Autor    : Gustavo Malta Guimarães.
   Data     : 22/10/2010
-- Alteração --
   Descrição: Buscando sem parametros
   Autor    : André Cristino Miguel.
   Data     : 25/10/2010

-- Alteração --
   Descrição: Melhorando a performance da procedure. agora retornande muito mais rápido.
   Autor    : André Cristino Miguel.
   Data     : 26/10/2010

-- Alteração --
   Descrição: Pesquisando cliente por qualquer uma de suas contas e retornando a principal.
   Autor    : Gustavo Malta Guimarães.
   Data     : 06/12/2010

*/

/* Alterado por André Cristino Miguel, visando melhorar a performance da PROC.*/
SELECT  distinct   
	 id_cliente
	,ds_nome
	,ds_cpfcnpj
	,tp_pessoa
	,cd_sexo
	,ds_email
	,st_passo
	,tp_cliente
	,ds_empresa
	,ds_nomefantasia
	,cd_assessor
	,st_pendencia
	,st_status
	, (select cd_codigo from tb_cliente_conta where tb_cliente_conta.id_cliente = tb_cliente_vw.id_cliente and tb_cliente_conta.cd_sistema = 'BOL'  and tb_cliente_conta.st_principal = 1) as cd_bovespa
	,cd_bmf
	,(select st_ativa from tb_cliente_conta where tb_cliente_conta.id_cliente = tb_cliente_vw.id_cliente and tb_cliente_conta.cd_sistema = 'BOL'  and tb_cliente_conta.st_principal = 1) as cd_bovespa_ativa
	,cd_bmf_ativa
	,dt_cadastro
	,dt_nascimento
FROM 
	tb_cliente_vw --> VIew Criada para retornar o resultado mais rápido.
WHERE     
	    (@ds_nome    IS NULL OR ds_nome    LIKE '%' + @ds_nome    + '%')
	AND (@ds_cpfcnpj IS NULL OR ds_cpfcnpj LIKE '%' + @ds_cpfcnpj + '%')
	AND (@ds_email   IS NULL OR ds_email   LIKE '%' + @ds_email   + '%')
	AND (@cd_bovespa IS NULL 
		OR (
				(cd_bovespa = @cd_bovespa or cd_bmf = @cd_bovespa ) 
			)
	)
	AND (@cd_assessor is null OR cd_assessor = @cd_assessor)
	AND(
		    (@st_visitante        = 1 OR (@st_visitante        = 0 AND (st_passo > 2)))  --> Verificando se é selecionado cliente visitante.
		AND (@st_cadastrado       = 1 OR (@st_cadastrado       = 0 AND (st_passo != 3))) --> Verificando se é selecionado cliente cadastrado.
		AND (@st_exportadosinacor = 1 OR (@st_exportadosinacor = 0 AND (st_passo != 4)))
		
        AND (@st_compendenciacadastral IS NULL OR st_pendencia = @st_compendenciacadastral)

        AND ((@st_ativo IS NULL OR st_loginativo = @st_ativo) OR (@st_inativo IS NULL OR st_loginativo = @st_inativo))
		)

	
	AND	(@tp_pessoa is null OR tp_pessoa = @tp_pessoa)
	

  






