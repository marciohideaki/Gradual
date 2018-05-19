using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.RoteadorOrdens.Lib;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.OMS.A4s.Lib
{
    public delegate void ChegadaDeAcompanhamentoHandler(OrdemInfo pAlteracao);
    public delegate void StatusConexaoBolsaHandler(StatusConexaoBolsaInfo status);

    /// <summary>
    /// Implementacao dos callbacks invocados pelo Roteador de Ordens
    /// </summary>
    public class OrdemAlteradaCallBack : IRoteadorOrdensCallback
    {
        private readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Timestamp da ultima mensagem recebida do Roteador
        /// </summary>
        public long Timestamp { get; set; }

        public OrdemAlteradaCallBack()
        {
            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
        }
        #region Eventos

        public event ChegadaDeAcompanhamentoHandler ChegadaDeAcompanhamento;
        public event StatusConexaoBolsaHandler OnStatusConexao;

        private void OnChegadaDeAcompanhamento(OrdemInfo pAlteracao)
        {
            if (ChegadaDeAcompanhamento != null)
                ChegadaDeAcompanhamento(pAlteracao);
        }

        #endregion

        #region IServicoRoteadorOrdensCallback Members

        public void OrdemAlterada(OrdemInfo pAlteracao)
        {
            gLog4Net.Debug("OrdemAlterada():");

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            OnChegadaDeAcompanhamento(pAlteracao);
        }

        public void StatusConexaoAlterada(StatusConexaoBolsaInfo status)
        {
            gLog4Net.Debug(string.Format("Ex [{0}] Chan [{1}] Conn [{2}]", status.Bolsa, status.Operador, status.Conectado));

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            if (OnStatusConexao != null)
                OnStatusConexao(status);
        }

        #endregion

        public long LastTimeStampInterval()
        {
            return (_getSecsFromTicks(DateTime.Now.Ticks) - Timestamp);
        }

        /// <summary>
        /// Converte DateTime.Ticks em segundos
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        private long _getSecsFromTicks(long ticks)
        {
            // From fucking MSDN:
            //A single tick represents one hundred nanoseconds or one
            //ten-millionth of a second. There are 10,000 ticks in a millisecond. 
            return ticks / 10000 / 1000;
        }
    }
}
