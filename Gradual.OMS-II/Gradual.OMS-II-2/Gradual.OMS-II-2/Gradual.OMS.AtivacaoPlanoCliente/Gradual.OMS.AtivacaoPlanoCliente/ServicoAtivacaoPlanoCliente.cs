#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.OMS.AtivacaoPlanoCliente.Lib;
using System.Threading;
using System.ServiceModel;
using System.Configuration;
using Gradual.OMS.Email.Lib;
using Gradual.OMS.PlanoCliente.Lib;
using System.IO;
using Gradual.OMS.Library;

#endregion

namespace Gradual.OMS.AtivacaoPlanoCliente
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ServicoAtivacaoPlanoCliente : IServicoControlavel, IServicoAtivacaoPlanoCliente, IServicoPlanoClienteIntegracaoSinacor
    {
        #region  Propriedades

        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private WaitOrTimerCallback _gThreadAtivacaoCliente = null;

        private WaitOrTimerCallback _gThreadAtivacaoClienteDirect = null;

        private WaitOrTimerCallback _gThreadGeracaoArquivo = null;

        private ServicoStatus _ServicoStatus  = ServicoStatus.Indefinido;
        
        private PersistenciaDB DB = new PersistenciaDB();

        private CobrancaDB DBCobranca = new CobrancaDB();

        private AutoResetEvent lThreadEvent = new AutoResetEvent(false);

        private static readonly object gSingleton = new object();

        private static string _gUltimoArquivoGerado = string.Empty;

        public int TemporizadorIntervaloVerificacao
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorIntervaloVerificacao"]);
            }
        }

     
        private int TemporizadorGeradorArquivo
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["temporizadorGeradorArquivo"]);
            }
        }
        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                //_gThreadAtivacaoCliente = new WaitOrTimerCallback(VerificaPosicaoClienteSinacor);

                //_gThreadAtivacaoClienteDirect = new WaitOrTimerCallback(VerificaPosicaoClienteAssessorDirect);

                _gThreadGeracaoArquivo  = new WaitOrTimerCallback(GerarArquivoPeriodico);

                logger.Info("Iniciando a Thread de Ativação de Planos do cliente (Integração com o Solicitações - Sinacor)");

                //ThreadPool.RegisterWaitForSingleObject(lThreadEvent, _gThreadAtivacaoCliente, null, this.TemporizadorIntervaloVerificacao, false);

                //ThreadPool.RegisterWaitForSingleObject(lThreadEvent, _gThreadAtivacaoClienteDirect, null, this.TemporizadorIntervaloVerificacao, false);

                ThreadPool.RegisterWaitForSingleObject(lThreadEvent, _gThreadGeracaoArquivo, null, this.TemporizadorGeradorArquivo, false);

                lThreadEvent.Set();

                _ServicoStatus = ServicoStatus.EmExecucao;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro ao iniciar o serviço de AtivacaoPlanoCliente  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);                  
            }
        }

        public void PararServico()
        {
            try
            {
                _ServicoStatus = ServicoStatus.Parado;

                logger.InfoFormat("Parando o serviço de AtivacaoPlanoCliente");
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Parando o serviço de AtivacaoPlanoCliente  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        private void GerarArquivoPeriodico(object state, bool signaled)
        {
            Thread.Sleep(TemporizadorGeradorArquivo);

            GerarArquivoRequest lRequest = new GerarArquivoRequest();

            string[] lHoraVerif = ConfigurationManager.AppSettings["HoraVerificacaoArquivo"].ToString().Split(';');

            List<string> lHorasVerif = lHoraVerif.ToList<string>();

            lRequest.StSituacao= 'A';

            if (lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
            {
                logger.InfoFormat("Entrou na verificação do dia {0}", DateTime.Now.ToString("HH:mm"));
                
                string lNomeArquivoNovo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                if (!lNomeArquivoNovo.Equals(_gUltimoArquivoGerado))
                {
                    this.GerarArquivo(lRequest);

                    this.GerarArquivoTravelCard(lRequest);

                    lock (gSingleton)
                    {
                        this.EnviaNotificacaoArquivoProntoImportacao();
                    }

                    _gUltimoArquivoGerado = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";
                }
            }
        }

        #region IServicoAtivacaoPlanoCliente Members
        public void VerificaPosicaoClienteAssessorDirect(object state, bool signaled)
        {
            try
            {
                Thread.Sleep(TemporizadorIntervaloVerificacao);

                DB = new PersistenciaDB();

                logger.InfoFormat("Verificação de posição de cliente com Assessor Direct no sinacor");

                List<PlanoClienteInfo> lClientesEmEspera = DB.SelecionaClientesAssessorDirectEmEspera();

                List<PlanoClienteInfo> lEnvio = new List<PlanoClienteInfo>();

                PlanoClienteInfo lInfo = null;

                lClientesEmEspera.ForEach(delegate(PlanoClienteInfo info)
                {
                    if (info.CdCblc != null && info.CdCblc != 0)
                    {
                        logger.InfoFormat("Achou o cliente com o CBLC -  {0}", info.CdCblc);

                        lInfo                = new PlanoClienteInfo();
                        lInfo.DtOperacao     = DateTime.Now;
                        lInfo.CdCblc         = info.CdCblc;
                        lInfo.DtAdesao       = info.DtAdesao;
                        lInfo.DtAdesao       = DateTime.Now;
                        lInfo.StSituacao     = 'A';
                        lInfo.DsCpfCnpj      = info.DsCpfCnpj;
                        lInfo.IdProdutoPlano = info.IdProdutoPlano;
                        lInfo.DsEmail        = info.DsEmail;
                        lInfo.NomeCliente    = info.NomeCliente;
                        lEnvio.Add(lInfo);

                        this.LogAtivacaoPlanoCliente(info);
                    }
                });

                logger.InfoFormat("Encontrou {0} cliente(s) do assessor direct em espera para ativação", lEnvio.Count);
 
                lEnvio = DB.AtualizaPlanoClienteExistente(lEnvio); //--> Atualiza os clientes novos e já existentes

                lEnvio.ForEach(delegate(PlanoClienteInfo info)
                {
                    this.EnviaNotificacaoAtivacaoPlanoCliente(info);
                });
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Parando o serviço de VerificaPosicaoClienteDirect  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }
        /// <summary>
        /// Verifica se há clientes que foram incluídos no cadastro e podem já 
        /// ser atualizados nos planos como "ativados"
        /// </summary>
        public void VerificaPosicaoClienteSinacor(object state, bool signaled)
        {
            try
            {
                Thread.Sleep(TemporizadorIntervaloVerificacao);

                string[] lHoraVerif = ConfigurationManager.AppSettings["HoraVerificacao"].ToString().Split(';');

                List<string> lHorasVerif = lHoraVerif.ToList<string>();

                if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
                    return;

                DB = new PersistenciaDB();

                logger.InfoFormat("Verificação de posição de cliente no sinacor");

                List<PlanoClienteInfo> lClientesEmEspera = DB.SelecionaClientesEmEspera();

                logger.InfoFormat("Encontrou {0} cliente(s) em espera para ativação", lClientesEmEspera.Count);
                
                List<PlanoClienteInfo> lEnvio = new List<PlanoClienteInfo>();

                PlanoClienteInfo lInfo = null;

                lClientesEmEspera.ForEach(delegate(PlanoClienteInfo info)
                {
                    if (info.CdCblc != null && info.CdCblc != 0)
                    {
                        logger.InfoFormat("Achou o cliente com o CBLC -  {0}", info.CdCblc);

                        lInfo                = new PlanoClienteInfo();

                        lInfo.DtOperacao     = DateTime.Now;

                        lInfo.CdCblc         = info.CdCblc;

                        lInfo.DtAdesao       = info.DtAdesao;

                        lInfo.DtAdesao       = DateTime.Now;

                        lInfo.StSituacao     = 'A';

                        lInfo.DsCpfCnpj      = info.DsCpfCnpj;

                        lInfo.IdProdutoPlano = info.IdProdutoPlano;

                        lInfo.DsEmail        = info.DsEmail;

                        lInfo.NomeCliente    = info.NomeCliente;

                        lEnvio.Add(lInfo);

                        this.LogAtivacaoPlanoCliente(info);
                    }
                });
                    

                lEnvio = DB.AtualizaPlanoClienteExistente(lEnvio); //--> Atualiza os clientes novos e já existentes

                lEnvio.ForEach(delegate(PlanoClienteInfo info)
                {
                    this.EnviaNotificacaoAtivacaoPlanoCliente(info);
                });
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Parando o serviço de VerificaPosicaoClienteSinacor  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Envia e-mail para o cliente para com a notificação de ativação do plano 
        /// </summary>
        private void EnviaNotificacaoAtivacaoPlanoCliente(PlanoClienteInfo info)
        {
            try
            {
                var lServico = Ativador.Get<IServicoEmail>();

                logger.InfoFormat("Conseguiu instanciar o serviço Ativador.Get<IServicoEmail>");

                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailNotificacao"].ToString(), "EmailNotificacaoAtivacaoPlano.txt"));

                string lCorpoEmail = lStream.ReadToEnd();
                
                var lEmail                  = new EnviarEmailRequest();

                lEmail.Objeto               = new EmailInfo();

                lEmail.Objeto.Assunto       = "Notificação de ativação do DirectTrade";

                lEmail.Objeto.Destinatarios = new List<string>();
                
                lEmail.Objeto.Destinatarios.Add(info.DsEmail);

                logger.InfoFormat(string.Format("Enviando e-mail de notificação de ativação de plano para para {0}", info.DsEmail));

                lEmail.Objeto.Remetente     = ConfigurationManager.AppSettings["EmailRemetenteNotificacao"].ToString();

                lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("###NOME###", info.NomeCliente);

                logger.InfoFormat("Entrou no método de EnviaEmailAvisoPlanoCliente");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    logger.Info("Email disparado com sucesso");
                }
                else
                {
                    logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em EnviaEmailAvisoPlanoCliente  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region EnviaNotificacaoArquivoProntoImportacao
        public void EnviaNotificacaoArquivoProntoImportacao()
        {
            var lServico = Ativador.Get<IServicoEmail>();

            try
            {
                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailAviso"].ToString(), "NotificacaoGeracaoArquivo.txt"));

                string lCorpoEmail = lStream.ReadToEnd();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                string lPathArquivoCobranca = Path.Combine(ConfigurationManager.AppSettings["pathArquivoCobranca"].ToString(), DateTime.Now.ToString("yyyy"), lNomeArquivo);

                var lEmail    = new EnviarEmailRequest();
                
                lEmail.Objeto = new EmailInfo();

                lEmail.Objeto.Assunto = "Notificação de arquivo pronto para importação";

                lEmail.Objeto.Destinatarios = new List<string>() { ConfigurationManager.AppSettings["EmailDestinatarioNotificacaoArquivo"].ToString() };

                lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacaoArquivo"].ToString();

                lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("##ARQUIVO##", lNomeArquivo);

                EmailAnexoInfo lAnexo = new EmailAnexoInfo() 
                {
                    Arquivo = StreamArquivoImportacao(lPathArquivoCobranca),
                    Nome = lNomeArquivo
                };

                lEmail.Objeto.Anexos = new List<EmailAnexoInfo>() { lAnexo };

                logger.InfoFormat("Entrou no método de EnviarEmailAviso");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    logger.Info("Email disparado com sucesso");
                }
                else
                {
                    logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro no método de EnviarEmailAviso - Descrição: {0} - Stacktrace: {1}", ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region IServicoAtivacaoPlanoCliente Members

        public void AtualizaPosicaoPlanoCliente()
        {
            
        }

        public void EnviaEmailAvisoPlanoCliente()
        {
            
        }

        #endregion

        #region IServicoPlanoClienteIntegracaoSinacor Members

        public void GerarArquivo(GerarArquivoRequest pRequest)
        {
            try
            {
                DBCobranca = new CobrancaDB();

                string lPath = ConfigurationManager.AppSettings["pathArquivoCobranca"].ToString();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                GerarArquivoRequest lRequest = new GerarArquivoRequest();

                lRequest.StSituacao = 'A';

                List<string> lDetalhes = DBCobranca.ListarLinhasArquivo(lRequest);

                lock (gSingleton)
                {
                    StreamWriter lStream = new StreamWriter(Path.Combine(lPath,DateTime.Now.ToString("yyyy") , lNomeArquivo));

                    foreach (string linha in lDetalhes)
                        lStream.WriteLine(linha);

                    lStream.Close();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em GerarArquivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }
        
        public void GerarArquivoTravelCard(GerarArquivoRequest pRequest)
        {
            try
            {
                CobrancaDB lCobranca = new CobrancaDB();

                string lPath = ConfigurationManager.AppSettings["pathArquivoCobrancaTravelCard"].ToString();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                GerarArquivoRequest lRequest = new GerarArquivoRequest();

                lRequest.StSituacao = 'A';

                List<string> lDetalhes = lCobranca.ListarLinhasArquivoTravelCard(lRequest);

                lock (gSingleton)
                {
                    lPath = Path.Combine(lPath, DateTime.Now.ToString("yyyy"), lNomeArquivo);

                    if(!Directory.Exists(Path.GetDirectoryName(lPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(lPath));
                    }

                    StreamWriter lStream = new StreamWriter(lPath);

                    foreach (string linha in lDetalhes)
                        lStream.WriteLine(linha);

                    lStream.Close();
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em GerarArquivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Métodos de Apoio
        private byte[] StreamArquivoImportacao(string pNomeArquivo)
        { 
            FileStream lStream = new FileStream(pNomeArquivo, FileMode.Open, FileAccess.Read);

            byte[] arrBytes = new byte[lStream.Length];

            lStream.Read(arrBytes, 0, (int)lStream.Length);

            lStream.Close();

            return arrBytes;
        }

        /// <summary>
        /// Log de Email ativando
        /// </summary>
        /// <param name="info">Informações do cliente e do produto</param>
        private void LogAtivacaoPlanoCliente(PlanoClienteInfo info)
        {
            logger.InfoFormat("Email..............: {0}", info.DsEmail);
            logger.InfoFormat("Nome...............: {0}", info.NomeCliente);
            logger.InfoFormat("Id Do Produto......: {0}", info.IdProdutoPlano);
            logger.InfoFormat("CPFCNPJ............: {0}", info.DsCpfCnpj);
            logger.InfoFormat("Codigo CBLC........: {0}", info.CdCblc);
            logger.InfoFormat("Situação...........: {0}", info.StSituacao);
            logger.InfoFormat("Dt Operação........: {0}", info.DtOperacao);
            logger.InfoFormat("DT Adesão..........: {0}", info.DtAdesao);
        }
        #endregion
    }
}
