using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace Gradual.OMS.WsIntegracao.Arena.Services
{
    public class ServicoBase
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string gNomeConexaoCadastro   = "Cadastro";
        public static string gNomeConexaoOracle     = "Sinacor";
        public const string gNomeConexaoContaMargem = "CONTAMARGEM";
        public const string gNomeConexaoRisco       = "Risco";
        #endregion
        public  ServicoBase()
        {
            //log4net.Config.XmlConfigurator.Configure();    
        }

    }
}