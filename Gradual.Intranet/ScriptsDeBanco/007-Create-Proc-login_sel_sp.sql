/*
	Realiza a seleção de um registro da 'tb_login' baseado no id.
*/
CREATE PROCEDURE login_sel_sp
                 @id_login                bigint
AS
    SELECT tb_login.id_login
         , tb_login.cd_senha               
		 , tb_login.cd_assinaturaeletronica
		 , tb_login.nr_tentativaserradas   
		 , tb_login.id_frase               
		 , tb_login.ds_respostafrase       
		 , tb_login.dt_ultimaexpiracao     
		 , tb_login.ds_login               
		 , tb_login.ds_email               
		 , tb_login.id_login               
    FROM   tb_login
    WHERE  tb_login.id_login = @id_login