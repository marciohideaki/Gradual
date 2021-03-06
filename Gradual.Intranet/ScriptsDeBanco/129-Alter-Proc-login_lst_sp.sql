set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
/*
	Realiza a seleção de todos os registros da 'tb_login'.
*/

--Alteração:   Inserir campos tp_acesso e cd_assessor, e retirar campo ds_login
--Realizada por: Gustavo
--Data da Alteração: 08/06/2010

--Alteração:   Inserir campo ds_nome para o controle de acesso
--Realizada por: Gustavo
--Data da Alteração: 09/06/2010

ALTER PROCEDURE [dbo].[login_lst_sp]
AS
    SELECT tb_login.id_login
         , tb_login.cd_senha               
		 , tb_login.cd_assinaturaeletronica
		 , tb_login.nr_tentativaserradas   
		 , tb_login.id_frase               
		 , tb_login.ds_respostafrase       
		 , tb_login.dt_ultimaexpiracao     
		 , tb_login.tp_acesso    
		,  tb_login.cd_assessor                
		 , tb_login.ds_email               
		 , tb_login.id_login 
		,  tb_login.ds_nome            
    FROM   tb_login

