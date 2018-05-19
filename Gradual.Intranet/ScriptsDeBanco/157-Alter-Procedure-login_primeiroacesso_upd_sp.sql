--Descrição: Salvando primeiro acesso.
--Autor: Gustavo Malta Guimarães
--Data de criação: 25/06/2010
Create PROCEDURE [dbo].login_primeiroacesso_upd_sp
	 @id_login int,
     @cd_assinaturaeletronica varchar(32),
     @cd_senha varchar(32),
     @ds_email varchar(80)
AS
	declare 
		@emailexiste int;
		
	select 
		@emailexiste = count(*)
	from 
		tb_login
	where 
		lower(rtrim(ltrim(ds_email))) = lower(rtrim(ltrim(@ds_email)))
		and id_login <> @id_login;

	if @emailexiste > 0
	begin
		--levantar Exception	
		RAISERROR('Email já cadastrado para outro usuário',16,1)
	end
	else
	begin
		
		update
			tb_login
		set
			cd_assinaturaeletronica = @cd_assinaturaeletronica,
			cd_senha = @cd_senha,
			ds_email = @ds_email,
			nr_tentativaserradas = 0,
			dt_ultimaexpiracao = getdate()
		where 
			id_login = @id_login;		
		
	end

	