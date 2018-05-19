/*
Criada por: André Cristino Miguel
Objetivo: 
	Melhorar a performance da busca de clientes.
*/
CREATE VIEW tb_cliente_vw 
AS

SELECT     
	cli.id_cliente
	,cli.ds_nome
	,cli.ds_cpfcnpj
	,cli.tp_pessoa
	,cli.cd_sexo
	,[log].ds_email
	,cli.st_passo
	,cli.tp_cliente
	,cli.ds_empresa
	,cli.ds_nomefantasia
	,
		CASE WHEN ([cli].[st_passo] = 4) 
				OR ([cli].[id_assessorinicial] IS NULL) 
				OR ([cli].[id_assessorinicial] = 0) 
			THEN [bov].[cd_assessor] 
			ELSE [cli].[id_assessorinicial] 
			END AS cd_assessor
	,	
		CASE WHEN (select count(id_cliente) 
				from tb_cliente_pendenciacadastral 
				where id_cliente = cli.id_cliente
				AND dt_resolucao is null) > 0
			THEN 1
			ELSE 0
			END AS st_pendencia
	,bov.st_ativa AS st_status
	,bov.cd_codigo AS cd_gradual
	,bmf.cd_codigo AS cd_bmf
	,bov.cd_codigo AS cd_bovespa
	,bov.st_ativa AS cd_bovespa_ativa
	,bmf.st_ativa AS cd_bmf_ativa 
	,cli.dt_passo1 AS dt_cadastro
	,cli.dt_nascimentofundacao AS dt_nascimento
FROM 
	dbo.tb_cliente AS cli 
	LEFT JOIN dbo.tb_login AS [log] 
		ON [log].id_login = cli.id_login 
	LEFT JOIN dbo.tb_cliente_conta AS bov 
		ON LOWER(bov.cd_sistema) = 'bol'
			AND cli.id_cliente = bov.id_cliente 
			AND bov.st_principal = 1 
	LEFT JOIN dbo.tb_cliente_conta AS bmf 
		ON bmf.cd_sistema = 'bmf' 
			AND cli.id_cliente = bmf.id_cliente 
			AND bmf.cd_codigo = ISNULL
            ((
				SELECT cd_codigo
				FROM   dbo.tb_cliente_conta
                WHERE  cd_sistema = 'bmf' 
					AND cd_codigo = bov.cd_codigo 
					AND id_cliente = cli.id_cliente),(
				SELECT MAX(cd_codigo) AS Expr1
				FROM dbo.tb_cliente_conta
                WHERE cd_sistema = 'bmf'
					AND   id_cliente = cli.id_cliente))


GO