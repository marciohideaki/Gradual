
-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 17/11/2010
-- Description:	Exportação de clientes para a central de atendimento
-- =============================================
ALTER PROCEDURE exportacao_central_atendimento_lst_sp
AS
BEGIN
	        select 
				tb_cliente.ds_nome as NomeCliente, 
				tb_cliente_conta.cd_codigo as CodigoCBLC,
				tb_cliente.ds_cpfcnpj as CPF, 
				tb_cliente.tp_cliente as TipoCliente, 
				tb_cliente.st_Passo as StatusCadastro, 
				CodigoAssessor = case tb_cliente.st_passo when 4 then tb_cliente_conta.cd_assessor else tb_cliente.id_assessorinicial end,
				-- Pegar Nome no Programa, pois está no Sinacor (Oracle)
				NomeAssessor = case tb_cliente.st_passo when 4 then tb_cliente_conta.cd_assessor else tb_cliente.id_assessorinicial end,  
				-- pegar passo 1 para passo < 4 e de primeira exportação para passo = 4 
				DataCadastro = case tb_cliente.st_passo when 4 then tb_cliente.dt_primeiraexportacao else tb_cliente.dt_passo1 end,
				tb_login.ds_email as Email, 
				tb_cliente.dt_nascimentofundacao as DataNascimento, 
				tb_cliente_endereco.ds_logradouro as Logradouro, 
				tb_cliente_endereco.ds_numero as Numero, 
				tb_cliente_endereco.ds_complemento as Complemento, 
				tb_cliente_endereco.ds_bairro as Bairro, 
				tb_cliente_endereco.ds_cidade as Cidade, 
				tb_cliente_endereco.cd_uf as UF, 
				tb_cliente_endereco.cd_pais as Pais,
				--Manter o antigo 
				TipoEndereco = case tb_cliente_endereco.id_tipo_endereco when 1 then 'C' when 2 then 'R' when 3 then 'O' else 'O' end, 
				tb_cliente_telefone.ds_ddd as DDD, 
				tb_cliente_telefone.ds_numero as Telefone,  
				tb_cliente_telefone.ds_ramal as Ramal, 
				--Manter o antigo
				TipoTelefone = case tb_cliente_telefone.id_tipo_telefone when 1 then 'R' when 2 then 'C' when 3 then 'P' when 4 then 'P' else 'P' end   
            from  
				tb_cliente  
				inner join 
					tb_login 
						on (tb_cliente.id_login=tb_login.id_login)   
				left  OUTER  join 
					tb_cliente_endereco 
						on (tb_cliente.id_cliente=tb_cliente_endereco.id_cliente and tb_cliente_endereco.st_principal = 1)  
				left OUTER  join 
					tb_cliente_telefone 
						on (tb_cliente.id_cliente=tb_cliente_telefone.id_cliente and tb_cliente_telefone.st_principal = 1 )  
				left OUTER  join 
					tb_cliente_conta 
						on (tb_cliente.id_cliente=tb_cliente_conta.id_cliente and tb_cliente_conta.st_principal = 1)    ;

				
END
GO
