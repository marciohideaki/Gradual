--Descri��o: Verificar Nome para Esqueci Senha e Esqueci Assinatura Eletr�nica.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 29/06/2010
Create PROCEDURE [dbo].login_sel_nome_sp
	 @ds_email varchar(80),
     @ds_nome  varchar(60) out
AS
	
	select 
		@ds_nome = tb_cliente.ds_nome
	from 
		tb_login,tb_cliente
	where 
		ds_email = @ds_email
		and tb_login.id_login = tb_cliente.id_login;
