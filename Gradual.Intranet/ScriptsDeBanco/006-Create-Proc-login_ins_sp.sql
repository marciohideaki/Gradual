CREATE PROCEDURE login_upd_sp
               , @cd_senha                varchar(32)
               , @cd_assinaturaeletronica varchar(32)
               , @nr_tentativaserradas    numeric(2, 0)
               , @id_frase                bigint
               , @ds_respostafrase        varchar(100)
               , @dt_ultimaexpiracao      datetime
               , @ds_login                varchar(50)
               , @ds_email                varchar(80)
AS
    INSERT INTO tb_login
           (    cd_senha
           ,    cd_assinaturaeletronica
           ,    nr_tentativaserradas
           ,    id_frase
           ,    ds_respostafrase
           ,    dt_ultimaexpiracao
           ,    ds_login
           ,    ds_email)
    VALUES (    @cd_senha
           ,    @cd_assinaturaeletronica
           ,    @nr_tentativaserradas
           ,    @id_frase
           ,    @ds_respostafrase
           ,    @dt_ultimaexpiracao
           ,    @ds_login
           ,    @ds_email)