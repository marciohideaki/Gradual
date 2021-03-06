USE [DirectTradeCadastro]
GO
/****** Object:  StoredProcedure [dbo].[login_sel_nome_sp]    Script Date: 10/08/2012 20:08:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Descrição: Obter CPF a partir do codigo conta
--Autor: Alexandre Ponso
--Data de criação: 08/10/2012
ALTER PROCEDURE [dbo].[cpfcnpj_sel_sp]
	 @cd_codigo INTEGER
AS
	
	SELECT 
		 tb_cliente.ds_cpfcnpj as ds_cpfcnpj
	FROM
		 tb_cliente, tb_cliente_conta
	WHERE tb_cliente_conta.cd_codigo = @cd_codigo
		and tb_cliente.id_cliente = tb_cliente_conta.id_cliente		
