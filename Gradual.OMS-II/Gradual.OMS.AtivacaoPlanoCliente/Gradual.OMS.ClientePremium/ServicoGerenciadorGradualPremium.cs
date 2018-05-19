using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gradual.OMS.Library;
using System.ServiceModel;
using Gradual.OMS.Library.Servicos;
using log4net;
using System.Threading;
using System.Configuration;
using Gradual.OMS.ClientePremium.Lib;
using Gradual.OMS.ClientePremium.DB;
using Gradual.OMS.Email.Lib;

namespace Gradual.OMS.ClientePremium
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class ServicoGerenciadorGradualPremium : IServicoControlavel, IServicoGerenciadorGradualPremium
    {
        #region Propriedades
        private readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private PersistenciaDB gDB = new PersistenciaDB();

        private ServicoStatus gServicoStatus = ServicoStatus.Indefinido;

        private AutoResetEvent gThreadEvent = new AutoResetEvent(false);

        public string[] gHoraGravacaoArquivoEstorno
        {
            get
            {
                return ConfigurationManager.AppSettings["HoraGravacaoArquivoEstorno"].ToString().Split(';');
            }
        }

        public int gTemporizadorIntervaloVerificacao
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["TemporizadorIntervaloVerificacao"]);
            }
        }

        public string gPathArquivoEstornoCustodia
        {
            get
            {
                return ConfigurationManager.AppSettings["pathArquivoEstornoCustodia"].ToString();
            }
        }

        public int gDiaMesFechamentoCustodia
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DiaMesFechamentoCustodia"].ToString());
            }
        }

        public string[] gHoraVerificacaoCorretagem
        {
            get
            {
                return ConfigurationManager.AppSettings["HoraVerificacaoCorretagem"].ToString().Split(';');
            }
        }

        public string[] gHoraVerificacaoCustodia
        {
            get
            {
                return ConfigurationManager.AppSettings["HoraVerificacaoCustodia"].ToString().Split(';');
            }
        }
        private static object gSingleton = new object();
        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                gLogger.Info("Iniciando serviço ServicoGerenciadorGradualPremium");

                ThreadPool.RegisterWaitForSingleObject(gThreadEvent, new WaitOrTimerCallback(RemoverCustodiaCliente), null, this.gTemporizadorIntervaloVerificacao, false);

                ThreadPool.RegisterWaitForSingleObject(gThreadEvent, new WaitOrTimerCallback(ReativarCustodiaCliente), null, this.gTemporizadorIntervaloVerificacao, false);

                //ThreadPool.RegisterWaitForSingleObject(gThreadEvent, new WaitOrTimerCallback(ReativarCorretagemCliente), null, this.gTemporizadorIntervaloVerificacao, false);

                //ThreadPool.RegisterWaitForSingleObject(gThreadEvent, new WaitOrTimerCallback(InativarCorretagemCliente), null, this.gTemporizadorIntervaloVerificacao, false);

                ThreadPool.RegisterWaitForSingleObject(gThreadEvent, new WaitOrTimerCallback(GerarArquivoEstornoCustodia), null, this.gTemporizadorIntervaloVerificacao, false);

                gServicoStatus = ServicoStatus.EmExecucao;

            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao iniciar o serviço de ServicoGerenciadorGradualPremium  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
            
        }

        public void PararServico()
        {
            try
            {
                gServicoStatus = ServicoStatus.Parado;
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro ao Parar o serviço de ServicoGerenciadorGradualPremium  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return gServicoStatus;
        }

        #endregion
        
        #region Gerenciamento Custódia
        public void RemoverCustodiaCliente(object state, bool signaled)
        {
            Thread.Sleep(gTemporizadorIntervaloVerificacao);

            string[] lHoraVerif = gHoraVerificacaoCustodia;

            List<string> lHorasVerif = lHoraVerif.ToList<string>();

            if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
            {
                return;
            }

            List<ClienteGradualPremiumInfo> lListClientes = gDB.SelecionarClienteParaTirarCustodia();

            gLogger.InfoFormat("Encontrou {0} clientes para retirar a custódia.", lListClientes.Count);

            gDB.AtualizaCustodiaSinacor(lListClientes, true);
        }

        public void ReativarCustodiaCliente(object state, bool signaled)
        {
            Thread.Sleep(gTemporizadorIntervaloVerificacao);

            string[] lHoraVerif = gHoraVerificacaoCustodia;

            List<string> lHorasVerif = lHoraVerif.ToList<string>();

            int lDiaAtual = Convert.ToInt32(DateTime.Now.ToString("dd"));

            int lUltimoDiaMes = Convert.ToInt32(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("dd"));

            if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")) || lUltimoDiaMes != lDiaAtual)
            {
                return;
            }

            List<ClienteGradualPremiumInfo> lListClientes = gDB.SelecionarClienteParaReativarCustodia();

            gLogger.InfoFormat("Encontrou {0} clientes para reativar a custódia.", lListClientes.Count);

            gDB.AtualizaCustodiaSinacor(lListClientes, false);
        }

        public void GerarArquivoEstornoCustodia(object state, bool signaled)
        {
            try
            {
                EstornoDB lDbEstorno = new EstornoDB();

                string lPath = gPathArquivoEstornoCustodia;

                int lDiaAtual = Convert.ToInt32(DateTime.Now.ToString("dd"));

                int lUltimoDiaMes = Convert.ToInt32( new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("dd"));

                string[] lHoraVerif = this.gHoraGravacaoArquivoEstorno;

                List<string> lHorasVerif = lHoraVerif.ToList<string>();

                if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    return;
                }

                if (lDiaAtual == lUltimoDiaMes)
                {
                    string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                    List<string> lRetorno = new List<string>();

                    lRetorno.Add("00OUTROS  OUT".PadRight(250, ' '));

                    lRetorno.AddRange(lDbEstorno.MontaDetalheArquivoEstornoCustodia());

                    lRetorno.Add("99OUTROSOUT".PadRight(250, ' '));

                    lock (gSingleton)
                    {
                        StreamWriter lStream = new StreamWriter(Path.Combine(lPath, DateTime.Now.ToString("yyyy"), lNomeArquivo));

                        foreach (string linha in lRetorno)
                        {
                            lStream.WriteLine(linha);
                        }

                        lStream.Close();

                        this.EnviaNotificacaoArquivoProntoImportacao();
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro em GerarArquivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }
        #endregion
        
        #region Gerenciamento Corretagem
        /// <summary>
        /// Verifica se o cliente já emitiu mais de 5 ordens no mês e reativa a corretagem 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="signaled"></param>
        public void ReativarCorretagemCliente(object state, bool signaled)
        {
            Thread.Sleep(gTemporizadorIntervaloVerificacao);

            //string[] lHoraVerif = this.gHoraVerificacaoCorretagem;

            //List<string> lHorasVerif = lHoraVerif.ToList<string>();

            //if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
            //{
            //    return;
            //}

            List<ClienteGradualPremiumInfo> lListClientes = gDB.SelecionarClienteParaAtivarCorretagem();

            gLogger.InfoFormat("Encontrou {0} clientes que já efetuaram mais de 5 ordens", lListClientes.Count);
            
            //gDB.AtualizaCorretagemSinacor(lListClientes, true, 1);
        }

        /// <summary>
        /// Rotina para rodar no último dia do mês para inativar a 
        /// corretagem de todos os clientes que tiveram a corretagem 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="signaled"></param>
        public void InativarCorretagemCliente(object state, bool signaled)
        {
            Thread.Sleep(gTemporizadorIntervaloVerificacao);

            string[] lHoraVerif = this.gHoraVerificacaoCorretagem;

            List<string> lHorasVerif = lHoraVerif.ToList<string>();

            //Somente é possível rodar o método no último dia do mês
            DateTime lHoje= DateTime.Today;

            DateTime lUltimoDiaDesseMes = new DateTime(lHoje.Year, lHoje.Month, 1).AddMonths(1).AddDays(-1);

            if (lUltimoDiaDesseMes.Day == lHoje.Day )
            {
                gLogger.InfoFormat("Último dia do mês, entrou na rotina de inativar corretagem dos clientes que ");

                if (!lHorasVerif.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    return;
                }

                List<ClienteGradualPremiumInfo> lListClientes = gDB.SelecionarClienteParaInativarCorretagem();

                gLogger.InfoFormat("Encontrou {0} clientes para ativar corretagem", lListClientes.Count);

                //gDB.AtualizaCorretagemSinacor(lListClientes, false, 2);
            }

        }

        #endregion

        public void EnviaNotificacaoArquivoProntoImportacao()
        {
            var lServico = Ativador.Get<IServicoEmail>();

            try
            {
                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailAviso"].ToString(), "NotificacaoGeracaoArquivo.txt"));

                string lCorpoEmail = lStream.ReadToEnd();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DAT";

                string lPathArquivoEstorno = Path.Combine(ConfigurationManager.AppSettings["pathArquivoEstornoCustodia"].ToString(), DateTime.Now.ToString("yyyy"), lNomeArquivo);

                var lEmail = new EnviarEmailRequest();

                lEmail.Objeto = new EmailInfo();

                lEmail.Objeto.Assunto = "Notificação de arquivo de Estorno pronto para importação";

                lEmail.Objeto.Destinatarios = new List<string>() { ConfigurationManager.AppSettings["EmailDestinatarioNotificacaoArquivo"].ToString() };

                lEmail.Objeto.Remetente = ConfigurationManager.AppSettings["EmailRemetenteNotificacaoArquivo"].ToString();

                lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("##ARQUIVO##", lNomeArquivo);

                EmailAnexoInfo lAnexo = new EmailAnexoInfo()
                {
                    Arquivo = StreamArquivoImportacao(lPathArquivoEstorno),
                    Nome    = lNomeArquivo
                };

                lEmail.Objeto.Anexos = new List<EmailAnexoInfo>() { lAnexo };

                gLogger.InfoFormat("Entrou no método de EnviarEmailAviso");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    gLogger.Info("Email disparado com sucesso");
                }
                else
                {
                    gLogger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta);
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro no método de EnviarEmailAviso - Descrição: {0} - Stacktrace: {1}", ex.Message, ex.StackTrace);
            }
        }

        private byte[] StreamArquivoImportacao(string pNomeArquivo)
        {
            FileStream lStream = new FileStream(pNomeArquivo, FileMode.Open, FileAccess.Read);

            byte[] arrBytes = new byte[lStream.Length];

            lStream.Read(arrBytes, 0, (int)lStream.Length);

            lStream.Close();

            return arrBytes;
        }


        #region IServicoGerenciadorGradualPremium Members

        public void SelecionarClienteGerenciadorGradualPremmium()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
