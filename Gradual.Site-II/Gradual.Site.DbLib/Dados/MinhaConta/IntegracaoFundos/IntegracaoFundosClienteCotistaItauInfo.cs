using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosClienteCotistaItauInfo
    {
        public string CodigoCotista                                     { get; set; }
        public char StAtivo                                             { get; set; }
        public string Banco                                             { get; set; }
        public string Agencia                                           { get; set; }
        public string Conta                                             { get; set; }
        public string DigitoConta                                       { get; set; }
        public string SubConta                                          { get; set; }

        public string NomeCliente                                       { get; set; }
        public DateTime DataNascimento                                  { get; set; }
        public string CodigoTipoPessoa                                  { get; set; }
        public string DsCpfCnpj                                         { get; set; }
        public string TipoDocumento                                     { get; set; }
        public string NumeroDocumento                                   { get; set; }
        public DateTime DataExpedicaoDocumento                          { get; set; }
        public string OrgaoEmissorDocumento                             { get; set; }
        public string EstadoEmissorDocumento                            { get; set; }
        public string CodigoTributacao                                  { get; set; }
        public string CodigoSituacaoLegal                               { get; set; }
        public string CodigoSexo                                        { get; set; }
        public string CodigoEstadoCivil                                 { get; set; }
        public string CodigoAtividadePessoaJuridica                     { get; set; }
        public string CodigoAtividadePessoaFisica                       { get; set; }
        public string CodigoFormaConstituicaoEmpresa                    { get; set; }
        public string TipoEnderecoCorrespondencia                       { get; set; }
        public string CodigoTipRemessa                                  { get; set; }
        public string DDD                                               { get; set; }
        public string NumeroTelResidencial                              { get; set; }
        public string NUmeroRamal                                       { get; set; }
        public string Email                                             { get; set; }
        public string Assessor                                          { get; set; }
        public string CodigoBovespa                                     { get; set; }
        public char EmissaoExtratoMensal                                { get; set; }
        public char EmissaoAvisoConfirmacaoMovimentacao                 { get; set; }
        public string ValorRendaMensal                                  { get; set; }
        public string ValorPatrimonial                                  { get; set; }
        public string CodigoTipoCliente                                 { get; set; }
        public string CodigoCetip                                       { get; set; }
        public string CodigoDistribuidor                                { get; set; }
        public string RazaoSocialAdministrador                          { get; set; }
        public string RazaoSocialGestor                                 { get; set; }
        public string RazaoSocialCustodiante                            { get; set; }
        public string NomePrimeiroContatoCustodiante                    { get; set; }
        public string DDDPrimeiroContatoCustodiante                     { get; set; }
        public string TelefonePrimeiroContatoCustodiante                { get; set; }
        public string RamalPrimeiroContatoCustodiante                   { get; set; }
        public string EmailPrimeiroContatoCustodiante                   { get; set; }
        public string NomeSegundoContatoCustodiante                     { get; set; }
        public string DDDSegundoContatoCustodiante                      { get; set; }
        public string TelefoneSegundoContatoCustodiante                 { get; set; }
        public string RamalSegundoContatoCustodiante                    { get; set; }
        public string EmailSegundoContatoCustodiante                    { get; set; }
        public string EnderecoResidencial                               { get; set; }
        public string NumeroResidencial                                 { get; set; }
        public string ComplementoResidencial                            { get; set; }
        public string BairroResidencial                                 { get; set; }
        public string CepResidencial                                    { get; set; }
        public string CidadeResidencial                                 { get; set; }
        public string EstadoResidencial                                 { get; set; }
        public string EnderecoComercial                                 { get; set; }
        public string NumeroComercial                                   { get; set; }
        public string ComplementoComercial                              { get; set; }
        public string BairroComercial                                   { get; set; }
        public string CepComercial                                      { get; set; }
        public string CidadeComercial                                   { get; set; }
        public string EstadoComercial                                   { get; set; }
        public string EnderecoAlternativo                               { get; set; }
        public string NumeroAlternativo                                 { get; set; }
        public string ComplementoAlternativo                            { get; set; }
        public string BairroAlternativo                                 { get; set; }
        public string CepAlternativo                                    { get; set; }
        public string CidadeAlternativo                                 { get; set; }
        public string EstadoAlternativo                                 { get; set; }
        public string IdentificadorArquivo                              { get; set; }
        public string PessoaVinculada                                   { get; set; }
        public string Emancipado                                        { get; set; }
    }
}
