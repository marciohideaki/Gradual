--Descri��o: Alterar Assinatura Eletr�nica.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 29/06/2010
alter PROCEDURE [dbo].login_assinatura_upd_sp
	 @id_login int,
     @cd_assinaturaantiga varchar(32),
     @cd_assinaturanova varchar(32)
AS
	declare 
		@assinaturaok int;
		
	select 
		@assinaturaok = count(*)
	from 
		tb_login
	where 
		id_login = @id_login
		and cd_assinaturaeletronica = @cd_assinaturaantiga;

	if @assinaturaok = 0
	begin
		--levantar Exception	
		RAISERROR('A assinatura eletr�nica atual n�o confere',16,1)
	end
	else
	begin
		
		update
			tb_login
		set
			cd_assinaturaeletronica = @cd_assinaturanova
		where 
			id_login = @id_login;		
		
	end