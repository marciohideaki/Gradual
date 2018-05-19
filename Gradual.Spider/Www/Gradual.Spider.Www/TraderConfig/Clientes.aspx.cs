using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Spider.Lib;
using Gradual.Spider.Lib.Dados;
using Gradual.Spider.Www.App_Codigo.Transporte;
using Newtonsoft.Json;
using Gradual.Spider.Lib.Mensagens;
using Gradual.Core.OMS.LimiteBMF.Lib;
using Gradual.Core.OMS.LimiteBMF;

namespace Gradual.Spider.Www
{
    public partial class Clientes : PaginaBase
    {
        #region Propriedades
        
        public static Dictionary<int, string> gListaSessao     = null;
        public static Dictionary<int, string> gListaLocalidade = null;
        public static List<SinacorListaInfo> gListaSinacor     = null;

        private string GetCodigoCliente
        {
            get
            {
                if (Request["CodigoCliente"] != null)
                    return Request["CodigoCliente"].ToString();
                else
                    return "";
            }
        }

        private string GetCodBmf
        {
            get
            {
                if (Request["CodigoClienteBmf"] != null)
                    return Request["CodigoClienteBmf"].ToString();
                else
                    return "";
            }
        }

        #endregion

        #region Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] 
                                            { "Cadastrar"
                                            , "Excluir"
                                            , "Atualizar"
                                            , "Selecionar"
                                            , "SalvarPermissoes"
                                            , "SalvarRestricoes"
                                            , "ListarDadosRiscoClientes"
                                            },
            new ResponderAcaoAjaxDelegate[] { this.ResponderCadastrar
                                            , this.ResponderExcluir 
                                            , this.ResponderAtualizar
                                            , this.ResponderSelecionar
                                            , this.ResponderSalvarPermissoes
                                            //, this.ResponderSalvarLimitesBovespa
                                            //, this.ResponderSalvarLimitesBmf
                                            , this.ResponderSalvarRestricoes
                                            , this.ResponderListarDadosRiscoClientes
                                            });

            if (!this.IsPostBack)
            {
                this.CarregarCombos();
                this.CarregarPermissoesLoad();
            }
        }

        #endregion

        #region Métodos Risco
        private void CarregarPermissoesLoad()
        {
            var lPermissoes = base.ListarPermissoesRisco(new RiscoListarPermissoesRequest());

            this.Cliente_Risco_Permissao.DataSource = lPermissoes.Permissoes;
            this.Cliente_Risco_Permissao.DataBind();
        }

        public string ResponderListarDadosRiscoClientes()
        {
            string lRetorno = string.Empty;

            var lPermissoes = base.ListarPermissoesRisco(new RiscoListarPermissoesRequest());
            
            var lRequest = new RiscoListarPermissoesClienteRequest();

            lRequest.CodigoCliente = GetCodigoCliente.DBToInt32();

            var lPermissoesClientes = base.ListarPermissoesRiscoClienteSpider(lRequest);

            var lRequestLimites = new RiscoListarParametrosClienteRequest();

            lRequestLimites.CodigoCliente = GetCodigoCliente.DBToInt32();

            var lLimites = base.ListarLimitePorClienteSpider(lRequestLimites);

            var lTransporte = new Transporte_Risco();

            var lLimitesBmf = this.ResponderCarregarLimitesBmf();

            lTransporte.gPermissoes    = new Transporte_Risco.Permissoes().Traduzir(lPermissoesClientes.PermissoesAssociadas);
            lTransporte.gLimiteBovespa = new Transporte_Risco.LimiteBovespa().TraduzirLista(lLimites.ParametrosRiscoCliente);
            lTransporte.gLimiteBmf     = new Transporte_Risco.LimiteBmf().TraduzirListas(lLimitesBmf);
            lTransporte.gRestricoes    = new Transporte_Risco.Restricoes().Traduzir();

            lRetorno = RetornarSucessoAjax(lTransporte, "Cliente com permissões");

            return lRetorno;
        }

        private ListarLimiteBMFResponse ResponderCarregarLimitesBmf()
        {
            var lRetorno = new ListarLimiteBMFResponse();

            var lServico = new ServicoLimiteBMF();

            ListarLimiteBMFRequest lRequest = new ListarLimiteBMFRequest();

            if (!string.IsNullOrEmpty(GetCodBmf))
            {
                lRequest.Account = GetCodBmf.DBToInt32();

                ListarLimiteBMFResponse lResponse = lServico.ObterLimiteBMFCliente(lRequest);

                if (lResponse.ListaLimites != null && lResponse.ListaLimitesInstrumentos != null)
                {
                    lRetorno.ListaLimites = lResponse.ListaLimites;
                    lRetorno.ListaLimitesInstrumentos = lResponse.ListaLimitesInstrumentos;
                }
            }

            return lRetorno;
        }

        private string ResponderSalvarPermissoes()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            var lDados = JsonConvert.DeserializeObject<Transporte_Risco.Permissoes>(lObjetoJson);



            return lRetorno;
        }

        private string ResponderSalvarRestricoes()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            var lDados = JsonConvert.DeserializeObject<Transporte_Risco.Restricoes>(lObjetoJson);

            return lRetorno;
        }
        #endregion

        #region Métodos
        private void CarregarCombos()
        {
            gListaSessao = base.ListarSessaoFix();
            gListaLocalidade = base.ListarLocalidadeAssessor();

            this.rptClienteFormSessao.DataSource = gListaSessao;
            this.rptClienteFormSessao.DataBind();

            this.rptClienteFormLocalidade.DataSource = gListaLocalidade;
            this.rptClienteFormLocalidade.DataBind();

            gListaSinacor =  base.ListarSinacor(new Lib.Dados.SinacorListaInfo() { Informacao = eInformacao.Assessor });

            this.rptClienteFormAssessor.DataSource = gListaSinacor;
            this.rptClienteFormAssessor.DataBind();

            var lListaCommodities = new Dictionary<string,string>();

            lListaCommodities.Add("DI1", "DI1.FUT" );
            lListaCommodities.Add("DOL", "DOL.FUT" );
            lListaCommodities.Add("IND", "IND.FUT" );
            lListaCommodities.Add("WIN", "WIN.FUT" );
            lListaCommodities.Add("WDL", "WDL.FUT" );
            lListaCommodities.Add("WDO", "WDO.FUT" );
            lListaCommodities.Add("BGI", "BGI.FUT" );
            lListaCommodities.Add("WBG", "WBG.FUT" );
            lListaCommodities.Add("EUR", "EUR.FUT" );
            lListaCommodities.Add("WEU", "WEU.FUT" );
            lListaCommodities.Add("ICF", "ICF.FUT" );
            lListaCommodities.Add("WCF", "WCF.FUT" );
            lListaCommodities.Add("ISU", "ISU.FUT" );
            lListaCommodities.Add("ETH", "ETH.FUT" );
            lListaCommodities.Add("ETN", "ETN.FUT" );
            lListaCommodities.Add("CCM", "CCM.FUT" );
            lListaCommodities.Add("SFI", "SFI.FUT" );
            lListaCommodities.Add("OZ1", "OZ1.FUT" );
            lListaCommodities.Add("DR1", "DR1.FUT" );
            lListaCommodities.Add("IR1", "IR1.FUT" );
            lListaCommodities.Add("BR1", "BR1.FUT" );
            lListaCommodities.Add("CR1", "CR1.FUT" );
            lListaCommodities.Add("MR1", "MR1.FUT" );
            lListaCommodities.Add("SR1", "SR1.FUT" );
            lListaCommodities.Add("ISP", "ISP.FUT");

            //this.rptCommodities.DataSource = lListaCommodities;
            //this.rptCommodities.DataBind();
        }

        private string ResponderCadastrar()
        {
            string lRetorno = string.Empty;
            
            try
            {
                string lObjetoJson = Request["ObjetoJson"];

                var lDados = JsonConvert.DeserializeObject<Transporte_Clientes>(lObjetoJson);

                var lRequest = new ClienteInfo();

                lRequest = new Transporte_Clientes().Traduzir(lDados);

                var lResponse = base.InserirCliente(lRequest);

                if (lResponse.Criticas.Count > 0)
                {
                    base.RetornarErroAjax(base.CompilarCriticas(lResponse.Criticas));
                }
                else
                {
                    base.RetornarSucessoAjax("Cliente Salvo com sucesso");
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.Message);
            }
            
            return lRetorno;
        }

        private string ResponderExcluir()
        {
            string lRetorno = string.Empty;

            return lRetorno;
        }

        private string ResponderAtualizar()
        {
            string lRetorno = string.Empty;

            try
            {
                string lObjetoJson = Request["ObjetoJson"];

                var lDados = JsonConvert.DeserializeObject<Transporte_Clientes>(lObjetoJson);

                var lRequest = new ClienteInfo();

                lRequest = new Transporte_Clientes().Traduzir(lDados);

                var lResponse = base.InserirCliente(lRequest);

                if (lResponse.Criticas.Count > 0)
                {
                    lRetorno = base.RetornarErroAjax(base.CompilarCriticas(lResponse.Criticas));
                }
                else
                {
                    lRetorno = base.RetornarSucessoAjax("Cliente Salvo com sucesso");
                }
            }
            catch (Exception ex)
            {
                lRetorno = base.RetornarErroAjax(ex.Message);
            }

            return lRetorno;
        }

        private string ResponderSelecionar()
        {
            string lRetorno = string.Empty;

            string lObjetoJson = Request["ObjetoJson"];

            var lDados = JsonConvert.DeserializeObject<Transporte_Clientes>(lObjetoJson);

            var lRequest = new ClienteInfo();

            lRequest.CodigoBovespa = Convert.ToBoolean(lDados.PesquisaBovespa)      ? lDados.PesquisaCodigo.DBToInt32() : 0;
            lRequest.CodigoBmf     = Convert.ToBoolean(lDados.PesquisaBmf)          ? lDados.PesquisaCodigo.DBToInt32() : 0;
            lRequest.ContaMae      = Convert.ToBoolean(lDados.PesquisaContaMaster)  ? lDados.PesquisaCodigo : "0";

            var lResponse = base.BuscarCliente(lRequest);

            if (lResponse != null)
            {
                var lTransporte = new Transporte_Clientes().Traduzir(lResponse);

                //this.CarregarPermissoes(lResponse.CodigoBovespa);

                lRetorno = base.RetornarSucessoAjax(lTransporte, "Cliente encontrado com sucesso!");
            }
            else
            {
                lRetorno = base.RetornarErroAjax("Cliente não encontrado", "");
            }

            
            
            return lRetorno;
        }
        #endregion
    }
}