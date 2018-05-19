#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Gradual.OMS.Library;
using Gradual.OMS.Library.Servicos;
using log4net;
using Gradual.IntegracaoSolicitacoes.PlanoCliente.Lib;
using System.Configuration;
using System.IO;
using System.Threading;
using Gradual.OMS.Email;
using Gradual.OMS.Email.Lib;
#endregion

namespace Gradual.IntegracaoSolicitacoes.PlanoCliente
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServicoIntegracaoSolicitacoes : IServicoControlavel, IServicoIntegracaoSolicitacoes
    {
        #region Propriedades
        
        private readonly ILog logger            = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private ServicoStatus _ServicoStatus    = ServicoStatus.Indefinido;
        
        private PersistenciaDB _gDb             = new PersistenciaDB();
        
        private static AutoResetEvent autoEvent = new AutoResetEvent(false);
        
        private static Timer stateTimer;

        private int TemporizadorGeradorArquivo
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["temporizadorGeradorArquivo"]);
            }
        }
        
        #endregion

        #region Métodos

        #endregion

        #region IServicoControlavel Members

        public void IniciarServico()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                _ServicoStatus = ServicoStatus.EmExecucao;

                GerarArquivoRequest lRequest = new GerarArquivoRequest();

                TimerCallback CallBack = GerarArquivoPeriodico;

                if (stateTimer == null)
                {
                    stateTimer = new Timer(CallBack, autoEvent, 0, TemporizadorGeradorArquivo);

                    logger.Info("Iniciando o serviço de Geração de Arquivo de Planos do cliente (Integração com o Solicitações - Sinacor)");
                }
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro ao iniciar o serviço de IntegracaoSolicitacoes  - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        public void PararServico()
        {
            _ServicoStatus = ServicoStatus.Parado;

            logger.Info("Parando o serviço de Geração de Arquivo de Planos do cliente (Integração com o Solicitações - Sinacor)");
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion

        private void GerarArquivoPeriodico(object sender)
        {
            GerarArquivoRequest lRequest = new GerarArquivoRequest();
            
            ///TODO: Verificar para puxar do sinacor
            string lDiaVencimento = ConfigurationManager.AppSettings["DiaVencimento"].ToString();

            lRequest.StAtivo = 'S';

            logger.InfoFormat ("Entrou na verificação do dia {0}", lDiaVencimento);

            if ( lDiaVencimento.Equals( DateTime.Now.ToString("dd") ) )
            {
                this.GerarArquivo(lRequest);

                this.EnviarEmailAviso();
            }
        }

        #region EnviarEmailAviso 
        public void EnviarEmailAviso()
        {
            logger.InfoFormat("Vai instanciar ");

            var lServico = Ativador.Get<IServicoEmail>();

            logger.InfoFormat("Conseguiu instanciar o serviço Ativador.Get<IServicoEmail>");

            try 
	        {
                StreamReader lStream = new StreamReader(Path.Combine(ConfigurationManager.AppSettings["pathArquivoEmailAviso"].ToString(), "NotificacaoGeracaoArquivo.txt" ));

                string lCorpoEmail = lStream.ReadToEnd();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DTA";

		        var lEmail                  = new EnviarEmailRequest();
                lEmail.Objeto               = new EmailInfo();
                lEmail.Objeto.Assunto       = "Aviso de arquivo pronto para importação";
                lEmail.Objeto.Destinatarios = new List<string> () { ConfigurationManager.AppSettings["EmailEmitenteAviso"].ToString()};
                lEmail.Objeto.Remetente     = ConfigurationManager.AppSettings["EmailEmitenteAviso"].ToString();
                lEmail.Objeto.CorpoMensagem = lCorpoEmail.Replace("##ARQUIVO##", lNomeArquivo).Replace("##DIRETORIO##", ConfigurationManager.AppSettings["pathArquivo"].ToString());

                logger.InfoFormat("Entrou no método de EnviarEmailAviso");

                EnviarEmailResponse lResponse = lServico.Enviar(lEmail);

                if (lResponse.StatusResposta.Equals(MensagemResponseStatusEnum.OK))
                {
                    logger.Info("Email disparado com sucesso");
                }
                else
                {
                    logger.ErrorFormat("O e-mail não foi disparado - Descrição: {0} ", lResponse.DescricaoResposta );
                }
	        }
	        catch (Exception ex)
	        {
                logger.ErrorFormat("Erro no método de EnviarEmailAviso - Descrição: {0} - Stacktrace: {1}", ex.Message, ex.StackTrace);
	        }
        }
        #endregion 

        #region IServicoIntegracaoSolicitacoes Members
        /// <summary>
        /// Gera o arquivo no diretório específico
        /// </summary>
        /// <param name="pRequest">Dados de requesta para geração o arquivo</param>
        public void GerarArquivo(GerarArquivoRequest pRequest)
        {
            try
            {
                _gDb = new PersistenciaDB();

                string lPath = ConfigurationManager.AppSettings["pathArquivo"].ToString();

                string lNomeArquivo = "CCOU" + DateTime.Now.ToString("MMdd") + ".DTA";

                GerarArquivoRequest lRequest = new GerarArquivoRequest();

                lRequest.StAtivo = 'S';

                List<string> lDetalhes = _gDb.ListarLinhasArquivo(lRequest);

                StreamWriter lStream = new StreamWriter(Path.Combine(lPath, lNomeArquivo));
                
                foreach (string linha in lDetalhes)
                    lStream.WriteLine(linha);

                lStream.Close();

            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Erro em GerarArquivo - {0} - StackTrace - {1}", ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Events
        #endregion
    }
}
