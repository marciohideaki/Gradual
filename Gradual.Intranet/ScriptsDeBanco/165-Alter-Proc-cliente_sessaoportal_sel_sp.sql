--Descrição: Montar a sessão no portal.
--Autor: Gustavo Malta Guimarães
--Data de criação: 01/07/2010
alter PROCEDURE [dbo].cliente_sessaoportal_sel_sp
     @Id_Cliente numeric ,
	 
     @Id_Login numeric out ,
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
    

