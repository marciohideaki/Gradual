ALTER PROCEDURE [dbo].[prc_UsuariosPorGrupo_sel] (@CodigoGrupo INT)
AS
  SELECT       [log].[id_login]
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
    ,          '' --, d.cd_codigo as CodigoCBLC
    ,          '' --, d.cd_assessor  as CodigoCBLCAssessor
    ,          [com].[CodigoPerfilRisco]
    ,          [com].[CodigoContaCorrente]
    ,          [com].[CodigoCustodia]
    ,          [com].[st_ativo]
    FROM       [DirectTradeCadastro].[dbo].[tb_login]             AS [log]
    INNER JOIN [DirectTradeCadastro].[dbo].[tb_login_complemento] AS [com] ON [log].[id_login] = [com].[id_login]
    INNER JOIN                       [dbo].[tb_UsuariosGrupos]    AS [gru] ON CONVERT(NVARCHAR,  [log].[id_login]) = [gru].[codigoUsuario]
    WHERE [gru].[codigogrupo] = @CodigoGrupo
    ORDER By [log].[ds_nome]