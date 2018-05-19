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
    public partial class GerenciadorPlataformas : PaginaBase
    {
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

        public string ResponderListar()
        {
            string lRetorno = string.Empty;

            var lLista = base.ListarGerenciadorPlataforma(new GerenciadorPlataformaInfo());

            var lListaPlataforma = new Transporte_GerenciadorPlataforma().TraduzirLista(lLista);

            lRetorno = RetornarSucessoAjax(lListaPlataforma, "OK");

            return lRetorno;
        }

        public void CarregarCombos()
        {
            this.rptPlataformas.DataSource = base.ListarPlataforma(new Lib.Dados.PlataformaSessaoInfo());
            this.rptPlataformas.DataBind();

            var listaSessao = base.ListarSessao(new Lib.Dados.PlataformaSessaoInfo());


            var lOperadores = base.ListarUsuarioOperadores();
            var lAssessor = base.ListarAssessorComplemento(new AssessorInfo());

            this.rptOperador.DataSource = new Transporte_Operadores().TraduzirLista(lOperadores.Usuarios, lAssessor);
            this.rptOperador.DataBind();

            this.rptSessoes.DataSource = listaSessao;
            this.rptSessoes.DataBind();


            /*
            this.rptCliente.DataSource=;
            this.rptCliente.DataBind();

            this.rptAssessor.DataSource =;
            this.rptAssessor.DataBind();
             * */
        }

        public string ResponderSelecionar()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new GerenciadorPlataformaInfo();

                lRequest.CodigoTrader = int.Parse(Request["CodigoTrader"]);

                var lSelecao = SelecionarGerenciadorPlataforma(lRequest);

                if (lSelecao != null)
                {
                    var lSelecaoPlataforma = new Transporte_GerenciadorPlataforma().TraduzirLista(lSelecao);

                    Session["Gerenciador_sessao_antes_" + lRequest.CodigoTrader] = lSelecaoPlataforma;
                    
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

            try
            {
                string lAntes = string.Empty;
                
                string lDepois = string.Empty;

                string lCodigoTrader = string.Empty;

                Transporte_GerenciadorPlataforma lDadosPlataforma = JsonConvert.DeserializeObject<Transporte_GerenciadorPlataforma>(lObjetoJson);

                lCodigoTrader = lDadosPlataforma.CodigoTrader;

                var ListaPlataformaSessao = lDadosPlataforma.ToPlataformaSessaoInfo(lDadosPlataforma);

                var lResponse = base.InserirGerenciadorPlataforma(ListaPlataformaSessao);

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    if (Session["Gerenciador_sessao_antes_" + lCodigoTrader] != null)
                    {
                        var lTransporte = Session["Gerenciador_sessao_antes_" + lCodigoTrader] ;

                        var lSerialize = new Transporte_GerenciadorPlataforma().TraduzirLog((List<Transporte_GerenciadorPlataforma>)lTransporte);

                        string lAntesSerializado =  JsonConvert.SerializeObject(lSerialize);

                        lAntes = "Antes -->> " + lAntesSerializado;
                        lDepois = "Depois -->> " + lObjetoJson;
                    }
                    else
                    {
                        lDepois = "Novo -->> " + lObjetoJson;
                    }

                    base.RegistrarLogInclusao(string.Concat(lAntes,lDepois));

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