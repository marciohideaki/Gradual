/*
	Realiza a seleção de todos os registros da 'tb_login'.
*/
CREATE PROCEDURE login_lst_sp
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