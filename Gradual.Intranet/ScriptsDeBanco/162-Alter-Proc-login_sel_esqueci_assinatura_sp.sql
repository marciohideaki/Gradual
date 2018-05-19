--Descrição: Gera uma nova assinatura eletrônica.
--Autor: Gustavo Malta Guimarães
--Data de criação: 29/06/2010
Create PROCEDURE [dbo].login_sel_esqueci_assinatura_sp
	@ds_email varchar(80),
    @ds_CpfCnpj varchar(15),
    @dt_NascimentoFundacao datetime,
    @cd_AssinaturaEletronica varchar(32) 
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
		RAISERROR('Os dados informados estão incorretos. Em caso de dúvidas, entre em contato com a Central de Atendimento.',16,1)
	end
	else
	begin
		
		update
			tb_login
		set
			cd_AssinaturaEletronica = @cd_AssinaturaEletronica
		where 
			ds_email = @ds_email;		
		
	end
