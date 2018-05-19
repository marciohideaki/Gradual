using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using Gradual.OMS.Contratos.Automacao.Ordens;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Contexto;
using Gradual.OMS.Library;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Contratos.Ordens.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Sistemas.Automacao.Persistencia;
using Gradual.OMS.Contratos.Ordens.Dados;



namespace Gradual.OMS.Sistemas.Automacao.Ordens
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoAutomacaoOrdens : IServicoAutomacaoOrdens
    {
        private MDSEventFactory EventMds = new MDSEventFactory();
        private List<string> Eventos = new List<string>();
        private AutomacaoOrdensDados _AutomacaoOrdensDados;
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        /// <summary>
        /// Objeto para recebimento de mensagens do servidor
        /// </summary>
        private CallbackEvento _CallbackEvento = null;

        /// <summary>
        /// Informacoes da sessao aberta com o servico de ordens servidor
        /// </summary>
        private SessaoOrdensInfo _SessaoInfo { get; set; }

        /// <summary>
        /// Referencia ao servico de ordens servidor
        /// </summary>
        private IServicoOrdensServidor _ServicoOrdensServidor { get; set; }

        public ServicoAutomacaoOrdens()
        {
            try
            {
                _AutomacaoOrdensDados = new AutomacaoOrdensDados();
                _AutomacaoOrdensDados.ExcluirUsuarioLogado(4444, 3);

                
                // Cria informações da sessão com o servidor de ordens
                this._SessaoInfo = new SessaoOrdensInfo() { CodigoSessao = Guid.NewGuid().ToString() };

                // Cria o callback para o serviço de ordens
                this._CallbackEvento = new CallbackEvento();

                // Registra o evento para tratamento das mensagens
                this._CallbackEvento.Evento += new EventHandler<EventoEventArgs>(_CallbackEvento_Evento);

                // Faz a assinatura no servidor de ordens
                this._ServicoOrdensServidor = Ativador.Get<IServicoOrdensServidor>(this._CallbackEvento, this._SessaoInfo);

                Event._MDSAuthenticationResponse +=
                    new Event._onMDSAuthenticationResponse(Event__MDSAuthenticationResponse);

                OMSEventHandlerClass omsEHC = new OMSEventHandlerClass(EventMds);

                EventMds.OnMDSStopStartEvent +=
                    new MDSEventFactory._OnMDSStopStartEvent(EventMds_OnMDSStopStartEvent);

                EventMds.OnMDSSRespostaAutenticacaoEvent +=
                    new MDSEventFactory._OnMDSSRespostaAutenticacaoEvent(EventMds_OnMDSSRespostaAutenticacaoEvent);

                EventMds.OnMDSSRespostaCancelamentoEvent +=
                    new MDSEventFactory._OnMDSSRespostaCancelamentoEvent(EventMds_OnMDSSRespostaCancelamentoEvent);

                Registrador.AddListener(EventMds);

                EnviarMensagemAutenticacao("4444", "3");

                //RetomarOrdensPendentes();
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
            }
        }

        private void RetomarOrdensPendentes()
        {
            ListarItensAutomacaoOrdemRequest reqPrincipal = new ListarItensAutomacaoOrdemRequest()
            {
                StatusAutomacaoOrdem = ItemAutomacaoStatusEnum.AceitoPeloMDS
            };

            ListarItensAutomacaoOrdemResponse res = ListarItemsAutomacaoOrdem(reqPrincipal);

            foreach (ItemAutomacaoOrdemInfo item in res.ListaDeAutomacaoOrdens)
            {
                switch (item.AutomacaoInfo.AutomacaoTipo)
                {
                    case ItemAutomacaoTipoEnum.StartCompra:
                        ArmarStopSimplesRequest reqStart = new ArmarStopSimplesRequest()
                        {
                            Instrumento = item.AutomacaoInfo.Instrumento,
                            IdStopStart = item.CodigoItemAutomacaoOrdem,
                            StopStartTipo = (int)item.AutomacaoInfo.AutomacaoTipo,
                            PrecoStart = item.AutomacaoInfo.PrecosTaxas[0].PrecoGatilho,
                            PrecoGain = 0,
                            PrecoLoss = 0
                        };
                        ArmarStopSimplesResponse resStart = Formatador.ArmarStopSimples(reqStart);
                        if (resStart.StatusResposta == MensagemResponseStatusEnum.ErroPrograma)
                        {
                            Log.EfetuarLog(String.Format("Erro ao reassinar Start/Stop com MDS para a ordem: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        else
                        {
                            Registrador.AddEvent(EventMds, Eventos);
                            Log.EfetuarLog(string.Format("Ordem Start/Stop Retomada: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        break;
                    case ItemAutomacaoTipoEnum.StopGain:
                        ArmarStopSimplesRequest reqGain = new ArmarStopSimplesRequest()
                        {
                            Instrumento = item.AutomacaoInfo.Instrumento,
                            IdStopStart = item.CodigoItemAutomacaoOrdem,
                            StopStartTipo = (int)item.AutomacaoInfo.AutomacaoTipo,
                            PrecoStart = 0,
                            PrecoGain = item.AutomacaoInfo.PrecosTaxas[0].PrecoGatilho,
                            PrecoLoss = 0
                        };
                        ArmarStopSimplesResponse resGain = Formatador.ArmarStopSimples(reqGain);
                        if (resGain.StatusResposta == MensagemResponseStatusEnum.ErroPrograma)
                        {
                            Log.EfetuarLog(String.Format("Erro ao reassinar Start/Stop com MDS para a ordem: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        else
                        {
                            Registrador.AddEvent(EventMds, Eventos);
                            Log.EfetuarLog(string.Format("Ordem Start/Stop Retomada: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        break;
                    case ItemAutomacaoTipoEnum.StopLoss:
                        ArmarStopSimplesRequest reqLoss = new ArmarStopSimplesRequest()
                        {
                            Instrumento = item.AutomacaoInfo.Instrumento,
                            IdStopStart = item.CodigoItemAutomacaoOrdem,
                            StopStartTipo = (int)item.AutomacaoInfo.AutomacaoTipo,
                            PrecoStart = 0,
                            PrecoGain = 0,
                            PrecoLoss = item.AutomacaoInfo.PrecosTaxas[0].PrecoGatilho
                        };
                        ArmarStopSimplesResponse resLoss = Formatador.ArmarStopSimples(reqLoss);
                        if (resLoss.StatusResposta == MensagemResponseStatusEnum.ErroPrograma)
                        {
                            Log.EfetuarLog(String.Format("Erro ao reassinar Start/Stop com MDS para a ordem: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        else
                        {
                            Registrador.AddEvent(EventMds, Eventos);
                            Log.EfetuarLog(string.Format("Ordem Start/Stop Retomada: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        break;
                    case ItemAutomacaoTipoEnum.StopSimultaneo:
                        AutomacaoPrecosTaxasInfo precoLoss =
                            (from itemPrecos in item.AutomacaoInfo.PrecosTaxas
                             where itemPrecos.AutomacaoTipo == ItemAutomacaoTipoEnum.StopLoss
                             select itemPrecos).FirstOrDefault();

                        AutomacaoPrecosTaxasInfo precoGain =
                            (from itemPrecos in item.AutomacaoInfo.PrecosTaxas
                             where itemPrecos.AutomacaoTipo == ItemAutomacaoTipoEnum.StopGain
                             select itemPrecos).FirstOrDefault();

                        ArmarStopSimplesRequest reqSimul = new ArmarStopSimplesRequest()
                        {
                            Instrumento = item.AutomacaoInfo.Instrumento,
                            IdStopStart = item.CodigoItemAutomacaoOrdem,
                            StopStartTipo = (int)item.AutomacaoInfo.AutomacaoTipo,
                            PrecoStart = 0,
                        };

                        reqSimul.PrecoGain = precoGain.PrecoGatilho;
                        reqSimul.PrecoLoss = precoLoss.PrecoGatilho;

                        ArmarStopSimplesResponse resSimul = Formatador.ArmarStopSimples(reqSimul);
                        if (resSimul.StatusResposta == MensagemResponseStatusEnum.ErroPrograma)
                        {
                            Log.EfetuarLog(String.Format("Erro ao reassinar Start/Stop com MDS para a ordem: {0}", item.CodigoItemAutomacaoOrdem), LogTipoEnum.Aviso);
                        }
                        else
                        {
                            Registrador.AddEvent(EventMds, Eventos);
                            Log.EfetuarLog(string.Format("Ordem Start/Stop Retomada: {0} - {1}", item.CodigoItemAutomacaoOrdem, item.AutomacaoInfo.AutomacaoTipo.ToString()), LogTipoEnum.Aviso);
                        }
                        break;
                }
            }
        }

        void _CallbackEvento_Evento(object sender, EventoEventArgs e)
        {
            Log.EfetuarLog(e.EventoInfo.Nome, LogTipoEnum.Aviso);
            //foreach (string item in e.EventoInfo.ParametrosNome)
            //{
            //    Log.EfetuarLog(item, LogTipoEnum.Aviso);
            //}
        }

        void EventMds_OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                CR_CancelamentoStopResposta _CR_CancelamentoStopResposta
                    = (CR_CancelamentoStopResposta)(sender);

                string id_stopstart = _CR_CancelamentoStopResposta.pStrIdStopStart;
                int id_status = int.Parse(_CR_CancelamentoStopResposta.pStrStatus);

                if (id_status == 1)
                {
                    _AutomacaoOrdensDados.CancelaOrdemStopStart(id_stopstart, (int)ItemAutomacaoStatusEnum.CanceladoPeloMDS);
                    Log.EfetuarLog(string.Format("Id_StopStart: {0} - Status de retorno: {1}", "Start/Stop cancelado com sucesso. ", id_stopstart, id_status), LogTipoEnum.Aviso);
                }
                else
                {
                    Log.EfetuarLog(string.Format("Id_StopStart: {0} - Status de retorno: {1}", "Start/Stop Não foi cancelado. ", id_stopstart, id_status), LogTipoEnum.Aviso);
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "EventMds_OnMDSSRespostaCancelamentoEvent: ", ex.Message));
            }
        }

        void EventMds_OnMDSSRespostaAutenticacaoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                RS_RespostaStop _RS_RespostaStop
                    = (RS_RespostaStop)(sender);

                string id_stopstart = _RS_RespostaStop.pStrIdStopstart;
                int id_status = int.Parse(_RS_RespostaStop.pStrStatus);

                if (id_status == 0)
                { // Stop Rejeitado

                    // Atualiza status da ordem para rejeitado.
                    AtualizaOrdemStartStopResponse res = this.AtualizaOrdemStartStop(new AtualizaOrdemStartStopRequest()
                    {
                        IdStartStop = id_stopstart,
                        IdStopStartStatus = (int)ItemAutomacaoStatusEnum.RejeitadoPeloMDS
                    });

                    Log.EfetuarLog(string.Format("{0} - {1} - {2}{3}", id_stopstart, id_status.ToString(),  "\r\n " + _RS_RespostaStop.pStrIdStopstart, " Stop rejeitado pelo MDS"), LogTipoEnum.Aviso);

                }
                else
                {
                    AtualizaOrdemStartStopResponse res1 = this.AtualizaOrdemStartStop(new AtualizaOrdemStartStopRequest()
                    {
                        IdStartStop = id_stopstart,
                        IdStopStartStatus = (int)ItemAutomacaoStatusEnum.AceitoPeloMDS
                    });

                    Log.EfetuarLog(string.Format("{0}{1}", "\r\n " + _RS_RespostaStop.pStrIdStopstart, " Stop aceito pelo MDS"), LogTipoEnum.Aviso);
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "EventMds_OnMDSSRespostaAutenticacaoEvent: ", ex.Message));
            }
        }

        void EventMds_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        {
            try
            {
                // Adiciona uma Thread no pool responsavel por executar o processamento do Stop.
                ThreadPool.QueueUserWorkItem(new WaitCallback(

                                        delegate(object required)
                                        {
                                            ProcessarEventoMDS(
                                                (object)(sender)
                                                );
                                        }));
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
            }
        }

        void Event__MDSAuthenticationResponse(object Response, System.Net.Sockets.Socket _ClientSocket)
        {
            try
            {
                if (Response.ToString().Trim() != string.Empty)
                {
                    switch (int.Parse(Response.ToString().Trim()))
                    {
                        case 0:
                            Log.EfetuarLog("Usuário Logado no sistema ", LogTipoEnum.Aviso);
                            break;
                        case 1:
                            Log.EfetuarLog("Cliente autenticado com sucesso ", LogTipoEnum.Aviso);
                            Contexto.SocketPrincipal = _ClientSocket;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "Event__MDSAuthenticationResponse: ", ex.Message));
            }
            _manualResetEvent.Set();

        }



        #region IServicoOrdens Members

        public AtualizaOrdemStartStopResponse AtualizaOrdemStartStop(AtualizaOrdemStartStopRequest req)
        {
            // Prepara resposta
            AtualizaOrdemStartStopResponse res = null;

            // Bloco de controle
            try
            {
                // Lógica da função
                res = _AutomacaoOrdensDados.AtualizaOrdemStop(req);
            }
            catch (Exception ex)
            {
                // Log
                Log.EfetuarLog(ex, req);

                // Cria mensagem padrão de resposta contendo informações do erro
                res = 
                    new AtualizaOrdemStartStopResponse() 
                    { 
                        CodigoMensagemRequest = req.CodigoMensagem,
                        DescricaoResposta = ex.ToString(),
                        StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                    };
            }
            
            // Retorna
            return res;
        }


        /// <summary>
        /// Cancela um ordem que esta em aberto no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        public CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req)
        {
            CancelarStartStopOrdensResponse res = new CancelarStartStopOrdensResponse();
            try
            {
                //_AutomacaoOrdensDados.CancelaOrdemStopStart(req.IdStopStart, req.IdStopStartStatus);
                Formatador.CancelarOrdemStop(req.Instrument, req.IdStopStart);
                AtualizaOrdemStartStopResponse r = AtualizaOrdemStartStop(new AtualizaOrdemStartStopRequest()
                {
                    IdStartStop = req.IdStopStart,
                    IdStopStartStatus =  (int)ItemAutomacaoStatusEnum.CancelamentoEnviado
                });

                
                res.StatusResposta = MensagemResponseStatusEnum.OK;
                res.CodigoItemAutomacaoOrdem = req.IdStopStart;
                res.DescricaoResposta = "Ordem Start/Stop Cancelamento enviado com sucesso.";
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res =
                    new CancelarStartStopOrdensResponse()
                    {
                        CodigoMensagemRequest = req.CodigoMensagem,
                        DescricaoResposta = ex.ToString(),
                        StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                    };
            }
            return res;
        }

        public SelecionarAutomacaoOrdemResponse SelecionarAutomacaoOrdem(SelecionarAutomacaoOrdemRequest req)
        {
            SelecionarAutomacaoOrdemResponse res = null;
            try
            {

                res = new SelecionarAutomacaoOrdemResponse();
                ListarItensAutomacaoOrdemRequest req2 = new ListarItensAutomacaoOrdemRequest()
                {
                    CodigoItemAutomacaoOrdem = req.IdStopStart
                };

                ListarItensAutomacaoOrdemResponse res2 = _AutomacaoOrdensDados.ListarOrdensStartStop(req2);
                if (res2.ListaDeAutomacaoOrdens.Count > 0)
                    res.AutomacaoOrdem = res2.ListaDeAutomacaoOrdens[0];
            }
            catch(Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res =
                    new SelecionarAutomacaoOrdemResponse()
                    {
                        CodigoMensagemRequest = req.CodigoMensagem,
                        DescricaoResposta = ex.ToString(),
                        StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                    };
            }
            return res;
        }

        public ListarItensAutomacaoOrdemResponse ListarItemsAutomacaoOrdem(ListarItensAutomacaoOrdemRequest req)
        {
            ListarItensAutomacaoOrdemResponse res = null;
            try
            {
                res = _AutomacaoOrdensDados.ListarOrdensStartStop(req);
                foreach (ItemAutomacaoOrdemInfo ordemInfo in res.ListaDeAutomacaoOrdens)
                {
                    ordemInfo.Historico = _AutomacaoOrdensDados.ListarHistorico(ordemInfo.CodigoItemAutomacaoOrdem);
                }
            }
            catch(Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res =
                    new ListarItensAutomacaoOrdemResponse()
                    {
                        CodigoMensagemRequest = req.CodigoMensagem,
                        DescricaoResposta = ex.ToString(),
                        StatusResposta = MensagemResponseStatusEnum.ErroPrograma
                    };
            }
            return res;
        }

        /// <summary>
        /// Gera uma automação de Start de Compra, Stop, Loss, Movel e Simultaneo.
        /// </summary>
        /// <param name="req">Parametros de Entrada.</param>
        /// <returns></returns>
        public ExecutarAutomacaoOrdemResponse ExecutarAutomacaoOrdem(ExecutarAutomacaoOrdemRequest req)
        {
            ExecutarAutomacaoOrdemResponse res = new ExecutarAutomacaoOrdemResponse();
            try
            {
                AutomacaoOrdensInfo _TOrder = new AutomacaoOrdensInfo();

                _TOrder.IdBolsa = req.CodigoBolsa == "BOVESPA" ? 1 : 2;
                _TOrder.IdCliente = int.Parse(req.CodigoCliente);
                _TOrder.IdStopstartStatus = (int)ItemAutomacaoStatusEnum.RegistradoNaAplicacao;
                _TOrder.IdStopStartTipo = (int)req.AutomacaoTipo;
                _TOrder.Instrumento = req.Instrumento;
                
                _TOrder.DataValidade = req.PrazoExecucao == ItemPrazoExecucaoEnum.Hoje ? DateTime.Now : DateTime.Now.AddDays(30); 
                _TOrder.Quantidade = Convert.ToInt32(req.Quantidade);
                _TOrder.PrecoEnvioGain = null;
                _TOrder.PrecoGain = null;
                _TOrder.PrecoLoss = null;
                _TOrder.PrecoEnvioLoss = null;
                _TOrder.ValorAjusteMovel = null;
                _TOrder.ValorInicioMovel = null;
                _TOrder.PrazoValidade = (int)req.PrazoExecucao;


                //Log.EfetuarLog(String.Format("Preco Evio:{0} - Preco Gatilho: {1}", req.PrecosTaxas[req.AutomacaoTipo].PrecoEnvio, req.PrecosTaxas[req.AutomacaoTipo].PrecoGatilho), LogTipoEnum.Aviso);

                switch (req.AutomacaoTipo)
                {
                    case ItemAutomacaoTipoEnum.StopLoss:
                        _TOrder.PrecoLoss = req.PrecosTaxas[0].PrecoGatilho;
                        _TOrder.PrecoEnvioLoss = req.PrecosTaxas[0].PrecoEnvio;
                        _TOrder.IdStopStart = this.ArmarStopLoss(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _TOrder }).IdStopStart;
                        Registrador.AddEvent(EventMds, Eventos);
                        break;
                    case ItemAutomacaoTipoEnum.StopGain:
                        _TOrder.PrecoGain = req.PrecosTaxas[0].PrecoGatilho;
                        _TOrder.PrecoEnvioGain = req.PrecosTaxas[0].PrecoEnvio;
                        _TOrder.IdStopStart = this.ArmarStopGain(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _TOrder }).IdStopStart;
                        Registrador.AddEvent(EventMds, Eventos);
                        break;
                    case ItemAutomacaoTipoEnum.StopSimultaneo:
                        AutomacaoPrecosTaxasInfo item1 =
                            (from item in req.PrecosTaxas
                             where item.AutomacaoTipo == ItemAutomacaoTipoEnum.StopLoss
                             select item).FirstOrDefault();

                        AutomacaoPrecosTaxasInfo item2 =
                            (from item in req.PrecosTaxas
                             where item.AutomacaoTipo == ItemAutomacaoTipoEnum.StopGain
                             select item).FirstOrDefault();

                        _TOrder.PrecoLoss = item1.PrecoGatilho;
                        _TOrder.PrecoEnvioLoss = item1.PrecoEnvio;
                        _TOrder.PrecoGain = item2.PrecoGatilho;
                        _TOrder.PrecoEnvioGain = item2.PrecoEnvio;
                        _TOrder.IdStopStart = this.ArmarStopSimultaneo(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _TOrder }).IdStopStart;
                        Registrador.AddEvent(EventMds, Eventos);
                        break;
                    case ItemAutomacaoTipoEnum.StartCompra:
                        _TOrder.PrecoStart = req.PrecosTaxas[0].PrecoGatilho;
                        _TOrder.PrecoEnvioStart = req.PrecosTaxas[0].PrecoEnvio;
                        _TOrder.IdStopStart = this.ArmarStartCompra(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _TOrder }).IdStopStart;
                        Registrador.AddEvent(EventMds, Eventos);
                        break;
                    case ItemAutomacaoTipoEnum.StopMovel:
                        _TOrder.ValorAjusteMovel = req.PrecosTaxas[0].TaxaAjusteMovel;
                        _TOrder.ValorInicioMovel = req.PrecosTaxas[0].TaxaAjusteInicio;
                        _TOrder.PrecoEnvioLoss = req.PrecosTaxas[0].PrecoEnvio;
                        _TOrder.PrecoLoss = req.PrecosTaxas[0].PrecoGatilho;
                        _TOrder.IdStopStart = this.ArmarStopMovel(new ArmarStartStopRequest() { _AutomacaoOrdensInfo = _TOrder }).IdStopStart;
                        Registrador.AddEvent(EventMds, Eventos);
                        break;
                }

                res.CodigoItemAutomacaoOrdem = _TOrder.IdStopStart;
                res.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                res.DescricaoResposta = ex.Message;
            }
            return res;
        }
        #endregion


        public ReceberItemAutomacaoOrdemResponse ReceberItemAutomacaoOrdem(ReceberItemAutomacaoOrdemRequest req)
        {
            return new ReceberItemAutomacaoOrdemResponse();
        }

#region Processamento interno
        private ArmarStartStopResponse ArmarStopMovel(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = _AutomacaoOrdensDados.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                ArmarStopSimplesResponse res2 = Formatador.ArmarStopSimples(new ArmarStopSimplesRequest()
                {
                    IdStopStart = req._AutomacaoOrdensInfo.IdStopStart,
                    Instrumento = req._AutomacaoOrdensInfo.Instrumento,
                    PrecoGain = req._AutomacaoOrdensInfo.PrecoGain == null ? 0 : (decimal)req._AutomacaoOrdensInfo.PrecoGain,
                    PrecoLoss = req._AutomacaoOrdensInfo.PrecoLoss == null ? 0 : (decimal)req._AutomacaoOrdensInfo.PrecoLoss,
                    PrecoStart = 0,
                    StopStartTipo = req._AutomacaoOrdensInfo.IdStopStartTipo,
                    AjusteMovel = req._AutomacaoOrdensInfo.ValorAjusteMovel == null ? 0 : (decimal)req._AutomacaoOrdensInfo.ValorAjusteMovel,
                    InicioMovel = req._AutomacaoOrdensInfo.ValorInicioMovel == null ? 0 : (decimal)req._AutomacaoOrdensInfo.ValorInicioMovel
                });

                if (res2.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                    res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                    res.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    res.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    _AutomacaoOrdensDados.ExcluirOrdem(res._AutomacaoOrdensInfo.IdStopStart);
                    Log.EfetuarLog(string.Format("{0} - {1}", res2.DescricaoResposta, ((ItemAutomacaoTipoEnum)req._AutomacaoOrdensInfo.IdStopStartTipo).ToString()), LogTipoEnum.Erro);
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                return res;
            }

            //ArmarStartStopResponse res = new ArmarStartStopResponse();
            //res.IdStopStart = string.Empty;
            //return res;
        }

        /// <summary>
        /// Registra um Stop Simultaneo no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        private ArmarStartStopResponse ArmarStopSimultaneo(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = _AutomacaoOrdensDados.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                ArmarStopSimplesResponse res2 = Formatador.ArmarStopSimples(new ArmarStopSimplesRequest()
                {
                    IdStopStart = req._AutomacaoOrdensInfo.IdStopStart,
                    Instrumento = req._AutomacaoOrdensInfo.Instrumento,
                    PrecoGain = (decimal)req._AutomacaoOrdensInfo.PrecoGain,
                    PrecoLoss = (decimal)req._AutomacaoOrdensInfo.PrecoLoss,
                    PrecoStart = 0,
                    StopStartTipo = req._AutomacaoOrdensInfo.IdStopStartTipo
                });

                if (res2.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                    res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                    res.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    res.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    _AutomacaoOrdensDados.ExcluirOrdem(res._AutomacaoOrdensInfo.IdStopStart);
                    Log.EfetuarLog(string.Format("{0} - {1}", res2.DescricaoResposta, ((ItemAutomacaoTipoEnum)req._AutomacaoOrdensInfo.IdStopStartTipo).ToString()), LogTipoEnum.Erro);
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                return res;
            }
        }

        /// <summary>
        /// Registra um Stop Loss no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        private ArmarStartStopResponse ArmarStopLoss(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = _AutomacaoOrdensDados.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                ArmarStopSimplesResponse res2 = Formatador.ArmarStopSimples(new ArmarStopSimplesRequest()
                {
                    IdStopStart = req._AutomacaoOrdensInfo.IdStopStart,
                    Instrumento = req._AutomacaoOrdensInfo.Instrumento,
                    PrecoGain = 0,
                    PrecoLoss = (decimal)req._AutomacaoOrdensInfo.PrecoLoss,
                    PrecoStart = 0,
                    StopStartTipo = req._AutomacaoOrdensInfo.IdStopStartTipo
                });
                if (res2.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                    res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                    res.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    res.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    _AutomacaoOrdensDados.ExcluirOrdem(res._AutomacaoOrdensInfo.IdStopStart);
                    Log.EfetuarLog(string.Format("{0} - {1}", res2.DescricaoResposta, ((ItemAutomacaoTipoEnum)req._AutomacaoOrdensInfo.IdStopStartTipo).ToString()), LogTipoEnum.Erro);
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                return res;
            }
        }

        /// <summary>
        /// Registra um Stop Gain no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        private ArmarStartStopResponse ArmarStopGain(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = _AutomacaoOrdensDados.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                ArmarStopSimplesResponse res2 = Formatador.ArmarStopSimples(new ArmarStopSimplesRequest()
                {
                    IdStopStart = req._AutomacaoOrdensInfo.IdStopStart,
                    Instrumento = req._AutomacaoOrdensInfo.Instrumento,
                    PrecoGain = (decimal) req._AutomacaoOrdensInfo.PrecoGain,
                    PrecoLoss = 0,
                    PrecoStart = 0,
                    StopStartTipo = req._AutomacaoOrdensInfo.IdStopStartTipo
                });
                if (res2.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                    res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                    res.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    res.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    _AutomacaoOrdensDados.ExcluirOrdem(res._AutomacaoOrdensInfo.IdStopStart);
                    Log.EfetuarLog(string.Format("{0} - {1}", res2.DescricaoResposta, ((ItemAutomacaoTipoEnum)req._AutomacaoOrdensInfo.IdStopStartTipo).ToString()), LogTipoEnum.Erro);
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                return res;
            }
        }

        /// <summary>
        /// Registra um start de compra.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        private ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();
            try
            {
                req._AutomacaoOrdensInfo.IdStopStart = _AutomacaoOrdensDados.EnviarOrdemStop(req._AutomacaoOrdensInfo);
                ArmarStopSimplesResponse res2 = Formatador.ArmarStopSimples(new ArmarStopSimplesRequest()
                {
                    IdStopStart = req._AutomacaoOrdensInfo.IdStopStart,
                    Instrumento = req._AutomacaoOrdensInfo.Instrumento,
                    PrecoGain = 0,
                    PrecoLoss = 0,
                    PrecoStart = (decimal) req._AutomacaoOrdensInfo.PrecoStart,
                    StopStartTipo = req._AutomacaoOrdensInfo.IdStopStartTipo
                });
                if (res2.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    res.IdStopStart = req._AutomacaoOrdensInfo.IdStopStart;
                    res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;
                    res.StatusResposta = MensagemResponseStatusEnum.OK;
                }
                else
                {
                    res.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
                    _AutomacaoOrdensDados.ExcluirOrdem(res._AutomacaoOrdensInfo.IdStopStart);
                    Log.EfetuarLog(string.Format("{0} - {1}", res2.DescricaoResposta, ((ItemAutomacaoTipoEnum)req._AutomacaoOrdensInfo.IdStopStartTipo).ToString()), LogTipoEnum.Erro);
                }
                return res;
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, req);
                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                return res;
            }
        }


        private void EnviarMensagemAutenticacao(string idCliente, string idSistema)
        {

            A1_SignIn A1 = new A1_SignIn("BV");

            A1.idCliente = idCliente;
            A1.idSistema = idSistema;

            try
            {
                ASSocketConnection _Client =
                new ASSocketConnection();

                _Client.ASSocketOpen();
                _Client.SendData(A1.getMessageA1());
                _Client = null;
                Log.EfetuarLog(String.Format("Mensagem de Autenticação enviada para o MDS Usuario:{0} - Sistema:{1}.", idCliente, idSistema), LogTipoEnum.Aviso);
                _manualResetEvent.WaitOne(new TimeSpan(0, 0, 5));
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "EnviarMensagemAutenticacao: ", ex.Message));
                _manualResetEvent.Set();
            }
        }

        /// <summary>
        /// Callback de respostas de mensagem de ordem Stop.
        /// </summary>
        /// <param name="sender"> Objeto de resposta.</param>
        void ProcessarEventoMDS(object sender)
        {
            try
            {

                SS_StopSimplesResposta _SS_StopSimplesResposta =
                    (SS_StopSimplesResposta)(sender);

                
                ExecutarOrdemRequest reqOrdem = new ExecutarOrdemRequest();
                AtualizaOrdemStartStopResponse res = this.AtualizaOrdemStartStop(new AtualizaOrdemStartStopRequest()
                {
                    IdStartStop = _SS_StopSimplesResposta.pStrIdStopStart,
                    IdStopStartStatus = (int)ItemAutomacaoStatusEnum.ExecutadoPeloMDS,
                    PrecoReferencia = decimal.Parse(_SS_StopSimplesResposta.pStrPrecoReferencia.ToString())
                });

       

                reqOrdem.ClOrdID = _SS_StopSimplesResposta.pStrIdStopStart;
                reqOrdem.CodigoExterno = _SS_StopSimplesResposta.pStrIdStopStart;
                   reqOrdem.CodigoBolsa = res.CodigoBolsa.ToUpper();
                   reqOrdem.Account = res.CodigoCliente.ToString();
                   reqOrdem.Symbol = _SS_StopSimplesResposta.pStrInstrument.Trim().ToUpper();
                   reqOrdem.OrderQty = Convert.ToDouble(res.Quantidade);
                   reqOrdem.Price = Convert.ToDouble(res.PrecoEnvio);
                   reqOrdem.Side = res.ItemAutomacaoTipo == ItemAutomacaoTipoEnum.StartCompra ? OrdemDirecaoEnum.Compra : OrdemDirecaoEnum.Venda;
                   reqOrdem.OrdType = OrdemTipoEnum.Limitada;
                   reqOrdem.ExpireDate = DateTime.Now.AddDays(30);
                   reqOrdem.DataReferencia = DateTime.Now;
                   reqOrdem.CodigoSistemaCliente = "3";
                   reqOrdem.CodigoCliente = res.CodigoCliente.ToString();

                
                ExecutarOrdemResponse resposta =
                    (ExecutarOrdemResponse)
                        _ServicoOrdensServidor.ProcessarMensagem(reqOrdem);


                res = this.AtualizaOrdemStartStop(new AtualizaOrdemStartStopRequest()
                {
                    IdStartStop = _SS_StopSimplesResposta.pStrIdStopStart,
                    IdStopStartStatus = (int)ItemAutomacaoStatusEnum.EnviadoParaServidorDeOrdens,
                    PrecoReferencia = decimal.Parse(_SS_StopSimplesResposta.pStrPrecoReferencia.ToString())
                });
                // Atualiza a ordem stop para o status executada , bem como atualiza seu preço de referencia com o valor disparado pelo MDS
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex, string.Format("{0}{1}", "ProcessarEventoMDS: ", ex.Message));
            }
        }
        
        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            Log.EfetuarLog("Retomando Ordens", LogTipoEnum.Aviso);
            try
            {
                this.RetomarOrdensPendentes();
            }
            catch (Exception ex)
            {
                Log.EfetuarLog(ex);
            }
        }

        public void PararServico()
        {
            _AutomacaoOrdensDados.ExcluirUsuarioLogado(4444, 3);
        }

        public ServicoStatus ReceberStatusServico()
        {
            return ServicoStatus.Indefinido;
        }

        #endregion
    }
}
