set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[cliente_resumido_sel_sp]
       @cd_bovespa                 bigint         
     , @ds_cpfcnpj                 numeric(15, 0)
     , @ds_nome                    varchar(60)
     , @tp_cliente                 numeric(2, 0)
     , @st_ativo                   bit
     , @st_inativo                 bit
     , @st_visitante               bit
     , @st_cadastrado              bit
     , @st_exportadosinacor        bit
     , @st_compendenciacadastral   bit
     , @st_comsolicitacaoalteracao bit
AS
/* Descrição      : Lista os clientes com base em critérios de seleção
   Autor          : Antônio Rodrigues
   Data de criação: 03/05/2010
-- Alteração --
   Descrição: Inclusão de + parâmetros de busca
   Autor    : Antônio Rodrigues
   Data     : 12/05/2010
*/
SELECT     [cli].[id_cliente]
,          [cli].[ds_nome]
,          [cli].[ds_cpfcnpj]
,          [cli].[st_passo]
,          [cli].[tp_cliente]
,          [cli].[cd_sexo]
,          [log].[ds_email]
,          [cli].[st_passo]
,          [cli].[tp_cliente]
,          ISNULL(dbo.status_cliente_ativo_fn([cli].[id_cliente]), 0)        AS [st_status]
,          ISNULL(dbo.pendencia_cadastral_cliente_fn([cli].[id_cliente]), 0) AS [st_pendencia]
,          [bov].[cd_codigo]             AS [cd_gradual]
,          [bmf].[cd_codigo]             AS [cd_bmf]
,          [bov].[cd_codigo]             AS [cd_bovespa]
,          [cli].[dt_passo1]             AS [dt_cadastro]
,          [cli].[dt_nascimentofundacao] AS [dt_nascimento]
FROM       [dbo].[tb_cliente]            AS [cli]
INNER JOIN [dbo].[tb_login]              AS [log] ON [log].[id_login] = [cli].[id_login]
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bov] ON lower([bov].[cd_sistema]) = 'bov' AND [cli].[id_cliente] = [bov].[id_cliente]
LEFT  JOIN [dbo].[tb_cliente_conta]      AS [bmf] ON lower([bmf].[cd_sistema]) = 'bmf' AND [cli].[id_cliente] = [bmf].[id_cliente]
WHERE      (		lower([cli].[ds_nome]) LIKE '%' + lower(@ds_nome) + '%'
				OR	[cli].[ds_cpfcnpj] = @ds_cpfcnpj
				OR  EXISTS
					  ( --> Valida a existência do cod Bovespa.
							SELECT  [clc].[id_cliente]
							FROM    [dbo].[tb_cliente_conta] AS [clc]
							WHERE   lower([clc].[cd_sistema]) = 'bov'
							AND     [clc].[id_cliente] = [cli].[id_cliente]
							AND     [clc].[cd_codigo]  = @cd_bovespa
							AND     ([clc].[st_ativa]  = @st_ativo OR [clc].[st_ativa] != @st_inativo)
					  )
		    )
AND		   (
			 		 @st_visitante  = 1 AND ([cli].[st_passo] = 1 OR [cli].[st_passo] = 2) --> Verificando se é selecionado cliente visitante.
				OR	(@st_cadastrado = 1 AND ([cli].[st_passo] = 3))                        --> Verificando se é selecionado cliente cadastrado.
				OR	(@st_exportadosinacor = 1 AND ([cli].[st_passo] = 4))
                OR  (@tp_cliente = [cli].[tp_cliente])
				OR	(@st_compendenciacadastral = 1 AND EXISTS                              --> Verificando na tabela tb_cliente_pendenciacadastral se o cliente possui alguma pendência.
						(
							SELECT [pen].[id_cliente] FROM [dbo].[tb_cliente_pendenciacadastral] AS [pen]
							WHERE  [pen].[id_cliente] = [cli].[id_cliente]
							AND    [pen].[dt_resolucao] IS NULL
						))
				OR  (@st_comsolicitacaoalteracao = 1)
			  )

