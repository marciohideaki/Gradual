using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using System.Globalization;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos.Dados.Cadastro;

namespace Gradual.Site.Www
{
    public class TransporteCadastro
    {
        #region Propriedades

        //Dados do "Passo 1":

        public string NomeCompleto      { get; set; }
        public string Email             { get; set; }
        public string EmailConfirmacao  { get; set; }
        public string Senha             { get; set; }
        public string SenhaConfirmacao  { get; set; }
        public string AssEletronica     { get; set; }
        public string AssEletronicaConfirmacao { get; set; }
        public string CPF               { get; set; }
        public string DataNascimento    { get; set; }
        public string Sexo              { get; set; }
        public string SexoDesc          { get; set; }
        public string Cel_ID            { get; set; }
        public string Cel_DDD           { get; set; }
        public string Cel_Numero        { get; set; }
        public string Tel_ID            { get; set; }
        public string Tel_DDD           { get; set; }
        public string Tel_Numero        { get; set; }
        public string TipoTelefone      { get; set; }
        public string ComoConheceu      { get; set; }
        public string Assessor          { get; set; }
        public string DesejaAplicar     { get; set; }
        
        public int Idade
        {
            get
            {
                CultureInfo lInfo = new CultureInfo("pt-BR");

                DateTime lData = DateTime.Parse(this.DataNascimento, lInfo);

                int lAnos = DateTime.Now.Year - lData.Year;

                if((lData.Month == DateTime.Now.Month && lData.Day > DateTime.Now.Day) || lData.Month > DateTime.Now.Month)
                {
                    lAnos--;    //ainda não fez aniversário esse ano...
                }

                return lAnos;
            }
        }

        //Dados do "Passo 2":
        
        public string Nacionalidade        { get; set; }
        public string NacionalidadeDesc    { get; set; }
        public string PaisNascimento       { get; set; }
        public string PaisNascimentoDesc   { get; set; }
        public string EstadoNascimento     { get; set; }
        public string EstadoNascimentoDesc { get; set; }
        public string CidadeNascimento     { get; set; }
        public string EstadoCivil          { get; set; }
        public string EstadoCivilDesc      { get; set; }
        public string Conjuge              { get; set; }
        public string Profissao            { get; set; }
        public string ProfissaoDesc        { get; set; }
        public string CargoFuncao          { get; set; }
        public string Empresa              { get; set; }
        public string TipoDocumento        { get; set; }
        public string TipoDocumentoDesc    { get; set; }
        public string NumeroDocumento      { get; set; }
        public string OrgaoEmissor         { get; set; }
        public string OrgaoEmissorDesc     { get; set; }
        public string EstadoEmissao        { get; set; }
        public string EstadoEmissaoDesc    { get; set; }
        public string DataEmissao          { get; set; }
        public string CodigoSegurancaCNH   { get; set; }

        public string NomeMae              { get; set; }
        public string NomePai              { get; set; }
        
        private string _PerfilSuitability = "n/d";
        // Suitability
        public string PerfilSuitability
        {
            get
            {
                return _PerfilSuitability;
            }

            set
            {
                _PerfilSuitability = value;

                switch (_PerfilSuitability.ToLower())
                {
                    case "arrojado" : _PerfilSuitability = "Arrojado"; break;
                    case "acessado" : _PerfilSuitability = "Conservador"; break;
                    case "cadastronaofinalizado" : _PerfilSuitability = "Conservador"; break;
                    case "medioriscocomrendavariavel" : _PerfilSuitability = "Moderado"; break;
                    case "medio risco com renda variavel" : _PerfilSuitability = "Moderado"; break;
                    case "medioriscosemrendavariavel" : _PerfilSuitability = "Moderado"; break;
                    case "conservador" : _PerfilSuitability = "Conservador"; break;
                    case "moderado" : _PerfilSuitability = "Moderado"; break;
                    case "naoresponderagora" : _PerfilSuitability = "Conservador"; break;
                    case "baixorisco" : _PerfilSuitability = "Conservador"; break;
                    case "naoresponder": _PerfilSuitability = "Conservador"; break;
                }
            }
        }

        public string Procurador { get; set; }

        // Dados alienígenas

        public string LiberadoParaOperar         { get; set; }
        public string CodigoBovespa              { get; set; }
        public string PessoaPoliticamenteExposta { get; set; }
        public string OperaPorContaPropria       { get; set; }
        public string USPerson                   { get; set; }
        public string CienteRegulamento          { get; set; }
        public string CienteProspecto            { get; set; }
        public string CienteLamina               { get; set; }
        public string PropositoGradual           { get; set; }

        public string NomeCliente                { get; set; }
        public string CPFCliente                 { get; set; }

        public string Emancipado                 { get; set; }
        public string PessoaVinculada            { get; set; }
        public string EmailComercial             { get; set; }
        public string DataCadastro               { get; set; }

        public List<TransporteCadastroEndereco> Enderecos { get; set; }

        public List<TransporteCadastroTelefone> Telefones { get; set; }

        public List<TransporteCadastroContaBancaria> Contas { get; set; }
        
        public List<TransporteCadastroDocumento> Documentos { get; set; }

        public TransporteCadastroSituacaoFinanceira SituacaoFinanceira { get; set; }

        public TransporteCadastroRepresentante Representante { get; set; }

        public TransporteCadastroSuitability Suitability { get; set; }

        #endregion

        #region Construtores

        public TransporteCadastro()
        {
            this.Enderecos = new List<TransporteCadastroEndereco>();
            this.Telefones = new List<TransporteCadastroTelefone>();
            this.Contas = new List<TransporteCadastroContaBancaria>();
            this.Documentos = new List<TransporteCadastroDocumento>();

            this.SituacaoFinanceira = new TransporteCadastroSituacaoFinanceira();
            this.Representante = new TransporteCadastroRepresentante();
        }

        public TransporteCadastro(TransporteSessaoClienteLogado pClienteBase) : this()
        {
            CarregarDados(pClienteBase);
        }

        #endregion

        #region Métodos Private

        private void CarregarDados(TransporteSessaoClienteLogado pClienteBase)
        {
            ReceberEntidadeCadastroRequest<ClienteInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteInfo>();
            ReceberEntidadeCadastroResponse<ClienteInfo> lResponse;

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            string lDataCadastro;

            lRequest.IdUsuarioLogado = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro = new ClienteInfo() { IdCliente = pClienteBase.IdCliente };

            lResponse = lServico.ReceberEntidadeCadastro<ClienteInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.CPF = pClienteBase.CpfCnpj.ToCpfCnpjString();

                this.Email               = pClienteBase.Email;
                this.NomeCompleto        = pClienteBase.Nome;
                this.LiberadoParaOperar  = (pClienteBase.Passo == 4) ? "Sim" : "Não";

                this.CodigoBovespa = string.IsNullOrWhiteSpace(pClienteBase.CodigoPrincipal) ? string.Empty : pClienteBase.CodigoPrincipal;

                this.Assessor = lResponse.EntidadeCadastro.IdAssessorInicial.ToString();

                this.PessoaPoliticamenteExposta  = (lResponse.EntidadeCadastro.StPPE != null && lResponse.EntidadeCadastro.StPPE.Value) ? "Sim" : "Não";

                this.PessoaVinculada   = lResponse.EntidadeCadastro.StPessoaVinculada.ToString();
                this.DataNascimento    = string.Format("{0:dd/MM/yyyy}", lResponse.EntidadeCadastro.DtNascimentoFundacao);
                this.Empresa           = lResponse.EntidadeCadastro.DsEmpresa;
                this.Conjuge           = lResponse.EntidadeCadastro.DsConjugue;
                this.NomeMae           = lResponse.EntidadeCadastro.DsNomeMae;
                this.NomePai           = lResponse.EntidadeCadastro.DsNomePai;
                this.EmailComercial    = lResponse.EntidadeCadastro.DsEmailComercial;
                this.CargoFuncao       = lResponse.EntidadeCadastro.DsCargo;
                this.CidadeNascimento  = lResponse.EntidadeCadastro.DsNaturalidade;
                this.NumeroDocumento   = lResponse.EntidadeCadastro.DsNumeroDocumento;
                this.DataEmissao       = (lResponse.EntidadeCadastro.DtEmissaoDocumento != null) ? lResponse.EntidadeCadastro.DtEmissaoDocumento.Value.ToString("dd/MM/yyyy") : string.Empty;

                this.Sexo              = lResponse.EntidadeCadastro.CdSexo.DBToString().ToUpper();
                this.SexoDesc          = (this.Sexo == "F") ? "Feminino" : "Masculino";

                this.Profissao         = lResponse.EntidadeCadastro.CdProfissaoAtividade.DBToString();
                this.ProfissaoDesc     = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.ProfissaoPF, this.Profissao);

                this.Nacionalidade     = lResponse.EntidadeCadastro.CdNacionalidade.DBToString();
                this.NacionalidadeDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Nacionalidade, this.Nacionalidade);

                if (lResponse.EntidadeCadastro.CdPaisNascimento != null)
                {
                    this.PaisNascimento     = lResponse.EntidadeCadastro.CdPaisNascimento.DBToString();
                    this.PaisNascimentoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Nacionalidade, this.PaisNascimento);
                }
                else
                {
                    this.PaisNascimento     = "BRA";
                    this.PaisNascimentoDesc = "BRASIL";
                }

                this.EstadoCivil       = lResponse.EntidadeCadastro.CdEstadoCivil.DBToString();
                this.EstadoCivilDesc   = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.EstadoCivil,   this.EstadoCivil);

                this.TipoDocumento     = lResponse.EntidadeCadastro.TpDocumento;
                this.TipoDocumentoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.TipoDocumento, this.TipoDocumento);

                this.OrgaoEmissor      = lResponse.EntidadeCadastro.CdOrgaoEmissorDocumento;
                this.OrgaoEmissorDesc  = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.OrgaoEmissor,  this.OrgaoEmissor);

                this.OrgaoEmissorDesc += string.Concat(" / ", lResponse.EntidadeCadastro.CdUfEmissaoDocumento);

                this.EstadoEmissao     = lResponse.EntidadeCadastro.CdUfEmissaoDocumento;
                this.EstadoEmissaoDesc = DadosDeAplicacao.BuscarDadoDoSinacor(eInformacao.Estado, this.EstadoEmissao);

                this.ComoConheceu      = lResponse.EntidadeCadastro.DsComoConheceu;

                this.DesejaAplicar = lResponse.EntidadeCadastro.TpDesejaAplicar;

                //this.NomeCliente = lResponse.EntidadeCadastro.dsnomecl

                lDataCadastro = string.Concat("Passo 1 realizado em: ", lResponse.EntidadeCadastro.DtPasso1.ToString("dd/MM/yyyy"));

                /*
                if (pClienteBase.Passo > 3)
                    lDataCadastro = string.Format("{0} - Cadastro efetivado em: {1}", lDataCadastro, lResponse.EntidadeCadastro.DtPrimeiraExportacao.Value.ToString("dd/MM/yyyy"));
                */

                this.DataCadastro = lDataCadastro;

                if (lResponse.EntidadeCadastro.CdNacionalidade.DBToString() == "1")
                {
                    this.EstadoNascimento = lResponse.EntidadeCadastro.CdUfNascimento;
                }
                else
                {
                    this.EstadoNascimento = lResponse.EntidadeCadastro.DsUfnascimentoEstrangeiro;
                }

                if (lResponse.EntidadeCadastro.StUSPerson.HasValue)
                {
                    this.USPerson = (lResponse.EntidadeCadastro.StUSPerson.Value == true ? "Sim" : "Não");
                }

                this.PropositoGradual = lResponse.EntidadeCadastro.DsPropositoGradual;

                if (lResponse.EntidadeCadastro.StCienteDocumentos.HasValue)
                {
                    /// <summary>
                    /// Regulamento, Prospecto, Lamina: 111 (7) RP_: 110 (6) R_L: 101 (5) R__: 100 (4) _PL: 011 (3) _P_: 010 (2) __L: 001
                    /// </summary>
                    /// 
                    if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 1)
                    {
                        this.CienteRegulamento = "Não";
                        this.CienteProspecto = "Não";
                        this.CienteLamina = "Sim";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 2)
                    {
                        this.CienteRegulamento = "Não";
                        this.CienteProspecto = "Sim";
                        this.CienteLamina = "Não";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 3)
                    {
                        this.CienteRegulamento = "Não";
                        this.CienteProspecto = "Sim";
                        this.CienteLamina = "Sim";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 4)
                    {
                        this.CienteRegulamento = "Sim";
                        this.CienteProspecto = "Não";
                        this.CienteLamina = "Não";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 5)
                    {
                        this.CienteRegulamento = "Sim";
                        this.CienteProspecto = "Não";
                        this.CienteLamina = "Sim";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 6)
                    {
                        this.CienteRegulamento = "Sim";
                        this.CienteProspecto = "Sim";
                        this.CienteLamina = "Não";
                    }
                    else if(lResponse.EntidadeCadastro.StCienteDocumentos.Value == 7)
                    {
                        this.CienteRegulamento = "Sim";
                        this.CienteProspecto = "Sim";
                        this.CienteLamina = "Sim";
                    }
                    else
                    {
                        this.CienteRegulamento = "Não";
                        this.CienteProspecto = "Não";
                        this.CienteLamina = "Não";
                    }
                }

                if((lResponse.EntidadeCadastro.StCarteiraPropria.HasValue && lResponse.EntidadeCadastro.StCarteiraPropria.Value == true) )
                {
                    this.OperaPorContaPropria = "Sim";
                }
                else
                {
                    this.OperaPorContaPropria = "Não";
                    
                    if (lResponse.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria != null)
                    {
                        this.NomeCliente = lResponse.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.DsNomeClienteRepresentado;
                        this.CPFCliente = lResponse.EntidadeCadastro.DadosClienteNaoOperaPorContaPropria.DsCpfCnpjClienteRepresentado;
                    }
                }

                CarregarEnderecos(pClienteBase);

                CarregarTelefones(pClienteBase);

                CarregarContas(pClienteBase);

                CarregarDadosSFP(pClienteBase);

                CarregarPerfil(pClienteBase);

                CarregarRepresentante(pClienteBase);

            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void CarregarEnderecos(TransporteSessaoClienteLogado pClienteBase)
        {
            ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>  lRequest = new ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo> lResponse;

            lRequest.IdUsuarioLogado        = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro       = new ClienteEnderecoInfo() { IdCliente = pClienteBase.IdCliente.Value };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

            this.Enderecos = new List<TransporteCadastroEndereco>();

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null)
                {
                    foreach (ClienteEnderecoInfo lEndereco in lResponse.Resultado)
                    {
                        this.Enderecos.Add(new TransporteCadastroEndereco(pClienteBase, lEndereco));
                    }
                }
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void CarregarTelefones(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteTelefoneInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteTelefoneInfo>();
            ConsultarEntidadeCadastroResponse<ClienteTelefoneInfo> lResponse;

            lRequest.IdUsuarioLogado = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro = new ClienteTelefoneInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteTelefoneInfo>(lRequest);

            this.Telefones = new List<TransporteCadastroTelefone>();

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null)
                {
                    foreach (ClienteTelefoneInfo lTelefone in lResponse.Resultado)
                    {
                        this.Telefones.Add(new TransporteCadastroTelefone(lTelefone));
                    }
                }
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void CarregarContas(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteBancoInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteBancoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteBancoInfo> lResponse;

            lRequest.IdUsuarioLogado = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro = new ClienteBancoInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteBancoInfo>(lRequest);

            this.Contas = new List<TransporteCadastroContaBancaria>();

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null)
                {
                    foreach (ClienteBancoInfo lConta in lResponse.Resultado)
                    {
                        this.Contas.Add(new TransporteCadastroContaBancaria(lConta));
                    }
                }
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void CarregarDadosSFP(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo> lRequest = new ReceberEntidadeCadastroRequest<ClienteSituacaoFinanceiraPatrimonialInfo>();
            ReceberEntidadeCadastroResponse<ClienteSituacaoFinanceiraPatrimonialInfo> lResponse;

            lRequest.IdUsuarioLogado = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro = new ClienteSituacaoFinanceiraPatrimonialInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ReceberEntidadeCadastro<ClienteSituacaoFinanceiraPatrimonialInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                this.SituacaoFinanceira = new TransporteCadastroSituacaoFinanceira(lResponse.EntidadeCadastro);
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        private void CarregarPerfil(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuitabilityInfo>();
            ConsultarEntidadeCadastroResponse<ClienteSuitabilityInfo> lResponse;

            lRequest.EntidadeCadastro = new ClienteSuitabilityInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteSuitabilityInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    this.PerfilSuitability = lResponse.Resultado[0].ds_perfil;
                }
                else
                {
                    this.PerfilSuitability = "n/d";
                }
            }
            else
            {
                string lMensagem = string.Format("Resposta do serviço com erro em PerfilCliente.aspx > BuscarPerfilDoCliente(IdCliente [{0}]) : [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdCliente
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta
                                                );

                throw new Exception(lMensagem);
            }
        }
        
        private void CarregarRepresentante(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteProcuradorRepresentanteInfo>();
            ConsultarEntidadeCadastroResponse<ClienteProcuradorRepresentanteInfo> lResponse;

            lRequest.EntidadeCadastro = new ClienteProcuradorRepresentanteInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteProcuradorRepresentanteInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    //this.PerfilSuitability = lResponse.Resultado[0].ds_perfil;
                    this.Procurador = "Sim";

                    this.Representante = new TransporteCadastroRepresentante(lResponse.Resultado[0]);
                }
                else
                {
                    this.Procurador = "Não";
                }
            }
            else
            {
                string lMensagem = string.Format("Resposta do serviço com erro em PerfilCliente.aspx > BuscarPerfilDoCliente(IdCliente [{0}]) : [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdCliente
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta
                                                );

                throw new Exception(lMensagem);
            }
        }

        private void CarregarDadosNaoOperaContaPropria(TransporteSessaoClienteLogado pClienteBase)
        {
            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            ConsultarEntidadeCadastroRequest<ClienteNaoOperaPorContaPropriaInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteNaoOperaPorContaPropriaInfo>();
            ConsultarEntidadeCadastroResponse<ClienteNaoOperaPorContaPropriaInfo> lResponse;

            lRequest.EntidadeCadastro = new ClienteNaoOperaPorContaPropriaInfo() { IdCliente = pClienteBase.IdCliente.Value };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteNaoOperaPorContaPropriaInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null && lResponse.Resultado.Count > 0)
                {
                    //this.PerfilSuitability = lResponse.Resultado[0].ds_perfil;
                    this.NomeCliente = lResponse.Resultado[0].DsNomeClienteRepresentado;
                    this.CPFCliente  = lResponse.Resultado[0].DsCpfCnpjClienteRepresentado;
                }
            }
            else
            {
                string lMensagem = string.Format("Resposta do serviço com erro em TransporteCadastro > CarregarDadosNaoOperaContaPropria(IdCliente [{0}]) : [{1}]\r\n{2}"
                                                , lRequest.EntidadeCadastro.IdCliente
                                                , lResponse.StatusResposta
                                                , lResponse.DescricaoResposta
                                                );

                throw new Exception(lMensagem);
            }
        }

        #endregion

        #region Métodos Públicos

        public Passo1Info ToPasso1Info()
        {
            Passo1Info lRetorno = new Passo1Info();

            lRetorno.CdAssinaturaEletronica  = this.AssEletronica;
            lRetorno.CdSenha                 = this.Senha;
            lRetorno.CdSexo                  = this.Sexo.ToUpper();
            lRetorno.DsCpfCnpj               = this.CPF;
            lRetorno.DsDdd                   = this.Tel_DDD;
            lRetorno.DsEmail                 = this.Email;
            lRetorno.DsNome                  = this.NomeCompleto.ToUpper();
            lRetorno.DsNumero                = this.Tel_Numero.Replace("-", "");

            lRetorno.ComoConheceu = this.ComoConheceu;

            lRetorno.DsCelDdd = this.Cel_DDD;
            lRetorno.DsCelNumero = this.Cel_Numero.Replace("-", "");

            //lRetorno.DsRamal                 = this.Ramal;
            lRetorno.DtNascimentoFundacao    = DateTime.ParseExact(this.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            lRetorno.IdTipoTelefone          = Convert.ToInt32(this.TipoTelefone);

            return lRetorno;
        }

        public ClienteInfo ToClienteInfo()
        {
            ClienteInfo lRetorno = new ClienteInfo();

            //lRetorno.StPasso                     = 2;
            //lRetorno.DtPasso2                    = DateTime.Now;

            lRetorno.CdNacionalidade             = Convert.ToInt32(this.Nacionalidade);
            //lRetorno.CdPaisNascimento            = this;
            lRetorno.CdUfNascimento              = this.EstadoNascimento;
            //lRetorno.DsUfnascimentoEstrangeiro   = this.UFNascimentoEstrangeiro;
            lRetorno.DsNaturalidade              = this.Nacionalidade;
            lRetorno.CdEstadoCivil               = Convert.ToInt32(this.EstadoCivil);
            lRetorno.DsConjugue                  = this.Conjuge;
            //lRetorno.CdEscolaridade              = this.IdEscolaridade;
            lRetorno.CdProfissaoAtividade        = Convert.ToInt32(this.Profissao);
            lRetorno.DsCargo                     = this.CargoFuncao;
            lRetorno.DsEmpresa                   = this.Empresa;
            lRetorno.DsEmailComercial            = this.Email;
            lRetorno.DsNomeMae                   = this.NomeMae;
            lRetorno.DsNomePai                   = this.NomePai;
            lRetorno.TpDocumento                 = this.TipoDocumento;
            lRetorno.CdUfEmissaoDocumento        = this.EstadoEmissao;
            lRetorno.DsNumeroDocumento           = this.NumeroDocumento;
            lRetorno.CdOrgaoEmissorDocumento     = this.OrgaoEmissor;

            if (!string.IsNullOrEmpty(this.DataEmissao))
            {
                lRetorno.DtEmissaoDocumento = DateTime.ParseExact(this.DataEmissao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return lRetorno;
        }

        public void TransferirClienteInfoPasso1(ref ClienteInfo pClienteBase)
        {
            pClienteBase.DsNome = this.NomeCompleto.ToUpper();
            pClienteBase.DsEmail = this.Email.ToUpper();
            pClienteBase.DsCpfCnpj = this.CPF;
            pClienteBase.DtNascimentoFundacao = DateTime.ParseExact(this.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            pClienteBase.CdSexo = this.Sexo[0];
            pClienteBase.IdAssessorInicial = string.IsNullOrEmpty(this.Assessor) ? 22 : Convert.ToInt32(this.Assessor);
            pClienteBase.DsComoConheceu = this.ComoConheceu;
        }

        public void TransferirClienteInfoPasso2(ref ClienteInfo pClienteBase)
        {
            
            pClienteBase.CdNacionalidade             = Convert.ToInt32(this.Nacionalidade);
            pClienteBase.CdPaisNascimento            = this.PaisNascimento.ToUpper();
            pClienteBase.CdUfNascimento              = this.EstadoNascimento.ToUpper();

            if(this.PaisNascimento != "BRA")
                pClienteBase.DsUfnascimentoEstrangeiro   = this.EstadoNascimento.ToUpper();

            pClienteBase.DsNaturalidade              = this.CidadeNascimento.ToUpper();
            pClienteBase.CdEstadoCivil               = Convert.ToInt32(this.EstadoCivil);
            pClienteBase.DsConjugue                  = this.Conjuge.ToUpper();

            pClienteBase.CdProfissaoAtividade        = Convert.ToInt32(this.Profissao);

            if(!string.IsNullOrEmpty(this.CargoFuncao))
                pClienteBase.DsCargo                     = this.CargoFuncao.ToUpper();

            if(!string.IsNullOrEmpty(this.Empresa))
                pClienteBase.DsEmpresa                   = this.Empresa.ToUpper();
            
            if(!string.IsNullOrEmpty(this.Email))
                pClienteBase.DsEmailComercial            = this.Email.ToUpper();
            
            if(!string.IsNullOrEmpty(this.NomeMae))
                pClienteBase.DsNomeMae                   = this.NomeMae.ToUpper();
            
            if(!string.IsNullOrEmpty(this.NomePai))
                pClienteBase.DsNomePai                   = this.NomePai.ToUpper();
            
            if(!string.IsNullOrEmpty(this.TipoDocumento))
                pClienteBase.TpDocumento                 = this.TipoDocumento.ToUpper();
            
            if(!string.IsNullOrEmpty(this.EstadoEmissao))
                pClienteBase.CdUfEmissaoDocumento        = this.EstadoEmissao.ToUpper();
            
            if(!string.IsNullOrEmpty(this.NumeroDocumento))
                pClienteBase.DsNumeroDocumento           = this.NumeroDocumento.ToUpper();
            
            if(!string.IsNullOrEmpty(this.OrgaoEmissor))
                pClienteBase.CdOrgaoEmissorDocumento     = this.OrgaoEmissor.ToUpper();

            pClienteBase.StCarteiraPropria = true;  
            // tem que colocar "true" aqui por enquanto porque é só no passo 3 que ele diz se 
            //opera ou não por conta própria, e se colocar "false" ele vai esperar dados do "operador"

            if (!string.IsNullOrEmpty(this.DataEmissao))
            {
                pClienteBase.DtEmissaoDocumento = DateTime.ParseExact(this.DataEmissao, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        public void ReavaliarTelefones()
        {
            this.Telefones = new List<TransporteCadastroTelefone>();

            TransporteCadastroTelefone lNovoTel;

            if (!string.IsNullOrEmpty(this.Cel_Numero))
            {
                lNovoTel = new TransporteCadastroTelefone();

                lNovoTel.DDD = this.Cel_DDD;
                lNovoTel.Numero = this.Cel_Numero;
                lNovoTel.IdTipo = 3;    //celular
                lNovoTel.Principal = true;

                if (!string.IsNullOrEmpty(this.Cel_ID))
                {
                    lNovoTel.IdTelefone = Convert.ToInt32(this.Cel_ID);
                }

                this.Telefones.Add(lNovoTel);
            }
            
            if (!string.IsNullOrEmpty(this.Tel_DDD))
            {
                lNovoTel = new TransporteCadastroTelefone();

                lNovoTel.DDD = this.Tel_DDD;
                lNovoTel.Numero = this.Tel_Numero;
                lNovoTel.IdTipo = Convert.ToInt32(this.TipoTelefone);
                lNovoTel.Principal = false;

                if (!string.IsNullOrEmpty(this.Tel_ID))
                {
                    lNovoTel.IdTelefone = Convert.ToInt32(this.Tel_ID);
                }

                this.Telefones.Add(lNovoTel);
            }
        }

        #endregion
    }
}