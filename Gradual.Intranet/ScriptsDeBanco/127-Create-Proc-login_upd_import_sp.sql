set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

/*
	Realiza a atualização dos dados da tb_login'
*/

--Motivo:   Importação de Logins
--Realizada por: Gustavo
--Data da Criação: 08/06/2010

CREATE PROCEDURE [dbo].[login_upd_import_sp]
                 @id_login                int
              , @nr_tentativaserradas    numeric(2, 0)
               , @id_frase                int
               , @ds_respostafrase        varchar(100)
               , @dt_ultimaexpiracao      datetime
              	,@tp_acesso               int
				,@cd_assessor             numeric(5,0)
               , @ds_email                varchar(80)
				,@cd_senha           varchar(32)
				,@cd_assinaturaeletronica   varchar(32)
AS
    UPDATE tb_login
    SET    tb_login.nr_tentativaserradas    = @nr_tentativaserradas
    ,      tb_login.id_frase                = @id_frase
    ,      tb_login.ds_respostafrase        = @ds_respostafrase
    ,      tb_login.dt_ultimaexpiracao      = @dt_ultimaexpiracao
  	,      tb_login.tp_acesso				= @tp_acesso            
	,		tb_login.cd_assessor			= @cd_assessor            
    ,      tb_login.ds_email                = @ds_email
	,      tb_login.cd_senha=    @cd_senha         
	,		tb_login.cd_assinaturaeletronica  =     @cd_assinaturaeletronica  
    WHERE  tb_login.id_login                = @id_login


