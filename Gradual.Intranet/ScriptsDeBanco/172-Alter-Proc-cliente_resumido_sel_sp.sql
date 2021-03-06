set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



ALTER PROCEDURE [dbo].[cliente_resumido_sel_sp]
       @cd_bovespa                 int          = null         
     , @ds_cpfcnpj                 nvarchar(15) = null
     , @ds_nome                    varchar(60)  = null
     , @st_visitante               bit          = null
     , @st_cadastrado              bit          = null
     , @st_exportadosinacor        bit          = null
     , @tp_pessoa                  varchar(2)   = null
     , @st_ativo                   bit          = null
     , @st_inativo                 bit          = null
     , @st_compendenciacadastral   bit          = null
     , @st_comsolicitacaoalteracao bit          = null
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
   Descrição: Inclusão da coluna cd_assessor
   Autor    : Antonio Rodrigues.
   Data     : 28/06/2010
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
,          [cli].[dt_passo1]             AS [dt_cadastro]
,          [cli].[dt_nascimentofundacao] AS [dt_nascimento]
FROM       [dbo].[tb_cliente]            AS [cli]
LEFT  JOIN [dbo].[tb_login]              AS [log] ON [log].[id_login] = [cli].[id_login]
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bmf] ON LOWER([bmf].[cd_sistema]) = 'bmf' AND [cli].[id_cliente] = [bmf].[id_cliente]
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bov] ON LOWER([bov].[cd_sistema]) = 'bol' 
      AND  [cli].[id_cliente]             = [bov].[id_cliente] 
      AND  [bov].[cd_codigo]              = 
           (
                SELECT MAX([tcc].[cd_codigo]) FROM [dbo].[tb_cliente_conta] AS [tcc] WHERE LOWER([tcc].[cd_sistema]) = 'bol' AND [tcc].[id_cliente] = [cli].[id_cliente]
           )
WHERE     (		
				LOWER([cli].[ds_nome])   LIKE '%' + LOWER(ISNULL(@ds_nome   , [cli].[ds_nome]))   + '%'
				AND   [cli].[ds_cpfcnpj] LIKE             ISNULL(@ds_cpfcnpj, [cli].[ds_cpfcnpj]) + '%'
				AND   [cli].[id_cliente] = ISNULL((SELECT [cco].[id_cliente] FROM [dbo].[tb_cliente_conta] AS [cco] WHERE LOWER([cco].[cd_sistema]) = 'bol' AND [cco].[cd_codigo] = @cd_bovespa), [cli].[id_cliente])
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







