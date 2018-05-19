using System;
using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Cotacao.Lib;
using Gradual.OMS.Custodia.Lib;
using Gradual.OMS.Custodia.Lib.Info;
using Gradual.OMS.Custodia.Lib.Mensageria;
using log4net;
using Gradual.OMS.RelatoriosFinanc.Lib;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;

namespace Gradual.Intranet.Www.Intranet.Clientes.Formularios.Dados
{
    public partial class Custodia : PaginaBaseAutenticada
    {
        #region | Atributos

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Métodos privados

        private List<TransporteCustodiaInfo> BuscarCustodiasDoCliente(int CodBovespa)
        {
            var lRetorno = new List<TransporteCustodiaInfo>();

            var lServico = this.InstanciarServico<IServicoRelatoriosFinanceiros>();

            var lResponse= lServico.ConsultarCustodia(new PosicaoCustodiaRequest()
            {
                ConsultaCdClienteBovespa = CodBovespa
            });

            if (lResponse.StatusResposta == OMS.Library.MensagemResponseStatusEnum.OK)
            {
                lRetorno = TransporteCustodiaInfo.TraduzirCustodiaInfo(lResponse.Objeto.ListaMovimento);

                RecuperarValoresUltimaCotacao(ref lRetorno);
            }
            else
            {
                throw new Exception(string.Format("Erro do serviço ao obter custódias: {0} {1}", lResponse.StatusResposta, lResponse.DescricaoResposta));
            }

            return lRetorno;
        }

        private string PopularCustodia()
        {
            int CodBMF, CodBovespa = 0;

            int.TryParse(Request["CodBMF"], out CodBMF);
            int.TryParse(Request["CodBovespa"], out CodBovespa);

            List<TransporteCustodiaInfo> lCustodias;

            lCustodias = BuscarCustodiasDoCliente(CodBovespa);

            rptCustodia.DataSource = lCustodias;
            rptCustodia.DataBind();

            return string.Empty;
        }

        //private string RepopularCustodia()
        //{
        //    string lResposta;

        //    List<TransporteCustodiaInfo> lCustodias;

        //    try
        //    {
        //        lCustodias = BuscarCustodiasDoCliente();

        //        lResposta = RetornarSucessoAjax(lCustodias, "{0} papéis em custódia encontrados", lCustodias.Count);
        //    }
        //    catch (Exception ex)
        //    {
        //        lResposta = RetornarErroAjax("Erro ao buscar custódias.", ex);
        //    }

        //    return lResposta;
        //}

        #endregion

        #region | Métodos de apoio

        private void RecuperarValoresUltimaCotacao(ref List<TransporteCustodiaInfo> pListaCustodias)
        {
            if (pListaCustodias != null)
            {
                Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio lCotacao;

                IServicoCotacao lServico = this.InstanciarServico<IServicoCotacao>();

                foreach (TransporteCustodiaInfo lTransporteCustodia in pListaCustodias)
                {
                    try
                    {
                        lCotacao = new Gradual.Intranet.Www.App_Codigo.TransporteJson.TransporteMensagemDeNegocio(lServico.ReceberTickerCotacao(lTransporteCustodia.CodigoNegocio));

                        lTransporteCustodia.ValorDeFechamento = lCotacao.ValorFechamento;

                        lTransporteCustodia.Cotacao = Convert.ToDecimal(lCotacao.Preco).ToString("n");

                        lTransporteCustodia.Variacao = lCotacao.Variacao;

                        try
                        {
                            lTransporteCustodia.Resultado = ((double)double.Parse(lCotacao.Preco) * double.Parse(lCotacao.Quantidade)).ToString("n");
                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        gLogger.Error("Erro em RecuperarValoresUltimaCotacao() na página Custodia.aspx", ex);
                    }
                }
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            RegistrarRespostasAjax(new string[] { "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[]{  
                                                  PopularCustodia
                                               });
        }

        #endregion
    }
}