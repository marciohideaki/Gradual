-- Descrição:       Realiza a consulta de dados para o login
-- Autor:           Antônio Rodrigues
-- Data de criação: 2011 01 20

CREATE PROCEDURE login_sel_email_sp
( @id_login                INT         = NULL
, @cd_codigo               INT         = NULL
, @ds_email                VARCHAR(80) = NULL
, @cd_assinaturaeletronica VARCHAR(32) = NULL
, @cd_senha                VARCHAR(32) = NULL
, @ds_nome                 VARCHAR(60) = NULL)
AS
   SELECT *
   FROM   vw_Login
   WHERE  id_login                = ISNULL(@id_login, id_login)
   AND    ds_email                = ISNULL(@ds_email, ds_email)
   AND    CodigoCBLC              = ISNULL(@cd_codigo, CodigoCBLC)
   AND    cd_assinaturaeletronica = ISNULL(@cd_assinaturaeletronica, cd_assinaturaeletronica)
   AND    cd_senha                = ISNULL(@cd_senha, cd_senha)
   AND    LOWER(ds_nome)          LIKE '%' + LOWER(ISNULL(@ds_nome, ds_nome)) + '%'



