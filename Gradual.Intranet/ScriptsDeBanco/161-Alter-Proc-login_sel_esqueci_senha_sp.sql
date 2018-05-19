--Descri��o: Gera uma nova Senha.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 29/06/2010
Create PROCEDURE [dbo].login_sel_esqueci_senha_sp
	@ds_email varchar(80),
    @ds_CpfCnpj varchar(15),
    @dt_NascimentoFundacao datetime,
    @cd_Senha varchar(32) 
AS
	declare 
		@dadosok int;
		
	select 
		@dadosok = count(*)
	from 
		tb_login,tb_cliente
	where 
		tb_login.id_login = tb_cliente.id_login
		and tb_login.ds_email = @ds_email
		and convert(numeric,tb_cliente.ds_cpfcnpj) = convert(numeric,@ds_CpfCnpj)
		and tb_cliente.dt_NascimentoFundacao = @dt_NascimentoFundacao ;

	if @dadosok = 0
	begin
		--levantar Exception	
		RAISERROR('Os dados informados est�o incorretos. Em caso de d�vidas, entre em contato com a Central de Atendimento.',16,1)
	end
	else
	begin
		
		update
			tb_login
		set
			cd_senha = @cd_senha
		where 
			ds_email = @ds_email;		
		
	end
