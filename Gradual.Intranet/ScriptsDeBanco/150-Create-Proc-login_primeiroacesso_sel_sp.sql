

--Descrição: Verificação de senha para identificar se cliente realizou o primeiro acesso.
--Autor: Bruno Gustavo Malta Guimarães
--Data de criação: 24/06/2010
ALTER PROCEDURE [dbo].login_primeiroacesso_sel_sp
	@ds_email  varchar(80), 
    @cd_codigo  numeric(7,0),
    @cd_senha  varchar(32) out
AS
	if  @cd_codigo = 0
	begin
		--por email
		select 
			@cd_senha =	cd_senha 
		from 
			tb_login 
		where 
			lower(ds_email) =  lower(@ds_email)
			and tp_acesso = 0;
	end
	else
	begin
		--por codigo
		select 
			@cd_senha = cd_senha 
		from 
			tb_login,tb_cliente,tb_cliente_conta 
		where tb_cliente_conta.cd_codigo = @cd_codigo
			and tb_login.id_login = tb_cliente.id_login
			and tb_cliente_conta.id_cliente = tb_cliente.id_cliente
			and tp_acesso = 0;
	end


