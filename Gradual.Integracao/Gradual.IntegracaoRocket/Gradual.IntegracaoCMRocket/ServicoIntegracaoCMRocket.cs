using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using Gradual.IntegracaoCMRocket.Lib;
using Gradual.IntegracaoCMRocket.Lib.Mensagens;
using System.Collections.Concurrent;
using Gradual.IntegracaoCMRocket.Lib.Dados;
using System.Threading;
using Gradual.OMS.Library;
using Newtonsoft.Json;
using Gradual.IntegracaoCMRocket.DB.Lib;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Configuration;
using System.Globalization;

namespace Gradual.IntegracaoCMRocket
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoIntegracaoCMRocket : IServicoControlavel, IServicoIntegracaoCMRocket
    {

        private ServicoStatus _status = ServicoStatus.Parado;
        private ConcurrentQueue<CMRocketProcessoInfo> filaRequisicaoRocket  = new ConcurrentQueue<CMRocketProcessoInfo>();
        private ConcurrentQueue<CMRocketProcessoInfo> filaMonitoracaoRocket = new ConcurrentQueue<CMRocketProcessoInfo>();
        private ConcurrentQueue<CMRocketProcessoInfo> filaRelatorioRocket   = new ConcurrentQueue<CMRocketProcessoInfo>();
        private ConcurrentQueue<CMRocketProcessoInfo> filaExportacao        = new ConcurrentQueue<CMRocketProcessoInfo>();

        private Thread thSolicitacaoRocket  = null;
        private bool bKeepRunning           = false;

        private string RocketEmpresa        = ConfigurationManager.AppSettings["RocketEmpresa"].ToString();
        private string RocketFluxo          = ConfigurationManager.AppSettings["RocketFluxo"].ToString();
        private string RocketUsuario        = ConfigurationManager.AppSettings["RocketUsuario"].ToString();
        private string RocketSenha          = ConfigurationManager.AppSettings["RocketSenha"].ToString();
        private string RocketURL            = ConfigurationManager.AppSettings["RocketURL"].ToString();

        private Thread thExportacaoPendencia = null;

        #region IServicoControlavel Members
        public void IniciarServico()
        {
            try
            {
                Gradual.Utils.Logger.Initialize();

                Gradual.Utils.Logger.Log("Monitor", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando serviço"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                bKeepRunning = true;
                
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Carregando processos pendentes"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                PersistenciaDB db = new PersistenciaDB();

                List<CMRocketProcessoInfo> processos = db.CarregarProcessosPendentes();

                foreach (CMRocketProcessoInfo processo in processos)
                {
                    if (processo.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_ESPERA))
                        filaRequisicaoRocket.Enqueue(processo);

                    if ( processo.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_SOLICITADO) ||
                        processo.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_EM_PROCESSAMENTO))
                        filaMonitoracaoRocket.Enqueue(processo);

                    if (processo.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_DISPONIVEL))
                        filaRelatorioRocket.Enqueue(processo);
                }
                
                thSolicitacaoRocket = new Thread(new ThreadStart(monitorSolicitacoesRocket));
                thSolicitacaoRocket.Start();

                thExportacaoPendencia = new Thread(new ThreadStart(procExportPend));
                thExportacaoPendencia.Start();


                _status = ServicoStatus.EmExecucao;

                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Serviço inciado"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }
        }

        public void PararServico()
        {
            Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Requisição de finalização recebida"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            bKeepRunning = false;


            while (thSolicitacaoRocket != null && thSolicitacaoRocket.IsAlive)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Aguardando finalizar thread de monitoracao dos processos Rocket"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                Thread.Sleep(1000);
            }

            while (thExportacaoPendencia != null && thExportacaoPendencia.IsAlive)
            {

                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Aguardando finalizar thread de exportacao ou pendencia"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                Thread.Sleep(1000);
            }

                
            _status = ServicoStatus.Parado;

            Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Serviço parado"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _status;
        }
        #endregion //IServicoControlavel Members

        #region IServicoIntegracaoCMRocket

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidarCadastroResponse ValidarCadastro(ValidarCadastroRequest request)
        {
            ValidarCadastroResponse response = new ValidarCadastroResponse();

            try
            {
                OperationContext context                = OperationContext.Current;
                MessageProperties prop                  = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint  = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

                string ip = endpoint.Address;

                
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("ValidarCadastro para CPF [{0}]  IP[{1}]", request.CamposRocket.CPF, ip)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });


                CMRocketProcessoInfo info = new CMRocketProcessoInfo();

                info.Cpf                        = request.CamposRocket.CPF;
                info.DataAtualizacao            = DateTime.Now;
                info.DataSolicitacaoRocket      = DateTime.Today;
                info.DataSolicitacaoIntranet    = DateTime.Now;
                info.StatusProcesso             = CMRocketProcessoInfo.STATUS_ROCKET_ESPERA;
                info.JsonInput                  = JsonConvert.SerializeObject(request.CamposRocket);
                info.IPOrigem                   = ip;

                PersistenciaDB db               = new PersistenciaDB();
                int idCapivara                  = db.InserirProcessoRocket(info);
                info.IDCapivara                 = idCapivara;
                response.IDCapivara             = idCapivara;

                
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Processo do CPF [{0}] inserido na base com ID [{1}]", info.Cpf, info.IDCapivara)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                filaRequisicaoRocket.Enqueue(info);

                
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Processo do CPF [{0}] enfileirado, aguardando solicitacao de processamento", info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                response.DescricaoResposta = ex.Message;
            }

            response.StatusResposta = MensagemResponseStatusEnum.OK;

            return response;
        }

        /// <summary>
        /// Retorna todos os processos rocket do cnpj/cpf informado
        /// </summary>
        /// <param name="request">cnpj e data de nascimento/fundacao</param>
        /// <returns></returns>
        public ObterRelatorioResponse ObterRelatorioPorCPFCNPJ(ObterRelatorioRequest request)
        {
            ObterRelatorioResponse response = new ObterRelatorioResponse();

            try
            {
                PersistenciaDB db = new PersistenciaDB();
                List<CMRocketProcessoInfo> capivaras = db.ObterRelatorios(request.CpfCnpj, request.DataNascFund);

                response.Capivaras = capivaras;
                response.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return response;
        }

        public ObterRelatorioDetalhadoResponse ObterRelatorios(ObterRelatorioDetalhadoRequest pParametros)
        {
            ObterRelatorioDetalhadoResponse response = new ObterRelatorioDetalhadoResponse();

            try
            {
                PersistenciaDB db = new PersistenciaDB();

                List<CMRocketProcessoDetalheInfo> capivaras = db.ObterRelatorios(pParametros);
                response.Capivaras = capivaras;
                response.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ObterDetalheRelatorioResponse ObterDetalhamentoRelatorio(ObterDetalheRelatorioRequest request)
        {
            ObterDetalheRelatorioResponse response = new ObterDetalheRelatorioResponse();
            try
            {
                PersistenciaDB db = new PersistenciaDB();

                string json = db.ObterDetalhamento(request.idCapivara);
                CMRocketReport report = JsonConvert.DeserializeObject<CMRocketReport>(json);

                response.Relatorio = report;
                response.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch(Exception ex)
            {
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return response;
        }

        public ObterEvolucaoProcessoResponse ObterEvolucaoProcesso(ObterEvolucaoProcessoRequest request)
        {
            ObterEvolucaoProcessoResponse response = new ObterEvolucaoProcessoResponse();

            try
            {
                PersistenciaDB db = new PersistenciaDB();

                List<CMRocketEvolucaoProcessoInfo> evolucoes = db.ObterEvolucaoProcesso(request.IDCapivara);

                response.Evolucao = evolucoes;
                response.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return response;
        }

        #endregion IServicoIntegracaoCMRocket

        #region Privates
        /// <summary>
        /// 
        /// </summary>
        private void monitorSolicitacoesRocket()
        {

            Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), "Iniciando monitorSolicitacoesRocket()"), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

            CMRocketProcessoInfo info = null;
            long lastLoop = 0;
            // O loop de processamento trata primeiro todas as solicitacoes de processamento,
            // entao todos os acompanhamentos de status, e por fim, em obter todos os processos
            // disponibilizados pelo rocket 
            while (bKeepRunning)
            {
                try
                {
                    TimeSpan sp = new TimeSpan(DateTime.Now.Ticks - lastLoop);
                    if ( (filaMonitoracaoRocket.Count == 0 &&
                        filaRequisicaoRocket.Count == 0 &&
                        filaRelatorioRocket.Count == 0 ) || sp.TotalSeconds < 15 )
                    {
                        Thread.Sleep(250);
                        continue;
                    }

                    lastLoop = DateTime.Now.Ticks;
                    
                
                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Fila de processos para enviar ao rocket com {0} itens", filaRequisicaoRocket.Count)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                    for (int i = 0; i < filaRequisicaoRocket.Count; i++)
                    {
                        if (this.filaRequisicaoRocket.TryDequeue(out info))
                        {
                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - info.DataSolicitacaoRocket.Ticks);
                            if (info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_ESPERA) &&
                                ts.TotalMinutes > 3)
                            {

                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Solicitando processamento do CPF [{0}]", info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                DateTime dtEvolStart = DateTime.Now;
                                if (solicitarProcessamentoRocket(ref info))
                                {

                                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Processamento solicitado com sucesso CPF [{0}] HASH [{1}] TICKET [{2}]", info.Cpf, info.Hash, info.Ticket)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                    info.StatusProcesso = CMRocketProcessoInfo.STATUS_ROCKET_SOLICITADO;

                                    info.IDPendenciaCadastral = incluirPendenciaCadastral(info);
                                }
                                DateTime dtEvolStop = DateTime.Now;

                                info.DataAtualizacao = DateTime.Now;
                                info.DataSolicitacaoRocket = DateTime.Now;

                                PersistenciaDB db = new PersistenciaDB();
                                db.AtualizarProcessoRocket(info);
                                db.InserirEvolucaoProcessoRocket(info.IDCapivara, dtEvolStart, dtEvolStop, info.StatusProcesso);

                                // Coloca o processo na fila de monitoracao do status se a solicitacao
                                // foi feita com sucesso
                                if (info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_SOLICITADO))
                                {
                                    filaMonitoracaoRocket.Enqueue(info);
                                    continue;
                                }
                            }
                            filaRequisicaoRocket.Enqueue(info);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }

                
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Fila de processos para pesquisar status com {0} itens", filaMonitoracaoRocket.Count)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                try
                {
                    for (int i = 0; i < filaMonitoracaoRocket.Count; i++)
                    {
                        if (this.filaMonitoracaoRocket.TryDequeue(out info))
                        {
                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - info.DataAtualizacao.Ticks);
                            if ((info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_SOLICITADO) ||
                                  info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_EM_PROCESSAMENTO)) &&
                                  ts.TotalMinutes > 1)
                            {

                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Solicitando status do processamento do CPF [{0}]", info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                DateTime dtEvolStart = DateTime.Now;
                                if (solicitarStatusRocket(ref info))
                                {

                                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Status com sucesso CPF [{0}] HASH [{1}] TICKET [{2}] STATUS[{3}]", info.Cpf, info.Hash, info.Ticket, info.StatusProcesso)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                    DateTime dtEvolStop = DateTime.Now;
                                    info.DataAtualizacao = DateTime.Now;
                                    PersistenciaDB db = new PersistenciaDB();
                                    db.AtualizarProcessoRocket(info);
                                    db.InserirEvolucaoProcessoRocket(info.IDCapivara, dtEvolStart, dtEvolStop, info.StatusProcesso);

                                    //Se estiver disponivel, coloca na outra fila
                                    if (info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_DISPONIVEL))
                                    {
                                        filaRelatorioRocket.Enqueue(info);
                                        continue;
                                    }
                                }
                            }
                            filaMonitoracaoRocket.Enqueue(info);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }

                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Fila de processos disponibilizados com {0} itens", filaRelatorioRocket.Count)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });

                try
                {
                    for (int i = 0; i < filaRelatorioRocket.Count; i++)
                    {
                        if (this.filaRelatorioRocket.TryDequeue(out info))
                        {
                            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - info.DataAtualizacao.Ticks);
                            if (info.StatusProcesso.Equals(CMRocketProcessoInfo.STATUS_ROCKET_DISPONIVEL) &&
                                  ts.TotalMinutes > 1)
                            {

                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Solicitando relatorio do processamento do CPF [{0}]", info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                DateTime dtEvolStart = DateTime.Now;
                                if (solicitarRelatorioRocket(ref info))
                                {

                                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Relatorio obtido com sucesso CPF [{0}] HASH [{1}] TICKET [{2}] STATUS[{3}]", info.Cpf, info.Hash, info.Ticket, info.StatusProcesso)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                    info.StatusProcesso = CMRocketProcessoInfo.STATUS_ROCKET_RELATORIO_FINALIZADO;

                                    info.DataAtualizacao = DateTime.Now;
                                    PersistenciaDB db = new PersistenciaDB();
                                    db.AtualizarProcessoRocket(info);
                                    db.InserirEvolucaoProcessoRocket(info.IDCapivara, dtEvolStart, DateTime.Now, info.StatusProcesso);

                                    //Exporta ou cadastra pendencia
                                    filaExportacao.Enqueue(info);

                                    continue;
                                }
                            }
                            filaRelatorioRocket.Enqueue(info);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool solicitarRelatorioRocket(ref CMRocketProcessoInfo info)
        {
            bool bRet = false;

            try
            {
                com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG objRequisicaoRocket = new com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG();

                // Gambiarra master para preencher todos os campos
                // do car**** do SOAP, mesmo se nao viermos a utilizar
                foreach (PropertyInfo prop in objRequisicaoRocket.GetType().GetProperties())
                {
                    if (prop.PropertyType.Equals(typeof(String)))
                        prop.SetValue(objRequisicaoRocket, String.Empty, null);
                }

                CMRocketFields camposRocket = JsonConvert.DeserializeObject<CMRocketFields>(info.JsonInput);
                Utilities.CopyPropertiesAsPossible(camposRocket, objRequisicaoRocket);

                com.cmsw.wsrocket.RocketProcessWS rckCli = new com.cmsw.wsrocket.RocketProcessWS();

                rckCli.AllowAutoRedirect = true;

                rckCli.Url = RocketURL; ;


                //CMSoftware.Rocket.statusProcess stproc = new CMSoftware.Rocket.statusProcess();

                //stproc.hash = "aaa";
                //stproc.ticket = "ticket";

                com.cmsw.wsrocket.ProcessHeaderVo xxx = new com.cmsw.wsrocket.ProcessHeaderVo();

                xxx.empresa = RocketEmpresa;
                xxx.fluxo = RocketFluxo;
                xxx.senha = RocketSenha;
                xxx.usuario = RocketUsuario;
                xxx.hash = info.Hash;
                xxx.ticket = info.Ticket;

                objRequisicaoRocket.header = xxx;

                com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOGResponse rsp = rckCli.WS_VALIDACAO_CADASTRAL_HOMOLOG(objRequisicaoRocket);

                com.cmsw.wsrocket.rocketWSReturn ret = rsp.retorno;

                com.cmsw.wsrocket.ProcessReturnVo retObj = ret.Item as com.cmsw.wsrocket.ProcessReturnVo;

                if (retObj != null)
                {
                    info.Ticket = retObj.ticket;
                    info.Hash = retObj.hash;

                    CMRocketReport report = new CMRocketReport();

                    com.cmsw.wsrocket.variavel[] variaveisContexto  = retObj.variaveisContexto;
                    com.cmsw.wsrocket.provedor[] provedores = retObj.provedores;

                    if (variaveisContexto != null )
                    {
                        foreach (com.cmsw.wsrocket.variavel variavel in variaveisContexto)
                        {
                            CMRocketContext varContx = new CMRocketContext();
                            varContx.IDOutput = Convert.ToInt32(variavel.idOutput);
                            varContx.Nome = variavel.nome;
                            varContx.TipoCampo = Convert.ToInt32(variavel.tipoCampo);
                            varContx.Valor = variavel.valor;
                            
                            report.Contextos.Add( varContx );

                            if (varContx.Nome.Equals("REGRA_APROVACAO_AUTOMATICA"))
                            {
                                
                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Flag de aprovacao automatica para CPF [{0}] [{1}]", info.Cpf, varContx.Valor)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                if (!String.IsNullOrEmpty(varContx.Valor) && varContx.Valor.Equals("APROVADO"))
                                    info.AprovacaoAutomatica = true;
                            }

                            if (varContx.Nome.Equals("ID_PROCESSO"))
                            {
                                
                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("ID_PROCESSO para CPF [{0}] [{1}]", info.Cpf, varContx.Valor)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                info.IDProcessoRocket = Convert.ToInt32(varContx.Valor);
                            }

                            if (varContx.Nome.Equals("ID_WORK_PROCESSO"))
                            {
                                
                                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("ID_WORK_PROCESSO para CPF [{0}] [{1}]", info.Cpf, varContx.Valor)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                                info.IDWorkProcessoRocket = Convert.ToInt32(varContx.Valor);
                            }
                        }
                    }

                    if (provedores != null)
                    {
                        foreach (com.cmsw.wsrocket.provedor provedor in provedores)
                        {
                            CMRocketProvider provider = new CMRocketProvider();

                            provider.IDOutput = Convert.ToInt32(provedor.idOutput);
                            provider.IDProvedor = Convert.ToInt32(provedor.idProvedor);
                            provider.Key = provedor.key;
                            provider.Nome = provedor.nome;

                            if (provedor.variaveisOut != null && provedor.variaveisOut.Length > 0)
                            {
                                foreach (com.cmsw.wsrocket.variavel variavel in provedor.variaveisOut)
                                {
                                    CMVariaveisOut varOut = new CMVariaveisOut();
                                    varOut.IDOutput = Convert.ToInt32(variavel.idOutput);
                                    varOut.Nome = variavel.nome;
                                    varOut.TipoCampo = Convert.ToInt32(variavel.tipoCampo);
                                    varOut.Valor = variavel.valor;

                                    provider.VariaveisOut.Add(varOut);
                                }
                            }

                            if (provedor.listas != null && provedor.listas.Length > 0)
                            {
                                foreach (com.cmsw.wsrocket.lista lista in provedor.listas)
                                {
                                    CMListas list = new CMListas();

                                    list.Chave = lista.chave;
                                    list.IDArray = Convert.ToInt32(lista.idArray);
                                    list.IDOutput = Convert.ToInt32(lista.idListOutput);

                                    foreach (com.cmsw.wsrocket.registro registro in lista.registros)
                                    {
                                        Dictionary<string, CMColunas> record = new Dictionary<string, CMColunas>();

                                        foreach (com.cmsw.wsrocket.coluna coluna in registro.colunas)
                                        {
                                            CMColunas column = new CMColunas();

                                            column.Chave = coluna.chave;
                                            column.IDCampo = Convert.ToInt32(coluna.idCampo);
                                            column.Desc = coluna.desc;

                                            record.Add(column.Chave, column);
                                        }

                                        list.Registros.Add(record);
                                    }
                                }
                            }

                        }
                    }

                    info.JsonOutput = JsonConvert.SerializeObject(report);
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return bRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool solicitarStatusRocket(ref CMRocketProcessoInfo info)
        {
            bool bRet = false;

            try
            {
                com.cmsw.wsrocket.statusProcess objRequisicaoRocket = new com.cmsw.wsrocket.statusProcess();

                // Gambiarra master para preencher todos os campos
                // do car**** do SOAP, mesmo se nao viermos a utilizar
                foreach (PropertyInfo prop in objRequisicaoRocket.GetType().GetProperties())
                {
                    if (prop.PropertyType.Equals(typeof(String)))
                        prop.SetValue(objRequisicaoRocket, String.Empty, null);
                }

                objRequisicaoRocket.hash = info.Hash;
                objRequisicaoRocket.ticket = info.Ticket;

                com.cmsw.wsrocket.RocketProcessWS rckCli = new com.cmsw.wsrocket.RocketProcessWS();

                rckCli.AllowAutoRedirect = true;

                rckCli.Url = RocketURL;

                com.cmsw.wsrocket.statusProcessResponse rsp = rckCli.statusProcess(objRequisicaoRocket);

                info.StatusProcesso = rsp.status_processo;

                bRet = true;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return bRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private bool solicitarProcessamentoRocket( ref CMRocketProcessoInfo info )
        {
            bool bRet = false;

            try
            {
                com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG objRequisicaoRocket = new com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOG();

                // Gambiarra master para preencher todos os campos
                // do car**** do SOAP, mesmo se nao viermos a utilizar
                foreach (PropertyInfo prop in objRequisicaoRocket.GetType().GetProperties())
                {
                    if (prop.PropertyType.Equals(typeof(String)))
                        prop.SetValue(objRequisicaoRocket, String.Empty, null);
                }

                CMRocketFields camposRocket = JsonConvert.DeserializeObject<CMRocketFields>(info.JsonInput);
                Utilities.CopyPropertiesAsPossible(camposRocket, objRequisicaoRocket);

                com.cmsw.wsrocket.RocketProcessWS rckCli = new com.cmsw.wsrocket.RocketProcessWS();

                rckCli.AllowAutoRedirect = true;

                rckCli.Url = RocketURL;


                //CMSoftware.Rocket.statusProcess stproc = new CMSoftware.Rocket.statusProcess();

                //stproc.hash = "aaa";
                //stproc.ticket = "ticket";

                com.cmsw.wsrocket.ProcessHeaderVo xxx = new com.cmsw.wsrocket.ProcessHeaderVo();

                xxx.empresa = RocketEmpresa;
                xxx.fluxo = RocketFluxo;
                xxx.senha = RocketSenha;
                xxx.usuario = RocketUsuario;
                xxx.hash = info.Hash;
                xxx.ticket = info.Ticket;

                objRequisicaoRocket.header = xxx;

                com.cmsw.wsrocket.WS_VALIDACAO_CADASTRAL_HOMOLOGResponse rsp = rckCli.WS_VALIDACAO_CADASTRAL_HOMOLOG(objRequisicaoRocket);

                com.cmsw.wsrocket.rocketWSReturn ret = rsp.retorno;

                com.cmsw.wsrocket.ProcessReturnVo retObj = ret.Item as com.cmsw.wsrocket.ProcessReturnVo;

                if (retObj != null)
                {
                    info.Ticket = retObj.ticket;
                    info.Hash = retObj.hash;
                }

                bRet = true;
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return bRet;
        }
        #endregion //Privates


        /// <summary>
        /// 
        /// </summary>
        private void procExportPend()
        {
            while (bKeepRunning || filaExportacao.Count > 0)
            {
                CMRocketProcessoInfo info = null;

                if (filaExportacao.TryDequeue(out info))
                {

                    // Segundo conversa com RSG em 2016-12-20
                    // Marcar a pendencia cadastral Rocket como resolvido
                    if (info.AprovacaoAutomatica)
                    {
                       
                        Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Capivara [{0}] do CPF [{1}] reprovado, removendo pendencia cadastral Rocket", info.IDCapivara, info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                        PersistenciaDB db = new PersistenciaDB();

                        if (info.IDPendenciaCadastral > 0)
                            db.ResolverPendenciaCadastral(info.IDPendenciaCadastral);

                        Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Info, String.Format("{0}: {1}", Gradual.Utils.MethodHelper.GetCurrentMethod(), String.Format("Capivara [{0}] do CPF [{1}] eleito para exportacao automatica", info.IDCapivara, info.Cpf)), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment });
                    }
                    else
                    {
                    }
                    continue;
                }

                Thread.Sleep(250);
            }
        }

        private int incluirPendenciaCadastral(CMRocketProcessoInfo info)
        {
            int retorno = -1;
            try
            {
                if (ConfigurationManager.AppSettings["DescricaoPendenciaRocket"] == null ||
                    ConfigurationManager.AppSettings["IDPendenciaRocket"] == null)
                    return retorno;

                int idPendenciaRocket = Convert.ToInt32(ConfigurationManager.AppSettings["IDPendenciaRocket"].ToString());
                string descPendenciaRocket = ConfigurationManager.AppSettings["DescricaoPendenciaRocket"].ToString();

                PersistenciaDB db = new PersistenciaDB();

                CMRocketFields  campos = JsonConvert.DeserializeObject<CMRocketFields>(info.JsonInput);

                DateTime dataNascFund;
                    
                if ( DateTime.TryParseExact(campos.DATA_NASCIMENTO, "dd/MM/yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dataNascFund) )
                {
                    Tuple<int, int> cliente = db.ObterIDCliente(info.Cpf, dataNascFund);

                    retorno = db.InserirPendenciaCadastral( cliente.Item1, cliente.Item2, idPendenciaRocket, descPendenciaRocket );
                }
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return retorno;
        }

        public ObterDescricoesResponse ObterDescricoes(ObterDescricoesRequest pParametros)
        {
            ObterDescricoesResponse response = new ObterDescricoesResponse();

            try
            {
                PersistenciaDB db = new PersistenciaDB();

                List<CMRocketDescricao> lDescricoes = db.ObterDescricoes(pParametros);
                response.Descricoes     = lDescricoes;
                response.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                response.StatusResposta = MensagemResponseStatusEnum.ErroPrograma;
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return response;
        }

        public InserirSolicitacaoDocumentacaoResponse InserirSolicitacaoDocumentos(InserirSolicitacaoDocumentacaoRequest pParametros)
        {

            InserirSolicitacaoDocumentacaoResponse lRetorno = new InserirSolicitacaoDocumentacaoResponse();

            try
            {
                PersistenciaDB db = new PersistenciaDB();

                lRetorno = db.InserirSolicitacaoDocumentos(pParametros);
            }
            catch (Exception ex)
            {
                Gradual.Utils.Logger.Log("Servico", Gradual.Utils.LoggingLevel.Error, Gradual.Utils.MethodHelper.GetCurrentMethod(), new { User = Gradual.Utils.Settings.User, Environment = Gradual.Utils.Settings.Environment }, ex);
            }

            return lRetorno;
        }
    }
}
