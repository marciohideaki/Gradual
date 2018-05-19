set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


/*
	Realiza a atualização do email da tb_login'
*/

--Alteração:         Altera o endereço de e-mail para um registro.
--Realizada por:     Antônio Rodrigues
--Data da Alteração: 14/06/2010

CREATE PROCEDURE [dbo].[login_email_upd_sp]
                 @id_login int
               , @ds_email varchar(80)
AS
    UPDATE tb_login
    SET    tb_login.ds_email = @ds_email
    WHERE  tb_login.id_login = @id_login



