using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Proventos.Lib;
using log4net;
using Gradual.OMS.Library.Servicos;
using System.Threading;
using Gradual.OMS.Risco.Persistencia.Lib;

namespace Gradual.OMS.Proventos
{

    public class ServicoProventos  :IServicoControlavel,IServicoProvento
    {
        public ServicoProventos()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region Variaveis Locais

        // Esse tipo de declaracao é preferivel sobre a outra
        // classes derivadas de MyClass automaticamente gravarao no log
        // com o nome da classe corrigido        

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region IServicoProvento Members

        private void InvokeThreadMethod()
        {
            string DataReferencia = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2,'0') + DateTime.Now.Day.ToString();           
            this.CancelarOrdensDistribuicaoVencida(DataReferencia);
        }

        public bool CancelarOrdensDistribuicaoVencida(string pDataReferencia)
        {
          
            bool bRetorno = false;

            try{

                bRetorno = new PersistenciaProventos().CancelarOrdensDistribuicaoVencida(pDataReferencia);
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao invocar o método CancelarOrdensDistribuicaoVencida. ");
                logger.Info("Descricao do erro: " + ex.Message);
            }

            return bRetorno;
        }

        #endregion

        #region IServicoControlavel Members

        private ServicoStatus _ServicoStatus { set; get; }

        public void IniciarServico()
        {
            try
            {               
                logger.Info("Tentando Startar o servico");
                _ServicoStatus = ServicoStatus.EmExecucao;
                logger.Info("Servico Ativado com sucesso");

                InvokeThreadMethod();
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao Iniciar o servico: " + ex.Message);
            }
        }     

        public void PararServico()
        {
            try
            {
                logger.Info("Tentando parar o servico");
                _ServicoStatus = ServicoStatus.Parado;
                logger.Info("Servico parado com sucesso");
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao Iniciar o servico: " + ex.Message);
            }
        }

        public ServicoStatus ReceberStatusServico()
        {
            return _ServicoStatus;
        }

        #endregion
    }
}
