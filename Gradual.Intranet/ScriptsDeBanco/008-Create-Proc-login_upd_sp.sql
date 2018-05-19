/*
	Realiza a atualização dos dados da tb_login'
*/
CREATE PROCEDURE login_upd_sp
                 @id_login                bigint
               , @cd_senha                varchar(32)
               , @cd_assinaturaeletronica varchar(32)
               , @nr_tentativaserradas    numeric(2, 0)
               , @id_frase                bigint
               , @ds_respostafrase        varchar(100)
               , @dt_ultimaexpiracao      datetime
               , @ds_login                varchar(50)
               , @ds_email                varchar(80)
AS
    UPDATE tb_login
    SET    tb_login.cd_senha                = @cd_senha
    ,      tb_login.cd_assinaturaeletronica = @cd_assinaturaeletronica
    ,      tb_login.nr_tentativaserradas    = @nr_tentativaserradas
    ,      tb_login.id_frase                = @id_frase
    ,      tb_login.ds_respostafrase        = @ds_respostafrase
    ,      tb_login.dt_ultimaexpiracao      = @dt_ultimaexpiracao
    ,      tb_login.ds_login                = @ds_login
    ,      tb_login.ds_email                = @ds_email
    WHERE  tb_login.id_login                = @id_login