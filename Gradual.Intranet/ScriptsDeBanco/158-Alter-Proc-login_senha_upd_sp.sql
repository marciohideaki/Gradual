--Descrição: Alterar Senha.
--Autor: Gustavo Malta Guimarães
--Data de criação: 29/06/2010
Create PROCEDURE [dbo].login_senha_upd_sp
	 @id_login int,
     @cd_senhaantiga varchar(32),
     @cd_senhanova varchar(32)
AS
	declare 
		@senhaok int;
		
	select 
		@senhaok = count(*)
	from 
		tb_login
	where 
		id_login = @id_login
		and cd_senha = @cd_senhaantiga;

	if @senhaok = 0
	begin
		--levantar Exception	
		RAISERROR('A senha atual não confere',16,1)
	end
	else
	begin
		
		update
			tb_login
		set
			cd_senha = @cd_senhanova
		where 
			id_login = @id_login;		
		
	end

	