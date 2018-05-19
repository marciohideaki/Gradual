set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descri��o: Listar clientes Inativos.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 04/11/2010

--Descri��o: Filtrar por assessor e CPF/CNPJ.
--Autor: Gustavo Malta Guimar�es
--Data de cria��o: 11/11/2010

--Descri��o: Inclus�o do filtro no campo TipoPessoa
--Autor: Bruno Varandas
--Data de cria��o: 30/11/2010

ALTER PROCEDURE [dbo].[rel_cliente_inativo_lst_sp]
(
	@id_assessor int,
	@ds_cpfcnpj varchar(15),
	@TipoPessoa varchar(1)
)
AS

(
SELECT 
        cli.id_cliente as IdCliente ,
        cli.Ds_Nome as DsNomeCliente ,
        'P'+Tp_Pessoa as TipoPessoa ,
        Dt_passo1 as DtCadastro ,
        Ds_CpfCnpj as DsCpfCnpj ,
        ds_email as DsEmail ,
        cli.id_assessorinicial as IdAssessor ,
        '0' as CdConta ,
        'Cliente Geral' as DsConta 
FROM 
	tb_cliente as cli
	inner join tb_login as lo on (lo.id_login = cli.id_login)
where 
	cli.st_passo = 4 
	and st_ativo_cliger = 0
	and isnull(@id_assessor,cli.id_assessorinicial) =  cli.id_assessorinicial
	and (@ds_cpfcnpj is null or cli.ds_cpfcnpj like '%' + @ds_cpfcnpj + '%')
	and ((@TipoPessoa ='') OR (cli.tp_pessoa =@TipoPessoa))
)
union
(
SELECT 
        cli.id_cliente as IdCliente ,
        cli.Ds_Nome as DsNomeCliente ,
        'P'+Tp_Pessoa as TipoPessoa ,
        Dt_passo1 as DtCadastro ,
        Ds_CpfCnpj as DsCpfCnpj ,
        ds_email as DsEmail ,
        con.cd_assessor as IdAssessor ,
        cd_codigo as CdConta ,
        DsConta = case cd_sistema when 'BOL' Then 'Bolsa' when 'BMF' then 'BM&F' when 'CC' then 'Conta Corrente' when 'CUS' then 'Cust�dia' else cd_sistema end
		--cd_sistema as DsConta 
FROM 
	tb_cliente as cli
	inner join tb_login as lo on (lo.id_login = cli.id_login)
	inner join tb_cliente_conta as con on (con.id_cliente = cli.id_cliente)
where 
	con.st_ativa = 0 
	and cli.st_passo = 4
	and isnull(@id_assessor,con.cd_assessor) =  con.cd_assessor
	and (@ds_cpfcnpj is null or cli.ds_cpfcnpj like '%' + @ds_cpfcnpj + '%')
	and ((@TipoPessoa ='') OR (cli.tp_pessoa =@TipoPessoa))
)
order by DsNomeCliente, CdConta, DsConta 






