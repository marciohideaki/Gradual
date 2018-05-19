using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using MdsBayeuxClient;

using Gradual.Core.OMS.SmartTrader.Lib.Dados;


namespace Gradual.Core.OMS.SmartTrader.Dados
{
    public class SmartInstrument
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        private MdsHttpClient.OnNegociosHandler OnNegociosHandler;
        private MdsAssinatura _signTrading;
        #endregion


        #region Properties
        public string Instrument { get; set; }
        public OrdemSmart Order { get; set; }
        #endregion

        /// Constructor / Destructor
        /// 
        public SmartInstrument()
        {
            this.Instrument = string.Empty;
            this.Order = null;
            _signTrading = new MdsAssinatura();
        }

        public SmartInstrument(string symbol, OrdemSmart order)
        {
            this.Instrument = symbol;
            this.Order = order;
            _signTrading = new MdsAssinatura();
        }


        public bool SignTrading()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na assinatura do Trading: " + ex.Message, ex);
                return false;
            }
        }
    }
}
