ALTER PROCEDURE [dbo].[prc_UsuariosPorPerfil_sel] (@CodigoPerfil int)
AS
	select     [log].[id_login]
    ,          [log].[cd_senha]
    ,          [log].[cd_assinaturaeletronica]
    ,          [log].[nr_tentativaserradas]
    ,          [log].[id_frase]
    ,          [log].[ds_respostafrase]
    ,          [log].[dt_ultimaexpiracao]
    ,          [log].[tp_acesso]
    ,          [log].[cd_assessor]
    ,          [log].[ds_email]
    ,          [log].[ds_nome]
    ,          '' AS [CodigoCBLC]
    ,          '' AS [CodigoCBLCAssessor]
    ,          '' AS [CodBMF]
    ,          [com].[CodigoPerfilRisco]
    ,          [com].[CodigoContaCorrente]
    ,          [com].[CodigoCustodia]
    ,          [com].[st_ativo]
	FROM                                   [dbo].[tb_login]             AS [log]
	INNER JOIN                             [dbo].[tb_login_complemento] AS [com] ON [log].[id_login] = [com].[id_login]
	INNER JOIN [DirectTradeControleAcesso].[dbo].[tb_UsuariosPerfis]    AS [per] ON CONVERT(NVARCHAR,  [log].[id_login]) = [per].[codigoUsuario]
	WHERE      [per].[codigoPerfil] = @CodigoPerfil
	ORDER By   [log].[ds_nome]