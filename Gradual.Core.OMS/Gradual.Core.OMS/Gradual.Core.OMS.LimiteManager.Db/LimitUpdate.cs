using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using log4net;

using Gradual.Core.OMS.LimiteManager.Lib.Mensageria;
using Gradual.Core.OMS.LimiteManager.Db.Database;
using Gradual.Core.OMS.LimiteManager.Db.Dados;



namespace Gradual.Core.OMS.LimiteManager.Db
{
    public class LimitUpdate
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private Variables
        DbAccess _db;

        #endregion


        public LimitUpdate()
        {
            _db = new DbAccess();
        }

        ~LimitUpdate()
        {
            _db = null;
        }

        public AtualizaMvtoBvspResponse AtualizaMvtoBvsp(AtualizaMvtoBvspRequest req)
        {
            AtualizaMvtoBvspResponse ret = new AtualizaMvtoBvspResponse();
            try
            {
                if (!_db.AtualizarMvtoBvsp(req.LimiteBovespa))
                {
                    logger.Info("Nao foi possivel atualizar a movimentacao financeira Bovespa");
                    ret.DescricaoErro = ErrorMessages.ERR_UPDT_MVTO_BVSP;
                    ret.StatusResponse = ErrorMessages.ERRO;
                }
                ret.StatusResponse = ErrorMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao financeira bovespa: " + ex.Message, ex);
                ret.DescricaoErro = ex.Message;
                ret.StackTrace = ex.StackTrace;
                ret.StatusResponse = ErrorMessages.ERRO;
                return ret;
            }
        }

        public InserirMvtoBvspResponse InserirMvtoBvsp(InserirMvtoBvspRequest req)
        {
            InserirMvtoBvspResponse ret = new InserirMvtoBvspResponse();
            try
            {
                if (!_db.InserirMvtoBvsp(req.LimiteBovespa))
                {
                    logger.Info("Nao foi possivel atualizar a movimentacao financeira Bovespa");
                    ret.DescricaoErro = ErrorMessages.ERR_UPDT_MVTO_BVSP;
                    ret.StatusResponse = ErrorMessages.ERRO;
                }
                ret.StatusResponse = ErrorMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao financeira bovespa: " + ex.Message, ex);
                ret.DescricaoErro = ex.Message;
                ret.StackTrace = ex.StackTrace;
                ret.StatusResponse = ErrorMessages.ERRO;
                return ret;
            }
        }

        public AtualizaMvtoBMFContratoResponse AtualizaMvtoBMFContrato(AtualizaMvtoBMFContratoRequest req)
        {
            AtualizaMvtoBMFContratoResponse ret = new AtualizaMvtoBMFContratoResponse();
            try
            {
                if (!_db.AtualizarMvtoBMFContrato(req.ContractLimit))
                {
                    logger.Info("Nao foi possivel atualizar a movimentacao financeira BMF Contrato");
                    ret.DescricaoErro = ErrorMessages.ERR_UPDT_MVTO_BMF_CONTRATO;
                    ret.StatusResponse = ErrorMessages.ERRO;
                }
                ret.StatusResponse = ErrorMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de contratos BMF: " + ex.Message, ex);
                ret.DescricaoErro = ex.Message;
                ret.StackTrace = ex.StackTrace;
                ret.StatusResponse = ErrorMessages.ERRO;
                return ret;
            }
        }

        public AtualizaMvtoBMFInstrumentoResponse AtualizaMvtoBMFInstrumento(AtualizaMvtoBMFInstrumentoRequest req)
        {
            AtualizaMvtoBMFInstrumentoResponse ret = new AtualizaMvtoBMFInstrumentoResponse();
            try
            {
                if (!_db.AtualizarMvtoBMFInstrumento(req.InstrumentLimit))
                {
                    logger.Info("Nao foi possivel atualizar a movimentacao BMF Contrato");
                    ret.DescricaoErro = ErrorMessages.ERR_UPDT_MVTO_BMF_INSTRUMENTO;
                    ret.StatusResponse = ErrorMessages.ERRO;
                }
                ret.StatusResponse = ErrorMessages.OK;
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de contratos BMF: " + ex.Message, ex);
                ret.DescricaoErro = ex.Message;
                ret.StackTrace = ex.StackTrace;
                ret.StatusResponse = ErrorMessages.ERRO;
                return ret;
            }
        }
        
    }
}
