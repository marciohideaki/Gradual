set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[prc_relatorio_palestra_lst]
( @dt_inicio     DATETIME
, @dt_fim        DATETIME
, @Id_tema       INT = NULL
, @Id_Localidade INT = NULL
, @ID_ASSESSOR   INT = NULL
, @St_Situacao   INT = NULL)
AS
   SELECT      [log].[ds_nome]
   ,           [cli].[cd_sexo]
   ,           [cli].[ds_cpfcnpj]
   ,           [log].[ds_email]
   ,           [cli].[dt_nascimentofundacao]
   ,           [cli].[id_cliente]
   ,           [tcp].[id_assessor]
   ,           CASE WHEN [cli].[st_passo] = 1 THEN 'Visitante' WHEN [cli].[st_passo] = 2 THEN 'Visitante' WHEN [cli].[st_passo] = 3 THEN 'Cadastrado' ELSE 'CBLC' END AS [cliente_gradual]
   ,           'S' AS [st_aut_cadasto_visitante]
   ,           'S' AS [st_aut_contato]
   ,           [ccp].[st_confirmainscricao]
   ,           [ccp].[st_presenca]
   ,           [tcp].[dt_datahoracurso]
   ,           [loc].[ds_localidade]
   ,           [tcp].[st_situacao]
   ,           [tma].[ds_titulo] AS [ds_palestra_tema]
   ,           REPLACE(REPLACE([fpl].[ds_faixa_etaria]  , 'rdb', ''), 'NI', 'NaoInformou') AS [ds_faixa_etaria]
   ,           [fpl].[ds_ocupacao]
   ,           REPLACE(REPLACE([fpl].[ds_conhecimento]  , 'rdb', ''), 'NI', 'NaoInformou') AS [ds_conhecimento]
   ,           REPLACE(REPLACE([fpl].[tp_investidor]    , 'rdb', ''), 'NI', 'NaoInformou') AS [tp_investidor]
   ,           REPLACE(REPLACE([fpl].[tp_investimento]  , 'rdb', ''), 'NI', 'NaoInformou') AS [tp_investimento]
   ,           REPLACE(REPLACE([fpl].[tp_instituicao]   , 'rdb', ''), 'NI', 'NaoInformou') AS [tp_instituicao]
   ,           REPLACE(REPLACE([fpl].[ds_renda_familiar], 'rdb', ''), 'NI', 'NaoInformou') AS [ds_renda_familiar]
   FROM        [educacional].[dbo].[tb_cliente_curso_palestra] AS [ccp]
   INNER JOIN  [educacional].[dbo].[tb_curso_palestra]         AS [tcp] ON [tcp].[id_cursopalestra] = [ccp].[id_cursopalestra]
   INNER JOIN  [educacional].[dbo].[tb_localidade]             AS [loc] ON [loc].[id_localidade]    = [tcp].[id_localidade]
   INNER JOIN  [educacional].[dbo].[tb_tema]                   AS [tma] ON [tma].[id_tema]          = [tcp].[id_tema]
   LEFT  JOIN  [educacional].[dbo].[tb_ficha_perfil]           AS [fpl] ON [fpl].[id_cliente]       = [ccp].[id_cliente]
   INNER JOIN  [DirectTradeCadastro].[dbo].[tb_cliente]                   AS [cli] ON [cli].[id_cliente]       = [ccp].[id_cliente]
   INNER JOIN  [DirectTradeCadastro].[dbo].[tb_login]                     AS [log] ON [log].[id_login]         = [cli].[id_login]
   WHERE       [tcp].[id_localidade]     = ISNULL(@id_localidade, [tcp].[id_localidade])
   AND         [tcp].[id_assessor]       = ISNULL(@id_assessor  , [tcp].[id_assessor])
   AND         [tcp].[id_tema]           = ISNULL(@id_tema      , [tcp].[id_tema])
   AND         [tcp].[st_situacao]       = ISNULL(@st_situacao  , [tcp].[st_situacao])
   AND         [tcp].[dt_datahoracurso] BETWEEN @dt_inicio AND @dt_fim
   ORDER BY    [ccp].[dt_cadastro];