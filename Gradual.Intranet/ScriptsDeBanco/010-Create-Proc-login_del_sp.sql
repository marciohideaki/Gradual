/*
	Realiza a exclusão de um registro da 'tb_login'.
*/
CREATE PROCEDURE login_del_sp
                 @id_login bigint
AS
    DELETE       
    FROM   tb_login
    WHERE  tb_login.id_login = @id_login