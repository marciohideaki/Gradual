alter  procedure PRC_SEL_CELULAATIVACAO_M1 (
    @DATAINICIAL  datetime ,
    @DATAFINAL datetime   ,
    @BOVESPA  int = null   ,
	@CODIGOASSESSOR  int = null)
 AS
 begin
      select
      ltrim(rtrim(tb_cliente.ds_Nome)) Nome,
      tb_cliente.ds_CPFcnpj Cpf,
      tb_Cliente.tp_cliente as TipoDeCliente, --DECODE(Cliente.Tipo,1,'PF',2,'PJ',3,'FUNDO')  TipoDeCliente , --Pegar Tipo
	  'P'+tb_Cliente.tp_pessoa as TipoDePessoa,
      tb_Cliente.cd_Sexo Sexo,
      tb_Login.ds_Email Email,
      tb_tipo_telefone.ds_telefone as TipoDeTelefone, 
      '('+tb_cliente_telefone.ds_Ddd+') '+tb_cliente_telefone.ds_numero Telefone,
      tb_cliente.id_assessorinicial as CodigoAssessor,
      CONVERT(VARCHAR(10), tb_cliente.dt_passo1 , 103) DataDeCadastroVisitante
     from
      tb_cliente
      inner join  tb_login on (tb_cliente.id_login=tb_login.id_login)
      left OUTER  join tb_cliente_telefone on (tb_cliente.id_cliente=tb_cliente_telefone.id_cliente )
	  left OUTER  join tb_tipo_telefone on (tb_cliente_telefone.id_tipo_telefone = tb_tipo_telefone.id_tipo_telefone and tb_cliente_telefone.st_principal = 1 )
     where
      tb_cliente.st_passo  in (1,2)     and
	  (tb_cliente.dt_passo1 between  @DATAINICIAL and DATEADD(DAY,1,@DATAFINAL)) and
      tb_cliente.id_assessorinicial =  isnull(@CODIGOASSESSOR,tb_cliente.id_assessorinicial)
     order
     by  tb_cliente.ds_nome asc;
 end;




/*--------------------------------------------------------------------------------------------------------------------*/




alter procedure PRC_SEL_CELULAATIVACAO_M2 (
       @DATAINICIAL  datetime              ,
       @DATAFINAL  datetime                ,
       @BOVESPA int = null   ,
       @CODIGOASSESSOR int = null)
AS
begin
   select
      ltrim(rtrim(tb_cliente.ds_Nome)) Nome,
      tb_cliente.ds_CPFcnpj Cpf,
      tb_Cliente.tp_cliente as TipoDeCliente,
	  'P'+tb_Cliente.tp_pessoa as TipoDePessoa,
      tb_Cliente.cd_Sexo Sexo,
      tb_Login.ds_Email Email,
     tb_cliente_endereco.ds_LOGRADOURO Logradouro,
     tb_cliente_endereco.ds_NUMERO Numero,
     tb_cliente_endereco.ds_COMPLEMENTO Complemento,
     tb_cliente_endereco.ds_BAIRRO Bairro,
     tb_cliente_endereco.ds_CIDADE Cidade,
     tb_cliente_endereco.cd_UF Uf,
     tb_cliente_endereco.cd_CEP+'-'+tb_cliente_endereco.cd_CEP_ext CEP,
        tb_tipo_telefone.ds_telefone as TipoDeTelefone, 
		'('+tb_cliente_telefone.ds_Ddd+') '+tb_cliente_telefone.ds_numero Telefone,
		tb_cliente.id_assessorinicial as CodigoAssessor,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo1 , 103) DataDeCadastroVisitante,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo2 , 103) DataDeCadastroPasso2,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo3 , 103) DataCadastroCompleto,
     (select count(*) from tb_cliente_pendenciacadastral where dt_resolucao is null and tb_cliente_pendenciacadastral.id_cliente = tb_cliente.id_cliente) as QuantidadeDePendencias
    from
     tb_cliente
     inner join tb_login on (tb_cliente.id_login = tb_login.id_login)
     left  OUTER  join tb_cliente_endereco on (tb_cliente.id_cliente = tb_cliente_endereco.id_cliente  and tb_cliente_endereco.id_tipo_endereco = 2) --Residencial
     left  OUTER  join tb_cliente_telefone on (tb_cliente.id_cliente = tb_cliente_telefone.id_cliente and tb_cliente_telefone.st_principal = 1  )
	 left OUTER  join tb_tipo_telefone on (tb_cliente_telefone.id_tipo_telefone = tb_tipo_telefone.id_tipo_telefone )
    where
     tb_cliente.st_passo = 3  and
	  (tb_cliente.dt_passo1 between  @DATAINICIAL and DATEADD(DAY,1,@DATAFINAL)) and
      tb_cliente.id_assessorinicial =  isnull(@CODIGOASSESSOR,tb_cliente.id_assessorinicial)
    order
    by
        tb_cliente.ds_Nome asc;
END;





/*--------------------------------------------------------------------------------------------------------------------*/




alter  procedure PRC_SEL_CELULAATIVACAO_M3 (
 @DATAINICIAL datetime ,
 @DATAFINAL datetime   ,
 @BOVESPA int = null   ,
 @CODIGOASSESSOR int = null)
AS
begin
  select
    tb_cliente_conta.cd_codigo Cblc,
      ltrim(rtrim(tb_cliente.ds_Nome)) Nome,
      tb_cliente.ds_CPFcnpj Cpf,
      tb_Cliente.tp_cliente as TipoDeCliente,
	  'P'+tb_Cliente.tp_pessoa as TipoDePessoa,
      tb_Cliente.cd_Sexo Sexo,
      tb_Login.ds_Email Email,
          tb_cliente_endereco.ds_LOGRADOURO Logradouro,
     tb_cliente_endereco.ds_NUMERO Numero,
     tb_cliente_endereco.ds_COMPLEMENTO Complemento,
     tb_cliente_endereco.ds_BAIRRO Bairro,
     tb_cliente_endereco.ds_CIDADE Cidade,
     tb_cliente_endereco.cd_UF Uf,
     tb_cliente_endereco.cd_CEP+'-'+tb_cliente_endereco.cd_CEP_ext CEP,
        tb_tipo_telefone.ds_telefone as TipoDeTelefone, 
		'('+tb_cliente_telefone.ds_Ddd+') '+tb_cliente_telefone.ds_numero Telefone,
 tb_cliente.id_assessorinicial as CodigoAssessor,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo1 , 103) DataDeCadastroVisitante,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo2 , 103) DataDeCadastroPasso2,
		CONVERT(VARCHAR(10), tb_cliente.dt_passo3 , 103) DataCadastroCompleto,
		CONVERT(VARCHAR(10), tb_cliente.dt_primeiraexportacao , 103) DataPrimeiraExportacaoSinacor
  from
     tb_cliente
     inner join tb_login on (tb_cliente.id_login = tb_login.id_login)
     left  OUTER  join tb_cliente_endereco on (tb_cliente.id_cliente = tb_cliente_endereco.id_cliente  and tb_cliente_endereco.id_tipo_endereco = 2) --Residencial
     left  OUTER  join tb_cliente_telefone on (tb_cliente.id_cliente = tb_cliente_telefone.id_cliente and tb_cliente_telefone.st_principal = 1  )
	 left OUTER  join tb_tipo_telefone on (tb_cliente_telefone.id_tipo_telefone = tb_tipo_telefone.id_tipo_telefone )
	left OUTER join tb_cliente_conta on (tb_cliente.id_cliente = tb_cliente_conta.id_cliente and tb_cliente_conta.cd_sistema = 'BOL' and tb_cliente_conta.st_principal =1)
    where
     tb_cliente.st_passo = 4 and
 	  (tb_cliente.dt_passo1 between  @DATAINICIAL and DATEADD(DAY,1,@DATAFINAL)) and
      tb_cliente_conta.cd_assessor =  isnull(@CODIGOASSESSOR,tb_cliente_conta.cd_assessor) and
	tb_cliente_conta.cd_codigo = isnull(@BOVESPA,tb_cliente_conta.cd_codigo)
    order
    by
        tb_cliente.dt_passo1 asc;
end;

