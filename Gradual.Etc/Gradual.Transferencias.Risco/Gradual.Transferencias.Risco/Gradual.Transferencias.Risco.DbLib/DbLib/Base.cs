using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Gradual.Transferencias.Risco.DbLib
{
    /// <summary>
    /// Classe de persistencia de banco de dados base de controle de tranferencia de arquivos 
    /// </summary>
    public class TransferenciasRiscoDbLibBase
    {
        #region Atributos
        protected string ConexaoSinacor = "SINACOR";
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public TransferenciasRiscoDbLibBase()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion
    }
}
