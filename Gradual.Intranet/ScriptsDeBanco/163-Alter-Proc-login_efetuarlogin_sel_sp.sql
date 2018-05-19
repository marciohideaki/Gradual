--Descrição: Efetuar Login.
--Autor: Gustavo Malta Guimarães
--Data de criação: 30/06/2010
Alter PROCEDURE [dbo].login_efetuarlogin_sel_sp
     @ds_email varchar(80),
     @cd_Codigo numeric,
     @Cd_Senha varchar(32),

     @Id_Cliente numeric out,
     @Id_Login numeric out,
     @Ds_Nome varchar(80) out,
     @Ds_EmailRetorno varchar(80) out,
     @Ds_CpfCnpj varchar(15) out,
     @Cd_CodigoPrincipal numeric out,
     @Cd_AssessorPrincipal numeric out,
     @St_Passo numeric out,
     @Dt_NascimentoFundacao datetime out,
     @Tp_Pessoa varchar(1) out,	 
	 @Dt_Passo1 datetime out

AS
	declare 
		@loginexiste int;

		if @cd_Codigo = 0
		Begin
		--Login por email
			select 
				@loginexiste = count(*)
			from 
				tb_login,tb_cliente
			where 
				lower(rtrim(ltrim(ds_email))) = lower(rtrim(ltrim(@ds_email)))
				and cd_senha = @cd_senha
				and tb_cliente.id_login = tb_login.id_login
				and tb_cliente.st_ativo = 1 
				and tb_login.tp_acesso = 0; --cliente
		end
		else
		Begin
		--Login por codigo
			select 
				@loginexiste = count(*)
			from 
				tb_login,tb_cliente,tb_cliente_conta
			where 
				cd_senha = @cd_senha
				and tb_cliente.id_login = tb_login.id_login
				and tb_cliente.st_ativo = 1 
				and tb_cliente_conta.id_cliente = tb_cliente.id_cliente
				and tb_cliente_conta.cd_Codigo = @cd_Codigo
				and tb_login.tp_acesso = 0; -- cliente
		end
		
	if @loginexiste = 0
	begin
		--levantar Exception	
		RAISERROR('O usuário e/ou senha estão incorretos. Em caso de dúvidas, entre em contato com a Central de Atendimento.',16,1);
	end
	else
	begin
	--montar retorno para a session
		if @cd_Codigo = 0 --Pegar IdLogin
		Begin
		--Login por email
			select
				@id_cliente = tb_cliente.id_cliente
			from
				tb_cliente,tb_login
			where 
				lower(rtrim(ltrim(ds_email))) = lower(rtrim(ltrim(@ds_email)))
				and cd_senha = @cd_senha
				and tb_cliente.id_login = tb_login.id_login;
		End
		else
		Begin
		--Login por codigo
			select distinct
				@id_cliente = id_cliente
			from
				tb_cliente_conta
			where
				cd_Codigo = @cd_Codigo;
		End

	--Pegar os outros dados
	
	select 
		@Id_Cliente = Id_Cliente,
		@Id_Login = Id_Login,
		@Ds_Nome = Ds_Nome,
		@Ds_CpfCnpj = Ds_CpfCnpj,
		@St_Passo = St_Passo,
		@Dt_NascimentoFundacao = Dt_NascimentoFundacao,
		@Tp_Pessoa = Tp_Pessoa,
		@Dt_Passo1 = Dt_Passo1
	from 
		tb_cliente
	where
		id_cliente = @id_cliente;

	--tb_login
	select
		@Ds_EmailRetorno = ds_email
	from 
		tb_login
	where 
		id_login = @id_login;

	--tb_cliente-conta
	select top(1)
		@Cd_CodigoPrincipal = Cd_Codigo,
		@Cd_AssessorPrincipal = Cd_Assessor
	from 
		tb_cliente_conta
	where 
		id_cliente = @id_cliente
		and st_principal = 1;    
    
		
	end
