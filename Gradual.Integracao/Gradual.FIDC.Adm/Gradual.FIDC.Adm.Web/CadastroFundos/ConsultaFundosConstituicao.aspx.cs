using System;
using System.Linq;
using Gradual.FIDC.Adm.DbLib.Mensagem;
using Gradual.FIDC.Adm.Web.App_Codigo.Transporte;
using Gradual.OMS.Library;
using Newtonsoft.Json;

namespace Gradual.FIDC.Adm.Web.CadastroFundos
{
    public partial class ConsultaFundosConstituicao : PaginaBase
    {
        #region Propriedades
        public bool GetIsPendente
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Request["Pendentes"]);
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool GetIsConcluido
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Request["Concluidos"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public DateTime? GetDtDe
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(Request["DtDe"]);
                }
                catch
                {
                    return null;
                }
            }
        }
        public DateTime? GetDtAte
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(Request["DtAte"]);
                }
                catch
                {
                    return null;
                }
            }
        }
        public int GetIdFundoCadastro
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoCadastro"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int GetIdFundoFluxoGrupo
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request["IdFundoFluxoGrupo"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public string GetCorpoEmail
        {
            get
            {
                try
                {
                    return Request["CorpoEmail"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string GetDestinatarios
        {
            get
            {
                try
                {
                    return Request["Destinatarios"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region Eventos
        protected new void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    base.Page_Load(sender, e);

                    RegistrarRespostasAjax(new[] {  "CarregarGridFundosConstituicao",
                                                                "CarregarSelectFundosEmConstituicao",
                                                                "CarregarSelectGruposAprovacaoFundosConstituicao",
                                                                "EnviarEmailConsultaFundos",
                                                                "CarregarDadosModalEnvioEmail"
                    },
                         new ResponderAcaoAjaxDelegate[] { 
                                                        ResponderCarregarGridConsulta,
                                                        ResponderCarregarSelectFundosConstituicao,
                                                        ResponderCarregarSelectGruposAprovacaoFundosConstituicao,
                                                        ResponderEnviarEmailConsultaFundos,
                                                        ResponderCarregarDadosModalEnvioEmail
                                                     });

                    CarregarDadosIniciais();
                }
                catch (Exception ex)
                {
                    Logger.Error("Erro ao carregar os dados de fundos na tela", ex);
                }
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carregar dados iniciais da página de carteiras
        /// </summary>
        private void CarregarDadosIniciais()
        {
            try
            {
                if (Page.IsPostBack) return;

                TituloDaPagina = "Consulta de Fundos em Constituição";
                LinkPreSelecionado = "lnkTL_CadastroFundos";
            }
            catch (Exception ex)
            {
                RetornarErroAjax(ex.Message, ex);
            }
        }

        /// <summary>
        /// Carrega dados no grid de fundos
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarGridConsulta()
        {
            string lRetorno = string.Empty;

            try
            {
                var lRequest = new ConsultaFundosConstituicaoRequest
                {
                    SelecionarConcluídos = GetIsConcluido,
                    SelecionarPendentes = GetIsPendente,
                    DtDe = GetDtDe,
                    DtAte = GetDtAte,
                    IdFundoCadastro = GetIdFundoCadastro,
                    IdFundoFluxoGrupo = GetIdFundoFluxoGrupo
                };
                
                ConsultaFundosConstituicaoResponse lResponse = BuscarFundosConsultaFundosConstituicao(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteConsultaFundosConstituicao().TraduzirLista(lResponse.ListaConsultaFundos);

                    TransporteDeListaPaginada lRetornoLista = new TransporteDeListaPaginada(lListaTransporte)
                    {
                        TotalDeItens = lResponse.ListaConsultaFundos.Count,
                        PaginaAtual = 1,
                        TotalDePaginas = 0
                    };


                    lRetorno = JsonConvert.SerializeObject(lRetornoLista);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar grid de consulta de fundos em constituição", ex);

                lRetorno = RetornarErroAjax("Erro no método CarregarGridConsulta ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega select de fundos em constituição para seleção por parte do usuário
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarSelectFundosConstituicao()
        {
            var lRetorno = string.Empty;

            try
            {
                //apenas fundos em constituição - id 4
                var lRequest = new CadastroFundoRequest {IdFundoCadastro = 4};
                
                var lResponse = BuscarFundosCadastradosPorCategoria(lRequest);
                
                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaFundos);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar lista de fundos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectFundos ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Carrega select de grupos de aprovação de fundos em constituição para seleção por parte do usuário
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarSelectGruposAprovacaoFundosConstituicao()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new FundoFluxoGrupoRequest();

                var lResponse = BuscarGruposFluxoAprovacao(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    lRetorno = JsonConvert.SerializeObject(lResponse.ListaGrupos);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar os lista de grupos na tela", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarSelectGruposAprovacaoFundosConstituicao ", ex);
            }

            return lRetorno;
        }

        /// <summary>
        /// Realiza o envio de e-mail da consulta de fundos
        /// </summary>
        /// <returns></returns>
        public string ResponderEnviarEmailConsultaFundos()
        {
            var dadosEmailSplit = GetCorpoEmail.Split(';');
            var destinatarios = GetDestinatarios.Split(',');

            var lDestinatarios = destinatarios.Select(t => t.Trim()).ToList();

            var corpoEmail = "<div style=\"font-family: Calibri; font-style: normal\"><h3>Fundo: " + dadosEmailSplit[0] + "</h3>" +
                        "<div class='row' style='height: 80px; padding-left: 16px; padding-top: 20px'>" +
                        "<div>Status geral: " + dadosEmailSplit[1] + "</div><br/>" +
                        "<div>Última etapa processada: " + dadosEmailSplit[2] + " | Status: " + dadosEmailSplit[3] + "</div></div>" +
                        "<div class='row' style='height: 50px'></div><br/><br/>" +
                        "<div>Obrigado.</div></div>";

            try
            {
                EnviarEmailEtapasAprovacaoFundoConstituicao(lDestinatarios, corpoEmail);

                foreach (var item in lDestinatarios)
                {
                    Logger.Info("Consulta de fundos - E-mail enviado para " + item);
                }

                return RetornarSucessoAjax("OK");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao enviar e-mail", ex);
                return RetornarErroAjax("Erro ao enviar e-mail", ex);
            }
        }

        /// <summary>
        /// Carrega os dados do fundo selecionado no modal de envio de e-mail
        /// </summary>
        /// <returns></returns>
        public string ResponderCarregarDadosModalEnvioEmail()
        {
            var lRetorno = string.Empty;

            try
            {
                var lRequest = new ConsultaFundosConstituicaoRequest {IdFundoCadastro = GetIdFundoCadastro};
                
                var lResponse = BuscarFundosConsultaFundosConstituicaoDadosGeraisUltimaEtapa(lRequest);

                if (lResponse != null && lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    var lListaTransporte = new TransporteConsultaFundosConstituicao().TraduzirLista(lResponse.ListaConsultaFundos);

                    lRetorno = JsonConvert.SerializeObject(lListaTransporte);

                    return lRetorno;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar modal de envio de e-mail", ex);

                lRetorno = RetornarErroAjax("Erro no método ResponderCarregarDadosModalEnvioEmail ", ex);
            }

            return lRetorno;
        }

        #endregion
    }
}