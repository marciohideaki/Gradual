using System;
using System.Text;
using log4net;
using Gradual.OMS.Contratos.Automacao.Ordens.Mensagens;

namespace Gradual.OMS.StopStart
{
    /// <summary>
    /// Classe auxiliar para logar as atividades de StopStart 
    /// </summary>
    public class StopStartLog
    {
        #region Properties

        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors
        public StopStartLog()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loga as informações de Stop Start
        /// </summary>
        /// <param name="pObjeto">Parametro do tipo AutomacaoOrdensInfo</param>
        /// <param name="pMensagem">Mensagem a ser logada no arquivo</param>
        public static void LogInfo(ArmarStartStopRequest pObjeto, string pMensagem)
        {
            string lMensagem =  string.Concat(pMensagem,
                " ID: ", pObjeto._AutomacaoOrdensInfo.IdStopStartTipo,
                " CODIGO CLIENTE: ", pObjeto._AutomacaoOrdensInfo.Account,
                " PAPEL: ", pObjeto._AutomacaoOrdensInfo.Symbol,
                " ID TIPO: ", pObjeto._AutomacaoOrdensInfo.OrdTypeID,
                " QUANTIDADE: ", pObjeto._AutomacaoOrdensInfo.OrderQty,
                " PRECO REFERENCIA: ", pObjeto._AutomacaoOrdensInfo.ReferencePrice,
                " PRECO ENVIADO START: ", pObjeto._AutomacaoOrdensInfo.SendStartPrice,
                " PRECO ENVIADO STOP GAIN: ", pObjeto._AutomacaoOrdensInfo.SendStopGainPrice,
                " PRECO ENVIADO STOP LOSS: ", pObjeto._AutomacaoOrdensInfo.SendStopLossValuePrice,
                " PRECO START:", pObjeto._AutomacaoOrdensInfo.StartPriceValue,
                " PRECO STOP GAIN: ", pObjeto._AutomacaoOrdensInfo.StopGainValuePrice,
                " PRECO STOP LOSS: ", pObjeto._AutomacaoOrdensInfo.StopLossValuePrice,
                " ID STATUS: ", pObjeto._AutomacaoOrdensInfo.StopStartStatusID,
                " AJUSTE MVL: ", pObjeto._AutomacaoOrdensInfo.AdjustmentMovelPrice,
                " INICIO MVL: ", pObjeto._AutomacaoOrdensInfo.InitialMovelPrice);

            logger.Info(lMensagem);
        }

        /// <summary>
        /// Loga a mensagem passada como parametro
        /// </summary>
        /// <param name="pMensagem">Mensagem a serlogada no arquivo</param>
        public static void LogInfo(string pMensagem)
        {
            logger.Info(pMensagem);
        }
        #endregion
    }
}
