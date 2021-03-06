set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO




ALTER PROCEDURE [dbo].[cliente_resumido_sel_sp]
       @cd_bovespa                 int          = null         
     , @ds_cpfcnpj                 nvarchar(15) = null
     , @ds_nome                    varchar(60)  = null
	 , @ds_email                  varchar(80)  = null
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
*/

SELECT     [cli].[id_cliente]
,          [cli].[ds_nome]
,          [cli].[ds_cpfcnpj]
,          [cli].[tp_pessoa]
,          [cli].[cd_sexo]
,          [log].[ds_email]
,          [cli].[st_passo]
,          [cli].[tp_cliente]
,		   [cli].[ds_empresa]
,		   [cli].[ds_nomefantasia]
,          CASE WHEN ([cli].[st_passo] = 4) OR ([cli].[id_assessorinicial] IS NULL) OR ([cli].[id_assessorinicial] = 0) THEN [bov].[cd_assessor] ELSE [cli].[id_assessorinicial] END AS [cd_assessor]
,          ISNULL(dbo.status_cliente_ativo_fn([cli].[id_cliente]), 0)        AS [st_status]
,          ISNULL(dbo.pendencia_cadastral_cliente_fn([cli].[id_cliente]), 0) AS [st_pendencia]
,          [bov].[cd_codigo]             AS [cd_gradual]
,          [bmf].[cd_codigo]             AS [cd_bmf]
,          [bov].[cd_codigo]             AS [cd_bovespa]
,          [bov].[st_ativa]             AS [cd_bovespa_ativa]
,          [bmf].[st_ativa]             AS [cd_bmf_ativa]
,          [cli].[dt_passo1]             AS [dt_cadastro]
,          [cli].[dt_nascimentofundacao] AS [dt_nascimento]
FROM       [dbo].[tb_cliente]            AS [cli]
LEFT  JOIN [dbo].[tb_login]              AS [log] ON [log].[id_login] = [cli].[id_login]
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bov] ON LOWER([bov].[cd_sistema]) = 'bol' AND  [cli].[id_cliente] = [bov].[id_cliente] and [bov].[st_principal] = 1 
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bmf] ON 
		( 
			LOWER([bmf].[cd_sistema]) = 'bmf' AND [cli].[id_cliente] = [bmf].[id_cliente] AND  [bmf].[cd_codigo] = 
				ISNULL( (select cd_codigo from tb_cliente_conta where LOWER(cd_sistema) = 'bmf' and cd_codigo = bov.cd_codigo and id_cliente = cli.id_cliente),
						(select max(cd_codigo) from tb_cliente_conta where LOWER(cd_sistema) = 'bmf' and id_cliente = cli.id_cliente) )
		)
WHERE     (		
				LOWER([cli].[ds_nome])   LIKE '%' + LOWER(ISNULL(@ds_nome   , [cli].[ds_nome]))   + '%'
				AND   [cli].[ds_cpfcnpj] LIKE '%' + ISNULL(@ds_cpfcnpj, [cli].[ds_cpfcnpj]) + '%'
				AND   [log].[ds_email] LIKE '%' + ISNULL(@ds_email, [log].[ds_email]) + '%'
				AND   (@cd_bovespa IS NULL OR [cli].[id_cliente] IN (SELECT [cco].[id_cliente] FROM [dbo].[tb_cliente_conta] AS [cco] WHERE LOWER([cco].[cd_sistema]) = 'bol' AND [cco].[cd_codigo] = @cd_bovespa))
		   )
AND       (
			 		 @st_visitante  = 1       AND ([cli].[st_passo] = 1 OR [cli].[st_passo] = 2) --> Verificando se é selecionado cliente visitante.
				OR	(@st_cadastrado = 1       AND ([cli].[st_passo] = 3))                        --> Verificando se é selecionado cliente cadastrado.
				OR	(@st_exportadosinacor = 1 AND ([cli].[st_passo] = 4))
           )
AND       (     [cli].[st_ativo] = @st_ativo OR [cli].[st_ativo] = @st_inativo)
AND		  (
                    (ISNULL(@tp_pessoa, [cli].[tp_pessoa]) = [cli].[tp_pessoa])
				OR	(@st_compendenciacadastral = 1 AND EXISTS             --> Verificando na tabela tb_cliente_pendenciacadastral se o cliente possui alguma pendência.
						(
							SELECT [pen].[id_cliente] FROM [dbo].[tb_cliente_pendenciacadastral] AS [pen]
							WHERE  [pen].[id_cliente]    = [cli].[id_cliente]
							AND    [pen].[dt_resolucao] IS NULL
						))
				--OR  (@st_comsolicitacaoalteracao = 1)
			  )
--filtrando por assessor
AND ( cli.st_passo < 4 and cli.id_assessorinicial = isnull(@cd_assessor,cli.id_assessorinicial)
	OR
	cli.st_passo = 4 and bov.cd_assessor = isnull(@cd_assessor,bov.cd_assessor )
  )




