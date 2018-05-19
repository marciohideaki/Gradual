

/*
	cliente_resumido_sel_sp 
		 @ds_cpfcnpj = 31940
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
     , @st_visitante               bit          = null
     , @st_cadastrado              bit          = null
     , @st_exportadosinacor        bit          = null
     , @tp_pessoa                  varchar(2)   = null
     , @st_ativo                   bit          = null
     , @st_inativo                 bit          = null
     , @st_compendenciacadastral   bit          = null
     , @st_comsolicitacaoalteracao bit          = null
	 , @cd_assessor                int          = null
AS

/* Descri��o      : Lista os clientes com base em crit�rios de sele��o
   Autor          : Ant�nio Rodrigues
   Data de cria��o: 03/05/2010
-- Altera��o --
   Descri��o: Inclus�o de + par�metros de busca
   Autor    : Ant�nio Rodrigues
   Data     : 12/05/2010
-- Altera��o --
   Descri��o: Altera��o do tipo de campo CPF de Numeric(15,0) para Varchar(15)
   Autor    : Andr� Miguel
   Data     : 18/05/2010
-- Altera��o --
   Descri��o: Inclus�o do parametro 'tp_pessoa' para identificar PJ ou PF
   Autor    : Antonio Rodrigues.
   Data     : 17/06/2010
-- Altera��o --
   Descri��o: Manuten��o para sele��o atrav�s do c�digo bovespa
   Autor    : Antonio Rodrigues.
   Data     : 18/06/2010
-- Altera��o --
   Descri��o: Manuten��o para sele��o atrav�s do passo do cadastro
   Autor    : Antonio Rodrigues.
   Data     : 22/06/2010
-- Altera��o --
   Descri��o: Inclus�o da coluna cd_assessor na consulta
   Autor    : Antonio Rodrigues.
   Data     : 28/06/2010
-- Altera��o --
   Descri��o: Inclus�o do filtro por assessor
   Autor    : Gustavo Malta Guimar�es.
   Data     : 14/09/2010
-- Altera��o --
   Descri��o: Arrumando Pesquisa para n�o duplicar quando o cliente possuir mais de uma conta BMF
   Autor    : Gustavo Malta Guimar�es.
   Data     : 13/10/2010
-- Altera��o --
   Descri��o: Pegar informa��o sobre ativa��o das contas
   Autor    : Gustavo Malta Guimar�es.
   Data     : 21/10/2010
-- Altera��o --
   Descri��o: Busca por email
   Autor    : Gustavo Malta Guimar�es.
   Data     : 22/10/2010
-- Altera��o --
   Descri��o: Buscando sem parametros
   Autor    : Andr� Cristino Miguel.
   Data     : 25/10/2010

-- Altera��o --
   Descri��o: Melhorando a performance da procedure. agora retornande muito mais r�pido.
   Autor    : Andr� Cristino Miguel.
   Data     : 26/10/2010
*/

/* Alterado por Andr� Cristino Miguel, visando melhorar a performance da PROC.*/
SELECT     
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
	,cd_gradual
	,cd_bmf
	,cd_bovespa
	,cd_bovespa_ativa
	,cd_bmf_ativa
	,dt_cadastro
	,dt_nascimento
FROM 
	tb_cliente_vw --> VIew Criada para retornar o resultado mais r�pido.
WHERE     
	(@ds_nome is null OR ds_nome   LIKE '%' + @ds_nome   + '%')
	AND (@ds_cpfcnpj is null OR ds_cpfcnpj LIKE '%' + @ds_cpfcnpj + '%')
	AND (@ds_email is null OR ds_email LIKE '%' + @ds_email + '%')
	AND (@cd_bovespa is null 
			OR (
				cd_bovespa = @cd_bovespa
				OR cd_bmf = @cd_bovespa
			)
	)
	AND (@cd_assessor is null OR cd_assessor = @cd_assessor)
	AND(
		   (@st_visitante is null OR (@st_visitante = 1 AND st_passo = 1 OR st_passo = 2))--> Verificando se � selecionado cliente visitante.
		OR (@st_cadastrado is null OR (@st_cadastrado = 1 AND st_passo = 3))              --> Verificando se � selecionado cliente cadastrado.
		OR (@st_exportadosinacor is null OR (@st_exportadosinacor = 1 AND st_passo = 4))
		OR ((@st_ativo is null 
			OR st_status = @st_ativo )
			OR (@st_inativo is null OR st_status = @st_inativo ))
			OR (@st_compendenciacadastral is null OR st_pendencia = @st_compendenciacadastral)
	)
	AND	(@tp_pessoa is null OR tp_pessoa = @tp_pessoa)
	

  




