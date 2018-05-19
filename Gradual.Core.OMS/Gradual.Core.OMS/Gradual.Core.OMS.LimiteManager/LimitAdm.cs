using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using Gradual.Core.OMS.LimiteManager.Lib;
using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.Core.OMS.LimiteManager.Database;
using Gradual.Core.OMS.LimiteManager.Dados;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;

namespace Gradual.Core.OMS.LimiteManager
{
    public class LimitAdm: ILimiteManager
    {

        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        // Constructor 
        public LimitAdm()
        {
//            _dbMemory = new DbMemory();
        }


        #region ILimiteManager Members
        public void DummyFunction()
        {
            logger.Info("DummyFunction Called");
            
        }
        
        public ReloadLimitsResponse ReloadLimitStructures(ReloadLimitsRequest req)
        {
            ReloadLimitsResponse resp = new ReloadLimitsResponse();
            try
            {

                resp = LimitControl.GetInstance().ReloadLimits(req.ReloadSecurityList);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na recarga de toda estrutura de controle de Limite: " + ex.Message, ex);
                resp.DescricaoErro = ex.Message;
                resp.StackTrace = ex.StackTrace;
                resp.StatusResponse = LimitMessages.ERRO;
            }
            return resp;
        }

        public ReloadClientLimitResponse ReloadLimitClientLimitStructures(ReloadClientLimitRequest req)
        {
            ReloadClientLimitResponse resp = new ReloadClientLimitResponse();
            try
            {
                resp = LimitControl.GetInstance().ReloadClientLimits(req.CodCliente, req.DeleteOnly);
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na recarga de estrutura de controle de Limite por cliente. CodCliente: " + req.CodCliente + " : " + ex.Message, ex);
                resp.DescricaoErro = ex.Message;
                resp.StackTrace = ex.StackTrace;
                resp.StatusResponse = LimitMessages.ERRO;
            }
            return resp;
        }
        #endregion
    }
}
