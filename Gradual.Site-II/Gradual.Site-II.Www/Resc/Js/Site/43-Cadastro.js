/// <reference path="00-Base.js" />
/// <reference path="01-Principal.js" />

var   txtCadastro_PFPasso1_NomeCompleto
    , txtCadastro_PFPasso1_Email
    , txtCadastro_PFPasso1_EmailConfirmacao
    , txtCadastro_PFPasso1_Senha
    , txtCadastro_PFPasso1_SenhaConfirmacao
    , txtCadastro_PFPasso1_AssEletronica
    , txtCadastro_PFPasso1_AssEletronicaConfirmacao
    , txtCadastro_PFPasso1_CPF
    , txtCadastro_PFPasso1_DataNascimento
    , cboCadastro_PFPasso1_Sexo
    , txtCadastro_PFPasso1_Cel_DDD
    , txtCadastro_PFPasso1_Cel_Numero
    , txtCadastro_PFPasso1_Tel_DDD
    , txtCadastro_PFPasso1_Tel_Numero
    , cboCadastro_PFPasso1_TipoTelefone
    , cboCadastro_PFPasso1_Conheceu 
    , txtCadastro_PFPasso1_Assessor

    , cboCadastro_PFPasso2_Nacionalidade
    , cboCadastro_PFPasso2_PaisNascimento
    , txtCadastro_PFPasso2_EstadoNascimento
    , cboCadastro_PFPasso2_EstadoNascimento
    , txtCadastro_PFPasso2_CidadeNascimento
    , cboCadastro_PFPasso2_EstadoCivil
    , txtCadastro_PFPasso2_Conjuge
    , cboCadastro_PFPasso2_Profissao
    , txtCadastro_PFPasso2_CargoFuncao
    , txtCadastro_PFPasso2_Empresa

    , cboCadastro_PFPasso2_TipoDocumento
    , txtCadastro_PFPasso2_NumeroDocumento
    , cboCadastro_PFPasso2_OrgaoEmissor
    , cboCadastro_PFPasso2_EstadoEmissao
    , txtCadastro_PFPasso2_DataEmissao
    , txtCadastro_PFPasso2_CodigoSegCNH

    , cboCadastro_PFPasso2_TipoDocumento2
    , txtCadastro_PFPasso2_NumeroDocumento2
    , cboCadastro_PFPasso2_OrgaoEmissor2
    , cboCadastro_PFPasso2_EstadoEmissao2
    , txtCadastro_PFPasso2_DataEmissao2
    , txtCadastro_PFPasso2_CodigoSegCNH2

    , txtCadastro_PFPasso2_NomeMae
    , txtCadastro_PFPasso2_NomePai
    , txtCadastro_PFPasso2_Endereco1_CEP
    , txtCadastro_PFPasso2_Endereco1_Logradouro
    , txtCadastro_PFPasso2_Endereco1_Numero
    , txtCadastro_PFPasso2_Endereco1_Complemento
    , txtCadastro_PFPasso2_Endereco1_Bairro
    , txtCadastro_PFPasso2_Endereco1_Cidade
    , cboCadastro_PFPasso2_Endereco1_Estado
    , cboCadastro_PFPasso2_Endereco1_Pais
    , txtCadastro_PFPasso2_Endereco2_CEP
    , txtCadastro_PFPasso2_Endereco2_Logradouro
    , txtCadastro_PFPasso2_Endereco2_Numero
    , txtCadastro_PFPasso2_Endereco2_Complemento
    , txtCadastro_PFPasso2_Endereco2_Bairro
    , txtCadastro_PFPasso2_Endereco2_Cidade
    , cboCadastro_PFPasso2_Endereco2_Estado
    , cboCadastro_PFPasso2_Endereco2_Pais
    , hidCadastro_PFPasso2_IdEndereco1
    , hidCadastro_PFPasso2_IdEndereco2

    , txtCadastro_PFPasso3_SalarioProlabore
    , txtCadastro_PFPasso3_ValorTotalEmAplicFin
    , txtCadastro_PFPasso3_OutrosRendimentos
    , txtCadastro_PFPasso3_OutrosRendimentosDesc
    , txtCadastro_PFPasso3_TotalEmBensMoveis
    , txtCadastro_PFPasso3_TotalEmBensImoveis
    , hidCadastro_PFPasso3_ContasBancarias
    , cboCadastro_PFPasso3_ContasBancarias_TipoConta
    , cboCadastro_PFPasso3_ContasBancarias_Banco
    , txtCadastro_PFPasso3_ContasBancarias_Agencia
    , txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito
    , txtCadastro_PFPasso3_ContasBancarias_Conta
    , txtCadastro_PFPasso3_ContasBancarias_ContaDigito
    , txtCadastro_PFPasso3_ContasBancarias_NomeTitular
    , txtCadastro_PFPasso3_ContasBancarias_CPFTitular
    , hidCadastro_PFPasso3_ContasBancariasCambio
    , cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio
    , cboCadastro_PFPasso3_ContasBancarias_BancoCambio
    , txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio
    , txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio
    , txtCadastro_PFPasso3_ContasBancarias_ContaCambio
    , txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio
    , txtCadastro_PFPasso3_ContasBancarias_NomeTitularCambio
    , txtCadastro_PFPasso3_ContasBancarias_CPFTitularCambio
    , rdoCadastro_PFPasso3_PessoaVinculada_SimG
    , rdoCadastro_PFPasso3_PessoaVinculada_Sim
    , rdoCadastro_PFPasso3_OperaContaPropria_Sim
    , rdoCadastro_PFPasso3_PPE_Sim
    , rdoCadastro_PFPasso3_Emancipado_Sim
    , rdoCadastro_PFPasso3_Procurador_Sim
    , txtCadastro_PFPasso3_CliCPF
    , txtCadastro_PFPasso3_CliNomeCompleto
    , cboCadastro_PFPasso3_RepSituacaoLegal
    , txtCadastro_PFPasso3_RepNomeCompleto
    , txtCadastro_PFPasso3_RepCPF
    , txtCadastro_PFPasso3_RepDataNascimento
    , cboCadastro_PFPasso3_RepTipoDocumento
    , cboCadastro_PFPasso3_RepOrgaoEmissor
    , txtCadastro_PFPasso3_RepNumeroDocumento
    , cboCadastro_PFPasso3_RepEstadoEmissao
    , chkCadastro_PFPasso3_Ciente_Doc_Regulamento
    , chkCadastro_PFPasso3_Ciente_Doc_Prospecto
    , chkCadastro_PFPasso3_Ciente_Doc_Lamina
    , txtCadastro_PFPasso3_Proposito
    , rdoCadastro_PFPasso3_USPerson_Nao
    , rdoCadastro_PFPasso3_USPerson_Sim
    , pnlFormNovaContaBancaria
    , pnlFormNovaContaBancariaCambio
;

var pnlLoader;

var gDadosDeCadastro;

var gPasso3_ListaDeContasBancarias = [];

function GradSite_Cadastro_Iniciar()
{
    txtCadastro_PFPasso1_NomeCompleto               = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_NomeCompleto");
    txtCadastro_PFPasso1_Email                      = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Email");
    txtCadastro_PFPasso1_EmailConfirmacao           = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_EmailConfirmacao");
    txtCadastro_PFPasso1_Senha                      = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Senha");
    txtCadastro_PFPasso1_SenhaConfirmacao           = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_SenhaConfirmacao");
    txtCadastro_PFPasso1_AssEletronica              = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_AssEletronica");
    txtCadastro_PFPasso1_AssEletronicaConfirmacao   = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_AssEletronicaConfirmacao");
    txtCadastro_PFPasso1_CPF                        = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_CPF");
    txtCadastro_PFPasso1_DataNascimento             = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_DataNascimento");
    cboCadastro_PFPasso1_Sexo                       = $("#ContentPlaceHolder1_cboCadastro_PFPasso1_Sexo");
    txtCadastro_PFPasso1_Cel_DDD                    = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Cel_DDD");
    txtCadastro_PFPasso1_Cel_Numero                 = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Cel_Numero");
    txtCadastro_PFPasso1_Tel_DDD                    = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Tel_DDD");
    txtCadastro_PFPasso1_Tel_Numero                 = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Tel_Numero");
    cboCadastro_PFPasso1_TipoTelefone               = $("#ContentPlaceHolder1_cboCadastro_PFPasso1_TipoTelefone");
    cboCadastro_PFPasso1_Conheceu                   = $("#ContentPlaceHolder1_cboCadastro_PFPasso1_Conheceu");
    txtCadastro_PFPasso1_Assessor                   = $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Assessor");

    pnlLoader = $("#pnlLoader");

    cboCadastro_PFPasso2_Nacionalidade          = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Nacionalidade");
    cboCadastro_PFPasso2_PaisNascimento         = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_PaisNascimento");
    txtCadastro_PFPasso2_EstadoNascimento       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_EstadoNascimento");
    cboCadastro_PFPasso2_EstadoNascimento       = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_EstadoNascimento");
    txtCadastro_PFPasso2_CidadeNascimento       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_CidadeNascimento");
    cboCadastro_PFPasso2_EstadoCivil            = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_EstadoCivil");
    txtCadastro_PFPasso2_Conjuge                = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Conjuge");
    cboCadastro_PFPasso2_Profissao              = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Profissao");
    txtCadastro_PFPasso2_CargoFuncao            = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_CargoFuncao");
    txtCadastro_PFPasso2_Empresa                = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Empresa");
    cboCadastro_PFPasso2_TipoDocumento          = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_TipoDocumento");

    txtCadastro_PFPasso2_NumeroDocumento        = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_NumeroDocumento");
    cboCadastro_PFPasso2_OrgaoEmissor           = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_OrgaoEmissor");
    cboCadastro_PFPasso2_EstadoEmissao          = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_EstadoEmissao");
    txtCadastro_PFPasso2_DataEmissao            = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_DataEmissao");
    txtCadastro_PFPasso2_CodigoSegCNH           = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_CodigoSegCNH");
    
    cboCadastro_PFPasso2_TipoDocumento2         = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_TipoDocumento2");
    txtCadastro_PFPasso2_NumeroDocumento2       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_NumeroDocumento2");
    cboCadastro_PFPasso2_OrgaoEmissor2          = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_OrgaoEmissor2");
    cboCadastro_PFPasso2_EstadoEmissao2         = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_EstadoEmissao2");
    txtCadastro_PFPasso2_DataEmissao2           = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_DataEmissao2");
    txtCadastro_PFPasso2_CodigoSegCNH2          = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_CodigoSegCNH2");

    txtCadastro_PFPasso2_NomeMae                = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_NomeMae");
    txtCadastro_PFPasso2_NomePai                = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_NomePai");

    txtCadastro_PFPasso2_Endereco1_CEP          = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_CEP");
    txtCadastro_PFPasso2_Endereco1_Logradouro   = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_Logradouro");
    txtCadastro_PFPasso2_Endereco1_Numero       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_Numero");
    txtCadastro_PFPasso2_Endereco1_Complemento  = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_Complemento");
    txtCadastro_PFPasso2_Endereco1_Bairro       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_Bairro");
    txtCadastro_PFPasso2_Endereco1_Cidade       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco1_Cidade");
    cboCadastro_PFPasso2_Endereco1_Estado       = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Endereco1_Estado");
    cboCadastro_PFPasso2_Endereco1_Pais         = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Endereco1_Pais");

    txtCadastro_PFPasso2_Endereco2_CEP          = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_CEP");
    txtCadastro_PFPasso2_Endereco2_Logradouro   = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_Logradouro");
    txtCadastro_PFPasso2_Endereco2_Numero       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_Numero");
    txtCadastro_PFPasso2_Endereco2_Complemento  = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_Complemento");
    txtCadastro_PFPasso2_Endereco2_Bairro       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_Bairro");
    txtCadastro_PFPasso2_Endereco2_Cidade       = $("#ContentPlaceHolder1_txtCadastro_PFPasso2_Endereco2_Cidade");
    cboCadastro_PFPasso2_Endereco2_Estado       = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Endereco2_Estado");
    cboCadastro_PFPasso2_Endereco2_Pais         = $("#ContentPlaceHolder1_cboCadastro_PFPasso2_Endereco2_Pais");

    hidCadastro_PFPasso2_IdEndereco1            = $("#ContentPlaceHolder1_hidCadastro_PFPasso2_IdEndereco1");
    hidCadastro_PFPasso2_IdEndereco2            = $("#ContentPlaceHolder1_hidCadastro_PFPasso2_IdEndereco2");

    txtCadastro_PFPasso3_SalarioProlabore       = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_SalarioProlabore");
    txtCadastro_PFPasso3_ValorTotalEmAplicFin   = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_ValorTotalEmAplicacoesFinanceiras");
    txtCadastro_PFPasso3_OutrosRendimentos      = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_OutrosRendimentos");
    txtCadastro_PFPasso3_OutrosRendimentosDesc  = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_OutrosRendimentosDesc");
    txtCadastro_PFPasso3_TotalEmBensMoveis      = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_TotalEmBensMoveis");
    txtCadastro_PFPasso3_TotalEmBensImoveis     = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_TotalEmBensImoveis");

    hidCadastro_PFPasso3_ContasBancarias = $("#ContentPlaceHolder1_hidCadastro_PFPasso3_ContasBancarias");

    txtCadastro_PFPasso3_OutrosRendimentosCambio = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_OutrosRendimentosCambio");

    var lContas = hidCadastro_PFPasso3_ContasBancarias.val();

    if(lContas != "" && lContas !== undefined)
    {
        gPasso3_ListaDeContasBancarias = $.parseJSON(lContas);
    }

    pnlFormNovaContaBancaria                                = $("#ContentPlaceHolder1_pnlFormNovaContaBancaria");
    pnlFormNovaContaBancariaCambio                          = $("#ContentPlaceHolder1_pnlFormNovaContaBancariaCambio");

    cboCadastro_PFPasso3_ContasBancarias_TipoConta          = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_TipoConta");
    cboCadastro_PFPasso3_ContasBancarias_Banco              = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_Banco");
    txtCadastro_PFPasso3_ContasBancarias_Agencia            = $("#txtCadastro_PFPasso3_ContasBancarias_Agencia");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito      = $("#txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito");
    txtCadastro_PFPasso3_ContasBancarias_Conta              = $("#txtCadastro_PFPasso3_ContasBancarias_Conta");
    txtCadastro_PFPasso3_ContasBancarias_ContaDigito        = $("#txtCadastro_PFPasso3_ContasBancarias_ContaDigito");
    txtCadastro_PFPasso3_ContasBancarias_NomeTitular        = $("#txtCadastro_PFPasso3_ContasBancarias_NomeTitular");
    txtCadastro_PFPasso3_ContasBancarias_CPFTitular         = $("#txtCadastro_PFPasso3_ContasBancarias_CPFTitular");


    cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio    = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio");
    cboCadastro_PFPasso3_ContasBancarias_BancoCambio        = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_BancoCambio");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio      = $("#txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio= $("#txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio");
    txtCadastro_PFPasso3_ContasBancarias_ContaCambio        = $("#txtCadastro_PFPasso3_ContasBancarias_ContaCambio");
    txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio  = $("#txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio");
    txtCadastro_PFPasso3_ContasBancarias_NomeTitularCambio  = $("#txtCadastro_PFPasso3_ContasBancarias_NomeTitularCambio");
    txtCadastro_PFPasso3_ContasBancarias_CPFTitularCambio   = $("#txtCadastro_PFPasso3_ContasBancarias_CPFTitularCambio");

    rdoCadastro_PFPasso3_PessoaVinculada_Sim        = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_PessoaVinculada_Sim");
    rdoCadastro_PFPasso3_PessoaVinculada_SimG       = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_PessoaVinculada_SimG");
    rdoCadastro_PFPasso3_OperaContaPropria_Sim      = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_OperaContaPropria_Sim");
    rdoCadastro_PFPasso3_PPE_Sim                    = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_PPE_Sim");
    rdoCadastro_PFPasso3_Emancipado_Sim             = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Emancipado_Sim");
    rdoCadastro_PFPasso3_Procurador_Sim             = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim");

    txtCadastro_PFPasso3_CliCPF                     = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_CliCPF");
    txtCadastro_PFPasso3_CliNomeCompleto            = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_CliNomeCompleto");

    cboCadastro_PFPasso3_RepSituacaoLegal           = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepSituacaoLegal");
    txtCadastro_PFPasso3_RepNomeCompleto            = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepNomeCompleto");
    txtCadastro_PFPasso3_RepCPF                     = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepCPF");
    txtCadastro_PFPasso3_RepDataNascimento          = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepDataNascimento");
    cboCadastro_PFPasso3_RepTipoDocumento           = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepTipoDocumento");
    cboCadastro_PFPasso3_RepOrgaoEmissor            = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepOrgaoEmissor");
    txtCadastro_PFPasso3_RepNumeroDocumento         = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepNumeroDocumento");
    cboCadastro_PFPasso3_RepEstadoEmissao           = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepEstadoEmissao");

    rdoCadastro_PFPasso3_ProcuradorCambio_Sim       = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_ProcuradorCambio_Sim");
    cboCadastro_PFPasso3_RepSituacaoLegalCambio     = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepSituacaoLegalCambio");
    txtCadastro_PFPasso3_RepNomeCompletoCambio      = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepNomeCompletoCambio");
    txtCadastro_PFPasso3_RepCPFCambio               = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepCPFCambio");
    txtCadastro_PFPasso3_RepDataNascimentoCambio    = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepDataNascimentoCambio");
    cboCadastro_PFPasso3_RepTipoDocumentoCambio     = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepTipoDocumentoCambio");
    cboCadastro_PFPasso3_RepOrgaoEmissorCambio      = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepOrgaoEmissorCambio");
    txtCadastro_PFPasso3_RepNumeroDocumentoCambio   = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_RepNumeroDocumentoCambio");
    cboCadastro_PFPasso3_RepEstadoEmissaoCambio     = $("#ContentPlaceHolder1_cboCadastro_PFPasso3_RepEstadoEmissaoCambio");
    
    chkCadastro_PFPasso3_Ciente_Doc_Regulamento     = $("#ContentPlaceHolder1_chkCadastro_PFPasso3_Ciente_Doc_Regulamento");
    chkCadastro_PFPasso3_Ciente_Doc_Prospecto       = $("#ContentPlaceHolder1_chkCadastro_PFPasso3_Ciente_Doc_Prospecto");
    chkCadastro_PFPasso3_Ciente_Doc_Lamina          = $("#ContentPlaceHolder1_chkCadastro_PFPasso3_Ciente_Doc_Lamina");
    txtCadastro_PFPasso3_Proposito                  = $("#ContentPlaceHolder1_txtCadastro_PFPasso3_Proposito");
    rdoCadastro_PFPasso3_USPerson_Sim               = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_USPerson_Sim");
    rdoCadastro_PFPasso3_USPerson_Nao               = $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_USPerson_Nao");

    $("#pnlTodosForms").attr("style", "");

    var lHash = window.location.hash;

    if(lHash != "")
    {
        if(lHash.toLowerCase() == "#dadospessoais")
        {
            $("#ContentPlaceHolder1_liDadosPessoais a").click();
        }

        if(lHash.toLowerCase() == "#dadosfinanceiros")
        {
            $("#ContentPlaceHolder1_liDadosFinanceiros a").click();
        }

        if(lHash.toLowerCase() == "#dadoscontratuais")
        {
            $("#ContentPlaceHolder1_liDadosContratuais a").click();
        }

        /*
        if(lHash == "#Passo3Perfil")
        {
            lnkAbaCadastro_Click( $("ul.AbasCadastro li:eq(2)") );

            btnCadastro_PFPasso3_RefazerSuitability_Click({});

            $(document).scrollTop(1600);
        }
        else
        {
            lHash = lHash.toLowerCase().replace("#passo", "");

            lHash = new Number(lHash - 1);

            lnkAbaCadastro_Click( $("ul.AbasCadastro li:eq(" + lHash + ")") );
        }
        */
    }

    $("[data-CaixaAlta='true']").css( { textTransform: "uppercase" } );

    cboTipoDocumento_Change(cboCadastro_PFPasso2_TipoDocumento);

    cboCadastro_PFPasso2_Profissao_Change(cboCadastro_PFPasso2_Profissao);
}

function lnkMeuPerfil_Click(pSender)
{
    pSender = $(pSender);

    if( pSender.parent().hasClass("inativo") )
    {
        GradSite_ExibirMensagem("I", "Você ainda não respondeu o questionário do perfil,<br/> por favor complete seu cadastro primeiro.");

        return false;
    }

    document.location = pSender.attr("href");
}

function lnkMeusProdutos_Click(pSender)
{
    pSender = $(pSender);

    if( pSender.parent().hasClass("inativo") )
    {
        GradSite_ExibirMensagem("I", "Para adquirir produtos, por favor complete seu cadastro primeiro.");

        return false;
    }

    document.location = pSender.attr("href");
}


function lnkMeuCadastro_Click(pSender)
{
    document.location = $(pSender).attr("href");
}

function lnkSeguranca_Click(pSender) {

    var lURL = $(pSender).attr("href");
    document.location = lURL + "?TipoTeclado=" + gTipoTeclado;
}

function GradSite_OnValidacaoFormulario(pFormulario, pStatus)
{
    //console.log("Validou como " + pStatus);

    return pStatus;
}


/*=====================================

    Funções - PASSO 1

=====================================*/



function btnCadastro_VerificarAssessor_Click(pSender)
{
    var lCodigo = txtCadastro_PFPasso1_Assessor.val();

    if(lCodigo != "")
    {
        GradSite_CarregarJsonVerificandoErro(GradSite_Cadastro_Url(), { Acao: "BuscarAssessor", Assessor: txtCadastro_PFPasso1_Assessor.val() }, VerificarAssessor_CallBack);
    }
    else
    {
        alert("Favor preencher um cógido de assessor para verificação");

        txtCadastro_PFPasso1_Assessor.focus();
    }

    return false;
}

function VerificarAssessor_CallBack(pResposta)
{
    if(pResposta.Mensagem == "NAO_ENCONTRADO")
    {
        $("#lblNomeAssessor").html("Assessor não encontrado!");
    }
    else
    {
        $("#lblNomeAssessor").html(pResposta.Mensagem);
    }
}


function btnCadastro_PassoAnterior_Click(pSender)
{
    var lField = $("fieldset[data-rel]:visible");

    var lPasso = (lField.attr("data-rel").replace("Passo", "") - 1);

    $("#ContentPlaceHolder1_lnkCadastro_P" + lPasso).click();

    return false;
}

/*
function btnCadastro_ProximoPasso_Click(pSender)
{
    var lField = $("fieldset[data-rel]:visible");

    var lPasso = lField.attr("data-rel");

    if(lPasso == "Passo1")
    {
        GradSite_Cadastro_PFPasso1_Verificar();
    }
    else if(lPasso == "Passo2")
    {
        GradSite_Cadastro_PFPasso2_Verificar();
    }
    else if(lPasso == "Passo3")
    {
        GradSite_Cadastro_PFPasso3_Verificar();
    }
    else
    {
    }

    return false;
}
*/


function lnkAbaCadastro_Click(pSender)
{
    pSender = $(pSender);

    if(pSender.hasClass("Habilitado"))
    {
        var lIndex = pSender.index();

        var lDiv = pSender.parent().parent();

        var lAlvo = lDiv.find(".FormularioCadastro:eq(" + lIndex + ")");

        if(lAlvo.length > 0)
        {
            pSender.parent().find(".Selecionado").removeClass("Selecionado");

            pSender.addClass("Selecionado");

            lDiv.find(".FormularioCadastro").hide();

            lAlvo.show();
        }
    }

    return false;
}

function rdoCadastroEndereco_Click(pSender)
{
    if( $("#ContentPlaceHolder1_rdoCadastro_PFPasso2_End1").is(":checked") )
    {
        $("#ContentPlaceHolder1_pnlSegundoEndereco").hide();

        txtCadastro_PFPasso2_Endereco2_CEP.removeClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Logradouro.removeClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Numero.removeClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Bairro.removeClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Cidade.removeClass("validate[required]");
        cboCadastro_PFPasso2_Endereco2_Estado.removeClass("validate[required]");
        cboCadastro_PFPasso2_Endereco2_Pais.removeClass("validate[required]");
    }
    else
    {
        $("#ContentPlaceHolder1_pnlSegundoEndereco").show();
        
        txtCadastro_PFPasso2_Endereco2_CEP.addClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Logradouro.addClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Numero.addClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Bairro.addClass("validate[required]");
        txtCadastro_PFPasso2_Endereco2_Cidade.addClass("validate[required]");
        cboCadastro_PFPasso2_Endereco2_Estado.addClass("validate[required]");
        cboCadastro_PFPasso2_Endereco2_Pais.addClass("validate[required]");
    }
}

function txtCadastro_PFPasso1_Cel_DDD_Blur(pSender)
{
    pSender = $(pSender);

    if(pSender.val() == "11" || pSender.val() == "21")
    {
        Validacao_HabilitarMascaraNumerica(txtCadastro_PFPasso1_Cel_Numero, "99999-9999");
    }
    else
    {
        Validacao_HabilitarMascaraNumerica(txtCadastro_PFPasso1_Cel_Numero, "9999-9999");
    }
}

function GradSite_ValidacaoComDoubleCheck(pContainer)
{
    //se o formulário indicado não tiver a classe do validationEngine, precisa achar o de cima que tenha:

    var lContainer = pContainer;

    if(!lContainer.hasClass("validationEngineContainer"))
    {
        lContainer = lContainer.closest(".validationEngineContainer");
    }

    var lRetorno = lContainer.validationEngine("validate");

    if(!lRetorno)
    {
        if(lContainer.find(".formError:visible").length == 0)
        {
            lContainer.find("input,select").blur(); //pra ativar a validacao e mostrar a mensagem
        }
    }

    return lRetorno;
}

function GradSite_Cadastro_PFPasso1_Verificar() {

    var TipoOperacaoCliente;

    if ($("#chkOperarEmBolsa").is(":checked") && !$("#chkOperarEmFundos").is(":checked") && !$("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 1;
    }
    else if (!$("#chkOperarEmBolsa").is(":checked") && $("#chkOperarEmFundos").is(":checked") && !$("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 2;
    }
    else if (!$("#chkOperarEmBolsa").is(":checked") && !$("#chkOperarEmFundos").is(":checked") && $("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 3;
    }
    else if ($("#chkOperarEmBolsa").is(":checked") && $("#chkOperarEmFundos").is(":checked") && !$("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 5;
    }
    else if ($("#chkOperarEmBolsa").is(":checked") && !$("#chkOperarEmFundos").is(":checked") && $("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 6;
    }
    else if (!$("#chkOperarEmBolsa").is(":checked") && $("#chkOperarEmFundos").is(":checked") && $("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 7;
    }
    else if ($("#chkOperarEmTodas").is(":checked") && !$("#chkOperarEmBolsa").is(":checked") && !$("#chkOperarEmFundos").is(":checked") && !$("#chkOperarEmCambio").is(":checked")) 
    {
        TipoOperacaoCliente = 4;
    }

    gDadosDeCadastro = 
    {
        NomeCompleto               : txtCadastro_PFPasso1_NomeCompleto.val()
        , Email                    : txtCadastro_PFPasso1_Email.val()
        , EmailConfirmacao         : txtCadastro_PFPasso1_EmailConfirmacao.val()
        , Senha                    : txtCadastro_PFPasso1_Senha.val()
        , SenhaConfirmacao         : txtCadastro_PFPasso1_SenhaConfirmacao.val()
        , AssEletronica            : txtCadastro_PFPasso1_AssEletronica.val()
        , AssEletronicaConfirmacao : txtCadastro_PFPasso1_AssEletronicaConfirmacao.val()
        , CPF                      : txtCadastro_PFPasso1_CPF.val()
        , DataNascimento           : txtCadastro_PFPasso1_DataNascimento.val()
        , Sexo                     : cboCadastro_PFPasso1_Sexo.val()
        , Cel_DDD                  : txtCadastro_PFPasso1_Cel_DDD.val()
        , Cel_Numero               : txtCadastro_PFPasso1_Cel_Numero.val()
        , Tel_DDD                  : txtCadastro_PFPasso1_Tel_DDD.val()
        , Tel_Numero               : txtCadastro_PFPasso1_Tel_Numero.val()
        , TipoTelefone             : cboCadastro_PFPasso1_TipoTelefone.val()
        , ComoConheceu             : cboCadastro_PFPasso1_Conheceu.val()
        , Assessor                 : txtCadastro_PFPasso1_Assessor.val()
        , CodigoTipoOperacaoCliente: TipoOperacaoCliente
    };

    if (gDadosDeCadastro.ComoConheceu == "CLIENTE" && gDadosDeCadastro.CodigoTipoOperacaoCliente != 3)
    {
        gDadosDeCadastro.Assessor = "18";
    }
    else if (gDadosDeCadastro.CodigoTipoOperacaoCliente == 3)
    {
        gDadosDeCadastro.Assessor = "602";
    }

    gDadosDeCadastro.Valido = GradSite_ValidacaoComDoubleCheck($("#ContentPlaceHolder1_pnlDadosBasicos"));

    if(gDadosDeCadastro.Valido)
    {
        GradSite_Cadastro_PFPasso1_Avancar();
    }

    return false;
}

function GradSite_Cadastro_ExibirLoader(pForm, pFuncao)
{
    //var lWidth = pForm.width() + 36;
    var lLeft = pForm.position().left;

    $(document).scrollTop(0);

    pnlLoader
        .removeClass("Sucesso")
        .removeClass("Erro")
        .find("div.Mensagem span")
            .html("Gravando dados, aguarde...");

    pnlLoader.find(".BotaoOk").hide();


    pnlLoader.css( {     width: 180
                     ,  height: (pForm.height() + 50)
                     ,    left: "30%"
                     , display: "block"
                     , opacity: 0.5
                   } );

    pnlLoader.animate( { width: "100%", left: "0%", opacity: 0.90 }, 250, pFuncao );
}

var gTimeOutOcultar;

function GradSite_Cadastro_OcultarLoader(pMensagem, pSucesso)
{
    if(pMensagem != null)
    {
        pnlLoader.find("div.Mensagem span").html(pMensagem);

        pnlLoader.find(".BotaoOk").show();
    }

    if(pSucesso)
    {
        pnlLoader
            .addClass("Sucesso")
            .removeClass("Erro");
    }
    else
    {
        pnlLoader
            .addClass("Erro")
            .removeClass("Sucesso");
    }
    
    if(pMensagem != null)
    {
        gTimeOutOcultar = window.setTimeout(function ()
        {
            pnlLoader.animate(    { width: 180, opacity: 0 }
                                , 250
                                , function ()
                                  {
                                     pnlLoader.hide(); 
                                     if (gProximoItem) { gProximoItem.click(); } 
                                  }
                             );
        }, 6000);
    }
    else
    {
        pnlLoader.hide(); 
        if (gProximoItem) { gProximoItem.click(); } 
    }
}

function rdoCadastro_PFPasso1_Doc_Click(pSender)
{
    if($("[name='rdoCadastro_PFPasso1_Doc']:checked").val() == "CPF")
    {
        txtCadastro_PFPasso1_CPF.val("").attr("class",  "validate[custom[validatecpf]] Mascara_CPF ProibirLetras EstiloCampoObrigatorio").show();
        txtCadastro_PFPasso1_CNPJ.val("").attr("class", "").hide();
    }
    else
    {
        txtCadastro_PFPasso1_CPF.val("").attr("class", "").hide();
        txtCadastro_PFPasso1_CNPJ.val("").attr("class", "validate[custom[validatecnpj]] Mascara_CNPJ ProibirLetras EstiloCampoObrigatorio ").show();
    }
}

function cboCadastro_PFPasso1_Conheceu_Change(pSender)
{
    var pnlAssessor = $("#ContentPlaceHolder1_pnlAssessor");

    if(cboCadastro_PFPasso1_Conheceu.val() == "ASSESSOR")
    {
        pnlAssessor.show();

        $("#ContentPlaceHolder1_txtCadastro_PFPasso1_Assessor").val("18");

        $("#lblNomeAssessor").html("0018 - 18 - PA - PEDRO PAULO - PF - SP");
    }
    else
    {
        pnlAssessor.hide();
    }
}

function btnCadastro_Ok_Click(pSender)
{
    pnlLoader.hide();

    window.clearTimeout(gTimeOutOcultar);

    if (gProximoItem) { gProximoItem.click(); } 

    return false;
}


function GradSite_Cadastro_Url()
{
    return (document.location.href.toLowerCase().indexOf("novocadastro") != -1) ? "NovoCadastro.aspx" : "MeuCadastro.aspx";
}

function GradSite_Cadastro_PFPasso1_Avancar()
{
    //alert("avança!");

    var lForm = $("#ContentPlaceHolder1_pnlDadosBasicos");

    lForm.find("input, select, button").attr("disabled", "disabled");

    var lDados = { Acao: "SalvarPasso1", DadosDeCadastro: $.toJSON(gDadosDeCadastro) };

    GradSite_Cadastro_ExibirLoader(lForm
                                   , function()
                                     {
                                        GradSite_CarregarJsonVerificandoErro(GradSite_Cadastro_Url(), lDados, GradSite_Cadastro_PFPasso1_Avancar_CallBack);
                                     }
                                  );
}

function GradSite_Cadastro_PFPasso1_Avancar_CallBack(pResposta)
{
    var lForm = $("#ContentPlaceHolder1_pnlDadosBasicos");

    lForm.find("input, select, button").attr("disabled", null);

    if (pResposta.TemErro)
    {
        GradSite_ExibirMensagem("E", pResposta.Mensagem, true, "");
    }
    else
    {
        if(pResposta.Mensagem == "erro: email já cadastrado")
        {
            GradSite_Cadastro_OcultarLoader("Erro: email já cadastrado no sistema.", false);

            GradSite_Formulario_ExibirMensagemDeValidacao("ContentPlaceHolder1_txtCadastro_PFPasso1_Email", "Email já cadastrado para outro cliente", false);
        }
        else if(pResposta.Mensagem == "erro: cpf já cadastrado")
        {
            GradSite_Cadastro_OcultarLoader("Erro: CPF já cadastrado no sistema.", false);

            GradSite_Formulario_ExibirMensagemDeValidacao("ContentPlaceHolder1_txtCadastro_PFPasso1_CPF", "CPF/CNPJ já cadastrado para outro cliente", false);
        }
        else if(pResposta.Mensagem == "redirecionar_novo")
        {
            //alert("Informações básicas cadastradas com sucesso.\r\n\r\nVocê será redirecionado(a) para completar seu cadastro na área de clientes.");

            document.location = GradSite_BuscarRaiz("/MinhaConta/Cadastro/MeuCadastro.aspx#Aba-DadosPessoais");
        }
        else
        {
            GradSite_Cadastro_PFPasso_AvancarPara(2);
        }
    }

    return false;
}

var gProximoItem;

function GradSite_Cadastro_PFPasso_AvancarPara(pPasso)
{
    if(pPasso == 2)
    {
        //gProximoItem = $("#ContentPlaceHolder1_pnlDadosFinanceiros").addClass("Habilitado");
        $("#ContentPlaceHolder1_liDadosPessoais").removeClass("inativo").attr("data-desabilitado", null);

        $("#ContentPlaceHolder1_liDadosPessoais a").click();

        Sauron_EnviarScreenView("Cadastro Passo 2");
    }
    else if(pPasso == 3)
    {
        //gProximoItem = $("#ContentPlaceHolder1_lnkCadastro_P" + pPasso).addClass("Habilitado");
        $("#ContentPlaceHolder1_liDadosFinanceiros").removeClass("inativo").attr("data-desabilitado", null);

        $("#ContentPlaceHolder1_liDadosFinanceiros a").click();

        Sauron_EnviarScreenView("Cadastro Passo 3");
    }
    else if(pPasso == 4)
    {
        //gProximoItem = $("#ContentPlaceHolder1_lnkCadastro_P" + pPasso).addClass("Habilitado");
        $("#ContentPlaceHolder1_liDadosContratuais").removeClass("inativo").attr("data-desabilitado", null);

        $("#ContentPlaceHolder1_liDadosContratuais a").click();

        Sauron_EnviarScreenView("Cadastro Passo 4");
    }

    GradSite_Cadastro_OcultarLoader(null, true);
}

function chkOperarEm_Selecionar_Click(sender) 
{
    if ($(sender).attr("id") == "chkOperarEmTodas")
    {
        var lListaDeChecks = $("#divOperarEm input[type='checkbox']");
        lListaDeChecks.each(function ()
        {
            if ($(this).attr("id") != "chkOperarEmTodas")
            {
                $(this).prop("checked", false);
            }
        });
    }
    else if($(sender).attr("id") != "chkOperarEmTodas")
    {
        $("#chkOperarEmTodas").prop("checked", false);
    }
//    if ($(sender).attr('id') == "rdoOperarEmBolsa") 
//    {
//        if ($(sender).is(":checked")) 
//        {
//        }
//    }
//    if ($(sender).attr('id') == "rdoOperarEmFundos") 
//    {
//        if ($(sender).is(":checked")) 
//        {
//        }
//    }
//    if ($(sender).attr('id') == "rdoOperarEmCambio") 
//    {
//        if ($(sender).is(":checked")) 
//        {
//        }
//    }
//    if ($(sender).attr('id') == "rdoOperarEmTodas") 
//    {
//        if ($(sender).is(":checked")) 
//        {
//        }
//    }
}


/*=====================================

    Funções - PASSO 2

=====================================*/



function GradSite_Cadastro_PFPasso2_Verificar()
{
    gDadosDeCadastro = {
                             Nacionalidade      : cboCadastro_PFPasso2_Nacionalidade.val()
                           , PaisNascimento     : cboCadastro_PFPasso2_PaisNascimento.val()
                           , EstadoNascimento   : cboCadastro_PFPasso2_EstadoNascimento.val()
                           , CidadeNascimento   : txtCadastro_PFPasso2_CidadeNascimento.val()
                           , EstadoCivil        : cboCadastro_PFPasso2_EstadoCivil.val()
                           , Conjuge            : txtCadastro_PFPasso2_Conjuge.val()
                           , Profissao          : cboCadastro_PFPasso2_Profissao.val()
                           , CargoFuncao        : txtCadastro_PFPasso2_CargoFuncao.val()
                           , Empresa            : txtCadastro_PFPasso2_Empresa.val()
                           , TipoDocumento      : cboCadastro_PFPasso2_TipoDocumento.val()
                           , NumeroDocumento    : txtCadastro_PFPasso2_NumeroDocumento.val()
                           , OrgaoEmissor       : cboCadastro_PFPasso2_OrgaoEmissor.val()
                           , EstadoEmissao      : cboCadastro_PFPasso2_EstadoEmissao.val()
                           , DataEmissao        : txtCadastro_PFPasso2_DataEmissao.val()
                           , CodigoSegurancaCNH : txtCadastro_PFPasso2_CodigoSegCNH.val()
                           , NomeMae            : txtCadastro_PFPasso2_NomeMae.val()
                           , NomePai            : txtCadastro_PFPasso2_NomePai.val()
                           , Enderecos:  []
                           , Documentos: []
                       };

    if(gDadosDeCadastro.Nacionalidade == "3")
    {
        gDadosDeCadastro.EstadoNascimento = txtCadastro_PFPasso2_EstadoNascimento.val();
    }

    if ($("#ContentPlaceHolder1_frmDocumento2").is(":visible"))
    {
        gDadosDeCadastro.Documentos.push(
            {
                 TipoDocumento      : cboCadastro_PFPasso2_TipoDocumento2.val()
               , NumeroDocumento    : txtCadastro_PFPasso2_NumeroDocumento2.val()
               , OrgaoEmissor       : cboCadastro_PFPasso2_OrgaoEmissor2.val()
               , EstadoEmissao      : cboCadastro_PFPasso2_EstadoEmissao2.val()
               , DataEmissao        : txtCadastro_PFPasso2_DataEmissao2.val()
               , CodigoSegurancaCNH : txtCadastro_PFPasso2_CodigoSegCNH2.val()
            }
        );
    }

    gDadosDeCadastro.Enderecos.push(
    {
          CEP          : txtCadastro_PFPasso2_Endereco1_CEP.val()
        , Logradouro   : txtCadastro_PFPasso2_Endereco1_Logradouro.val()
        , Numero       : txtCadastro_PFPasso2_Endereco1_Numero.val()
        , Complemento  : txtCadastro_PFPasso2_Endereco1_Complemento.val()
        , Bairro       : txtCadastro_PFPasso2_Endereco1_Bairro.val()
        , Cidade       : txtCadastro_PFPasso2_Endereco1_Cidade.val()
        , Estado       : cboCadastro_PFPasso2_Endereco1_Estado.val()
        , Pais         : cboCadastro_PFPasso2_Endereco1_Pais.val()
        , IdEndereco   : hidCadastro_PFPasso2_IdEndereco1.val()
        , TipoEndereco : 2  //residencial
        , Principal    : "Sim"
    });
    
    if( $("#ContentPlaceHolder1_pnlSegundoEndereco").is(":visible") )
    {
        gDadosDeCadastro.Enderecos[0].Principal = "Não";

        gDadosDeCadastro.Enderecos.push(
        {
              CEP          : txtCadastro_PFPasso2_Endereco2_CEP.val()
            , Logradouro   : txtCadastro_PFPasso2_Endereco2_Logradouro.val()
            , Numero       : txtCadastro_PFPasso2_Endereco2_Numero.val()
            , Complemento  : txtCadastro_PFPasso2_Endereco2_Complemento.val()
            , Bairro       : txtCadastro_PFPasso2_Endereco2_Bairro.val()
            , Cidade       : txtCadastro_PFPasso2_Endereco2_Cidade.val()
            , Estado       : cboCadastro_PFPasso2_Endereco2_Estado.val()
            , Pais         : cboCadastro_PFPasso2_Endereco2_Pais.val()
            , IdEndereco   : hidCadastro_PFPasso2_IdEndereco2.val()
            , TipoEndereco : ($("#ContentPlaceHolder1_rdoCadastro_PFPasso2_End2").is(":checked") ? 1 : 3) // 1: comercial, 3:outros
            , Principal    : "Sim"
        });
    }

    gDadosDeCadastro.Valido = GradSite_ValidacaoComDoubleCheck($("#ContentPlaceHolder1_pnlDadosPessoais"));

    if(gDadosDeCadastro.Valido)
    {
        GradSite_Cadastro_PFPasso2_Avancar();
    }
}

function GradSite_Cadastro_PFPasso2_Avancar()
{
    //alert("avança!");

    var lForm = $("#ContentPlaceHolder1_pnlDadosPessoais");

    lForm.find("input, select, button").attr("disabled", "disabled");

    var lDados = { Acao: "SalvarPasso2", DadosDeCadastro: $.toJSON(gDadosDeCadastro) };

    
    GradSite_Cadastro_ExibirLoader(lForm
                                   , function()
                                     {
                                        GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso2_Avancar_CallBack);
                                     }
                                  );
}

function GradSite_Cadastro_PFPasso2_Avancar_CallBack(pResposta)
{
    var lForm = $("#ContentPlaceHolder1_pnlDadosPessoais");

    lForm.find("input, select, button").attr("disabled", null);

    if (pResposta.TemErro)
    {
        GradSite_ExibirMensagem("E", pResposta.Mensagem, true, "");
    }
    else
    {
        if(pResposta.Mensagem == "erro: email já cadastrado")
        {
        }
        else if(pResposta.Mensagem == "erro: cpf já cadastrado")
        {
        }
        else
        {
            GradSite_Cadastro_PFPasso_AvancarPara(3);
        }
    }

    return false;
}


function cboCadastro_PFPasso2_Nacionalidade_Change(pSender)
{
    pSender = $(pSender);

    var lPanel = $("#ContentPlaceHolder1_pnlPaisDeNascimento");

    if(pSender.val() == "3")
    {
        lPanel.show();

        lPanel.prev("div").removeClass("col3").addClass("col4");

        lPanel.next("div")
                .removeClass("col3").addClass("col4")
                .next("div")
                    .removeClass("col3").addClass("col4");
        
        cboCadastro_PFPasso2_PaisNascimento.val("").attr("disabled", null);
        cboCadastro_PFPasso2_PaisNascimento.find("option[value='BRA']").hide();
    }
    else
    {
        lPanel.hide();

        lPanel.prev("div").removeClass("col4").addClass("col3");

        lPanel.next("div")
                .removeClass("col4").addClass("col3")
                .next("div")
                    .removeClass("col4").addClass("col3");
        
        cboCadastro_PFPasso2_PaisNascimento.find("option[value='BRA']").show();
        cboCadastro_PFPasso2_PaisNascimento.val("BRA").attr("disabled", "disabled");
    }

    cboCadastro_PFPasso2_PaisNascimento_Change(cboCadastro_PFPasso2_PaisNascimento);
}

function cboCadastro_PFPasso2_PaisNascimento_Change(pSender)
{
    pSender = $(pSender);

    if(pSender.val() == "BRA")
    {
        cboCadastro_PFPasso2_EstadoNascimento.show();
        txtCadastro_PFPasso2_EstadoNascimento.hide();

        cboCadastro_PFPasso2_EstadoNascimento.addClass("validate[required]")
                                             .addClass("EstiloCampoObrigatorio");

        txtCadastro_PFPasso2_CidadeNascimento.addClass("validate[required]")
                                             .addClass("EstiloCampoObrigatorio");

        //txtCadastro_PFPasso2_EstadoNascimento.removeClass("validate[required]")
        //                                     .removeClass("EstiloCampoObrigatorio");

        txtCadastro_PFPasso2_EstadoNascimento.hide();

    }
    else
    {
        cboCadastro_PFPasso2_EstadoNascimento.hide();
        txtCadastro_PFPasso2_EstadoNascimento.show();

        cboCadastro_PFPasso2_EstadoNascimento.removeClass("validate[required]")
                                             .removeClass("EstiloCampoObrigatorio");

        txtCadastro_PFPasso2_CidadeNascimento.removeClass("validate[required]")
                                             .removeClass("EstiloCampoObrigatorio");

        //txtCadastro_PFPasso2_EstadoNascimento.addClass("validate[required]")
        //                                     .addClass("EstiloCampoObrigatorio");
    }

    return false;
}

function cboTipoDocumento_Change(pSender)
{
    var lCombo = $(pSender);

    var lParagrafo = lCombo.closest("div").find("p:last");

    if (lCombo.val() == "CH")
    {
        //lCombo.parent().css({ marginLeft: "" });

        lParagrafo.show().find("input").attr("class", "validate[required] EstiloCampoObrigatorio");
    }
    else
    {
        //lCombo.parent().css({ marginLeft: "4.5em" });

        lParagrafo.hide().find("input").attr("class", "");
    }

}

function lnkSegundoDocumento_Click(pSender)
{
    if (pSender == null)
        pSender = $("div.FormDocumento:first a");

    var lForm = $("#ContentPlaceHolder1_frmDocumento2");

    if (lForm.is(":visible"))
    {
        lForm.find("input").attr("class", "");
        lForm.find("select").attr("class", "");

        lForm.hide();

        $(pSender).html("clique aqui");
    }
    else
    {
        lForm.find("input").attr("class", "validate[required] EstiloCampoObrigatorio");
        lForm.find("select").attr("class", "validate[required]");
        lForm.find("input[maxlength=10]").attr("class", "validate[required,custom[data]] Mascara_Data EstiloCampoObrigatorio");

        lForm.show();

        $(pSender).html("(cancelar)");
    }

    cboTipoDocumento_Change(lForm.find("select:first"));

    return false;
}

function lblImagemAjudaCNH_Click(pSender)
{
    $(pSender).next(".ImagemAjudaCNH").toggle();
}

function imgImagemAjudaCNH_Click(pSender)
{
    $(pSender).hide();
}




/*=====================================

    Funções - PASSO 3

=====================================*/


function GradSite_Cadastro_PFPasso3_CarregarSuitability()
{
    var respostas = document.querySelectorAll('[name=rdoSuit_09]:checked');
    var values = [];
    var opcoes = [];
    for (var i = 0; i < respostas.length; i++)
    {
        opcoes += (parseInt(respostas[i].value.split("|")[0]))+"|"; // Devido a troca para valor quebrado
        values.push(parseFloat(respostas[i].value.split("|")[1]));
    }


    var pontos = values.reduce(function (a, b) { return a + b; }, 0);

    var lSuit = {
                      Resp1 : $("input[name='rdoSuit_01']:checked").val()
                    , Resp2 : $("input[name='rdoSuit_02']:checked").val()
                    , Resp3 : $("input[name='rdoSuit_03']:checked").val()
                    , Resp4 : $("input[name='rdoSuit_04']:checked").val()
                    , Resp5 : $("input[name='rdoSuit_05']:checked").val()
                    , Resp6 : $("input[name='rdoSuit_06']:checked").val()
                    , Resp7 : $("input[name='rdoSuit_07']:checked").val()
                    , Resp8 : $("input[name='rdoSuit_08']:checked").val()
                      //, Resp9 : $("input[name='rdoSuit_09']:checked").val()
                    , Resp9 : opcoes
                    , Resp10: $("input[name='rdoSuit_10']:checked").val()
                    , Resp11: $("input[name='rdoSuit_11']:checked").val()
                };

                lSuit.Valido = (lSuit.Resp1 !== undefined) && (lSuit.Resp2 !== undefined) && (lSuit.Resp3 !== undefined) && (lSuit.Resp4 !== undefined) && (lSuit.Resp5 !== undefined) && (lSuit.Resp6 !== undefined) && (lSuit.Resp7 !== undefined) && (lSuit.Resp8 !== undefined) && (lSuit.Resp9 !== undefined) && (lSuit.Resp10 !== undefined) && (lSuit.Resp11 !== undefined);

    // se já preencheu alguma vez, pode ignorar...
    //if($("#ContentPlaceHolder1_hidCadastro_PFPasso3_JaPreencheuSuit").val() == "true")
    //    lSuit.Valido = true;

    return lSuit;
}

function GradSite_Cadastro_PFPasso3_Verificar()
{
    var lRepresentante = {};

    if( txtCadastro_PFPasso3_OutrosRendimentos.val() == "")
    {
        txtCadastro_PFPasso3_SalarioProlabore.attr("class",  "ValorMonetario validate[required,custom[numeroFormatado]]");
    }
    else
    {
        txtCadastro_PFPasso3_SalarioProlabore.attr("class",  "ValorMonetario validate[custom[numeroFormatado]]");
    }

    if(rdoCadastro_PFPasso3_Procurador_Sim.is(":checked"))
    {
        lRepresentante = {
                              SituacaoLegal     : cboCadastro_PFPasso3_RepSituacaoLegal.val()
                            , Nome              : txtCadastro_PFPasso3_RepNomeCompleto.val()
                            , CPF               : txtCadastro_PFPasso3_RepCPF.val()
                            , DataNascimento    : txtCadastro_PFPasso3_RepDataNascimento.val()
                            , TipoDocumento     : cboCadastro_PFPasso3_RepTipoDocumento.val()
                            , OrgaoEmissor      : cboCadastro_PFPasso3_RepOrgaoEmissor.val()
                            , NumeroDocumento   : txtCadastro_PFPasso3_RepNumeroDocumento.val()
                            , EstadoEmissor     : cboCadastro_PFPasso3_RepEstadoEmissao.val()
                         };
    }

    var lSuit = GradSite_Cadastro_PFPasso3_CarregarSuitability();

    gDadosDeCadastro = {
                             SituacaoFinanceira : {
                                                        VlTotalSalarioProLabore      : txtCadastro_PFPasso3_SalarioProlabore.val()
                                                      , VlTotalAplicacaoFinanceira   : txtCadastro_PFPasso3_ValorTotalEmAplicFin.val()
                                                      , VlTotalOutrosRendimentos     : txtCadastro_PFPasso3_OutrosRendimentos.val()
                                                      , VlTotalOutrosRendimentosDesc : txtCadastro_PFPasso3_OutrosRendimentosDesc.val()
                                                      , VlTotalBensImoveis           : txtCadastro_PFPasso3_TotalEmBensImoveis.val()
                                                      , VlTotalBensMoveis            : txtCadastro_PFPasso3_TotalEmBensMoveis.val()
                                                  }
                           , Contas                     : gPasso3_ListaDeContasBancarias
                           , PessoaVinculada            : ($("input[name='ctl00$ContentPlaceHolder1$rdoPessoaVinculada']:checked").val())
                           , PessoaPoliticamenteExposta : (rdoCadastro_PFPasso3_PPE_Sim.is(":checked")                ? "Sim" : "Não")
                           , OperaPorContaPropria       : (rdoCadastro_PFPasso3_OperaContaPropria_Sim.is(":checked")  ? "Sim" : "Não")
                           , USPerson                   : (rdoCadastro_PFPasso3_USPerson_Sim.is(":checked")           ? "Sim" : "Não")
                           , CienteRegulamento          : (chkCadastro_PFPasso3_Ciente_Doc_Regulamento.is(":checked") ? "Sim" : "Não")
                           , CienteProspecto            : (chkCadastro_PFPasso3_Ciente_Doc_Prospecto.is(":checked")   ? "Sim" : "Não")
                           , CienteLamina               : (chkCadastro_PFPasso3_Ciente_Doc_Lamina.is(":checked")      ? "Sim" : "Não")
                           , PropositoGradual           : txtCadastro_PFPasso3_Proposito.val()
                           , Emancipado                 : (rdoCadastro_PFPasso3_Emancipado_Sim.is(":checked")         ? "Sim" : "Não")
                           , Procurador                 : (rdoCadastro_PFPasso3_Procurador_Sim.is(":checked")         ? "Sim" : "Não")
                           , NomeCliente                : txtCadastro_PFPasso3_CliNomeCompleto.val()
                           , CPFCliente                 : txtCadastro_PFPasso3_CliCPF.val()
                           , Representante              : lRepresentante
                           , Suitability                : lSuit
                       };

    GradSite_Cadastro_PFPasso3_RemoverValidacaoContas();

    if(gDadosDeCadastro.Contas.length > 0)
    {
        gDadosDeCadastro.Valido = GradSite_ValidacaoComDoubleCheck($("#ContentPlaceHolder1_pnlDadosFinanceiros"));

        if(gDadosDeCadastro.Valido)
        {
            if(gDadosDeCadastro.Suitability.Valido)
            {
                GradSite_Cadastro_PFPasso3_Avancar();
            }
            else
            {
                alert("Favor preencher os dados de perfil.");

                $(document).scrollTop(1200);
            }
        }
        else
        {
            alert("Existem dados inválidos, favor verificar...");

            $(document).scrollTop(530);
        }
    }
    else
    {
        alert("Favor adicionar ao menos uma conta bancária.");

        $(document).scrollTop(780);
    }

    return false;
}


function GradSite_Cadastro_PFPasso3Cambio_Verificar() 
{
    var lRepresentante = {};

    txtCadastro_PFPasso3_OutrosRendimentosCambio.attr("class", "ValorMonetario validate[required,custom[numeroFormatado]]");

    if (rdoCadastro_PFPasso3_ProcuradorCambio_Sim.is(":checked")) {
        lRepresentante = {
                            SituacaoLegal       : cboCadastro_PFPasso3_RepSituacaoLegalCambio.val()
                            , Nome              : txtCadastro_PFPasso3_RepNomeCompletoCambio.val()
                            , CPF               : txtCadastro_PFPasso3_RepCPFCambio.val()
                            , DataNascimento    : txtCadastro_PFPasso3_RepDataNascimentoCambio.val()
                            , TipoDocumento     : cboCadastro_PFPasso3_RepTipoDocumentoCambio.val()
                            , OrgaoEmissor      : cboCadastro_PFPasso3_RepOrgaoEmissorCambio.val()
                            , NumeroDocumento   : txtCadastro_PFPasso3_RepNumeroDocumentoCambio.val()
                            , EstadoEmissor     : cboCadastro_PFPasso3_RepEstadoEmissaoCambio.val()
        };
    }

    //var lSuit = GradSite_Cadastro_PFPasso3_CarregarSuitability();

    gDadosDeCadastro = 
    {
        SituacaoFinanceira: 
        {
              VlTotalSalarioProLabore       : ""
            , VlTotalAplicacaoFinanceira    : ""
            , VlTotalOutrosRendimentos      : txtCadastro_PFPasso3_OutrosRendimentosCambio.val()
            , VlTotalOutrosRendimentosDesc  : "Renda Mensal Câmbio"
            , VlTotalBensImoveis            : ""
            , VlTotalBensMoveis             : ""
        }
        , Contas                            : gPasso3_ListaDeContasBancarias
        , PessoaVinculada                   : 0
        , PessoaPoliticamenteExposta        : "Não"
        , OperaPorContaPropria              : "Sim"
        , USPerson                          : "Não"
        , CienteRegulamento                 : "Não"
        , CienteProspecto                   : "Não"
        , CienteLamina                      : "Não"
        , PropositoGradual                  : "Câmbio"
        , Emancipado                        : "Não"
        , Procurador                        : (rdoCadastro_PFPasso3_ProcuradorCambio_Sim.is(":checked") ? "Sim" : "Não")
        , NomeCliente                       : txtCadastro_PFPasso3_CliNomeCompleto.val()
        , CPFCliente                        : txtCadastro_PFPasso3_CliCPF.val()
        , Representante                     : lRepresentante
        //, Suitability                       : lSuit
    };

    gDadosDeCadastro.Valido = GradSite_ValidacaoComDoubleCheck($("#ContentPlaceHolder1_pnlDadosFinanceirosCambio"));

    if (gDadosDeCadastro.Valido) 
    {
        GradSite_Cadastro_PFPasso3Cambio_Avancar();
    }
    else
    {
        alert("Existem dados inválidos, favor verificar...");

        $(document).scrollTop(530);
    }

    return false;
}

function GradSite_Cadastro_PFPasso3_Avancar()
{
    var lForm = $("#ContentPlaceHolder1_pnlDadosFinanceiros");

    lForm.find("input, select, button").attr("disabled", "disabled");

    var lDados = { Acao: "SalvarPasso3", DadosDeCadastro: $.toJSON(gDadosDeCadastro) };

    GradSite_Cadastro_ExibirLoader(  lForm
                                   , function()
                                     {
                                        GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso3_Avancar_CallBack);
                                     }
                                  );
    return false;
}

function GradSite_Cadastro_PFPasso3Cambio_Avancar() 
{
    var lForm = $("#ContentPlaceHolder1_pnlDadosFinanceirosCambio");

    lForm.find("input, select, button").attr("disabled", "disabled");

    var lDados = { Acao: "SalvarPasso3Cambio", DadosDeCadastro: $.toJSON(gDadosDeCadastro) };

    GradSite_Cadastro_ExibirLoader(lForm
                                   , function () {
                                       GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso3Cambio_Avancar_CallBack);
                                   }
                                  );
    return false;
}


function GradSite_Cadastro_PFPasso3_Avancar_CallBack(pResposta)
{
//    var lForm = $("#ContentPlaceHolder1_pnlDadosFinanceiros");
//    lForm.find("input, select, button").attr("disabled", null);
//    if (pResposta.TemErro)
//    {
//        GradSite_ExibirMensagem("E", pResposta.Mensagem, true, "");
//    }
//    else
//    {
//        $("#ContentPlaceHolder1_ucAbasMeuCadastro1_liMeuPerfil").removeClass("inativo");
//        Cadastro_PreencherListaDeFundosRecomendados(pResposta.ObjetoDeRetorno);
//        GradSite_Cadastro_PFPasso_AvancarPara(4);
    //    }
    GradSite_Cadastro_PFPasso_AvancarPara(4);
    $("#ContentPlaceHolder1_pnlContratos").show();
    $("#ContentPlaceHolder1_pnlContratos").removeClass("SoFundos");

    return false;
}

function GradSite_Cadastro_PFPasso3Cambio_Avancar_CallBack(pResposta) 
{

    var lForm = $("#ContentPlaceHolder1_pnlDadosFinanceirosCambio");
    lForm.find("input, select, button").attr("disabled", null);

    if (pResposta.TemErro)
    {
        GradSite_ExibirMensagem("E", pResposta.Mensagem, true, "");
    }
    else 
    {
        $("#ContentPlaceHolder1_liDadosContratuais").removeClass("inativo").attr("data-desabilitado", null);

        //Sauron_EnviarScreenView("Cadastro Passo 4");
        //GradSite_Cadastro_PFPasso4_Finalizar();
        //var lDados = { Acao: "SalvarPasso4" };
        //GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso4_Finalizar_CallBack);
        //$("#ContentPlaceHolder1_liDadosContratuais a").click();
        //GradSite_Cadastro_OcultarLoader(null, true);
        

        var lURLs = pResposta.Mensagem.split(",");

//        if ($("#ContentPlaceHolder1_pnlContratos").hasClass("SoFundos")) 
//        {
//            $("#lnkFichaCadastral_Fundo").attr("href", lURLs[0]);
//            //$("#lnkTermo").attr("href", lURLs[1]);

//            if ($("#lnkFichaCadastralCambio").length) {
//                $("#lnkFichaCadastralCambio").attr("href", lURLs[2]);
//            }

//            $("#pnlCadastroRealizado_Fundos").show();

//            $("#ContentPlaceHolder1_pnlBotaoFinalizar").hide();
//        }
//        else 
//        {
            $("#lnkFichaCadastral").attr("href", lURLs[0]);

            if ($("#lnkFichaCadastralCambio").length) 
            {
                $("#lnkFichaCadastralCambio").attr("href", lURLs[2]);
            }

            $("#lnkTermo").attr("href", lURLs[1]);

            $("#pnlCadastroRealizado").show();

            $("#ContentPlaceHolder1_pnlBotaoFinalizar").hide();
//        }

        //$("#ContentPlaceHolder1_pnlContratos").hide();
        //GradSite_Cadastro_OcultarLoader("Cadastro finalizado com sucesso!", false);
        GradSite_Cadastro_OcultarLoader(null, true);
        $("#ContentPlaceHolder1_liDadosContratuais a").click();

    }

    return false;
}

function GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(pCampo, pMensagem)
{
    GradSite_Formulario_ExibirMensagemDeValidacao(pCampo.attr("id"), pMensagem, false);

    pCampo.focus();
}


function GradSite_Cadastro_PFPasso3_ImplementarValidacaoContas()
{
    cboCadastro_PFPasso3_ContasBancarias_TipoConta.addClass("validate[required]");
    cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.addClass("validate[required]");
    cboCadastro_PFPasso3_ContasBancarias_Banco.addClass("validate[required]");
    cboCadastro_PFPasso3_ContasBancarias_BancoCambio.addClass("validate[required]");
    txtCadastro_PFPasso3_ContasBancarias_Agencia.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_Conta.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_ContaCambio.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_ContaDigito.addClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.addClass("validate[required,custom[onlyNumber]]");

    if($("#rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_Nao").is(":checked"))
    {
        txtCadastro_PFPasso3_ContasBancarias_NomeTitular.addClass("validate[required]");
        txtCadastro_PFPasso3_ContasBancarias_CPFTitular.addClass("validate[required,custom[validatecpf]]");
    }
    else
    {
        txtCadastro_PFPasso3_ContasBancarias_NomeTitular.removeClass("validate[required]");
        txtCadastro_PFPasso3_ContasBancarias_CPFTitular.removeClass("validate[required,custom[validatecpf]]");
    }
}

function GradSite_Cadastro_PFPasso3_RemoverValidacaoContas()
{
    $("#pnlFormInformacoesBancarias").find(".formError").click();

    cboCadastro_PFPasso3_ContasBancarias_TipoConta.removeClass("validate[required]");
    cboCadastro_PFPasso3_ContasBancarias_Banco.removeClass("validate[required]");
    txtCadastro_PFPasso3_ContasBancarias_Agencia.removeClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.removeClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_Conta.removeClass("validate[required,custom[onlyNumber]]");
    txtCadastro_PFPasso3_ContasBancarias_ContaDigito.removeClass("validate[required,custom[onlyNumber]]");

    txtCadastro_PFPasso3_ContasBancarias_NomeTitular.removeClass("validate[required]");
    txtCadastro_PFPasso3_ContasBancarias_CPFTitular.removeClass("validate[required,custom[validatecpf]]");
}

function GradSite_Cadastro_PFPasso3_InstanciarContaBancaria()
{
    $("#ContentPlaceHolder1_pnlFormNovaContaBancaria").find(".formError").remove();

    if(cboCadastro_PFPasso3_ContasBancarias_Banco.val() == "745" && txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val() == "")
    {
        var lConta = txtCadastro_PFPasso3_ContasBancarias_Conta.val();
        var lDigito;

        if(lConta != "" && lConta.length > 1)
        {
            lDigito = lConta.charAt(lConta.length - 1);
            lConta = lConta.substr(0, lConta.length - 1);

            txtCadastro_PFPasso3_ContasBancarias_Conta.val(lConta);
            txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val(lDigito);
        }
    }

    var lConta = { 
                   Banco           : cboCadastro_PFPasso3_ContasBancarias_Banco.val()
                 , BancoDesc       : cboCadastro_PFPasso3_ContasBancarias_Banco.find("option:selected").text()
                 , TipoConta       : cboCadastro_PFPasso3_ContasBancarias_TipoConta.val()
                 , TipoContaDesc   : cboCadastro_PFPasso3_ContasBancarias_TipoConta.find("option:selected").text()
                 , Agencia         : txtCadastro_PFPasso3_ContasBancarias_Agencia.val()
                 , AgenciaDigito   : txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.val()
                 , Conta           : txtCadastro_PFPasso3_ContasBancarias_Conta.val()
                 , ContaDigito     : txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val()
                 , Titular         : $("#rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_Sim").is(":checked")
                 , NomeTitular     : txtCadastro_PFPasso3_ContasBancarias_NomeTitular.val()
                 , CPFTitular      : txtCadastro_PFPasso3_ContasBancarias_CPFTitular.val()
                 , Valida          : true
                 };

    if(lConta.Banco == "")
    {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(cboCadastro_PFPasso3_ContasBancarias_Banco, "Selecione o banco.");

        lConta.Valida = false;
    }
    
    if(lConta.TipoConta == "")
    {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(cboCadastro_PFPasso3_ContasBancarias_TipoConta, "Selecione o tipo de conta.");

        lConta.Valida = false;
    }

    //adiciona os validadores antes:
    GradSite_Cadastro_PFPasso3_ImplementarValidacaoContas();

    pnlFormNovaContaBancaria.addClass("validationEngineContainer").validationEngine(gValidationEngineDefaults);

    lConta.Valida = GradSite_ValidacaoComDoubleCheck(pnlFormNovaContaBancaria);

    /*
    Agência :
        Regra geral: 04 números + 01 dígito 
    Regra Banco do Brasil: substituir o X por 0   
    Conta Corrente:
        Itaú:  05 números e 01 digito   Ex:- 52524 - 6 
        Hsbc:  05 números e 02 digitos   Ex:- 65235 – 23
        Citibank:  colocar o último número como digito   Ex:- 1234567   irá ficar  123456 - 7 

    */
    if (lConta.Valida && lConta.Agencia.length < 4)
    {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Agencia, "Campo 'Agência' exige ao menos 4 dígitos.");

        lConta.Valida = false;
    }

    if (lConta.Valida && lConta.Banco == "001")
    {
        //banco do brasil
        lConta.Agencia = lConta.Agencia.toUpperCase().replace("X", "0");

        lConta.AgenciaDigito = lConta.AgenciaDigito.toUpperCase().replace("X", "0");
    }
    
    if (lConta.Valida && lConta.Banco == "341")
    {
        //itaú

        if (lConta.Valida && lConta.Conta.length < 5)
        {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Conta, "Campo 'Conta' exige ao menos 5 dígitos.");

            lConta.Valida = false;
        }
    }

    if (lConta.Valida && lConta.Banco == "399")
    {
        //HSBC

        if (lConta.Valida && lConta.Conta.length < 5)
        {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Conta, "Campo 'Conta' exige ao menos 5 dígitos.");

            lConta.Valida = false;
        }

        if (lConta.Valida && lConta.ContaDigito.length < 2)
        {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_ContaDigito, "Campo 'Dígito da Conta' exige ao menos 2 dígitos.");

            lConta.Valida = false;
        }
    }

    if (lConta.Valida && lConta.Banco == "745")
    {
        //Citibank

        if(lConta.Valida && lConta.Conta.length > 2 && lConta.ContaDigito.length == 0)
        {
            //se não colocar nada como dígito, assume que o último número da conta é o dígito

            lConta.ContaDigito = lConta.Conta.charAt(lConta.Conta.length - 1);
            lConta.Conta       = lConta.Conta.substr(0, lConta.Conta.length - 1);
        }
    }
    /*
    if (lConta.Valida && !lConta.Titular)
    {
        if(lConta.NomeTitular == "")
        {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_NomeTitular, "Campo 'Nome do Titular' é obrigatório.");

            lConta.Valida = false;
        }
        else
        {
            if(lConta.CPFTitular == "")
            {
                GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_CPFTitular, "Campo 'CPF do Titular' é obrigatório.");

                lConta.Valida = false;
            }
        }
    }*/

    if(lConta.Valida)
    {
        $("#pnlFormInformacoesBancarias").find("div.formError").remove();

        $("#pnlFormInformacoesBancarias").find(".CampoComErro").removeClass("CampoComErro");

        GradSite_Cadastro_PFPasso3_RemoverValidacaoContas();
    }

    return lConta;
}

function GradSite_Cadastro_PFPasso3_InstanciarContaBancariaCambio() {
    $("#ContentPlaceHolder1_pnlFormNovaContaBancariaCambio").find(".formError").remove();

    if (cboCadastro_PFPasso3_ContasBancarias_BancoCambio.val() == "745" && txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val() == "") {
        var lConta = txtCadastro_PFPasso3_ContasBancarias_ContaCambio.val();
        var lDigito;

        if (lConta != "" && lConta.length > 1) {
            lDigito = lConta.charAt(lConta.length - 1);
            lConta = lConta.substr(0, lConta.length - 1);

            txtCadastro_PFPasso3_ContasBancarias_ContaCambio.val(lConta);
            txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val(lDigito);
        }
    }

    var lConta = {
        Banco: cboCadastro_PFPasso3_ContasBancarias_BancoCambio.val()
                 , BancoDesc: cboCadastro_PFPasso3_ContasBancarias_BancoCambio.find("option:selected").text()
                 , TipoConta: cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.val()
                 , TipoContaDesc: cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.find("option:selected").text()
                 , Agencia: txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio.val()
                 , AgenciaDigito: txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio.val()
                 , Conta: txtCadastro_PFPasso3_ContasBancarias_ContaCambio.val()
                 , ContaDigito: txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val()
                 , Titular: $("#rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_SimCambio").is(":checked")
                 , NomeTitular: txtCadastro_PFPasso3_ContasBancarias_NomeTitularCambio.val()
                 , CPFTitular: txtCadastro_PFPasso3_ContasBancarias_CPFTitularCambio.val()
                 , Valida: true
    };

    if (lConta.Banco == "") {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(cboCadastro_PFPasso3_ContasBancarias_BancoCambio, "Selecione o banco.");

        lConta.Valida = false;
    }

    if (lConta.TipoConta == "") {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio, "Selecione o tipo de conta.");

        lConta.Valida = false;
    }

    //adiciona os validadores antes:
    GradSite_Cadastro_PFPasso3_ImplementarValidacaoContas();

    pnlFormNovaContaBancariaCambio.addClass("validationEngineContainer").validationEngine(gValidationEngineDefaults);

    lConta.Valida = GradSite_ValidacaoComDoubleCheck(pnlFormNovaContaBancariaCambio);

    /*
    Agência :
    Regra geral: 04 números + 01 dígito 
    Regra Banco do Brasil: substituir o X por 0   
    Conta Corrente:
    Itaú:  05 números e 01 digito   Ex:- 52524 - 6 
    Hsbc:  05 números e 02 digitos   Ex:- 65235 – 23
    Citibank:  colocar o último número como digito   Ex:- 1234567   irá ficar  123456 - 7 

    */
    if (lConta.Valida && lConta.Agencia.length < 4) {
        GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Agencia, "Campo 'Agência' exige ao menos 4 dígitos.");

        lConta.Valida = false;
    }

    if (lConta.Valida && lConta.Banco == "001") {
        //banco do brasil
        lConta.Agencia = lConta.Agencia.toUpperCase().replace("X", "0");

        lConta.AgenciaDigito = lConta.AgenciaDigito.toUpperCase().replace("X", "0");
    }

    if (lConta.Valida && lConta.Banco == "341") {
        //itaú

        if (lConta.Valida && lConta.Conta.length < 5) {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Conta, "Campo 'Conta' exige ao menos 5 dígitos.");

            lConta.Valida = false;
        }
    }

    if (lConta.Valida && lConta.Banco == "399") {
        //HSBC

        if (lConta.Valida && lConta.Conta.length < 5) {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_Conta, "Campo 'Conta' exige ao menos 5 dígitos.");

            lConta.Valida = false;
        }

        if (lConta.Valida && lConta.ContaDigito.length < 2) {
            GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_ContaDigito, "Campo 'Dígito da Conta' exige ao menos 2 dígitos.");

            lConta.Valida = false;
        }
    }

    if (lConta.Valida && lConta.Banco == "745") {
        //Citibank

        if (lConta.Valida && lConta.Conta.length > 2 && lConta.ContaDigito.length == 0) {
            //se não colocar nada como dígito, assume que o último número da conta é o dígito

            lConta.ContaDigito = lConta.Conta.charAt(lConta.Conta.length - 1);
            lConta.Conta = lConta.Conta.substr(0, lConta.Conta.length - 1);
        }
    }
    /*
    if (lConta.Valida && !lConta.Titular)
    {
    if(lConta.NomeTitular == "")
    {
    GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_NomeTitular, "Campo 'Nome do Titular' é obrigatório.");

    lConta.Valida = false;
    }
    else
    {
    if(lConta.CPFTitular == "")
    {
    GradSite_Cadastro_ExibirMensagemDeErroDeValidacao(txtCadastro_PFPasso3_ContasBancarias_CPFTitular, "Campo 'CPF do Titular' é obrigatório.");

    lConta.Valida = false;
    }
    }
    }*/

    if (lConta.Valida) {
        $("#pnlFormInformacoesBancarias").find("div.formError").remove();

        $("#pnlFormInformacoesBancarias").find(".CampoComErro").removeClass("CampoComErro");

        GradSite_Cadastro_PFPasso3_RemoverValidacaoContas();
    }

    return lConta;
}

function btnCadastro_PFPasso3_ExcluirContaBancaria_Click(pSender)
{
    if (confirm("Deseja realmente excluir esta conta bancária?"))
    {
        pSender = $(pSender);

        var lTR = pSender.closest("tr");

        var lTBody = lTR.parent();

        var lIndice = (lTR.index() - 1);

        pSender.closest("tr").remove();

        gPasso3_ListaDeContasBancarias.splice(lIndice, 1);

        if(lTBody.find("tr").length == 1)
        {
            //só sobrou o template

            lTBody.append("<tr class='NenhumItem'><td colspan='9'>(nenhuma conta incluída)</td></tr>");
        }
    }

    return false;
}

function btnCadastro_PFPasso3_ExcluirContaBancariaCambio_Click(pSender) {
    if (confirm("Deseja realmente excluir esta conta bancária?")) {
        pSender = $(pSender);

        var lTR = pSender.closest("tr");

        var lTBody = lTR.parent();

        var lIndice = (lTR.index() - 1);

        pSender.closest("tr").remove();

        gPasso3_ListaDeContasBancarias.splice(lIndice, 1);

        if (lTBody.find("tr").length == 1) {
            //só sobrou o template

            lTBody.append("<tr class='NenhumItem'><td colspan='9'>(nenhuma conta incluída)</td></tr>");
        }
    }

    return false;
}

function btnCadastro_PFPasso3_AdicionarContaBancaria_Click(pSender)
{
    var lTabela = $("#tblCadastro_PFPasso3_ContaBancaria");

    if(txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val().toUpperCase() == "X")
    {
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val("0");
    }

    var lConta = GradSite_Cadastro_PFPasso3_InstanciarContaBancaria();

    if (lConta.Valida)
    {
        var lTemplateTr = lTabela.find(".Template").clone();

        lTemplateTr.find("td:eq(0)").html( lConta.BancoDesc );
        lTemplateTr.find("td:eq(1)").html( lConta.Banco );
        lTemplateTr.find("td:eq(2)").html( lConta.TipoContaDesc );
        lTemplateTr.find("td:eq(3)").html( lConta.Agencia );
        lTemplateTr.find("td:eq(4)").html( lConta.AgenciaDigito );
        lTemplateTr.find("td:eq(5)").html( lConta.Conta );
        lTemplateTr.find("td:eq(6)").html( ((lConta.ContaDigito != "") ? lConta.ContaDigito : "&nbsp;" ));

        lTemplateTr.removeClass("Template");
        lTemplateTr.show();

        lTabela.find("tbody").append(lTemplateTr);

        lTabela.find("tr.NenhumItem").remove();

        cboCadastro_PFPasso3_ContasBancarias_Banco.focus();
        txtCadastro_PFPasso3_ContasBancarias_Agencia.val("");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.val("");
        txtCadastro_PFPasso3_ContasBancarias_Conta.val("");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val("");

        gPasso3_ListaDeContasBancarias.push( lConta );
    }

    return false;
}

function btnCadastro_PFPasso3_AdicionarContaBancariaCambio_Click(pSender) {
    var lTabela = $("#tblCadastro_PFPasso3_ContaBancariaCambio");

    if (txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val().toUpperCase() == "X") {
        txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val("0");
    }

    var lConta = GradSite_Cadastro_PFPasso3_InstanciarContaBancariaCambio();

    if (lConta.Valida) {
        var lTemplateTr = lTabela.find(".Template").clone();

        lTemplateTr.find("td:eq(0)").html(lConta.BancoDesc);
        lTemplateTr.find("td:eq(1)").html(lConta.Banco);
        lTemplateTr.find("td:eq(2)").html(lConta.TipoContaDesc);
        lTemplateTr.find("td:eq(3)").html(lConta.Agencia);
        lTemplateTr.find("td:eq(4)").html(lConta.AgenciaDigito);
        lTemplateTr.find("td:eq(5)").html(lConta.Conta);
        lTemplateTr.find("td:eq(6)").html(((lConta.ContaDigito != "") ? lConta.ContaDigito : "&nbsp;"));

        lTemplateTr.removeClass("Template");
        lTemplateTr.show();

        lTabela.find("tbody").append(lTemplateTr);

        lTabela.find("tr.NenhumItem").remove();

        cboCadastro_PFPasso3_ContasBancarias_Banco.focus();
        txtCadastro_PFPasso3_ContasBancarias_Agencia.val("");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.val("");
        txtCadastro_PFPasso3_ContasBancarias_Conta.val("");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val("");

        gPasso3_ListaDeContasBancarias.push(lConta);
    }

    return false;
}

function rdoCadastro_PFPasso3_ContasBancarias_Click(pSender)
{
    if($("#rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_Nao").is(":checked"))
    {
        $("#pnlTitularConta").show();
    }
    else
    {
        $("#pnlTitularConta").hide();
    }
}

function rdoCadastro_PFPasso3_ContasBancariasCambio_Click(pSender) {
    if ($("#rdoCadastro_PFPasso3_ContasBancarias_ContaTitular_NaoCambio").is(":checked")) {
        $("#pnlTitularContaCambio").show();
    }
    else {
        $("#pnlTitularContaCambio").hide();
    }
}

function rdoCadastro_PFPasso3_Opera_Click(pSender)
{
    if($("#ContentPlaceHolder1_rdoCadastro_PFPasso3_OperaContaPropria_Nao").is(":checked"))
    {
        $("#ContentPlaceHolder1_pnlDadosClienteOpera").show();

        txtCadastro_PFPasso3_CliNomeCompleto.addClass("validate[required]");
        txtCadastro_PFPasso3_CliCPF.addClass("validate[required,custom[validatecpfcnpj]]");
    }
    else
    {
        $("#ContentPlaceHolder1_pnlDadosClienteOpera").hide();
        
        txtCadastro_PFPasso3_CliNomeCompleto.removeClass("validate[required]");
        txtCadastro_PFPasso3_CliCPF.removeClass("validate[required,custom[validatecpfcnpj]]");
    }
}


function cboCadastro_PFPasso3_ContasBancarias_Banco_OnChange(pSender) {

    if (pSender.id == "ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_Banco") {
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("disabled", null);

        txtCadastro_PFPasso3_ContasBancarias_Agencia.attr("maxlength", 4);
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.attr("maxlength", 1);

        var lVal;

        switch ($(pSender).val()) {
            case "001": //--> Banco do Brasil

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 1);

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val(lVal.charAt(0));

                break;

            case "341": //--> Itaú

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 5);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 1);

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val(lVal.charAt(0));

                break;

            case "399": //--> HSBC

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 5);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 2);

                break;

            case "745": //--> Citibank

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 2);

                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr('disabled', 'disabled');

                break;

            case "151": //--> Nossa Caixa

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 4);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 1);

                $(pSender).val("001")

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val(lVal.charAt(0));

                break;

            default:   //--> Demais bancos

                txtCadastro_PFPasso3_ContasBancarias_Conta.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigito.attr("maxlength", 2);

                break;
        }
    }

    if (pSender.id == "ContentPlaceHolder1_cboCadastro_PFPasso3_ContasBancarias_BancoCambio") {
        txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("disabled", null);

        txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio.attr("maxlength", 4);
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio.attr("maxlength", 1);

        var lVal;

        switch ($(pSender).val()) {
            case "001": //--> Banco do Brasil

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 1);

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val(lVal.charAt(0));

                break;

            case "341": //--> Itaú

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 5);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 1);

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val(lVal.charAt(0));

                break;

            case "399": //--> HSBC

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 5);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 2);

                break;

            case "745": //--> Citibank

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 2);

                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr('disabled', 'disabled');

                break;

            case "151": //--> Nossa Caixa

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 4);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 1);

                $(pSender).val("001")

                lVal = txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val();

                if (lVal.length > 1) txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val(lVal.charAt(0));

                break;

            default:   //--> Demais bancos

                txtCadastro_PFPasso3_ContasBancarias_ContaCambio.attr("maxlength", 8);
                txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.attr("maxlength", 2);

                break;
        }
    }

    return false;
}


function rdoCadastro_PFPasso3_Emancipado_Click(pSender)
{
    if ($(pSender).val() == "Nao")
    {
        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Nao").attr("disabled", "disabled");
        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim").attr("disabled", "disabled");

        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim").attr("checked", "checked");

        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim").click();
    }
    else
    {
        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Nao").attr("disabled", null);
        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim").attr("disabled", null);
    }
}


function rdoCadastro_PFPasso3_RepresentanteLegal_Click(pSender)
{
    if ($(pSender).val() == "Sim")
    {
        $("#ContentPlaceHolder1_pnlDadosRepresentanteLegal").show();

        cboCadastro_PFPasso3_RepSituacaoLegal.addClass("validate[required]");
        txtCadastro_PFPasso3_RepNomeCompleto.addClass("validate[required]");
        txtCadastro_PFPasso3_RepCPF.addClass("validate[required,custom[validatecpf]]");
        txtCadastro_PFPasso3_RepDataNascimento.addClass("validate[required,custom[data],custom[DataNoPassado]]");
        cboCadastro_PFPasso3_RepTipoDocumento.addClass("validate[required]");
        cboCadastro_PFPasso3_RepOrgaoEmissor.addClass("validate[required]");
        txtCadastro_PFPasso3_RepNumeroDocumento.addClass("validate[required]");
        cboCadastro_PFPasso3_RepEstadoEmissao.addClass("validate[required]");
    }
    else
    {
        $("#ContentPlaceHolder1_pnlDadosRepresentanteLegal").hide();

        cboCadastro_PFPasso3_RepSituacaoLegal.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepNomeCompleto.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepCPF.removeClass("validate[required,custom[validatecpf]]");
        txtCadastro_PFPasso3_RepDataNascimento.removeClass("validate[required,custom[data],custom[DataNoPassado]]");
        cboCadastro_PFPasso3_RepTipoDocumento.removeClass("validate[required]");
        cboCadastro_PFPasso3_RepOrgaoEmissor.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepNumeroDocumento.removeClass("validate[required]");
        cboCadastro_PFPasso3_RepEstadoEmissao.removeClass("validate[required]");
    }

    return true;
}

function rdoCadastro_PFPasso3_RepresentanteLegalCambio_Click(pSender) {
    if ($(pSender).val() == "Sim") 
    {
        $("#ContentPlaceHolder1_pnlDadosRepresentanteLegalCambio").show();

        cboCadastro_PFPasso3_RepSituacaoLegalCambio.addClass("validate[required]");
        txtCadastro_PFPasso3_RepNomeCompletoCambio.addClass("validate[required]");
        txtCadastro_PFPasso3_RepCPFCambio.addClass("validate[required,custom[validatecpf]]");
        txtCadastro_PFPasso3_RepDataNascimentoCambio.addClass("validate[required,custom[data],custom[DataNoPassado]]");
        cboCadastro_PFPasso3_RepTipoDocumentoCambio.addClass("validate[required]");
        cboCadastro_PFPasso3_RepOrgaoEmissorCambio.addClass("validate[required]");
        txtCadastro_PFPasso3_RepNumeroDocumentoCambio.addClass("validate[required]");
        cboCadastro_PFPasso3_RepEstadoEmissaoCambio.addClass("validate[required]");
    }
    else 
    {
        $("#ContentPlaceHolder1_pnlDadosRepresentanteLegalCambio").hide();

        cboCadastro_PFPasso3_RepSituacaoLegalCambio.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepNomeCompletoCambio.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepCPFCambio.removeClass("validate[required,custom[validatecpf]]");
        txtCadastro_PFPasso3_RepDataNascimentoCambio.removeClass("validate[required,custom[data],custom[DataNoPassado]]");
        cboCadastro_PFPasso3_RepTipoDocumentoCambio.removeClass("validate[required]");
        cboCadastro_PFPasso3_RepOrgaoEmissorCambio.removeClass("validate[required]");
        txtCadastro_PFPasso3_RepNumeroDocumentoCambio.removeClass("validate[required]");
        cboCadastro_PFPasso3_RepEstadoEmissaoCambio.removeClass("validate[required]");
    }

    return true;
}

function btnCadastro_PFPasso3_AvaliarSuitability(pSender)
{
    GradSite_Cadastro_PFPasso3_ReavaliarSuitability();

    return false;
}

function GradSite_Cadastro_PFPasso3_ReavaliarSuitability()
{
    var lDados = { Acao: "ReavaliarSuitability", Suitability: $.toJSON(GradSite_Cadastro_PFPasso3_CarregarSuitability()) };

    GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso3_ReavaliarSuitability_CallBack);
}

function GradSite_Cadastro_PFPasso3_ReavaliarSuitability_CallBack(pResposta)
{
    var lResultado = pResposta.Mensagem;  // Conservador :: Moderado :: Arrojado

    Cadastro_BuscarListaDeFundosRecomendados();

    $("div.PainelResultadoSuitability").hide();

    $("#ContentPlaceHolder1_divResultado_" + lResultado).show();

    $("#ContentPlaceHolder1_pnlFormPerfil").hide();
    
    $("#ContentPlaceHolder1_pnlImagemPerfil").show();

}

function Cadastro_BuscarListaDeFundosRecomendados()
{
    console.log("Cadastro_BuscarListaDeFundosRecomendados");

    var lDados = { Acao: "BuscarListaDeFundosRecomendados" };

    GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, Cadastro_BuscarListaDeFundosRecomendados_CallBack);
}

function Cadastro_PreencherListaDeFundosRecomendados(pListaDeFundos)
{
    var lLista = pListaDeFundos;

    var lCombo = $("#cboFundoParaAplicar");

    lCombo.find("option").remove();

    $("[name='TipoDeAplicacao']").attr("disabled", null);

    if(lLista)
    {
        for(var a = 0; a < lLista.length; a++)
        {
            lCombo.append("<option value='" + lLista[a].IdProduto + "' data-termo='../.." + lLista[a].FullPathTermoPF + "'>" + lLista[a].Fundo + "</option>");
        }
    }
}

function Cadastro_BuscarListaDeFundosRecomendados_CallBack(pResposta)
{
    Cadastro_PreencherListaDeFundosRecomendados(pResposta.ObjetoDeRetorno);
}

function btnCadastro_PFPasso3_RefazerSuitability_Click(pSender)
{
    $("#ContentPlaceHolder1_pnlFormPerfil").show();
    
    $("#ContentPlaceHolder1_pnlImagemPerfil").hide();

    return false;
}

function rdoTipoDeAplicacao_Click()
{
    if($("[name='TipoDeAplicacao']:checked").val() == "2")
    {
        $("#pnlFundoParaAplicar").show();
    }
    else
    {
        $("#pnlFundoParaAplicar").hide();
    }
}

function btnSalvarDesejaAplicar_Click(pSender)
{
    var Resposta = { Mensagem: "ok_bov" };
    GradSite_Cadastro_SalvarDesejaAplicar_CallBack(Resposta);
    return false;
}

function GradSite_Cadastro_SalvarDesejaAplicar_CallBack(pResposta)
{
    $("#ContentPlaceHolder1_pnlEscolherAplicacao").hide();
    $("#ContentPlaceHolder1_pnlFundoParaAplicar").hide();

    $("#ContentPlaceHolder1_pnlContratos").show();

    if(pResposta.Mensagem == "ok_bov")
    {
        $("#ContentPlaceHolder1_pnlContratos").removeClass("SoFundos");
    }
    else
    {
        $("#ContentPlaceHolder1_pnlContratos").addClass("SoFundos");
    }

    return false;
}

/*=====================================

    Funções - PASSO 4

=====================================*/


function btnCadastro_PFPasso4_Click(pSender)
{
    if($("#chkCadastro_PFPasso4_ConcordoContratos").is(":checked"))
    {
        GradSite_Cadastro_PFPasso4_Finalizar();
    }
    else
    {
        alert("Favor marcar 'Li e, concordo e estou ciente dos termos'");
    }

    return false;
}


function GradSite_Cadastro_PFPasso4_Finalizar()
{
    var lDados = { Acao: "SalvarPasso4" };

    GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Cadastro_PFPasso4_Finalizar_CallBack);
}

function GradSite_Cadastro_PFPasso4_Finalizar_CallBack(pResposta)
{
    GradSite_Cadastro_OcultarLoader("Cadastro finalizado com sucesso!", true);

    var lURLs = pResposta.Mensagem.split(",");

    if($("#ContentPlaceHolder1_pnlContratos").hasClass("SoFundos"))
    {
        $("#lnkFichaCadastral_Fundo").attr("href", lURLs[0]);
        //$("#lnkTermo").attr("href", lURLs[1]);

        if ($("#lnkFichaCadastralCambio").length) 
        {
            $("#lnkFichaCadastralCambio").attr("href", lURLs[2]);
        }


        if ($("#lnkTermo").length) 
        {
            $("#lnkTermo").attr("href", lURLs[1]);
        }
        

        $("#pnlCadastroRealizado_Fundos").show();

        $("#ContentPlaceHolder1_pnlBotaoFinalizar").hide();
    }
    else
    {
        $("#lnkFichaCadastral").attr("href", lURLs[0]);

        if ($("#lnkFichaCadastralCambio").length) 
        {
            $("#lnkFichaCadastralCambio").attr("href", lURLs[2]);
        }

        $("#lnkTermo").attr("href", lURLs[1]);

        $("#pnlCadastroRealizado").show();

        $("#ContentPlaceHolder1_pnlBotaoFinalizar").hide();
    }

    $("#ContentPlaceHolder1_pnlContratos").hide();

    /*
    $("#ContentPlaceHolder1_pnlDadosContratuais > h5").hide();
    $("#ContentPlaceHolder1_pnlDadosContratuais > table").hide();
    */
}


/*=====================================

    Funções - Refazer suitability (página separada)

=====================================*/

function GradSite_Suitability_Refazer(pSender)
{
    $("#ContentPlaceHolder1_pnlResultado").hide();

    $("#ContentPlaceHolder1_pnlFormPerfil").show();

    return false;
}

function GradSite_Suitability_Reavaliar(pSender)
{
    var lSuit = GradSite_Cadastro_PFPasso3_CarregarSuitability();

    if(lSuit.Valido)
    {
        var lDados = { Acao: "ReavaliarSuitability", Suitability: $.toJSON(lSuit) };

        GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, GradSite_Suitability_Reavaliar_CallBack);
    }
    else
    {
        GradSite_ExibirMensagem("I", "Favor responder todas as questões");
    }

    return false;
}

function GradSite_Suitability_Reavaliar_CallBack(pResposta)
{
    var lMsg = pResposta.Mensagem;

    var lDiv = $("#ContentPlaceHolder1_divResultado_" + lMsg);

    if(lDiv.length > 0)
    {
        $("#ContentPlaceHolder1_pnlResultado").show();
        $("#ContentPlaceHolder1_pnlFormPerfil").hide();

        lDiv.parent().find(">div").hide();

        lDiv.show();
    }
    else
    {
        GradSite_ExibirMensagem("A", lMsg);
    }

    return false;
}



/* outras */

function Cadastro_PFPasso5_ContratoIntermediacao_OnClick(pSender)
{
    window.open('../../Resc/PDFs/Contrato_de_Intermediacao_1501.pdf', 'pdf2', 'resizable=yes', 'menubar=no', 'scrollbars=no', 'width=350', 'height=350');

    return false;
}

function Cadastro_PFPasso5_FichaCadastral_OnClick(pSender, pCaminhoFichaCadastralDownload)
{
    window.open(pCaminhoFichaCadastralDownload, 'pdf1', 'resizable=yes', 'menubar=no', 'scrollbars=no', 'width=550', 'height=350');

    return false;
}



function Cadastro_AlterarSenha_ValidarRequest(pSender)
{
    if (! GradSite_ValidacaoComDoubleCheck($("#form1")))
        return false;

    return confirm("Você realmente deseja alterar sua senha?");
}

function Cadastro_AlterarAssinaturaEletronica_ValidarRequest(pSender)
{
    if (! GradSite_ValidacaoComDoubleCheck($("#form1")))
        return false;

    return confirm("Você realmente deseja alterar sua senha?");
}

function cboCadastro_SolicitarAlteracao_Tipo_OnChange(pSender)
{
    pSender = $(pSender);

    var lValor = pSender.val();

    var lCombo = $("#ContentPlaceHolder1_cboCadastro_SolicitarAlteracao_InformacaoAlterada");

    lCombo
        .find("option[data-rel]")
        .hide();

    lCombo.val("");

    if(lValor != "")
        lCombo.find("option[data-rel='" + lValor + "']").show();
}

function cboCadastro_SolicitarAlteracao_Informacao_OnChange(pSender)
{
    pSender = $(pSender);

    if(pSender.val().toLowerCase().indexOf("contrato de intermediação") != -1)
    {
        $("#pnlAlertaAlteracaoContrato").show();
    }
    else
    {
        $("#pnlAlertaAlteracaoContrato").hide();
    }
}

function Cadastro_PF_Passo3_EnviarEmail_Click(pSender)
{
    GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", { Acao: "EnviarEmailSuitability" }, Cadastro_PF_Passo3_EnviarEmail_CallBack);
}

function Cadastro_PF_Passo3_EnviarEmail_CallBack(pResposta)
{
    GradSite_ExibirMensagem("A", pResposta.Mensagem);
}

function Cadastro_PF_Passo4_DownloadTeste_Click(pSender)
{
    //var lCaminhoDownloadPDF = document.URL.replace("PerfilCliente.aspx", "");
    
    //lCaminhoDownloadPDF += "../../Resc/PDFs/" +  $(".DsPerfilAtualDoCliente").html();

    var lPerfil = "Conservador";

    if($("#ContentPlaceHolder1_divResultado_Moderado").is(":visible"))
    {
        lPerfil = "Moderado";
    }

    if($("#ContentPlaceHolder1_divResultado_Arrojado").is(":visible"))
    {
        lPerfil = "Arrojado";
    }

    var lCaminhoDownloadPDF = "../../Resc/PDFs/Perfil_" + lPerfil + ".pdf";

    window.open(lCaminhoDownloadPDF);
}

function btnCadastro_SolicitarAlteracao_GravarDados_Validar(pSender)
{
    var lValido = GradSite_ValidacaoComDoubleCheck($(".validationEngineContainer"));

    if ($("#ContentPlaceHolder1_txtCadastro_SolicitarAlteracao_DescricaoMotivo").val() == "")
        lValido = false;

    return lValido;
}

function btnGenerico_Validar()
{
    var lValido = GradSite_ValidacaoComDoubleCheck( $(".validationEngineContainer") );

    return lValido;
}





/*  old stuff   */



/*=====================================

    Event Handlers - PASSO 2

=====================================*/




function cboCadastro_PFPasso2_EstadoCivil_Change(pSender)
{
    var lValor = $(pSender).val();

    var lValorTxt = txtCadastro_PFPasso2_Conjuge.val();

    var lValorAnterior;

    if(lValor != "")
    {
        if ( lValor == "1"
        || ( lValor == "2" )
        || ( lValor == "3" )
        || ( lValor == "4" ) 
        || ( lValor == "9" ) )
        {
            lValorAnterior = lValorTxt;

            if(lValorAnterior == "" && txtCadastro_PFPasso2_Conjuge.attr("data-ValorAnterior") != "" && txtCadastro_PFPasso2_Conjuge.attr("data-ValorAnterior") !== undefined)
            {
                lValorAnterior = txtCadastro_PFPasso2_Conjuge.attr("data-ValorAnterior");
            }

            txtCadastro_PFPasso2_Conjuge.removeClass("validate[required]")
                                        .removeClass("EstiloCampoObrigatorio")
                                        .val("")
                                        .attr("data-ValorAnterior", lValorAnterior)
                                        .attr("disabled", "disabled");

            txtCadastro_PFPasso2_Conjuge.parent().find(".formError").remove();
        }
        else
        {
            if(lValorTxt == "")
            {
                lValorAnterior = txtCadastro_PFPasso2_Conjuge.attr("data-ValorAnterior");

                txtCadastro_PFPasso2_Conjuge.addClass("validate[required]")
                                            .addClass("EstiloCampoObrigatorio")
                                            .val(lValorAnterior)
                                            .attr("disabled", null);
            }
        }
    }
}


function cboCadastro_PFPasso2_Profissao_Change(pSender)
{
    var lVal = $(pSender).val();

    if( lVal == 932 || lVal == 939 || lVal == 923 || lVal == 951)   //magic number: "estudante"
    {
        txtCadastro_PFPasso2_CargoFuncao.removeClass("validate[required] EstiloCampoObrigatorio").removeClass("CampoComErro").parent().find(".formError").remove();
        txtCadastro_PFPasso2_Empresa.removeClass("validate[required] EstiloCampoObrigatorio").removeClass("CampoComErro").parent().find(".formError").remove();
    }
    else
    {
        txtCadastro_PFPasso2_CargoFuncao.addClass("validate[required] EstiloCampoObrigatorio");
        txtCadastro_PFPasso2_Empresa.addClass("validate[required] EstiloCampoObrigatorio");
    }
}


function btnCadastro_PFPasso4_AlterarSenha_Click(pSender)
{
    var lDados = {
                    Acao: "AlterarSenha"
                    , SenhaAtual: $("#txtCadastro_PFPasso4_SenhaAtual").val()
                    , SenhaNova : $("#txtCadastro_PFPasso4_SenhaNova").val()
                    , SenhaNovaC: $("#txtCadastro_PFPasso4_SenhaNovaC").val()
                 };

                 if (ValidatePasswordField(pSender)) 
                 {
                     if (lDados.SenhaAtual == "") 
                     {
                         GradSite_ExibirMensagem("A", "Preencha os campos obrigatórios");
                     }
                     else if (lDados.SenhaNova != lDados.SenhaNovaC) 
                     {
                         GradSite_ExibirMensagem("A", "A nova senha não confere com o valor de confirmação");
                     }
                     else 
                     {
                         GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, Cadastro_PFPasso4_AlterarSenha_CallBack);
                     }
                 }

    return false;
}

function PasswordChange_Click(sender, event, param)
{
    if (gTipoTeclado == 0 || gTipoTeclado == 3)
    {
        ref = $("#txtCadastro_PFPasso4_SenhaAtual").val();
    }

    var lDados = 
    {
        //Acao          : "AlterarSenhaDinamica"
        //, SenhaAtual    : $("#txtCadastro_PFPasso4_SenhaAtual").val()
        //, SenhaNova     : $("#txtCadastro_PFPasso4_SenhaNova").val()
        //, SenhaNovaC    : $("#txtCadastro_PFPasso4_SenhaNovaC").val()
        //, TipoTeclado   : gTipoTeclado
        Acao: "AlterarSenhaDinamica"
        , SenhaAtual    : ref
        , SenhaNova     : refNova
        , SenhaNovaC    : refConfirmacao
        , TipoTeclado   : gTipoTeclado
    };

    if (ValidatePasswordField(refNova)) 
    {
        if (lDados.SenhaAtual == "") 
        {
            GradSite_ExibirMensagem("A", "Preencha os campos obrigatórios");
        }
        else if (lDados.SenhaNova != lDados.SenhaNovaC) 
        {
            GradSite_ExibirMensagem("A", "A nova senha não confere com o valor de confirmação");
        }
        else 
        {
            GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, Cadastro_PFPasso4_AlterarSenha_CallBack);
        }
    }

    return false;
}


function NewPassword_Click(sender, event, param)
{
    var lDados =
    {
        Acao: "NovaSenha"
        , SenhaNova: refNova
        , SenhaNovaC: refConfirmacao
        , TipoTeclado: gTipoTeclado
    };

    if (ValidatePasswordField(refNova))
    {
        if (lDados.SenhaNova == "")
        {
            GradSite_ExibirMensagem("A", "Preencha os campos obrigatórios");
        }
        else if (lDados.SenhaNova != lDados.SenhaNovaC)
        {
            GradSite_ExibirMensagem("A", "A senha não confere com o valor de confirmação");
        }
        else
        {
            GradSite_CarregarJsonVerificandoErro("NovaSenha.aspx", lDados, NewPassword_Click_Callback);
        }
    }

    return false;
}

function NewPassword_Click_Callback(pResposta)
{
    arrayRef = [];
    ref = [];
    refNova = [];
    refConfirmacao = [];
    controlClick = 0;
    $(field).val("");

    if (pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "A nova senha foi enviada para seu e-mail cadastrado.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
        document.location = GradSite_BuscarRaiz("/MinhaConta/Login.aspx");
    }
    else if (pResposta.Mensagem == "JA_UTILIZADA")
    {
        GradSite_ExibirMensagem("A", "Esta senha já foi utilizada, favor escolher outra.");
    }
//    else if (pResposta.Mensagem == "SENHA_ERRADA")
//    {
//        GradSite_ExibirMensagem("A", "Senha atual incorreta.");
//    }
    else
    {
        GradSite_ExibirMensagem("A", pResposta.Mensagem);
        document.location = GradSite_BuscarRaiz("/MinhaConta/Login.aspx");
    }

    
}

function ValidatePasswordField(pSender) 
{
    var lCaller = pSender;

    if (lCaller.length != 6) 
    {
        GradSite_ExibirMensagem("A", "Campo deve ter 6 caracteres numéricos");
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter de 8 a 15 caracteres";
        return false;
    }

//    var goodChars = "ABCDEFGHIJKLMNOPQRSTUXYWVZ";
//    var lContQuantidadeMinimoLetraMaiuscula = 1;
//    var lContQuantidadeMinimoLetraMinuscula = 1;
//    var lContLetraMaiuscula = 0;
//    var lContLetraMinuscula = 0;

    var lContNumeros = 0;

    var goodNumerics = "0123456789";

    var lContQuantidadeMinimoNumero = 6;



    for (i = 0; i < lCaller.length; i++) 
    {
//        if (goodChars.indexOf(lCaller[i]) != -1) 
//        {
//            lContLetraMaiuscula++;
//        }

//        if (goodChars.toLowerCase().indexOf(lCaller[i]) != -1) 
//        {
//            lContLetraMinuscula++;
//        }

        if (goodNumerics.indexOf(lCaller[i]) != -1) 
        {
            lContNumeros++;
        }
    }

//    if (lContLetraMaiuscula < lContQuantidadeMinimoLetraMaiuscula) 
//    {
//        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter mai&uacutesculo";
//        GradSite_ExibirMensagem("A", "Campo deve ter pelo menos um caracter mai&uacutesculo");
//        return false;
//    }

//    if (lContLetraMinuscula < lContQuantidadeMinimoLetraMinuscula) 
//    {
//        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter min&uacutesculo";
//        GradSite_ExibirMensagem("A", "Campo deve ter pelo menos um caracter min&uacutesculo");
//        return false;
//    }

    if (lContNumeros < lContQuantidadeMinimoNumero) 
    {
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter pelo menos um caracter Num&eacuterico";
        GradSite_ExibirMensagem("A", "Campo deve ter 6 caracteres num&eacuterico");
        return false;
    }

    return true;
}

function Cadastro_PFPasso4_AlterarSenha_CallBack(pResposta)
{
    arrayRef        = [];
    ref             = [];
    refNova         = [];
    refConfirmacao  = [];
    controlClick    = 0;
    $(field).val("");

    if(pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "A nova senha foi enviada para seu e-mail cadastrado.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
    }
    else if(pResposta.Mensagem == "JA_UTILIZADA")
    {
        GradSite_ExibirMensagem("A", "Esta senha já foi utilizada, favor escolher outra.");
    }
    else if(pResposta.Mensagem == "SENHA_ERRADA")
    {
        GradSite_ExibirMensagem("A", "Senha atual incorreta.");
    }
    else
    {
        GradSite_ExibirMensagem("A", pResposta.Mensagem);
    }
}


function btnCadastro_PFPasso4_AlterarAssinatura_Click(pSender)
{
    if ($('#txtCadastro_PFPasso4_AssinaturaNovaC').val().length != 6)
    {
        GradSite_ExibirMensagem("A", "O campo assinatura deve ter 6 caracteres numéricos");
        //$.validationEngine.settings.allrules["ValidatePasswordField"].alertText = "Campo deve ter de 8 a 15 caracteres";
        return false;
    }

    if (gTipoTeclado == 0 || gTipoTeclado == 2)
    {
        ref = $("#txtCadastro_PFPasso4_AssinaturaAtual").val();
    }

    var lDados = {
                    //Acao: "AlterarAssinatura"
                    //Acao: "AlterarAssinaturaDinamica"
                    //, AssinaturaAtual: $("#txtCadastro_PFPasso4_AssinaturaAtual").val()
                    //, AssinaturaNova : $("#txtCadastro_PFPasso4_AssinaturaNova").val()
                    //, AssinaturaNovaC: $("#txtCadastro_PFPasso4_AssinaturaNovaC").val()
                    //, TipoTeclado: gTipoTeclado
                    Acao: "AlterarAssinaturaDinamica"
                    , AssinaturaAtual: ref
                    , AssinaturaNova : refNova
                    , AssinaturaNovaC: refConfirmacao
                    , TipoTeclado: gTipoTeclado
                 };

    if(lDados.AssinaturaAtual == "")
    {
        GradSite_ExibirMensagem("A", "Preencha os campos obrigatórios");
    }
    else if(lDados.AssinaturaNova != lDados.AssinaturaNovaC)
    {
        GradSite_ExibirMensagem("A", "A nova assinatura não confere com o valor de confirmação");
    }
    else
    {
        GradSite_CarregarJsonVerificandoErro("MeuCadastro.aspx", lDados, Cadastro_PFPasso4_AlterarAssinatura_CallBack);
    }

    return false;
}

function Cadastro_PFPasso4_AlterarAssinatura_CallBack(pResposta)
{
    arrayRef = [];
    ref = [];
    refNova = [];
    refConfirmacao = [];
    controlClick = 0;
    $(field).val("");

    if(pResposta.Mensagem == "ok")
    {
        GradSite_ExibirMensagem("I", "A nova assinatura eletrônica foi enviada para seu e-mail cadastrado.<br/><br/>Por segurança, sua sessão foi terminada; favor efetuar login novamente.");
    }
    else
    {
        if (pResposta.Mensagem.indexOf("não encontrado para alteração de assinatura digital") > 0)
        {
            GradSite_ExibirMensagem("I", "Cliente não encontrado para alteração de Assinatura, por favor entrar em contato com a Central de Atendimento – ETD");
        }
        else
        {
            GradSite_ExibirMensagem("A", pResposta.Mensagem);
        }
    }
}

function GradSite_Cadastro_AlterarDadosRedirecionando(pSender)
{
    document.location = "SolicitarAlteracao.aspx";

    return false;
}


/*=====================================

    Funções de teste

=====================================*/


function Cadastro_PreencherTeste_P1()
{
    var lEmail, lCPF;

    lCPF = prompt("CPF de teste (com ou sem pontuação, branco para randomizar):", "");

    if(lCPF = null || lCPF == "")
    {
        lCPF = cpf_gerar(false);
    }

    if(lCPF.indexOf(".") == -1)
    {
        lCPF = lCPF.substr(0,3) + "." + lCPF.substr(3, 3) + "." + lCPF.substr(6, 3) + "-" + lCPF.substr(9, 2);
    }

    lEmail = prompt("Email de teste", "teste____@teste.com");

    txtCadastro_PFPasso1_NomeCompleto.val("TESTER TESTERSON DE ALMEIDA");
    txtCadastro_PFPasso1_Email.val(lEmail.toUpperCase());
    txtCadastro_PFPasso1_EmailConfirmacao.val(lEmail.toUpperCase());
    txtCadastro_PFPasso1_Senha.val("111111");
    txtCadastro_PFPasso1_SenhaConfirmacao.val("111111");
    txtCadastro_PFPasso1_AssEletronica.val("111111");
    txtCadastro_PFPasso1_AssEletronicaConfirmacao.val("111111");
    txtCadastro_PFPasso1_CPF.val(lCPF);
    txtCadastro_PFPasso1_DataNascimento.val("01/01/1981");
    cboCadastro_PFPasso1_Sexo.val("M");

    txtCadastro_PFPasso1_Cel_DDD.val("11");
    txtCadastro_PFPasso1_Cel_Numero.val("91999-9191");
    
    txtCadastro_PFPasso1_Tel_DDD.val("11");
    txtCadastro_PFPasso1_Tel_Numero.val("91999-9090");

    cboCadastro_PFPasso1_TipoTelefone.val("1");
    cboCadastro_PFPasso1_Conheceu.val("7");

    cboCadastro_PFPasso1_Conheceu.val("EMAIL");

    return false;
}

function Cadastro_PreencherTeste_P2()
{
    cboCadastro_PFPasso2_Nacionalidade.val("1");
    cboCadastro_PFPasso2_EstadoNascimento.val("SP");
    txtCadastro_PFPasso2_CidadeNascimento.val("SÃO PAULO");
    cboCadastro_PFPasso2_EstadoCivil.val("1");
    txtCadastro_PFPasso2_Conjuge.val("");
    cboCadastro_PFPasso2_Profissao.val("163");
    txtCadastro_PFPasso2_CargoFuncao.val("COMPOSITOR");
    txtCadastro_PFPasso2_Empresa.val("Própria");
    cboCadastro_PFPasso2_TipoDocumento.val("RG");
    txtCadastro_PFPasso2_NumeroDocumento.val("3498778998");
    cboCadastro_PFPasso2_OrgaoEmissor.val("SSP");
    cboCadastro_PFPasso2_EstadoEmissao.val("SP");
    txtCadastro_PFPasso2_DataEmissao.val("01/08/1998");
    txtCadastro_PFPasso2_NomeMae.val("MARIA APARECIDA TESTE");
    txtCadastro_PFPasso2_NomePai.val("MARCOS TESTE");


    txtCadastro_PFPasso2_Endereco1_CEP.val("01404-000");
    txtCadastro_PFPasso2_Endereco1_Logradouro.val("AL. CAMPINAS");
    txtCadastro_PFPasso2_Endereco1_Numero.val("123");
    //txtCadastro_PFPasso2_Endereco1_Complemento
    txtCadastro_PFPasso2_Endereco1_Bairro.val("JD. PAULISTA");
    txtCadastro_PFPasso2_Endereco1_Cidade.val("SÃO PAULO");
    cboCadastro_PFPasso2_Endereco1_Estado.val("SP");
    cboCadastro_PFPasso2_Endereco1_Pais.val("BRA");

    return false;
}

function Cadastro_PreencherTeste_P3(pCompleto)
{
    txtCadastro_PFPasso3_SalarioProlabore.val("4.200,00");
    txtCadastro_PFPasso3_ValorTotalEmAplicFin.val("44.200,00");
    txtCadastro_PFPasso3_OutrosRendimentos.val("1.200,00");
    txtCadastro_PFPasso3_TotalEmBensImoveis.val("400.000,00");

    if(pCompleto)
    {
        cboCadastro_PFPasso3_ContasBancarias_TipoConta.val("CC");
        cboCadastro_PFPasso3_ContasBancarias_Banco.val("001");
        txtCadastro_PFPasso3_ContasBancarias_Agencia.val("0101");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.val("0");
        txtCadastro_PFPasso3_ContasBancarias_Conta.val("010101");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val("1");

        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_OperaContaPropria_Nao").click();

        txtCadastro_PFPasso3_CliNomeCompleto.val("NOME DO MEU CLIENTE");
        txtCadastro_PFPasso3_CliCPF.val( cpf_gerar(true) );

        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_Procurador_Sim").click();

        cboCadastro_PFPasso3_RepSituacaoLegal.val("7");

        txtCadastro_PFPasso3_RepNomeCompleto.val("NOME DO REPRESENTADO");
        txtCadastro_PFPasso3_RepCPF.val( cpf_gerar(true) );
        txtCadastro_PFPasso3_RepDataNascimento.val("10/10/1910");
        cboCadastro_PFPasso3_RepTipoDocumento.val("JZ");
        cboCadastro_PFPasso3_RepOrgaoEmissor.val("AGU");
        txtCadastro_PFPasso3_RepNumeroDocumento.val("123456789");
        cboCadastro_PFPasso3_RepEstadoEmissao.val("SP");
    }
    else
    {
        cboCadastro_PFPasso3_ContasBancarias_TipoConta.val("CC");
        cboCadastro_PFPasso3_ContasBancarias_Banco.val("341");
        txtCadastro_PFPasso3_ContasBancarias_Agencia.val("0390");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigito.val("0");
        txtCadastro_PFPasso3_ContasBancarias_Conta.val("42042");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigito.val("2");
    }

    $("#ContentPlaceHolder1_txtCadastro_PFPasso3_Proposito").val("Preenchimento automatico pelo portal");

    $("#rdoSuit_01_A").prop("checked", true);
    $("#rdoSuit_02_A").prop("checked", true);
    $("#rdoSuit_03_A").prop("checked", true);
    $("#rdoSuit_04_A").prop("checked", true);
    $("#rdoSuit_05_A").prop("checked", true);
    $("#rdoSuit_06_A").prop("checked", true);
    $("#rdoSuit_07_A").prop("checked", true);
    $("#rdoSuit_08_A").prop("checked", true);
    $("#rdoSuit_09_A").prop("checked", true);
    $("#rdoSuit_10_A").prop("checked", true);
    $("#rdoSuit_11_A").prop("checked", true);

    btnCadastro_PFPasso3_AdicionarContaBancaria_Click();

    $("#rdbCadastro_PFPasso3_PainelRespostas_DefineInvestidor_GanhosExpressivos").click();
    $("#rdbCadastro_PFPasso3_PainelRespostas_DefinePercentualMensal_Entre20e45").click();
    $("#rdbCadastro_PFPasso3_PainelRespostas_Exp_Baixa").click();
    $("#rdbCadastro_PFPasso3_PainelRespostas_InvestimentoPerdasTemporarias_Ate10").click();
    $("#rdbCadastro_PFPasso3_PainelRespostas_PortTotal_4").click();

    return false;
}


function Cadastro_PreencherTesteCambio_P3(pCompleto) 
{
    txtCadastro_PFPasso3_OutrosRendimentosCambio.val('4.500,00');

    if (pCompleto) 
    {
        cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.val("CC");
        cboCadastro_PFPasso3_ContasBancarias_BancoCambio.val("001");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio.val("0101");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio.val("x");
        txtCadastro_PFPasso3_ContasBancarias_ContaCambio.val("010101");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val("1");

        
        $("#ContentPlaceHolder1_rdoCadastro_PFPasso3_ProcuradorCambio_Sim").click();

        cboCadastro_PFPasso3_RepSituacaoLegalCambio.val("7");

        txtCadastro_PFPasso3_RepNomeCompletoCambio.val("NOME DO REPRESENTADO");
        txtCadastro_PFPasso3_RepCPFCambio.val(cpf_gerar(true));
        txtCadastro_PFPasso3_RepDataNascimentoCambio.val("10/10/1910");
        cboCadastro_PFPasso3_RepTipoDocumentoCambio.val("JZ");
        cboCadastro_PFPasso3_RepOrgaoEmissorCambio.val("AGU");
        txtCadastro_PFPasso3_RepNumeroDocumentoCambio.val("123456789");
        cboCadastro_PFPasso3_RepEstadoEmissaoCambio.val("SP");
    }
    else 
    {
        cboCadastro_PFPasso3_ContasBancarias_TipoContaCambio.val("CC");
        cboCadastro_PFPasso3_ContasBancarias_BancoCambio.val("341");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaCambio.val("0390");
        txtCadastro_PFPasso3_ContasBancarias_AgenciaDigitoCambio.val("0");
        txtCadastro_PFPasso3_ContasBancarias_ContaCambio.val("42042");
        txtCadastro_PFPasso3_ContasBancarias_ContaDigitoCambio.val("2");
    }

    btnCadastro_PFPasso3_AdicionarContaBancariaCambio_Click();

    return false;
}

function cpf_randomiza(n)
{
    var ranNum = Math.round(Math.random()*n);
    return ranNum;
}

function cpf_mod(dividendo,divisor)
{
    return Math.round(dividendo - (Math.floor(dividendo/divisor)*divisor));
}

function cpf_gerar(comPontos)
{
    var n = 9;
    var n1 = cpf_randomiza(n);
    var n2 = cpf_randomiza(n);
    var n3 = cpf_randomiza(n);
    var n4 = cpf_randomiza(n);
    var n5 = cpf_randomiza(n);
    var n6 = cpf_randomiza(n);
    var n7 = cpf_randomiza(n);
    var n8 = cpf_randomiza(n);
    var n9 = cpf_randomiza(n);
    var d1 = n9*2+n8*3+n7*4+n6*5+n5*6+n4*7+n3*8+n2*9+n1*10;

    d1 = 11 - ( cpf_mod(d1,11) );
    if (d1>=10) d1 = 0;

    var d2 = d1*2+n9*3+n8*4+n7*5+n6*6+n5*7+n4*8+n3*9+n2*10+n1*11;
    d2 = 11 - ( cpf_mod(d2,11) );

    if (d2>=10) d2 = 0;

    var retorno = '';

    if (comPontos) retorno = ''+n1+n2+n3+'.'+n4+n5+n6+'.'+n7+n8+n9+'-'+d1+d2;
    else retorno = ''+n1+n2+n3+n4+n5+n6+n7+n8+n9+d1+d2;

    return retorno;
}

