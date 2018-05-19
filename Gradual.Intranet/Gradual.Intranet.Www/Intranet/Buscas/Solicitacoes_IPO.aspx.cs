using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Vendas;
using Newtonsoft.Json;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Buscas
{
    public partial class Solicitacoes_IPO : PaginaBaseAutenticada
    {
        #region Properties
        private List<TransporteDadosIPOCliente> SessionUltimoResultadoDeBusca
        {
            get
            {
                return (List<TransporteDadosIPOCliente>)Session["UltimoResultadoDeGerenciamentoIPO"];
            }

            set
            {
                Session["UltimoResultadoDeGerenciamentoIPO"] = value;
            }
        }
        #endregion

        #region Events
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

        #region Methods
        private string ResponderBuscarItensParaSelecao()
        {
            string lRetorno = "Erro...";

            ConsultarEntidadeCadastroRequest<IPOClienteInfo> lRequest = new ConsultarEntidadeCadastroRequest<IPOClienteInfo>();
            ConsultarEntidadeCadastroResponse<IPOClienteInfo> lResponse;

            lRequest.EntidadeCadastro = new IPOClienteInfo();

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
                            lRequest.EntidadeCadastro.CodigoCliente= lCblc;
                        }
                    }
                    else
                    {
                        lRequest.EntidadeCadastro.CpfCnpj = lTermoDeBusca;
                    }
                }

                if (DateTime.TryParse(Request.Form["DataInicial"], out lData)) lRequest.EntidadeCadastro.DataDe = lData;

                if (DateTime.TryParse(Request.Form["DataFinal"], out lData))
                {
                    lRequest.EntidadeCadastro.DataAte = new DateTime(lData.Year, lData.Month, lData.Day, 23, 59, 59);
                }

                string lStatus = Request.Form["Status"];

                if (lStatus != "0")
                {
                    lRequest.EntidadeCadastro.Status = (eStatusIPO)Enum.Parse(typeof(eStatusIPO), lStatus);
                }
                else
                {
                    lRequest.EntidadeCadastro.Status = eStatusIPO.Nenhum;
                }
            }
            catch { }

            //lRequest.EntidadeCadastro = lDadosDeBusca;

            try
            {
                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<IPOClienteInfo>(lRequest);

                var lResultados = new List<TransporteDadosIPOCliente>();

                if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
                {
                    // a view vem com left join da base, então conforme existem vários pagamentos a venda vem repetida.

                    //inclui no resultado somente as únicas:

                    foreach (IPOClienteInfo lIPOCliente in lResponse.Resultado)
                    {
                        lResultados.Add(new TransporteDadosIPOCliente(lIPOCliente));
                    }

                    this.SessionUltimoResultadoDeBusca = lResultados;

                    TransporteDeListaPaginada lListaPaginada = BuscarPaginaDeResultados(1);

                    lRetorno = RetornarSucessoAjax(lListaPaginada, "Encontradas [{0}] Reservas", lResultados.Count);

                    base.RegistrarLogConsulta("Consulta de Dados de Gerencimaneto de IPO ");
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

        /// <summary>
        /// Método de paginação jquery
        /// </summary>
        /// <returns>Retorna uma string json com parte da paginação.</returns>
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

        /// <summary>
        /// Método que solicita a paginação na sessão
        /// </summary>
        /// <param name="pPagina">Págian a ser retornada da lista ena sessão</param>
        /// <returns>Retorna a lista paginada</returns>
        private TransporteDeListaPaginada BuscarPaginaDeResultados(int pPagina)
        {
            var lRetorno = new TransporteDeListaPaginada();

            var lLista = new List<TransporteDadosIPOCliente>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = ((pPagina - 1) * TransporteDeListaPaginada.ItensPorPagina);
            lIndiceFinal = (pPagina) * TransporteDeListaPaginada.ItensPorPagina;

            for (int a = lIndiceInicial; a < lIndiceFinal; a++)
            {
                if (a < this.SessionUltimoResultadoDeBusca.Count)
                {
                    //lLista.Add(new TransporteDadosResumidosCliente(this.SessionUltimoResultadoDeBusca[a]));

                    lLista.Add(this.SessionUltimoResultadoDeBusca[a]);
                }
            }

            lRetorno = new TransporteDeListaPaginada(lLista);

            lRetorno.TotalDeItens = this.SessionUltimoResultadoDeBusca.Count;
            lRetorno.TotalDePaginas = Convert.ToInt32(Math.Ceiling((double)lRetorno.TotalDeItens / (double)TransporteDeListaPaginada.ItensPorPagina));
            lRetorno.PaginaAtual = pPagina;

            return lRetorno;
        }
        #endregion
    }
}