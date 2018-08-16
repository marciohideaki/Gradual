using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.OMS.Termo.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Termo.Lib;
using Gradual.OMS.Termo.Lib.Info;
using Gradual.Intranet.Www.App_Codigo;
using log4net;

namespace Gradual.Intranet.Www.Intranet.Monitoramento
{
    public partial class ResultadoTermos : PaginaBaseAutenticada
    {
        #region Globais

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Métodos Private

        private List<TransporteAcompanhamentoDeTermo> ConverterLista(List<AcompanhamentoOrdemTermoInfo> pLista)
        {
            List<TransporteAcompanhamentoDeTermo> lRetorno = new List<TransporteAcompanhamentoDeTermo>();

            foreach (AcompanhamentoOrdemTermoInfo lTermo in pLista)
            {
                lRetorno.Add(new TransporteAcompanhamentoDeTermo(lTermo));
            }

            return lRetorno;
        }

        private List<TransporteAcompanhamentoDeTermo> ConverterLista(List<AcompanhamentoOrdemTermoConsolidadoInfo> pLista)
        {
            List<TransporteAcompanhamentoDeTermo> lRetorno = new List<TransporteAcompanhamentoDeTermo>();

            foreach (AcompanhamentoOrdemTermoConsolidadoInfo lTermo in pLista)
            {
                lRetorno.AddRange( ConverterLista(lTermo.lstAcompanhamentoOrdemTermo) );
            }

            return lRetorno;
        }

        private List<TransporteAcompanhamentoDeTermo> BuscarListaDeTermos()
        {
            List<TransporteAcompanhamentoDeTermo> lLista = new List<TransporteAcompanhamentoDeTermo>();

            try
            {
                IServicoTermo lServico = Ativador.Get<IServicoTermo>();

                AcompanhamentoConsolidadoOrdemTermoRequest  lRequest = new AcompanhamentoConsolidadoOrdemTermoRequest();
                AcompanhamentoConsolidadoOrdemTermoResponse lResponse;


                lRequest.AcompanhamentoOrdemTermoInfo = new OMS.Termo.Lib.Info.AcompanhamentoOrdemTermoInfo();

                //TODO: Alterar esses IDs na mão
                //lRequest.AcompanhamentoOrdemTermoInfo.IdCliente = 31940;

                lResponse = lServico.AcompanhamentoOrdensConsolidadoSolicitacoes(lRequest);

                if (lResponse.CriticaResposta == OMS.Termo.Lib.Info.StatusRespostaEnum.Sucesso)
                {
                    lLista.AddRange( ConverterLista(lResponse.ListaAcompanhamentoConsolidado) );
                }
                else
                {
                    Logger.ErrorFormat("Resposta com erro de IServicoTermo.ObterAcompanhamentoOrdemTermoSinacor(pIdCliente: [{0}]) > [{1}] [{2}]"
                                        , lRequest.AcompanhamentoOrdemTermoInfo.IdCliente
                                        , lResponse.CriticaResposta
                                        , lResponse.DescricaoResposta);
                }

                lLista = new List<TransporteAcompanhamentoDeTermo>(lLista.OrderByDescending(i => i.DateSolicitacao));
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Erro em AcompanhamentoDeTermo > CarregarTermos() [{0}]\r\n{1}", ex.Message, ex.StackTrace);
            }

            return lLista;
        }

        private void CarregarTermos()
        {
            List<TransporteAcompanhamentoDeTermo> lLista = BuscarListaDeTermos();

            if (lLista.Count > 0)
            {
                List<TransporteAcompanhamentoDeTermo> lListaFiltrada = new List<TransporteAcompanhamentoDeTermo>();

                /*
                 Requests:
                   
                    Acao            CarregarHtml
                    Cliente         123
                    Estado          Novo
                    HoraFinal       18:00
                    HoraInicial     08:00
                    IdAssessor      1
                    IdAssessorDesc  0001 - 1 - PAULO CÉSAR DE LIMA
                    Papel           lala
                    Sentido         N
                    SentidoDesc     Novo
                    Status          S
                    StatusDesc      Solicitado
                 
                 */

                string lCliente = Request["Cliente"];

                string lPapel = Request["Papel"];

                string lStatus = Request["Status"];

                string lSolicitacao = Request["Sentido"];

                string lHoraDe  = Request["HoraInicial"];

                string lHoraAte = Request["HoraFinal"];

                string lIdAssessor = Request["IdAssessor"];

                if (!string.IsNullOrEmpty(lPapel))
                {
                    lPapel = lPapel.ToUpper();

                    if (!lPapel.EndsWith("T"))
                        lPapel += "T";
                }

                bool lAdicionar;

                foreach (TransporteAcompanhamentoDeTermo lTermo in lLista)
                {
                    lAdicionar = true;
                    
                    if (!string.IsNullOrEmpty(lCliente) && (lTermo.IdCliente != lCliente))
                        lAdicionar = false;

                    if (!string.IsNullOrEmpty(lPapel) && (lTermo.Instrumento != lPapel))
                        lAdicionar = false;

                    if (!string.IsNullOrEmpty(lStatus) && (lTermo.StatusOrdemTermo != lStatus))
                        lAdicionar = false;

                    if (!string.IsNullOrEmpty(lSolicitacao) && (lTermo.TipoSolicitacao != lSolicitacao))
                        lAdicionar = false;
                    
                    if (!string.IsNullOrEmpty(lHoraDe) && !string.IsNullOrEmpty(lHoraDe) && !lTermo.DateSolicitacao.EntreHoras(lHoraDe, lHoraAte))
                        lAdicionar = false;
                    
                    if (!string.IsNullOrEmpty(lIdAssessor) && (lTermo.IdAssessor != lIdAssessor))
                        lAdicionar = false;

                    if(lAdicionar)
                        lListaFiltrada.Add(lTermo);
                }

                rptResultadoTermos.DataSource = lListaFiltrada;
                rptResultadoTermos.DataBind();

                rowLinhaDeNenhumItem.Visible = false;
            }
            else
            {
                rowLinhaDeNenhumItem.Visible = true;
            }
        }

        private string ResponderEfetuarTermo()
        {
            string lRetorno = "";

            try
            {
                int lIdTermo   = Convert.ToInt32(Request["IdTermo"]);
                int lIdCliente = Convert.ToInt32(Request["IdCliente"]);

                string lTipo = Request["TipoSolicitacao"];

                IServicoTermo lServico = Ativador.Get<IServicoTermo>();

                OrdemTermoRequest  lRequest = new OrdemTermoRequest();
                OrdemTermoResponse lResponse;

                lRequest.OrdemTermoInfo = new OrdemTermoInfo();

                lRequest.OrdemTermoInfo.IdOrdemTermo = lIdTermo;
                lRequest.OrdemTermoInfo.IdCliente    = lIdCliente;

                lRequest.OrdemTermoInfo.TipoSolicitacao  = (EnumTipoSolicitacao)Enum.Parse(typeof(EnumTipoSolicitacao), lTipo);
                lRequest.OrdemTermoInfo.StatusOrdemTermo = EnumStatusTermo.SolicitacaoExecutada;

                /*
                if (lEnviarRolagem)
                {
                    lRequest.OrdemTermoInfo.TipoSolicitacao   = EnumTipoSolicitacao.Rolagem;

                    lRequest.OrdemTermoInfo.precoDireto = Convert.ToDecimal(RequestPreco);
                    lRequest.OrdemTermoInfo.IdTaxa = RequestIdTaxaTermo;
                }
                else
                {
                    //lRequest.OrdemTermoInfo.TipoSolicitacao = EnumTipoSolicitacao.;

                    lRequest.OrdemTermoInfo.PrecoLimite = Convert.ToDecimal(RequestPreco);
                }
                */

                lResponse = lServico.AlterarStatusSolicitacaoTermo(lRequest);

                if (lResponse.CriticaResposta == OMS.Termo.Lib.Info.StatusRespostaEnum.Sucesso)
                {
                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    gLogger.ErrorFormat("Resposta com erro de IServicoTermo.AlterarStatusSolicitacaoTermo(IdCliente: [{0}], IdTermo: [{1}]) [{4}] > [{5}]"
                                        , lRequest.OrdemTermoInfo.IdCliente
                                        , lRequest.OrdemTermoInfo.IdOrdemTermo
                                        , lResponse.CriticaResposta
                                        , lResponse.DescricaoResposta);

                    lRetorno = RetornarErroAjax("Erro do serviço de termo: [{0}] [{1}]", lResponse.CriticaResposta, lResponse.DescricaoResposta);
                }

                //lRetorno = RetornarSucessoAjax("ok");
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao efetuar termo: [{0}], [{1}]", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao efetuar termo", ex);
            }

            return lRetorno;
        }

        private string ResponderCancelarTermo()
        {
            string lRetorno = "";
            
            try
            {
                IServicoTermo lServico = Ativador.Get<IServicoTermo>();

                OrdemTermoRequest  lRequest = new OrdemTermoRequest();
                OrdemTermoResponse lResponse;

                lRequest.OrdemTermoInfo = new OrdemTermoInfo();
                
                lRequest.OrdemTermoInfo.IdCliente    = Convert.ToInt32(Request["IdCliente"]);
                lRequest.OrdemTermoInfo.IdOrdemTermo = Convert.ToInt32(Request["IdTermo"]);

                lRequest.OrdemTermoInfo.StatusOrdemTermo = EnumStatusTermo.SolicitacaoExecutada;
                lRequest.OrdemTermoInfo.TipoSolicitacao  = EnumTipoSolicitacao.Cancelamento;

                lResponse = lServico.AlterarStatusSolicitacaoTermo(lRequest);

                if (lResponse.bSucesso)
                {
                    lRetorno = RetornarSucessoAjax("ok");
                }
                else
                {
                    gLogger.ErrorFormat("Erro ao cancelar termo: IdCliente: [{0}] IdOrdemTermo: [{1}] TipoSolicitacao: [{2}] > [{3}]"
                                        , lRequest.OrdemTermoInfo.IdCliente
                                        , lRequest.OrdemTermoInfo.IdOrdemTermo
                                        , lRequest.OrdemTermoInfo.TipoSolicitacao
                                        , lResponse.DescricaoResposta);

                    lRetorno = RetornarErroAjax("Erro ao cancelar termo", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao registrar termo: [{0}], [{1}]", ex.Message, ex.StackTrace);

                lRetorno = RetornarErroAjax("Erro ao registrar termo", ex);
            }

            return lRetorno;
        }

        private void ResponderPopupTermo()
        {
            Response.Clear();

            Response.Write("<html> <body style='background:#46576b; text-align:center; padding-top:10%'> <h1 style='font-family:Trebuchet MS; color:#e2e7ed; font-weight:normal'>Nova solicitação de Termo!</h1> </body> </html>");

            Response.End();
        }

        #endregion

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            string lAcao = Request["Acao"];

            if (Request["msg"] == "termo")
            {
                //função pra retornar um HTML simples pra exibir na popup de alerta de novo termo
                ResponderPopupTermo();

                return;
            }

            if (string.IsNullOrEmpty(lAcao) || lAcao == "CarregarHtml")
            {
                CarregarTermos();
            }
            else
            {
                RegistrarRespostasAjax(new string[] {
                                                        "EfetuarTermo",
                                                        "CancelarTermo"
                                                    },
                    new ResponderAcaoAjaxDelegate[] { 
                                                        ResponderEfetuarTermo,
                                                        ResponderCancelarTermo
                                                    });
            }
        }

        #endregion

    }
}