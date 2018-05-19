using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Newtonsoft.Json;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Solicitacoes_VendasDeFerramentas : PaginaBaseAutenticada
    {
        #region Propriedades

        private List<TransporteVendaDeFerramentaInfo> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<TransporteVendaDeFerramentaInfo>)Session["UltimoResultadoDeBuscaDeVendas"];
            }

            set
            {
                Session["UltimoResultadoDeBuscaDeVendas"] = value;
            }
        }

        #endregion

        #region Métodos Private

        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            TransporteDeListaPaginada lRetorno = new TransporteDeListaPaginada();

            List<TransporteVendaDeFerramentaInfo> lLista = new List<TransporteVendaDeFerramentaInfo>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    //lLista.Add(new TransporteDadosResumidosCliente(this.SessionUltimoResultadoDeBusca[a]));

                    lLista.Add( this.SessionUltimoResultadoDeBusca[a]);
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }

        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = "Erro...";

            ConsultarEntidadeCadastroRequest<VendaDeFerramentaInfo>  lRequest = new ConsultarEntidadeCadastroRequest<VendaDeFerramentaInfo>();
            ConsultarEntidadeCadastroResponse<VendaDeFerramentaInfo> lResponse;

            VendaDeFerramentaInfo lDadosDeBusca = new VendaDeFerramentaInfo();

            string lTermoDeBusca, lBuscarPor;

            int lCblc;

            DateTime lData;

            lTermoDeBusca = Request.Form["TermoDeBusca"];
            lBuscarPor    = Request.Form["BuscarPor"];

            try
            {
                if (!string.IsNullOrEmpty(lBuscarPor))
                {
                    if (lBuscarPor.ToLower() == "codbovespa")
                    {
                        if (int.TryParse(lTermoDeBusca, out lCblc))
                        {
                            lDadosDeBusca.Busca_CdCBLC = lCblc;
                        }
                    }
                    else
                    {
                        lDadosDeBusca.Busca_DsCpfCnpj = lTermoDeBusca;
                    }
                }

                if(DateTime.TryParse(Request.Form["DataInicial"], out lData)) lDadosDeBusca.Busca_DataDe = lData;

                if (DateTime.TryParse(Request.Form["DataFinal"], out lData))
                {
                    lDadosDeBusca.Busca_DataAte = new DateTime(lData.Year, lData.Month, lData.Day, 23, 59, 59);
                }

                if (int.TryParse(Request.Form["Status"], out lCblc)) lDadosDeBusca.Busca_StStatus = lCblc;
            }
            catch { }

            lRequest.EntidadeCadastro = lDadosDeBusca;

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<VendaDeFerramentaInfo>(lRequest);

                List<TransporteVendaDeFerramentaInfo> lResultados = new List<TransporteVendaDeFerramentaInfo>();

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    // a view vem com left join da base, então conforme existem vários pagamentos a venda vem repetida.

                    //inclui no resultado somente as únicas:

                    foreach (VendaDeFerramentaInfo lVenda in lResponse.Resultado)
                    {
                        if (lResultados.Find(i => i.Id == lVenda.IdVenda.ToString()) == null)
                        {
                            lResultados.Add(new TransporteVendaDeFerramentaInfo(lVenda));
                        }
                    }

                    this.SessionUltimoResultadoDeBusca = lResultados;

                    TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                    lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontradas [{0}] vendas", lResultados.Count);
                }
                else
                {
                    lRetorno = RetornarErroAjax("Erro durante a busca.", string.Format("{0}\r\n\r\n{1}\r\n\r\n{2}", lResponse.StatusResposta, lResponse.StatusResposta, lResponse.DescricaoResposta));
                }

            }
            catch (Exception exBusca)
            {
                RetornarErroAjax("Erro durante a busca", exBusca);
            }

            return lRetorno;
        }

        private string ResponderPaginar()
        {
            string lRetorno = string.Empty;

            TransporteDeListaPaginada lLista = new TransporteDeListaPaginada();

            //_search=false&nd=1275415807834&rows=10&page=2&sidx=invid&sord=desc

            if (this.SessionUltimoResultadoDeBusca != null)
            {
                int lPagina;

                if (int.TryParse(Request["page"], out lPagina))
                {
                    lLista = BuscarPaginaDeResultados(lPagina);

                }
            }
            else
            {
                //lLista ;
            }

            lRetorno = JsonConvert.SerializeObject(lLista); //o grid espera o objeto direto, sem estar encapsulado

            return lRetorno;
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                    "BuscarItensParaSelecao"
                                                  , "Paginar"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderBuscarItensParaSelecao
                                                  , ResponderPaginar
                                                });
        }

        #endregion
    }
}