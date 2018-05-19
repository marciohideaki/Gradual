
-- Author:		Gustavo Malta Guimaraes
-- Create date: 23/11/2010
-- Description:	Listar os Assessores
-- =============================================
Alter PROCEDURE assessor_lst_sp 
AS
BEGIN
	select 
		cd_assessor as ID,
		ds_email as Email
		--O mome vem do Sinacor
	from 
		tb_login
	where
		tp_acesso = 2 and cd_assessor is not null
END
GO



-- Author:		Gustavo Malta Guimaraes
-- Create date: 23/11/2010
-- Description:	Listar os Clientes com Pendencias
-- =============================================
alter PROCEDURE clientesPendencias_lst_sp 
AS
BEGIN
	(
		select 
			cliente.ds_nome as nome,
			cliente.ds_cpfcnpj as cpfcnpj,
			cliente.st_passo as passo,
			login.ds_email as email,
			tipo.ds_pendencia as pendencia,
			cliente.id_assessorinicial as assessor,
			'' as codigo
		from tb_cliente as cliente
		inner join tb_cliente_pendenciacadastral as pendencia on (pendencia.id_cliente = cliente.id_cliente)
		inner join tb_tipo_pendenciacadastral as tipo on (tipo.id_tipo_pendencia = pendencia.id_tipo_pendencia and tipo.st_automatica = 0)
		inner join tb_login as login on (cliente.id_login = login.id_login)
		where cliente.st_passo = 3
	union
		select 
			cliente.ds_nome as nome,
			cliente.ds_cpfcnpj as cpfcnpj,
			cliente.st_passo as passo,
			login.ds_email as email,
			tipo.ds_pendencia as pendencia,
			conta.cd_assessor as assessor,
			convert(varchar, conta.cd_codigo) as codigo
		from tb_cliente as cliente
		inner join tb_cliente_pendenciacadastral as pendencia on (pendencia.id_cliente = cliente.id_cliente)
		inner join tb_tipo_pendenciacadastral as tipo on (tipo.id_tipo_pendencia = pendencia.id_tipo_pendencia and tipo.st_automatica = 0)
		inner join tb_login as login on (cliente.id_login = login.id_login)
		inner join tb_cliente_conta as conta on(conta.id_cliente = cliente.id_cliente and conta.st_principal = 1)
		where cliente.st_passo = 4
	)
	order by cpfcnpj,passo,pendencia

END
GO


exec clientesPendencias_lst_sp 