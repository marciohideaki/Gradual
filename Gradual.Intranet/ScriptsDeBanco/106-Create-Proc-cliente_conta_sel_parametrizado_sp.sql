CREATE PROCEDURE cliente_conta_sel_parametrizado_sp
	@ds_nome    varchar(30) = NULL,
	@cd_sistema VARCHAR(4)  = NULL,
	@ds_cpfcnpj varchar(30) = NULL,
	@id_login   int
AS
SELECT     MAX([con].[cd_codigo])   AS [cd_codigo]
FROM       [dbo].[tb_cliente_conta] AS [con]
INNER JOIN [dbo].[tb_cliente]       AS [cli] ON [cli].[id_cliente] = [con].[id_cliente]
WHERE      LOWER([con].[cd_sistema])LIKE  '%' + LOWER(@cd_sistema) + '%'
AND        LOWER([cli].[ds_nome])   LIKE  '%' + ISNULL(LOWER(@ds_nome), LOWER([cli].[ds_nome])) + '%'
AND        [cli].[ds_cpfcnpj] LIKE ISNULL(@ds_cpfcnpj, [cli].[ds_cpfcnpj])
AND        [cli].[id_login]   LIKE ISNULL(@id_login,   [cli].[id_login]);