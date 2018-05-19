
--Descrição: Verificação id Login para o primeiro acesso.
--Autor: Gustavo Malta Guimarães
--Data de criação: 25/06/2010
Alter PROCEDURE [dbo].login_primeiroacesso_sel_login_sp
	@ds_cpfcnpj  varchar(15), 
    @cd_codigo  numeric(7,0),
    @id_login  int out
AS 
		select 
			@id_login = tb_cliente.id_login 
		from 
			tb_cliente,tb_cliente_conta 
		where 
			tb_cliente_conta.id_cliente = tb_cliente.id_cliente
			and convert(numeric(15,0),ds_cpfcnpj) = convert(numeric(15,0),@ds_cpfcnpj)
			and tb_cliente_conta.cd_codigo = @cd_codigo

