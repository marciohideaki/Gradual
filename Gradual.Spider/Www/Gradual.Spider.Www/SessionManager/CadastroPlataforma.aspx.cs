using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Spider.Www.App_Codigo.Transporte;
using Newtonsoft.Json;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www
{
    public partial class CadastroPlataforma : PaginaBase
    {
        #region Propriedades
        public int CodigoPlataforma { get; set; }
        #endregion

        public string RaizDoSite
        {
            get
            {
                return ConfiguracoesValidadas.RaizDoSite;
            }
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            base.RegistrarRespostasAjax(new string[] 
                                            { "Cadastrar"
                                            , "Excluir"
                                            , "Atualizar"
                                            , "Selecionar"
                                            , "Buscar"
                                            , "Listar"
                                            },
            new ResponderAcaoAjaxDelegate[] { this.ResponderCadastrar
                                            , this.ResponderExcluir 
                                            , this.ResponderAtualizar
                                            , this.ResponderSelecionar
                                            , this.ResponderBuscar
                                            , this.ResponderListar
                                            });
            if (!this.IsPostBack)
            {
                this.CarregarCombos();
                this.ResponderListar();
            }
        }

        public string ResponderListar()
        {
            string lRetorno = string.Empty;

            var lLista = base.ListarPlataformaSessao(new PlataformaSessaoInfo());

            var lListaPlataforma = new Transporte_Plataforma().TraduzirLista(lLista);

            lRetorno = RetornarSucessoAjax(lListaPlataforma, "OK");

            return lRetorno;
        }

        public void CarregarCombos()
        {
            this.rptPlataforma.DataSource = base.ListarPlataforma(new Lib.Dados.PlataformaSessaoInfo());
            this.rptPlataforma.DataBind();

            var listaSessao = base.ListarSessao(new Lib.Dados.PlataformaSessaoInfo());

            this.rptSessaoCliente.DataSource = listaSessao;
            this.rptSessaoCliente.DataBind();

            this.rptSessaoMesa.DataSource = listaSessao;
            this.rptSessaoMesa.DataBind();

            this.rptSessaoRepassador.DataSource = listaSessao;
            this.rptSessaoRepassador.DataBind();
        }

        public string ResponderBuscar()
        {
            string lRetorno = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para BUscar Plataforma", ex);
            }

            return lRetorno;
        }

        public string ResponderSelecionar()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new PlataformaSessaoInfo();

                lRequest.CodigoPlataforma = int.Parse(Request["CodigoPlataforma"]);

                var lSelecao = SelecionarPlataformaSessao(lRequest);

                if (lSelecao != null)
                {
                    var lSelecaoPlataforma = new Transporte_Plataforma().TraduzirLista(lSelecao);

                    Session["Plataforma_antes_" + lRequest.CodigoPlataforma] = lSelecaoPlataforma;

                    lRetorno = RetornarSucessoAjax(lSelecaoPlataforma, "OK");
                }
            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para Selecionar Plataforma", ex);
            }

            return lRetorno;
        }

        public string ResponderAtualizar()
        {
            string lRetorno = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para Atualizar Plataforma", ex);
            }

            return lRetorno;
        }

        public string ResponderExcluir()
        {
            string lRetorno = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para Excluir Plataforma", ex);
            }
            return lRetorno;
        }

        public string ResponderCadastrar()
        {
            string lRetorno = string.Empty;
            
            string lObjetoJson = Request["ObjetoJson"];

            string lCodigoPlataforma = string.Empty;

            try
            {
                Transporte_Plataforma lDadosPlataforma = JsonConvert.DeserializeObject<Transporte_Plataforma>(lObjetoJson);

                var ListaPlataformaSessao = lDadosPlataforma.ToPlataformaSessaoInfo(lDadosPlataforma);

                lCodigoPlataforma = lDadosPlataforma.CodigoPlataforma;

                var lResponse =  base.InserirPlataformaSessao(ListaPlataformaSessao);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    string lAntes  = "";
                    string lDepois = "";

                    if (Session["Plataforma_antes_" + lCodigoPlataforma] != null)
                    {
                        var lTransporte = Session["Plataforma_antes_" + lCodigoPlataforma];

                        var lSerialize = new Transporte_Plataforma().TraduzirLog((List<Transporte_Plataforma>)lTransporte);

                        string lAntesSerializado = JsonConvert.SerializeObject(lSerialize);

                        lAntes  = "Antes -->> " + lAntesSerializado;
                        lDepois = "Depois -->> " + lObjetoJson;
                    }
                    else
                    {
                        lDepois = "Novo -->> " + lObjetoJson;
                    }

                    base.RegistrarLogInclusao(string.Concat(lAntes, lDepois));

                    lRetorno = RetornarSucessoAjax(lResponse.DescricaoResposta);
                }

            }
            catch (Exception ex)
            {
                lRetorno = RetornarErroAjax("Erro durante o envio do pedido para cadastrar Plataforma x Sessão", ex);
            }
            return lRetorno;
        }
    }
}