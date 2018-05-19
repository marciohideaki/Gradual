using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using Gradual.OMS.Comunicacao.Automacao.Ordens;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Eventos;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Recebidas;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using Gradual.OMS.Ordens.Comunicacao.DB;
using Gradual.OMS.Ordens.Comunicacao.Mensagens.Enviadas;
using Gradual.OMS.Ordens.Lib;
using Gradual.OMS.Ordens.Lib.Info;
using Gradual.OMS.Ordens.Lib.Mensageria;
using Gradual.OMS.Ordens.StartStop.Lib;
using Gradual.OMS.Ordens.StartStop.Lib.Enum;
using Gradual.OMS.Risco.Lib;
using Gradual.OMS.Risco.Lib.Info;
using Gradual.OMS.Risco.Lib.Mensageria;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;
using log4net;
using Gradual.OMS.StopStartAdm.Lib;
using Gradual.OMS.StopStartAdm.Lib.Dados;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;

namespace Gradual.OMS.StopStart
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ServicoOrdemStopStart : IServicoOrdemStopStart, IServicoControlavel, IServicoStopStartAdm
    { 

        #region ||Properties

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        AutomacaoOrdensDados _Servico = new AutomacaoOrdensDados();
        protected ServicoStatus _ServicoStatus = ServicoStatus.Indefinido;
        protected DateTime lastPongReceived = DateTime.MinValue;
        protected bool Autenticado = false;
        protected bool bKeepRunning = false;
        protected Thread thrMonitor = null;
        private MDSPackageSocket mdssocket = null;

        /// <summary>
        /// Código do cliente
        /// </summary>
        protected string IdCliente
        {
            get
            {
                return ConfigurationManager.AppSettings["IdCliente"].ToString();
            }
        }

        /// <summary>
        /// Id do sistema
        /// </summary>
        protected string IdSistema
        {
            get
            {
                return ConfigurationManager.AppSettings["IdSistema"].ToString();
            }
        }
        #endregion

        #region ||ValidaConexaoMDS
        protected virtual void ValidarStatusConexaoMDS()
        {
            try
            {
                if ((this.mdssocket == null) ||
                    (this.mdssocket.IsConectado() == false) ||
                    Autenticado == false)
                {
                    Autenticado = false;
                    if (this.mdssocket != null && this.mdssocket.IsConectado())
                    {
                        this.mdssocket.CloseConnection();
                        this.mdssocket = null;
                    }

                    PersistenciaAutenticacaoMDS dados = new PersistenciaAutenticacaoMDS();

                    dados.ExcluirMDSAuthentication(int.Parse(IdCliente), int.Parse(IdSistema));

                    EnviarMensagemAutenticacao();

                    lastPongReceived = DateTime.Now;

                    logger.Info(string.Format("Tentativa de reconexao com MDS às {0}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") ));

                }
                else
                {
                    PG_Ping pg = new PG_Ping();

                    this.mdssocket.SendData(pg.GetMessage(),true);

                    TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - lastPongReceived.Ticks);
                    if ( ts.TotalSeconds > 60  )
                    {
                        logger.Info("Timeout no hearbeat. Reconectando");
                        this.mdssocket.CloseConnection();
                        Autenticado = false;
                    }
                }

                logger.Info(string.Format("SocketPrincipal conectado? {0} -  ValidarStatusConexaoMDS - Método de reconexao com MDS às {1}", this.mdssocket!= null?this.mdssocket.IsConectado().ToString():"false", DateTime.Now.ToString()));
            }
            catch (Exception ex)
            {
                logger.Error("ValidarStatusConexaoMDS(): " + ex.Message, ex);
            }
        }
        #endregion 


        #region ||IServicoOrdens Members
        /// <summary>
        /// Redireciona e chama o método certo para armar o stopstart solicitado pelo cliente 
        /// </summary>
        /// <param name="req">Entidade do Tipo ArmarStartStopRequest</param>
        /// <param name="pReenviando">Verifica se está sendo Reenviado do banco para o MDS</param>
        /// <returns>Retorna um entidade</returns>
        public ArmarStartStopResponse ArmarStopStartGeral(ArmarStartStopRequest req)
        {
            ArmarStartStopResponse lReturn = new ArmarStartStopResponse();
            
            try
            {
                if (!VerificaPermissaoRisco(req._AutomacaoOrdensInfo.Account))
                {
                    lReturn.DescricaoResposta = "Permissão";
                    lReturn.StatusResposta = MensagemResponseStatusEnum.AcessoNaoPermitido;

                    lReturn.Criticas = new List<CriticaInfo>();
                    lReturn.Criticas.Add(new CriticaInfo()
                    {
                        Descricao = string.Format("Cliente {0} sem permissão para operar StopStart.", req._AutomacaoOrdensInfo.Account),
                        Status = CriticaStatusEnum.Informativo,
                    });

                    return lReturn;
                }

                switch (req._AutomacaoOrdensInfo.IdStopStartTipo)
                {
                    case StopStartTipoEnum.StopMovel:

                        lReturn = ArmarStopMovel(req, false);

                        break;

                    case StopStartTipoEnum.StopSimultaneo:

                        lReturn = ArmarStopSimultaneo(req, false);

                        break;

                    case StopStartTipoEnum.StopLoss:

                        lReturn = ArmarStopLoss(req, false);

                        break;

                    case StopStartTipoEnum.StopGain:

                        lReturn = ArmarStopGain(req, false);

                        break;

                    case StopStartTipoEnum.StartCompra:

                        lReturn = ArmarStartCompra(req, false);

                        break;
                }
            }
            catch (Exception ex)
            {
                lReturn.DataResposta      = DateTime.Now;
                lReturn.DescricaoResposta = ex.Message;
                lReturn.StatusResposta    = MensagemResponseStatusEnum.ErroPrograma;
            }

            return lReturn;
        }

        /// <summary>
        /// Arma o Stop Móvel
        /// </summary>
        /// <param name="req">Entidade do tipo ArmarStartStopRequest</param>
        /// <returns></returns>
        protected ArmarStartStopResponse ArmarStopMovel(ArmarStartStopRequest req, bool pReenviando = false)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();

            try
            {
                req._AutomacaoOrdensInfo.IdStopStartTipo = StopStartTipoEnum.StopMovel;

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();
                if(!pReenviando)
                    req._AutomacaoOrdensInfo.StopStartID = dados.EnviarOrdemStop(req._AutomacaoOrdensInfo);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopMovel ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));

                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);

                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;

                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;

                dados.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopMovel ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));

//                Registrador.AddEvent(EventMds, Eventos);

                res.DataResposta = DateTime.Now;

                res.DescricaoResposta = "Stop móvel armado com sucesso";

                res.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                StopStartLog.LogInfo(string.Format("{0}{1}\n{2}", "ArmarStopMovel: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Erro ao armar Stop móvel - {0} - {1}", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }

        /// <summary>
        /// Registra um Stop Simultaneo no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        protected ArmarStartStopResponse ArmarStopSimultaneo(ArmarStartStopRequest req,  bool pReenviando = false)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();

            try
            {
                req._AutomacaoOrdensInfo.IdStopStartTipo = StopStartTipoEnum.StopSimultaneo;

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();
                if (!pReenviando)
                    req._AutomacaoOrdensInfo.StopStartID = dados.EnviarOrdemStop(req._AutomacaoOrdensInfo);

                StopStartLog.LogInfo(req ,string.Format("{0}{1}{2}", "ArmarStopSimultaneo ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;

                dados.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopSimultaneo ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));

//                Registrador.AddEvent(EventMds, Eventos);

                res.DataResposta = DateTime.Now;

                res.DescricaoResposta = "Stop Simultaneo armado com sucesso";

                res.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                StopStartLog.LogInfo(string.Format("{0}{1}\n{2}", "ArmarStopSimultaneo: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Erro ao armar Stop simultaneo - {0} - {1} ", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }

        /// <summary>
        /// Registra um Stop Loss no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        protected ArmarStartStopResponse ArmarStopLoss(ArmarStartStopRequest req, bool pReenviando = false)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();

            try
            {
                req._AutomacaoOrdensInfo.IdStopStartTipo = StopStartTipoEnum.StopLoss;

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();

                if (!pReenviando)
                    req._AutomacaoOrdensInfo.StopStartID = dados.EnviarOrdemStop(req._AutomacaoOrdensInfo);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopLoss ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;

                dados.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopLoss ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));

//                Registrador.AddEvent(EventMds, Eventos);

                res.DataResposta = DateTime.Now;

                res.DescricaoResposta = "Stop Loss armado com sucesso";

                res.StatusResposta = MensagemResponseStatusEnum.OK;

            }
            catch (Exception ex)
            {
                StopStartLog.LogInfo(string.Format("{0}{1}\n{2}", "ArmarStopLoss: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Armar Stop Loss - {0} - {1} ", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }

        /// <summary>
        /// Registra um Stop Gain no MDS Server.
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        protected ArmarStartStopResponse ArmarStopGain(ArmarStartStopRequest req, bool pReenviando = false)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();

            try
            {
                req._AutomacaoOrdensInfo.IdStopStartTipo = StopStartTipoEnum.StopGain;

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();

                if (!pReenviando)
                    req._AutomacaoOrdensInfo.StopStartID = dados.EnviarOrdemStop(req._AutomacaoOrdensInfo);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopGain ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;

                dados.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStopGain ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));

//                Registrador.AddEvent(EventMds, Eventos);

                res.DataResposta = DateTime.Now;

                res.DescricaoResposta = "Stop Gain armado com sucesso";

                res.StatusResposta = MensagemResponseStatusEnum.OK;

            }
            catch (Exception ex)
            {

                StopStartLog.LogInfo(string.Format("{0}{1}\n{2}", "ArmarStopGain: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Erro ao armar Stop Gain - {0} - {1}", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }

        /// <summary>
        /// Arma um StopStart de compra no MDS
        /// </summary>
        /// <param name="TemplateOrder">Atributos da Ordem </param>
        /// <returns>Id do stopstart </returns>
        protected ArmarStartStopResponse ArmarStartCompra(ArmarStartStopRequest req, bool pReenviando = false)
        {
            ArmarStartStopResponse res = new ArmarStartStopResponse();

            try
            {
                req._AutomacaoOrdensInfo.IdStopStartTipo = StopStartTipoEnum.StartCompra;

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();

                if (!pReenviando)
                    req._AutomacaoOrdensInfo.StopStartID = dados.EnviarOrdemStop(req._AutomacaoOrdensInfo);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStartCompra ", req._AutomacaoOrdensInfo.StopStartID, " Registrado na aplicação"));
                
                Formatador.ArmarStopSimples(req._AutomacaoOrdensInfo);
                
                res.IdStopStart = req._AutomacaoOrdensInfo.StopStartID;
                
                res._AutomacaoOrdensInfo = req._AutomacaoOrdensInfo;

                dados.AtualizaOrdemStop(req._AutomacaoOrdensInfo.StopStartID, (int)OrdemStopStatus.EnviadaMDS);

                StopStartLog.LogInfo(req, string.Format("{0}{1}{2}", "ArmarStartCompra ", req._AutomacaoOrdensInfo.StopStartID, " Enviado para o MDS"));

//                Registrador.AddEvent(EventMds, Eventos);

                res.DataResposta = DateTime.Now;

                res.DescricaoResposta = "Start de Compra armado com sucesso";

                res.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                StopStartLog.LogInfo(string.Format("{0}{1}{2}", "ArmarStartCompra: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Erro ao armar Start Compra - {0} - {1}", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }

        /// <summary>
        /// Cancela um StopStart que já se encontra armado no MDS
        /// </summary>
        /// <param name="Instrument">Código do Instrumento</param>
        /// <param name="id_stopstart">Código da Ordem a ser cancelada </param>
        /// <param name="id_stopstart_status"> Status da ordem</param>
        public CancelarStartStopOrdensResponse CancelaOrdemStopStart(CancelarStartStopOrdensRequest req)
        {
            CancelarStartStopOrdensResponse res = new CancelarStartStopOrdensResponse();

            try
            {
                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();

                dados.CancelaOrdemStopStart(req.IdStopStart, req.IdStopStartStatus);

                StopStartLog.LogInfo(string.Format("{0}{1}{2}", "Solicitação de Cancelamento do StopStart ", req.IdStopStart, " Registrado na aplicação"));
                
                Formatador.CancelarOrdemStop(req.Instrument, req.IdStopStart);

                StopStartLog.LogInfo(string.Format("{0}{1}{2}", " Cancelamento do StopStart ", req.IdStopStart, " Enviado para o MDS"));
                
                res.DataResposta = DateTime.Now;

                res.StatusResposta = MensagemResponseStatusEnum.OK;

                res.DescricaoResposta = "Cancelamento de StopStart efetuado com sucesso";

//                Registrador.AddEvent(EventMds, Eventos);

            }
            catch (Exception ex)
            {
                StopStartLog.LogInfo(string.Format("{0}{1}{2}", "CancelaOrdemStopStart: ", ex.Message, ex.StackTrace));

                res.DescricaoResposta = string.Format("Erro ao cancelar stop/start - {0} - {1}", ex.Message, ex.StackTrace);

                res.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;

                res.DataResposta = DateTime.Now;
            }

            return res;
        }


        /// <summary>
        /// Envia a ordem StopStart para o serviço de ordens
        /// </summary>
        /// <param name="pInfo">ENtidade do cliente Ordem Info</param>
        protected void EnviaServicoOrdem(ClienteOrdemInfo pInfo, int pIdStopStart)
        {
            try
            {
                 AutomacaoOrdensInfo OrderResumidaInfo = null;
                try
                {
                    logger.Info("Chama metodo para obter a porta de origem do roteador.");
                    OrderResumidaInfo = new AutomacaoOrdensDados().ListarOrdemStopStartResumido(pIdStopStart);
                }
                catch (Exception ex)
                {
                    logger.Info("Erro ao chamar método para obter a porta de origem do roteador. " + ex.Message);
                }

                IServicoOrdens lServicoOrdens = Ativador.Get<IServicoOrdens>();

                logger.Info("Porta de destino do roteador de ordens: " + OrderResumidaInfo.ControlePorta);
                pInfo.PortaControleOrdem = OrderResumidaInfo.ControlePorta;

                EnviarOrdemRequest lRequest = new EnviarOrdemRequest() { ClienteOrdemInfo = pInfo };

                logger.Info("Prepara chamada para o roteador de ordens.");
                EnviarOrdemResponse lResponse = lServicoOrdens.EnviarOrdem(lRequest);

                if (lResponse.StatusResposta == Ordens.Lib.Enum.CriticaRiscoEnum.Sucesso)
                {
                    //Atualiza o status no banco de dados para ordem executada
                    new AutomacaoOrdensDados().AtualizaOrdemStop(pIdStopStart,(int)OrdemStopStatus.Execucao,Convert.ToDecimal(pInfo.Preco));

                    logger.Info(string.Format("Ordem aceita pelo roteador de ordens:{0}", lResponse.DescricaoResposta));
                }
                else 
                {
                    if (lResponse.CriticaInfo != null)
                    {
                        if (lResponse.CriticaInfo.Count > 0)
                        {
                            lResponse.CriticaInfo.ForEach(delegate(Ordens.Lib.Info.PipeLineCriticaInfo info)
                            {
                                string strCritica = string.Format("Critica {0} - Tipo: {1} - Data/Hora: {2}\n", info.Critica, info.CriticaTipo, info.DataHoraCritica);
                                logger.Info(strCritica);
                                new AutomacaoOrdensDados().AtualizaOrdemStop(pIdStopStart, (int)OrdemStopStatus.CancelamentoRegistradoAplicacao,strCritica);
                            });
                        }
                    }

                    logger.Info(string.Format("Apresentou erro ao inserir ordem no roteador de ordens:{0}", lResponse.DescricaoResposta));
                }
            }
            catch (Exception ex)
            {
                new AutomacaoOrdensDados().AtualizaOrdemStop(pIdStopStart, (int)OrdemStopStatus.CancelamentoAceitoMDS,"Cancelado pela corretora");
                logger.Error(string.Format("Erro ao enviar ordem Message: {0} -\nStacktrace: {1} ", ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Verifica se o Cliente têm permissão para operar opções
        /// </summary>
        /// <param name="pIdCliente"></param>
        /// <returns>Retorna Verdadeiro se o cliente pode operar StopStart</returns>
        protected bool VerificaPermissaoRisco(int pIdCliente)
        {
            bool lRetorno = false;

            try
            {
                IServicoRisco ServicoRisco = Ativador.Get<IServicoRisco>();

                ClienteParametrosPermissoesResponse<ClienteParametroPermissaoInfo> lResponse = ServicoRisco.VerificarPermissaoCliente(

                    new ClienteParametrosPermissoesRequest()
                    {
                        IdCliente = pIdCliente,
                        ParametroPermissaoEnum = Gradual.OMS.Risco.Lib.Enum.ParametroPermissaoRiscoEnum.PERMISSAO_OPERAR_STOPSTART
                    });

                if (lResponse.StatusResposta == Risco.Lib.Enum.CriticaMensagemEnum.Sucesso)
                {
                    //TODO: IF usado para teste....futuramente este IF Deve ser excluído
                    //Em caso de teste, incluímos cliente sem efetuar todo o fluxo de cadastro. Uma vez pronto,
                    //Não será necessário a verificação da coleção nula
                    if (lResponse.ColecaoObjeto == null)
                    {
                        lRetorno = true;
                        logger.Info(string.Format("Permissão verificada com sucesso. O cliente {0} têm permissão para efetuar StopStart", pIdCliente));

                    }else if (lResponse.ColecaoObjeto.Count > 0)
                    {
                        lRetorno = true;
                        logger.Info(string.Format("Permissão verificada com sucesso. O cliente {0} têm permissão para efetuar StopStart", pIdCliente));
                    }
                    else
                    {
                        lRetorno = false;
                        logger.Info(string.Format("Permissão negada! O cliente {0} NÃO têm permissão para efetuar StopStart", pIdCliente));
                    }
                }
                else
                {
                    lRetorno = false;
                    logger.Info(string.Format("A Permissão NÃO FOI verificada com sucesso para o cliente {0} efetuar StopStart", pIdCliente));
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Message: {0} -\nStacktrace: {1} ", ex.Message, ex.StackTrace));
                lRetorno = false;
            }

            return lRetorno;
        }

        #endregion

        #region ||Métodos de Apoio e autenticações
        /// <summary>
        /// Envia a Mensagem de Autenticação do Usuário para o MDS
        /// </summary>
        protected void EnviarMensagemAutenticacao()
        {

            A1_SignIn A1 = new A1_SignIn("BV");

            A1.idCliente = IdCliente;
            A1.idSistema = IdSistema;

            try
            {
                ASSocketConnection _Client = new ASSocketConnection();

                _Client.OnASAuthenticationResponse += new ASSocketConnection.ASAuthenticationResponseEventHandler(OnASAuthenticationResponse);

                logger.Info("Iniciando autenticacao no MDS: conectando com AuthenticationService [" + _Client.IpAddr + ":" + _Client.Port + "]");

                _Client.ASSocketOpen();

                string msg = A1.getMessageA1();

                logger.Info("Msg de autenticacao [" + msg + "]");

                _Client.SendData(msg);

                lastPongReceived = DateTime.Now;

                logger.Info("Autenticacao enviada");
            }

            catch (Exception ex)
            {
                logger.Error(string.Format("{0}{1}", "EnviarMensagemAutenticacao: ", ex.Message));
                throw new Exception(ex.Message);
            }
        }

        protected virtual void OnASAuthenticationResponse(object Response, ASEventArgs e)
        {
            try
            {
                string message = e.Message;

                message = message.Replace("A2", "A3");

                logger.Info("Enviando token de autenticacao para o MDS [" + message + "]");

                mdssocket = new MDSPackageSocket();

                mdssocket.OnMDSAuthenticationResponse += new MDSPackageSocket.MDSAuthenticationResponseEventHandler(mdssocket_OnMDSAuthenticationResponse);
                mdssocket.OnMDSSRespostaCancelamentoEvent += new MDSPackageSocket.MDSSRespostaCancelamentoEventHandler(mdssocket_OnMDSSRespostaCancelamentoEvent);
                mdssocket.OnMDSSRespostaSolicitacaoEvent += new MDSPackageSocket.MDSSRespostaSolicitacaoEventHandler(mdssocket_OnMDSSRespostaSolicitacaoEvent);
                mdssocket.OnMDSStopStartEvent += new MDSPackageSocket.MDSStopStartEventHandler(mdssocket_OnMDSStopStartEvent);
                mdssocket.OnMDSPing += new MDSPackageSocket.MDSPingEventHandler(mdssocket_OnMDSPing);

                Formatador.SetMDSPackageSocket(mdssocket);

                mdssocket.IpAddr = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();
                mdssocket.Port = ConfigurationManager.AppSettings["ASConnMDSPort"].ToString();
                mdssocket.OpenConnection();

                mdssocket.SendData(message, true);
            }
            catch (Exception ex)
            {
                logger.Error("OnASAuthenticationResponse(): " + ex.Message, ex);
            }
        }

        #region MDS EventHandlers
        protected void mdssocket_OnMDSPing(object Response, MDSEventArgs e)
        {
            logger.Debug("Recebeu PONG: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
            lastPongReceived = DateTime.Now;
        }

        protected void mdssocket_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        {
            try
            {
                lastPongReceived = DateTime.Now;

                // Adiciona uma Thread no pool responsavel por executar o processamento do Stop.
                //Teste para saber da instabilidade da chamada de serviço quando MDS é execultado
                ThreadPool.QueueUserWorkItem(new WaitCallback(
                                        delegate(object required)
                                        {
                                            ProcessarEventoExecutouMDS(e.StopStartEventObject);
                                        }));
            }
            catch (Exception ex)
            {
                logger.Error("mdssocket_OnMDSStopStartEvent(): " + ex.Message, ex);
            }
        }

        protected void mdssocket_OnMDSSRespostaSolicitacaoEvent(object sender, MDSEventArgs e)
        {
            try
            {

                lastPongReceived = DateTime.Now;

                RS_RespostaStop _RS_RespostaStop = (RS_RespostaStop) e.StopStartEventObject;

                int id_stopstart = int.Parse(_RS_RespostaStop.pStrIdStopStart);

                int id_status = int.Parse(_RS_RespostaStop.pStrStatus);

                OrdemStopStatus StopEnum = OrdemStopStatus.RejeitadoMDS;

                if (id_status == (int)RespostaOrdem.Rejeitado) // Stop Rejeitado
                {
                    StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop rejeitado pelo MDS"));

                    StopEnum = OrdemStopStatus.RejeitadoMDS;

                    Formatador.RemoverStopExecutado(_RS_RespostaStop.pStrIdStopStart);
                }
                else if (id_status == (int)RespostaOrdem.Aceito) // Stop Aceito
                {
                    StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop aceito pelo MDS"));

                    StopEnum = OrdemStopStatus.AceitoMDS;
                }

                // Atualiza status da ordem para aceito.
                _Servico.AtualizaOrdemStop(int.Parse(_RS_RespostaStop.pStrIdStopStart), (int)StopEnum);
            }
            catch (Exception ex)
            {
                logger.Error("mdssocket_OnMDSSRespostaSolicitacaoEvent(): " + ex.Message, ex);
            }
        }

        protected void mdssocket_OnMDSAuthenticationResponse(object sender, MDSEventArgs e)
        {
            try
            {
                lastPongReceived = DateTime.Now;
                int status = 0;
                A4_ResponseSignIn response = (A4_ResponseSignIn)e.StopStartEventObject;

                if (response.pStrStatusRequest != null && response.pStrStatusRequest.Trim() != string.Empty)
                {
                    logger.Info("Resposta autenticacao do MDS [" + response.pStrStatusRequest + "]");
                    status = int.Parse(response.pStrStatusRequest.Trim());
                    switch (status)
                    {
                        case 0:
                            logger.Fatal("Cliente NÃO FOI AUTENTICADO ...");
                            Autenticado = false;
                            break;
                        case 1:
                            logger.Info("Cliente autenticado com sucesso!!");
                            Autenticado = true;

                            // Nao rearma stops nao armados
                            // ficou como interface admistrativa
                            // ATP: 2011-10-19
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(
                            //     delegate(object required)
                            //     {
                            //         this.ArmaStopStartNaoArmado();
                            //     }));

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("mdssocket_OnMDSAuthenticationResponse: " + ex.Message, ex);
            }
        }

        protected void mdssocket_OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e)
        {
            try
            {
                CR_CancelamentoStopResposta _CR_CancelamentoStopResposta
                    = (CR_CancelamentoStopResposta) e.StopStartEventObject;

                int id_stopstart = int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart);
                int id_status = int.Parse(_CR_CancelamentoStopResposta.pStrStatus);
                OrdemStopStatus ordemStopEnum = OrdemStopStatus.CancelamentoAceitoMDS;

                lastPongReceived = DateTime.Now;

                if (id_status == (int)RespostaOrdem.Aceito)
                {
                    ordemStopEnum = OrdemStopStatus.CancelamentoAceitoMDS;

                    StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " Stop Cancelado pelo MDS"));
                }

                if (id_status == (int)RespostaOrdem.Rejeitado)
                {
                    ordemStopEnum = OrdemStopStatus.CancelamentoRejeitadoMDS;

                    StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n Cancelamento do stop " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " foi rejeitado pelo MDS"));
                }

                //Altera o status no banco de dados
                _Servico.CancelaOrdemStopStart(id_stopstart, (int)ordemStopEnum);

            }
            catch (Exception ex)
            {
                logger.Error("mdssocket_OnMDSAuthenticationResponse: " + ex.Message, ex);
            }}

        #endregion // MDS EventHandlers

        /// <summary>
        /// Método para armar stops/start atrasados 
        /// </summary>
        public void ArmaStopStartNaoArmado()
        {
            List<AutomacaoOrdensInfo> lstOrdens = _Servico.ListarOrdemStopStartNaoEnviadasMDS();

            foreach( AutomacaoOrdensInfo info in lstOrdens)
            {
                ArmarStartStopRequest req = new ArmarStartStopRequest();

                req._AutomacaoOrdensInfo = new AutomacaoOrdensInfo();

                req._AutomacaoOrdensInfo.Account                = info.Account;
                req._AutomacaoOrdensInfo.AdjustmentMovelPrice   = info.AdjustmentMovelPrice;
                req._AutomacaoOrdensInfo.ExecutionTime          = info.ExecutionTime;
                req._AutomacaoOrdensInfo.ExpireDate             = info.ExpireDate;
                req._AutomacaoOrdensInfo.IdStopStartTipo        = info.IdStopStartTipo;
                req._AutomacaoOrdensInfo.InitialMovelPrice      = info.InitialMovelPrice;
                req._AutomacaoOrdensInfo.OrderQty               = info.OrderQty;
                req._AutomacaoOrdensInfo.OrdTypeID              = info.OrdTypeID;
                req._AutomacaoOrdensInfo.ReferencePrice         = info.ReferencePrice;
                req._AutomacaoOrdensInfo.RegisterTime           = info.RegisterTime;
                req._AutomacaoOrdensInfo.SendStartPrice         = info.SendStartPrice;
                req._AutomacaoOrdensInfo.SendStopGainPrice      = info.SendStopGainPrice;
                req._AutomacaoOrdensInfo.SendStopLossValuePrice = info.SendStopLossValuePrice;
                req._AutomacaoOrdensInfo.StartPriceValue        = info.StartPriceValue;
                req._AutomacaoOrdensInfo.StopGainValuePrice     = info.StopGainValuePrice;
                req._AutomacaoOrdensInfo.StopLossValuePrice     = info.StopLossValuePrice;
                req._AutomacaoOrdensInfo.StopStartID            = info.StopStartID;
                req._AutomacaoOrdensInfo.StopStartStatusID      = info.StopStartStatusID;
                req._AutomacaoOrdensInfo.Symbol                 = info.Symbol;

                logger.Info("Rearma stop ID: " + info.StopStartID );

                //switch (info.IdStopStartTipo)
                //{
                //    case StopStartTipoEnum.StartCompra:
                //        this.ArmarStartCompra(req, true);
                //        break;
                //    case StopStartTipoEnum.StopGain:
                //        this.ArmarStopGain(req, true);
                //        break;
                //    case StopStartTipoEnum.StopLoss:
                //        this.ArmarStopLoss(req, true);
                //        break;
                //    case StopStartTipoEnum.StopMovel:
                //        this.ArmarStopMovel(req, true);
                //        break;
                //    case StopStartTipoEnum.StopSimultaneo:
                //        this.ArmarStopSimultaneo(req, true);
                //        break;
                //}
            }
        }
        #endregion


        #region ||Events and CallBacks
        ///// <summary>
        ///// Inicializa os callback's que o programa faz referencia.
        ///// </summary>
        //protected void Initialize()
        //{
        //    try
        //    {

        //        if (Registrador.RegistradorItems() == 0)
        //        {

        //            Event._MDSAuthenticationResponse +=
        //                new Event.MDSAuthenticationResponseEventHandler(Event__MDSAuthenticationResponse);

        //            OMSEventHandlerClass omsEHC =
        //                new OMSEventHandlerClass(EventMds);

        //            // Callback de execucao de ordem
        //            EventMds.OnMDSStopStartEvent +=
        //                new MDSEventFactory.MDSStopStartEventHandler(EventMds_OnMDSStopStartEvent);

        //            EventMds.OnMDSSRespostaSolicitacaoEvent
        //                += new MDSEventFactory.MDSSRespostaSolicitacaoEventHandler(EventMds_OnMDSSRespostaSolicitacaoEvent);

        //            EventMds.OnMDSSRespostaCancelamentoEvent +=
        //                new MDSEventFactory.MDSSRespostaCancelamentoEventHandler(EventMds_OnMDSSRespostaCancelamentoEvent);

        //            EventMds.OnMDSSRespostaPingEvent +=
        //                new MDSEventFactory.MDSSRespostaPingEventHandler(EventMds_OnMDSSRespostaPingEvent);

        //            Registrador.AddListener(EventMds);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Initialize(): " + ex.Message, ex);
        //    }
        //}

        ///// <summary>
        ///// Resposta de permissão de conexão no sistema MDS.
        ///// </summary>
        ///// <param name="Response"> Objeto de autenticação. </param>
        ///// <param name="_ClientSocket"> Socket com a conexão do cliente conectado.</param>
        //void Event__MDSAuthenticationResponse(object Response, System.Net.Sockets.Socket _ClientSocket)
        //{
        //    try
        //    {
        //        if (Response.ToString().Trim() != string.Empty)
        //        {
        //            switch (int.Parse(Response.ToString().Trim()))
        //            {
        //                case 0:
        //                    StopStartLog.LogInfo("Cliente NÃO FOI AUTENTICADO ...");
        //                    Autenticado = false;
        //                    break;
        //                case 1:
        //                    StopStartLog.LogInfo("Cliente autenticado com sucesso ");
        //                    Autenticado = true;
        //                    break;

        //            }

        //            Contexto.SocketPrincipal = _ClientSocket;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StopStartLog.LogInfo(string.Format("{0}{1}", "Event__MDSAuthenticationResponse: ", ex.Message));
        //    }
        //}

        ///// <summary>
        ///// Evento de disparo acionado quando o MDS responde o Cancelamento
        ///// </summary>
        //void EventMds_OnMDSSRespostaCancelamentoEvent(object sender, MDSEventArgs e)
        //{
        //    try
        //    {
        //        CR_CancelamentoStopResposta _CR_CancelamentoStopResposta
        //            = (CR_CancelamentoStopResposta)(sender);

        //        int id_stopstart              = int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart);
        //        int id_status                 = int.Parse(_CR_CancelamentoStopResposta.pStrStatus);
        //        OrdemStopStatus ordemStopEnum = OrdemStopStatus.CancelamentoAceitoMDS;

        //        lastPongReceived = DateTime.Now;

        //        if (id_status == (int)RespostaOrdem.Aceito)
        //        {
        //            ordemStopEnum = OrdemStopStatus.CancelamentoAceitoMDS;

        //            StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " Stop Cancelado pelo MDS"));
        //        }

        //        if (id_status == (int)RespostaOrdem.Rejeitado)
        //        {
        //            ordemStopEnum = OrdemStopStatus.CancelamentoRejeitadoMDS;

        //            StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n Cancelamento do stop " + int.Parse(_CR_CancelamentoStopResposta.pStrIdStopStart).ToString(), " foi rejeitado pelo MDS"));
        //        }

        //        //Altera o status no banco de dados
        //        _Servico.CancelaOrdemStopStart(id_stopstart, (int)ordemStopEnum);

        //    }
        //    catch (Exception ex)
        //    {
        //        StopStartLog.LogInfo(string.Format("{0}{1}{2}", "EventMds_OnMDSSRespostaCancelamentoEvent: ", ex.Message, ex.StackTrace));
        //    }
        //}

        ///// <summary>
        ///// Evento acionado no disparo de reposta do MDS ao Armar um StartStop
        ///// </summary>
        //public void EventMds_OnMDSSRespostaSolicitacaoEvent(object sender, MDSEventArgs e)
        //{
        //    try
        //    {
        //        if (sender == null)
        //            logger.Error("O sender do evento EventMds_OnMDSSRespostaSolicitacaoEvent está nulo");

        //        lastPongReceived = DateTime.Now;

        //        RS_RespostaStop _RS_RespostaStop = (RS_RespostaStop)(sender);

        //        int id_stopstart = int.Parse(_RS_RespostaStop.pStrIdStopStart);

        //        int id_status = int.Parse(_RS_RespostaStop.pStrStatus);

        //        OrdemStopStatus StopEnum = OrdemStopStatus.RejeitadoMDS;

        //        if (id_status == (int)RespostaOrdem.Rejeitado) // Stop Rejeitado
        //        {
        //            StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop rejeitado pelo MDS"));

        //            StopEnum = OrdemStopStatus.RejeitadoMDS;
        //        }
        //        else if (id_status == (int)RespostaOrdem.Aceito) // Stop Aceito
        //        {
        //            StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_RS_RespostaStop.pStrIdStopStart).ToString(), " Stop aceito pelo MDS"));

        //            StopEnum = OrdemStopStatus.AceitoMDS;
        //        }

        //        // Atualiza status da ordem para aceito.
        //        _Servico.AtualizaOrdemStop(int.Parse(_RS_RespostaStop.pStrIdStopStart), (int)StopEnum);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(string.Format("EventMds_OnMDSSRespostaSolicitacaoEvent: Message: {0} \nStackTrace: {1} ", ex.Message, ex.StackTrace));
        //    }
        //}

        ///// <summary>
        ///// Evento que recebe uma resposta de execução de ordens do sistema MDS.
        ///// </summary>
        ///// <param name="sender">Objeto com os atributos da ordem </param>
        ///// <param name="e"></param>
        //void EventMds_OnMDSStopStartEvent(object sender, MDSEventArgs e)
        //{
        //    try
        //    {
        //        lastPongReceived = DateTime.Now;

        //        // Adiciona uma Thread no pool responsavel por executar o processamento do Stop.
        //        //Teste para saber da instabilidade da chamada de serviço quando MDS é execultado
        //        ThreadPool.QueueUserWorkItem(new WaitCallback(
        //                                delegate(object required)
        //                                {
        //                                    ProcessarEventoExecutouMDS((object)(sender));
        //                                }));
        //    }
        //    catch (Exception ex)
        //    {
        //        StopStartLog.LogInfo(string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
        //        //throw new Exception(string.Format("{0}{1}", "EventMds_OnMDSStopStartEvent: ", ex.Message));
        //    }
        //}

        ///// <summary>
        ///// Evento que recebe uma resposta de execução de ordens do sistema MDS.
        ///// </summary>
        ///// <param name="sender">Objeto com os atributos da ordem </param>
        ///// <param name="e"></param>
        //void EventMds_OnMDSSRespostaPingEvent(object sender, MDSEventArgs e)
        //{
        //    logger.Debug("Recebeu PONG: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
        //    lastPongReceived = DateTime.Now;
        //}
        /// <summary>
        /// Callback de respostas de mensagem de ordem Stop (Disparado)
        /// </summary>
        /// <param name="sender"> Objeto de resposta.</param>
        void ProcessarEventoExecutouMDS(object sender)
        {
            SS_StopStartResposta _SS_StopSimplesResposta =
                    (SS_StopStartResposta)(sender);

            try
            {
                // Remove o stop da lista de stops enviados ao MDS e mantidos em 
                // memoria 
                Formatador.RemoverStopExecutado(_SS_StopSimplesResposta.pStrIdStopStart);

                //Recupera os dados do Stop/Start para ser enviado na ordem
                AutomacaoOrdensInfo lDados = new AutomacaoOrdensDados().SelecionaOrdemStopStart(int.Parse(_SS_StopSimplesResposta.pStrIdStopStart));

                ClienteOrdemInfo lOrdem    = new ClienteOrdemInfo();

                lOrdem.CodigoCliente       = lDados.Account;
                lOrdem.DataHoraSolicitacao = DateTime.Now;
                
                lOrdem.DataValidade        = lDados.ExpireDate; //new DateTime(dtTemp.Year, dtTemp.Month, dtTemp.Day, 23, 59, 59);
                lOrdem.DirecaoOrdem        = ((int)lDados.IdStopStartTipo == (int)StopStartTipoEnum.StartCompra) ? OrdemDirecaoEnum.Compra : OrdemDirecaoEnum.Venda;
                lOrdem.Instrumento         = lDados.Symbol;

                logger.InfoFormat("{0} preço formatado {1}", lDados.SendStopLossValuePrice, lDados.SendStopLossValuePrice.Value.ToString("N2"));

                //Verifica qual o tipo de operação para Enviar o preço 
                //do ítem de stopstart certo
                switch ((StopStartTipoEnum)lDados.IdStopStartTipo)
                { 
                    case StopStartTipoEnum.StopLoss:
                        lOrdem.Preco = Convert.ToDouble(lDados.SendStopLossValuePrice);
                        break;

                    case StopStartTipoEnum.StopGain:
                        lOrdem.Preco = Convert.ToDouble(lDados.SendStopGainPrice);
                        break;

                    case StopStartTipoEnum.StartCompra:
                        lOrdem.Preco = Convert.ToDouble(lDados.SendStartPrice);
                        break;

                    case StopStartTipoEnum.StopMovel:
                        lOrdem.Preco = Convert.ToDouble(_SS_StopSimplesResposta.pStrPrecoReferencia, new CultureInfo("pt-BR"));
                        break;

                    case StopStartTipoEnum.StopSimultaneo:
                        {
                            double lValor = Convert.ToDouble(_SS_StopSimplesResposta.pStrPrecoReferencia, new CultureInfo("pt-BR"));

                            logger.InfoFormat("Reference Price Stop Simultaneo: {0}", lValor);

                            if ( Convert.ToDouble(lDados.StopGainValuePrice) <= lValor )
                                lOrdem.Preco = Convert.ToDouble(lDados.SendStopGainPrice);
                            
                            else if ( Convert.ToDouble(lDados.StopLossValuePrice ) >= lValor )
                                lOrdem.Preco = Convert.ToDouble(lDados.SendStopLossValuePrice);
                        }
                        break;
                }

                logger.InfoFormat("{0} preço formatado {1}", lOrdem.Preco, lOrdem.Preco.ToString("N2"));

                lOrdem.CodigoStopStart = int.Parse(_SS_StopSimplesResposta.pStrIdStopStart);
                lOrdem.Quantidade      = lDados.OrderQty;
                lOrdem.TipoDeOrdem     = OrdemTipoEnum.Limitada;
                lOrdem.ValidadeOrdem   = OrdemValidadeEnum.ValidaParaODia;

                if (!lDados.StopStartStatusID.Equals(5))
                {
                    //Envia a ordem para o roteador de ordens
                    this.EnviaServicoOrdem(lOrdem, lOrdem.CodigoStopStart);
                }

                StopStartLog.LogInfo(string.Format("{0}{1}", "\r\n " + int.Parse(_SS_StopSimplesResposta.pStrIdStopStart).ToString(), " StopStart Executado no MDS."));
            }
            catch (Exception ex)
            {
                new AutomacaoOrdensDados().CancelaOrdemStopStart(int.Parse( _SS_StopSimplesResposta.pStrIdStopStart), (int)OrdemStopStatus.CancelamentoAceitoMDS);
                logger.Error(string.Format("ProcessarEventoExecutouMDS Message:{1} - StackTrace: {2}\n", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region ||IServicoControlavel Members
        /// <summary>
        /// OnStart do Serviço 
        /// </summary>
        public virtual void IniciarServico()
        {
            bKeepRunning = true;

            try
            {
                logger.Info("Iniciando ServicoOrdemStopStart");
                    
                //EnviarMensagemAutenticacao();

                thrMonitor = new Thread(new ThreadStart(_monitorMDS));
                thrMonitor.Start();

                //TimerCallback CallBack = ValidarStatusConexaoMDS;

                //WaitOrTimerCallback lCallBackExpiracaoOrdem = new WaitOrTimerCallback(VerificaOrdemParaExpiracao);

                //ThreadPool.RegisterWaitForSingleObject(_autoEventVerificacao, lCallBackExpiracaoOrdem, null, this.TemporizadorIntervaloVerificacao, false);

                //_autoEventVerificacao.Set();

                //if (_StackTimerMDS == null)
                //{
                //    _StackTimerMDS = new Timer(CallBack, _autoEvent,0, 30000);

                //    logger.Info("Ao inicializar o serviço, entrou no ticker do timer == null para chamar com o callback");
                //}

                //ArmaStopStartNaoArmado();

                logger.Info("Serviço inicializado com sucesso");

                _ServicoStatus = ServicoStatus.EmExecucao;

            }
            catch (Exception ex)
            {
                logger.Error(string.Format("O Serviço de StopStart não foi inicializado - ERRO: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado ;
        }

        /// <summary>
        /// Retorna o estado do serviço
        /// </summary>
        /// <returns></returns>
        public virtual ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        #region Rotina de verificação
        /// <summary>
        /// Temporizador de verificação de ordens de expiração
        /// </summary>
        public int TemporizadorIntervaloVerificacao
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorIntervaloVerificacao"]);
            }
        }

        protected void _monitorMDS()
        {
            while (bKeepRunning)
            {
                ValidarStatusConexaoMDS();

                VerificaOrdemParaExpiracao();

                Thread.Sleep(TemporizadorIntervaloVerificacao);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="signaled"></param>
        protected void VerificaOrdemParaExpiracao()
        {
            logger.Info("Entrou no método de verificação de Expiração de ordens");

            try
            {

                AutomacaoOrdensDados dados = new AutomacaoOrdensDados();

                Dictionary<int, string> lIdStops = dados.VerificaOrdemEmDataExpiracao();

                foreach (KeyValuePair<int, string> stop in lIdStops)
                {
                    logger.Warn("Cancelando ordem expirada [" + stop.Key + "] [" + stop.Value + "] no MDS");

                    Formatador.CancelarOrdemStop(stop.Value, stop.Key);
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Erro em VerificaOrdemParaExpiracao - ERRO: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region IServicoStopStartAdm Members
        public void ConectarMDS()
        {
            logger.Info("Reabrindo conexao com MDS por solicitacao via ADM...");

            ValidarStatusConexaoMDS();
        }

        public void DesconectarMDS()
        {
            logger.Warn("Finalizando conexao com MDS por solicitacao via ADM!");
            this.mdssocket.CloseConnection();
            Autenticado = false;
        }

        public void TrocarServidorMDS(string host)
        {
            logger.Warn("Trocando servidor para [" + host + "] via ADM");

            // Remove a nova senha, no arquivo de configuracao inclusive
            Configuration stmconfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            stmconfig.AppSettings.Settings.Remove("ASConnMDSIp");
            stmconfig.AppSettings.Settings.Add("ASConnMDSIp", host);

            stmconfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public void LigarEnvioAlertas()
        {
            throw new NotImplementedException();
        }

        public void DesligarEnvioAlertas()
        {
            throw new NotImplementedException();
        }

        public string ObterServidorMDS()
        {
            string ret = ConfigurationManager.AppSettings["ASConnMDSIp"].ToString();

            return ret;
        }

        public bool IsConectado()
        {
            return mdssocket.IsConectado();
        }

        public DateTime LastPacket()
        {
            return lastPongReceived;
        }

        public bool RecadastrarStopsMDS()
        {
            logger.Warn("Reenviando STOPs cadastrados ao MDS");

            Formatador.ReenviarStops();

            _logWCFContext();

            return true;
        }

        public bool ArmarStopsNaoArmadosMDS()
        {
            logger.Info("Armando STOPs nao cadastrados no MDS");

            _logWCFContext();

            this.ArmaStopStartNaoArmado();

            return true;
        }

        public List<OrdemStopStartAdmInfo> ListarStopsArmados()
        {
            logger.Info("Listando STOPs cadastrados no MDS");

            List<SS_StopStart> lista = new List<SS_StopStart>();
            List<OrdemStopStartAdmInfo> retorno = new List<OrdemStopStartAdmInfo>();

            lista = Formatador.ListarStopsArmados();
            foreach (SS_StopStart stop in lista)
            {
                OrdemStopStartAdmInfo info = new OrdemStopStartAdmInfo();

                info.AjusteMovel = stop.AjusteMovel;
                info.CodigoInstrumento = stop.CodigoInstrumento;
                info.IdStopStart = stop.IdStopStart;
                info.IdTipoOrdem = stop.IdTipoOrdem;
                info.InicioMovel = stop.InicioMovel;
                info.PrecoGain = stop.PrecoGain;
                info.PrecoLoss = stop.PrecoLoss;
                info.PrecoStart = stop.PrecoStart;

                retorno.Add(info);
            }

            return retorno;
        }


        protected void _logWCFContext()
        {
            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties messageProperties = context.IncomingMessageProperties;

                foreach (KeyValuePair<string, object> entry in messageProperties)
                {
                    logger.Debug("messageProperties[" + entry.Key + "]=" + entry.Value.GetType().ToString());
                }
                RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                if (endpointProperty != null)
                {
                    logger.Info("End Origem ....: " + endpointProperty.Address);
                    logger.Info("Porta Origem ..: " + endpointProperty.Port);
                }

                SecurityMessageProperty securityProperty = messageProperties["Security"] as SecurityMessageProperty;

                if (securityProperty != null)
                {
                    if (securityProperty.HasIncomingSupportingTokens && securityProperty.IncomingSupportingTokens != null)
                    {
                        foreach (SupportingTokenSpecification token in securityProperty.IncomingSupportingTokens)
                        {
                            //logger.Debug("Tk " + token.SecurityToken);
                        }
                    }

                    if ( securityProperty.InitiatorToken != null )
                    {
                        //logger.Debug(" x " + securityProperty.InitiatorToken.SecurityTokenPolicies);
                    }

                    if (securityProperty.SenderIdPrefix != null)
                    {
                        logger.Debug(" y " + securityProperty.SenderIdPrefix);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("_logWCFContext(): " + ex.Message, ex);
            }
        }

        #endregion //IServicoStopStartAdm Members
    }
}
