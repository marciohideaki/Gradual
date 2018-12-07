using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.OMS.Library;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Site.Www
{
    public class TransporteDadosCadastraisDoCliente : TransporteComBuscaDoSinacor
    {
        #region Propriedades

        public string NomeCompleto { get; set; }

        public string CPF { get; set; }

        public string Email { get; set; }

        public string LiberadoParaOperar { get; set; }

        public string CodigoBovespa { get; set; }

        public string Assessor { get; set; }

        public string PessoaPoliticamenteExposta { get; set; }

        public string PessoaVinculada { get; set; }

        public string DataDeNascimento { get; set; }

        public string Sexo { get; set; }

        public string NomeDaEmpresa { get; set; }

        public string NomeDoConjuge { get; set; }

        public string NomeDaMae { get; set; }

        public string NomeDoPai { get; set; }

        public string EmailComercial { get; set; }

        public string CargoAtualOuFuncao { get; set; }

        public string Naturalidade { get; set; }

        public string NumeroDoDocumento { get; set; }

        public string DataDeEmissaoDoDocumento { get; set; }

        public string Profissao { get; set; }

        public string Nacionalidade { get; set; }

        public string PaisDeNascimento { get; set; }

        public string EstadoCivil { get; set; }

        public string TipoDoDocumento { get; set; }

        public string OrgaoEmissor { get; set; }

        public string DataCadastro { get; set; }

        public string UFDeNascimento { get; set; }
        
        public List<TransporteEndereco> Enderecos { get; set; }

        public List<TransporteTelefone> Telefones { get; set; }

        #endregion

        #region Private Methods
        
        private void CarregarEnderecos(TransporteSessaoClienteLogado pClienteBase)
        {
            ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>  lRequest = new ConsultarEntidadeCadastroRequest<ClienteEnderecoInfo>();
            ConsultarEntidadeCadastroResponse<ClienteEnderecoInfo> lResponse;

            lRequest.IdUsuarioLogado        = pClienteBase.IdLogin;
            lRequest.DescricaoUsuarioLogado = pClienteBase.Nome;
            lRequest.EntidadeCadastro       = new ClienteEnderecoInfo() { IdCliente = pClienteBase.IdCliente.DBToInt32() };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteEnderecoInfo>(lRequest);

            this.Enderecos = new List<TransporteEndereco>();

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null)
                {
                    foreach (ClienteEnderecoInfo lEndereco in lResponse.Resultado)
                    {
                        this.Enderecos.Add(new TransporteEndereco(pClienteBase, lEndereco));
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
            lRequest.EntidadeCadastro = new ClienteTelefoneInfo() { IdCliente = pClienteBase.IdCliente.DBToInt32() };

            lResponse = lServico.ConsultarEntidadeCadastro<ClienteTelefoneInfo>(lRequest);

            this.Telefones = new List<TransporteTelefone>();

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lResponse.Resultado != null)
                {
                    foreach (ClienteTelefoneInfo lTelefone in lResponse.Resultado)
                    {
                        this.Telefones.Add(new TransporteTelefone(lTelefone));
                    }
                }
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

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
                //--> Dados de sessao
                this.CPF                 = pClienteBase.CpfCnpj.ToCpfCnpjString();
                this.Email               = pClienteBase.Email;
                this.NomeCompleto        = pClienteBase.Nome;
                this.LiberadoParaOperar  = pClienteBase.Passo == 4 ? "Sim" : "Não";

                this.CodigoBovespa = string.IsNullOrWhiteSpace(pClienteBase.CodigoPrincipal) ? string.Empty : pClienteBase.CodigoPrincipal;

                this.Assessor = pClienteBase.AssessorPrincipal;

                this.PessoaPoliticamenteExposta  = (null != lResponse.EntidadeCadastro.StPPE && lResponse.EntidadeCadastro.StPPE.Value) ? "Sim" : "Não";
                this.PessoaVinculada             = lResponse.EntidadeCadastro.StPessoaVinculada.ToString();
                this.DataDeNascimento            = string.Format("{0:dd/MM/yyyy}", lResponse.EntidadeCadastro.DtNascimentoFundacao);
                this.Sexo                        = lResponse.EntidadeCadastro.CdSexo.DBToString().ToUpper() == "F" ? "Feminino" : "Masculino";
                this.NomeDaEmpresa               = lResponse.EntidadeCadastro.DsEmpresa;
                this.NomeDoConjuge               = lResponse.EntidadeCadastro.DsConjugue;
                this.NomeDaMae                   = lResponse.EntidadeCadastro.DsNomeMae;
                this.NomeDoPai                   = lResponse.EntidadeCadastro.DsNomePai;
                this.EmailComercial              = lResponse.EntidadeCadastro.DsEmailComercial;
                this.CargoAtualOuFuncao          = lResponse.EntidadeCadastro.DsCargo;
                this.Naturalidade                = lResponse.EntidadeCadastro.DsNaturalidade;
                this.NumeroDoDocumento           = lResponse.EntidadeCadastro.DsNumeroDocumento;
                this.DataDeEmissaoDoDocumento    = (lResponse.EntidadeCadastro.DtEmissaoDocumento != null) ? lResponse.EntidadeCadastro.DtEmissaoDocumento.Value.ToString("dd/MM/yyyy") : string.Empty;

                this.Profissao         = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.ProfissaoPF,    lResponse.EntidadeCadastro.CdProfissaoAtividade.DBToString());
                this.Nacionalidade     = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.Nacionalidade,  lResponse.EntidadeCadastro.CdNacionalidade.DBToString());
                this.PaisDeNascimento  = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.Pais,           lResponse.EntidadeCadastro.CdPaisNascimento.DBToString());
                this.EstadoCivil       = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.EstadoCivil,    lResponse.EntidadeCadastro.CdEstadoCivil.DBToString());
                this.TipoDoDocumento   = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.TipoDocumento,  lResponse.EntidadeCadastro.TpDocumento);
                this.OrgaoEmissor      = this.RecuperarDadoDoSinacor(pClienteBase, eInformacao.OrgaoEmissor,   lResponse.EntidadeCadastro.CdOrgaoEmissorDocumento);

                this.OrgaoEmissor   += string.Concat(" / ", lResponse.EntidadeCadastro.CdUfEmissaoDocumento);

                lDataCadastro = string.Concat("Passo 1 realizado em: ", lResponse.EntidadeCadastro.DtPasso1.ToString("dd/MM/yyyy"));

                if (pClienteBase.Passo > 3)
                    lDataCadastro = string.Format("{0} - Cadastro efetivado em: {1}", lDataCadastro, lResponse.EntidadeCadastro.DtPrimeiraExportacao.Value.ToString("dd/MM/yyyy"));

                this.DataCadastro = lDataCadastro;

                if (lResponse.EntidadeCadastro.CdNacionalidade.DBToString() == "1")
                {
                    this.UFDeNascimento = lResponse.EntidadeCadastro.CdUfNascimento;
                }
                else
                {
                    this.UFDeNascimento = lResponse.EntidadeCadastro.DsUfnascimentoEstrangeiro;
                }

                if (pClienteBase.IdCliente != null)
                {
                    CarregarEnderecos(pClienteBase);

                    CarregarTelefones(pClienteBase);
                }
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }
        }

        #endregion

        #region Construtores

        public TransporteDadosCadastraisDoCliente() { }

        public TransporteDadosCadastraisDoCliente(TransporteSessaoClienteLogado pClienteBase)
        {
            CarregarDados(pClienteBase);
        }

        #endregion
    }
}