using System;
using System.ServiceModel;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Risco.Persistencia.Lib;
using log4net;

namespace Gradual.OMS.ContaCorrente
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ServicoContaCorrente : IServicoContaCorrente
    {
        private ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IServicoContaCorrente Members

        /// <summary>
        /// Obtem o saldo de conta corrente do cliente { D0,D1,D2,D3,CM }
        /// </summary>
        /// <param name="pParametro"> Código do cliente</param>
        /// <returns> Objeto de conta corrente populado</returns>
        public SaldoContaCorrenteResponse<ContaCorrenteInfo> ObterSaldoContaCorrente(SaldoContaCorrenteRequest pParametro)
        {
            gLogger.Debug("Inicio ObterSaldoContaCorrente()");

            SaldoContaCorrenteResponse<ContaCorrenteInfo> response = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            try
            {
                response = new PersistenciaContaCorrente().ObterSaldoContaCorrente(pParametro);
            }
            catch (Exception ex)
            {
                response.DataResposta = DateTime.Now;
                response.StackTrace = ex.StackTrace;
                response.StatusResposta = Lib.Enum.CriticaMensagemEnum.Exception;
                gLogger.Error(string.Concat("ObterSaldoContaCorrente():", ex.Message), ex);
            }

            return response;
        }

        /// <summary>
        /// Obtem o saldo de BMF do cliente
        /// </summary>
        /// <param name="pParametro"> Código do cliente</param>
        /// <returns> Objeto de saldo BMF populado</returns>
        public SaldoContaCorrenteResponse<ContaCorrenteBMFInfo> ObterSaldoContaCorrenteBMF(SaldoContaCorrenteRequest pParametro)
        {
            return new PersistenciaContaCorrente().ObtemSaldoContaBMF(pParametro);
        }
    }

    #endregion
}
