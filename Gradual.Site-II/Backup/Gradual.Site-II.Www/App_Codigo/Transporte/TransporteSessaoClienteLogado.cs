using System;

using System.Collections.Generic;
using Gradual.OMS.PoupeDirect.Lib.Dados;
using Gradual.Site.DbLib.Dados.MinhaConta.Comercial;
using System.ComponentModel;
using Gradual.Educacional.Entidade;
using Gradual.Educacional.Dados;
using Gradual.Site.Www.WsCadastro;
using Gradual.Site.DbLib.Dados.MinhaConta;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.Site.DbLib.Mensagens;
using Gradual.Site.DbLib.Persistencias;

namespace Gradual.Site.Www
{
    public class TransporteSessaoClienteLogado : ObjetoComLogger
    {
        #region Enum

        public enum EnumTipoCliente
        {
            Administrador,
            AnaliseGraficas,
            AnaliseFundamentalista,
            AnaliseEconomica,
            VisitanteAte30Dias,
            VisitanteExpirado,
            Cadastrado,
            CadastradoEExportado,
            Direct,
        }

        public enum EnumTipoPessoa
        {
            Fisica,
            Juridica
        }

        public enum PermissoesPertinentesAoSite
        {
              EditarCMS
            , EditarAnaliseEconomica
            , EditarAnaliseFundamentalista
            , EditarAnaliseGrafica
            , EditarCarteirasRecomendadas
            , EditarNikkei
            , EditarGradiusGestao
        }

        #endregion

        #region Propriedades

        public Nullable<int> IdCliente { get; set; }

        public int IdLogin { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string CpfCnpj { get; set; }
        
        public string DesejaAplicar { get; set; }

        private string _CodigoPrincipal;

        public string CodigoPrincipal
        {
            get
            {
                return _CodigoPrincipal;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || value == "0")
                {
                    _CodigoPrincipal = "";
                }
                else
                {
                    _CodigoPrincipal = value;
                }
            }
        }

        public int CodigoBMF { get; set; }

        public string CodigoDaSessao { get; set; }

        public string AssessorPrincipal { get; set; }

        public EnumTipoCliente TipoAcesso { get; set; } // calc

        public Nullable<int> NumeroDiasAcesso { get; set; } // calc

        public Nullable<int> Passo { get; set; }

        public Nullable<DateTime> NascimentoFundacao { get; set; }

        public EnumTipoPessoa TipoPessoa { get; set; }

        public string Senha { get; set; }

        public List<ProdutoTransporte> ListaSessaoProdutos { get; set; }

        public TransporteDadosCarrinho DadosDoCarrinho { get; set; }

        public Gradual.OMS.PoupeDirect.Lib.Dados.ClienteProdutoInfo ClienteProdutoInfoSessao { get; set; }

        public Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoIRInfo IntegracaoIRInfoSessaoPlanoAberto { get; set; }

        public Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoIRInfo IntegracaoIRInfoSessaoPlanoFechado { get; set; }

        public Dictionary<string, VendaInfo> VendasEmAndamento { get; set; }

        public List<ProdutoCompradoInfo> ProdutosAdquiridos { get; set; }

        public bool ExpiracaoDeSenhaJaValidada { get; set; }

        public bool PrimeiroLoginJaVerificado { get; set; }

        public List<PermissoesPertinentesAoSite> Permissoes { get; set; }

        public string PlanoCalculadoraIR { get; set; }

        private BindingList<CETb_cliente_curso_palestra> PalestrasInscritas { get; set; }

        public List<ClienteContratoInfo> Contratos { get; set; }

        private List<ContaBancariaInfo> _ContasBancarias = null;

        public List<ContaBancariaInfo> ContasBancarias
        {
            get
            {
                if (_ContasBancarias == null)
                {
                    ContaBancariaRequest lRequest = new ContaBancariaRequest();
                    ContaBancariaResponse lResponse;

                    ServicoPersistenciaSite lServico = new ServicoPersistenciaSite();

                    lRequest.ContaBancaria = new Site.DbLib.Dados.MinhaConta.ContaBancariaInfo();

                    lRequest.ContaBancaria.CodigoCliente = this.CodigoPrincipal.DBToInt32();

                    lResponse = lServico.BuscarContasBancariasDoCliente(lRequest);

                    if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                    {
                        _ContasBancarias = lResponse.ListaContaBancaria;
                    }
                    else
                    {
                        throw new Exception(string.Format("Erro ao carregar contas bancárias: [{0}]\r\n{1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
                    }
                }

                return _ContasBancarias;
            }

            set
            {
                _ContasBancarias = value;
            }
        }

        private string _PerfilSuitability = "n/d";

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

        public string IdPerfilSuitability { get; set; }

        public bool JaPreencheuSuitability
        {
            get
            {
                return (!string.IsNullOrEmpty(this.PerfilSuitability) && this.PerfilSuitability != "n/d");
            }
        }

        public string NomeArquivoFichaCadastral { get; set; }

        public DateTime DataDeUltimoLogin { get; set; }

        #endregion

        #region Construtores

        public TransporteSessaoClienteLogado()
        {
            this.Permissoes = new List<PermissoesPertinentesAoSite>();
            this.DadosDoCarrinho = new TransporteDadosCarrinho();
        }

        #endregion

        #region Métodos Públicos

        public bool Pode(PermissoesPertinentesAoSite pPermissao)
        {
            return this.Permissoes.Contains(pPermissao);
        }

        public bool TemProduto (int pIdProduto)
        {
            foreach (ProdutoCompradoInfo lProduto in this.ProdutosAdquiridos)
            {
                if (lProduto.IdProduto == pIdProduto && lProduto.Status < 5)
                    return true;
            }

            return false;
        }

        public ProdutoCompradoInfo DadosDoProduto (int pIdProduto)
        {
            foreach (ProdutoCompradoInfo lProduto in this.ProdutosAdquiridos)
            {
                if (lProduto.IdProduto == pIdProduto)
                    return lProduto;
            }

            return null;
        }

        public bool InscreveuseNaPalestra(int pIdDaPalestra)
        {
            if(this.PalestrasInscritas == null)
            {
                try 
                {
                    CDTb_cliente_curso_palestra lPalestras = new CDTb_cliente_curso_palestra();

                    this.PalestrasInscritas = lPalestras.Listar(this.IdCliente, null);
                }
                catch(Exception ex)
                {
                    this.PalestrasInscritas = new BindingList<CETb_cliente_curso_palestra>();

                    gLogger.ErrorFormat("Erro em CDTb_cliente_curso_palestra.Listar(pIdCliente: [{0}]) em TransporteSessaoClienteLogado.InscreveuseNaPalestra() > [{1}]\r\n{2}\r\n>>Assumindo que o cliente não foi inscrito..."
                                        , this.IdCliente
                                        , ex.Message
                                        , ex.StackTrace);
                }
            }

            foreach (CETb_cliente_curso_palestra lPalestra in this.PalestrasInscritas)
            {
                if(lPalestra.IdCursoPalestra == pIdDaPalestra)
                    return true;
            }

            return false;
        }
        /*
        public List<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo> BuscarDocumentos()
        {
            List<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo> lRetorno = null;

            ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo>  lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo>();
            ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo> lResponse;

            lRequest.IdUsuarioLogado        = this.IdCliente.Value;
            lRequest.DescricaoUsuarioLogado = this.Nome;
            lRequest.EntidadeCadastro       = new Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo() { IdCliente = this.IdCliente.Value };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteDocumentoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }*/

        public List<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo> BuscarEnderecos()
        {
            List<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo> lRetorno = null;

            ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo>  lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo>();
            ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo> lResponse;

            lRequest.IdUsuarioLogado        = this.IdCliente.Value;
            lRequest.DescricaoUsuarioLogado = this.Nome;
            lRequest.EntidadeCadastro       = new Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo() { IdCliente = this.IdCliente.Value };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteEnderecoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        public List<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo> BuscarTelefones()
        {
            List<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo> lRetorno = null;

            ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo>  lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo>();
            ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo> lResponse;

            lRequest.IdUsuarioLogado        = this.IdCliente.Value;
            lRequest.DescricaoUsuarioLogado = this.Nome;
            lRequest.EntidadeCadastro       = new Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo() { IdCliente = this.IdCliente.Value };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteTelefoneInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        public List<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo> BuscarContasBancarias()
        {
            List<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo> lRetorno = null;

            ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>  lRequest = new ConsultarEntidadeCadastroRequest<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>();
            ConsultarEntidadeCadastroResponse<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo> lResponse;

            lRequest.IdUsuarioLogado        = this.IdCliente.Value;
            lRequest.DescricaoUsuarioLogado = this.Nome;
            lRequest.EntidadeCadastro       = new Gradual.Intranet.Contratos.Dados.ClienteBancoInfo() { IdCliente = this.IdCliente.Value };

            IServicoPersistenciaCadastro lServico = Ativador.Get<IServicoPersistenciaCadastro>();

            lResponse = lServico.ConsultarEntidadeCadastro<Gradual.Intranet.Contratos.Dados.ClienteBancoInfo>(lRequest);

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                lRetorno = lResponse.Resultado;
            }
            else
            {
                throw new Exception(lResponse.DescricaoResposta);
            }

            return lRetorno;
        }

        #endregion
    }
}