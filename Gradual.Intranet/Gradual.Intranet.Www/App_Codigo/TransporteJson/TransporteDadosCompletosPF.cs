using System;
using System.Globalization;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Www.App_Codigo.Excessoes;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    /// <summary>
    /// Classe que representa as propriedades do cliente que vêm direto dos campos da tela, via json no ajax
    /// </summary>
    public class TransporteDadosCompletosPF : TransporteDadosCompletosClienteBase
    {
        #region Propriedades

        public string Assessor { get; set; }

        public string Nacionalidade { get; set; }

        public string DataNascimento { get; set; }

        public string PaisDeNascimento { get; set; }

        public string EstadoDeNascimento { get; set; }

        public string CidadeDeNascimento { get; set; }

        public string EstadoCivil { get; set; }

        public string Sexo { get; set; }

        public string Escolaridade { get; set; }

        public string Conjuge { get; set; }

        public string Profissao { get; set; }

        public string RamoDeAtividade { get; set; }

        public string CargoAtual { get; set; }

        public string Empresa { get; set; }

        //public string Email { get; set; }

        public string EmailComercial { get; set; }

        public string NomeDaMae { get; set; }

        public string NomeDoPai { get; set; }

        public string TipoDeDocumento { get; set; }

        public string OrgaoEmissor { get; set; }

        public string Documento_Numero { get; set; }

        public string Documento_DataValidade { get; set; }

        public string Documento_DataEmissao { get; set; }

        public string Documento_EstadoEmissao { get; set; }

        public string Passo { get; set; }

        public string NaoOperaPorContaPropriaNome { get; set; }

        public string NaoOperaPorContaPropriaCPF_CNPJ { get; set; }

        public string DesejaAplicar { get; set; }

        #endregion

        #region Construtor

        public TransporteDadosCompletosPF() { }

        public TransporteDadosCompletosPF(ClienteInfo pDadosDoCliente) : base(pDadosDoCliente)
        {

            if (pDadosDoCliente.IdCliente == null || pDadosDoCliente.IdCliente == -1) return;

            this.Id = pDadosDoCliente.IdCliente.DBToString();

            this.NomeCliente = pDadosDoCliente.DsNome;

            this.Email = pDadosDoCliente.DsEmail;

            this.Assessor = pDadosDoCliente.IdAssessorInicial.DBToString();
            this.Passo = pDadosDoCliente.StPasso.DBToString();
            this.CargoAtual = pDadosDoCliente.DsCargo;
            this.CidadeDeNascimento = pDadosDoCliente.DsNaturalidade;
            this.Conjuge = pDadosDoCliente.DsConjugue;
            this.CPF_CNPJ = pDadosDoCliente.DsCpfCnpj.ToCpfCnpjString();
            this.DataNascimento = pDadosDoCliente.DtNascimentoFundacao.Value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat);
            this.Empresa = pDadosDoCliente.DsEmpresa;
            this.Escolaridade = pDadosDoCliente.CdEscolaridade.DBToString();
            this.EstadoCivil = pDadosDoCliente.CdEstadoCivil.DBToString();
            this.EstadoDeNascimento = pDadosDoCliente.CdUfNascimento;
            this.Flag_CVM387 = pDadosDoCliente.StCVM387.Value;
            this.Flag_Emancipado = pDadosDoCliente.StEmancipado.Value;
            this.Flag_PPE = pDadosDoCliente.StPPE.Value;
            this.Nacionalidade = pDadosDoCliente.CdNacionalidade.DBToString();
            this.OrgaoEmissor = pDadosDoCliente.CdOrgaoEmissorDocumento;
            this.PaisDeNascimento = pDadosDoCliente.CdPaisNascimento;
            this.Profissao = pDadosDoCliente.CdProfissaoAtividade.DBToString();
            this.RamoDeAtividade = pDadosDoCliente.CdAtividadePrincipal.DBToString();
            this.Sexo = pDadosDoCliente.CdSexo.DBToString();
            this.TipoCliente = pDadosDoCliente.TpPessoa.DBToString();
            this.Tipo = pDadosDoCliente.TpCliente.ToString();
            this.TipoDeDocumento = pDadosDoCliente.TpDocumento;
            this.Documento_EstadoEmissao = pDadosDoCliente.CdUfEmissaoDocumento;
            this.NomeDoPai = pDadosDoCliente.DsNomePai;
            this.NomeDaMae = pDadosDoCliente.DsNomeMae;
            this.Documento_Numero = pDadosDoCliente.DsNumeroDocumento;
            this.EmailComercial = pDadosDoCliente.DsEmailComercial;
            this.Email = pDadosDoCliente.DsEmail;
            this.IdLogin = pDadosDoCliente.IdLogin;
            this.Flag_PessoaVinculada = pDadosDoCliente.StPessoaVinculada;
            this.Documento_DataValidade = string.Empty;

            this.DesejaAplicar = pDadosDoCliente.TpDesejaAplicar;

            this.NaoOperaPorContaPropriaNome = pDadosDoCliente.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado;
            this.NaoOperaPorContaPropriaCPF_CNPJ = pDadosDoCliente.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado.ToCpfCnpjString();

            if (pDadosDoCliente.DtEmissaoDocumento.HasValue)
                this.Documento_DataEmissao = pDadosDoCliente.DtEmissaoDocumento.Value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat);

            if(pDadosDoCliente.StUSPerson.HasValue)
                this.USPerson = pDadosDoCliente.StUSPerson.Value;

            this.PropositoGradual = pDadosDoCliente.DsPropositoGradual;

        }

        #endregion

        #region Métodos Públicos

        public ClienteInfo ToClienteInfo(ClienteInfo pParametro)
        {
            pParametro.DadosClienteNaoOperaPorContaPropria = new ClienteNaoOperaPorContaPropriaInfo();

            pParametro.IdAssessorInicial = this.Assessor.DBToInt32();
            pParametro.StPasso = this.Passo.DBToInt32();
            pParametro.DsNome = base.NomeCliente;
            pParametro.CdSexo = this.Sexo[0];
            pParametro.TpPessoa = 'F';
            pParametro.TpCliente = int.Parse(this.Tipo);
            pParametro.CdNacionalidade = this.Nacionalidade.DBToInt32();
            pParametro.CdPaisNascimento = this.PaisDeNascimento;
            pParametro.CdUfNascimento = this.EstadoDeNascimento;
            pParametro.CdEstadoCivil = this.EstadoCivil.DBToInt32();
            pParametro.DsConjugue = this.Conjuge;
            pParametro.TpDocumento = this.TipoDeDocumento;
            // pParametro.CdAtividadePrincipal = this.RamoDeAtividade.DBToInt32();
            pParametro.DtNascimentoFundacao = this.DataNascimento.DBToDateTime();
            pParametro.CdOrgaoEmissorDocumento = this.OrgaoEmissor;
            pParametro.CdUfEmissaoDocumento = this.Documento_EstadoEmissao;
            pParametro.CdProfissaoAtividade = this.Profissao.DBToInt32();
            pParametro.DsCargo = this.CargoAtual;
            pParametro.DsEmpresa = this.Empresa;
            pParametro.StEmancipado = this.Flag_Emancipado;

            pParametro.TpDesejaAplicar = this.DesejaAplicar;

            if(!string.IsNullOrEmpty(this.Escolaridade))
                pParametro.CdEscolaridade = this.Escolaridade.DBToInt32();

            //pParametro.StCadastroPortal              = false;  //TODO: Verificar. Vai direto?
            pParametro.DsNomeMae = this.NomeDaMae;
            pParametro.DsNomePai = this.NomeDoPai;
            pParametro.DsNaturalidade = this.CidadeDeNascimento;
            pParametro.DtEmissaoDocumento = this.Documento_DataEmissao.DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite);
            pParametro.DsEmailComercial = this.EmailComercial;
            pParametro.DsNumeroDocumento = this.Documento_Numero;
            pParametro.DsEmail = this.Email;
            pParametro.IdLogin = this.IdLogin;
            pParametro.StPessoaVinculada = this.Flag_PessoaVinculada;
            pParametro.StPPE = this.Flag_PPE;
            pParametro.StCarteiraPropria = this.Flag_OperaPorContaPropria;
            pParametro.StCVM387 = this.Flag_CVM387;
            pParametro.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado = this.NaoOperaPorContaPropriaCPF_CNPJ.Replace(".", "").Replace(".", "").Replace(".", "").Replace("-", "");
            pParametro.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado = this.NaoOperaPorContaPropriaNome;
            
            /// <summary>
            /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
            /// </summary>
            if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 7;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 6;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 5;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 4;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 3;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == false)
            {
                pParametro.StCienteDocumentos = 2;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == false && this.CienteLamina == true)
            {
                pParametro.StCienteDocumentos = 1;
            }

            pParametro.DsPropositoGradual = this.PropositoGradual.ToUpper();

            pParametro.StUSPerson = this.USPerson;

            return pParametro;
        }

        public override ClienteInfo ToClienteInfo()
        {
            ClienteInfo lRetorno = base.ToClienteInfo();
            lRetorno.IdAssessorInicial = this.Assessor.DBToInt32();
            lRetorno.DtPasso1 = DateTime.Now;
            lRetorno.StPasso = this.Passo.DBToInt32();
            lRetorno.CdSexo = this.Sexo[0];
            lRetorno.TpPessoa = 'F';
            lRetorno.TpCliente = int.Parse(this.Tipo);
            lRetorno.CdNacionalidade = this.Nacionalidade.DBToInt32();
            lRetorno.CdPaisNascimento = this.PaisDeNascimento;
            lRetorno.CdUfNascimento = this.EstadoDeNascimento;
            lRetorno.CdEstadoCivil = this.EstadoCivil.DBToInt32();
            lRetorno.DsConjugue = this.Conjuge;
            lRetorno.TpDocumento = this.TipoDeDocumento;

            lRetorno.DtNascimentoFundacao = this.DataNascimento.DBToDateTime();
            lRetorno.CdOrgaoEmissorDocumento = this.OrgaoEmissor;
            lRetorno.CdUfEmissaoDocumento = this.Documento_EstadoEmissao;
            lRetorno.CdProfissaoAtividade = this.Profissao.DBToInt32();
            lRetorno.DsCargo = this.CargoAtual;
            lRetorno.DsEmpresa = this.Empresa;
            lRetorno.StEmancipado = this.Flag_Emancipado;
            //lRetorno.CdEscolaridade = this.Escolaridade.DBToInt32();
            //lRetorno.StCadastroPortal              = false;  //TODO: Verificar. Vai direto?
            lRetorno.DsNomeMae = this.NomeDaMae;
            lRetorno.DsNomePai = this.NomeDoPai;
            lRetorno.DsNaturalidade = this.CidadeDeNascimento;
            lRetorno.DtEmissaoDocumento = this.Documento_DataEmissao.DBToDateTime(Contratos.Dados.Enumeradores.eDateNull.Permite);
            lRetorno.DsEmailComercial = this.EmailComercial;
            lRetorno.DsNumeroDocumento = this.Documento_Numero;
            lRetorno.DsEmail = this.Email;
            lRetorno.IdLogin = this.IdLogin;
            lRetorno.StPessoaVinculada = this.Flag_PessoaVinculada;
            lRetorno.DsSenhaGerada = PaginaBase.GerarSenha();

            lRetorno.DadosClienteNaoOperaPorContaPropria = new ClienteNaoOperaPorContaPropriaInfo();
            lRetorno.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado = this.NaoOperaPorContaPropriaCPF_CNPJ;
            lRetorno.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado = this.NaoOperaPorContaPropriaNome;

            lRetorno.TpDesejaAplicar = this.DesejaAplicar;
            
            /// <summary>
            /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
            /// </summary>
            if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == true)
            {
                lRetorno.StCienteDocumentos = 7;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == true && this.CienteLamina == false)
            {
                lRetorno.StCienteDocumentos = 6;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == true)
            {
                lRetorno.StCienteDocumentos = 5;
            }
            else if (this.CienteRegulamento == true && this.CienteProspecto == false && this.CienteLamina == false)
            {
                lRetorno.StCienteDocumentos = 4;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == true)
            {
                lRetorno.StCienteDocumentos = 3;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == true && this.CienteLamina == false)
            {
                lRetorno.StCienteDocumentos = 2;
            }
            else if (this.CienteRegulamento == false && this.CienteProspecto == false && this.CienteLamina == true)
            {
                lRetorno.StCienteDocumentos = 1;
            }

            lRetorno.DsPropositoGradual = this.PropositoGradual.ToUpper();

            return lRetorno;
        }

        #endregion
    }
}
