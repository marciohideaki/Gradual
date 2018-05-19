using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Core.OMS.DropCopy.Lib;
using log4net;
using Gradual.OMS.RoteadorOrdens.Lib.Dados;

namespace Gradual.OMS.ServicoA4S
{
    public delegate void FIXDropCopyCallbackHandler(OrdemInfo pAlteracao);
    public delegate void FIXHeartBeatHandler();

    /// <summary>
    /// Implementacao dos callbacks invocados pelo Roteador de Ordens
    /// </summary>
    public class DropCopyCallbackImpl : IDropCopyCallback
    {
        private readonly ILog gLog4Net = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Timestamp da ultima mensagem recebida do Roteador
        /// </summary>
        public long Timestamp { get; set; }

        public DropCopyCallbackImpl()
        {
            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);
        }
        #region Eventos

        public event FIXDropCopyCallbackHandler OnFIXDropCopy;
        public event FIXHeartBeatHandler OnHeartBeat;

        #endregion

        #region IServicoRoteadorOrdensCallback Members

        public void OrdemAlterada(OrdemInfo info)
        {
            gLog4Net.Debug("DropCopy OrdemAlterada():");

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            if (OnFIXDropCopy != null)
                OnFIXDropCopy(info);
        }

        public void HeartBeat()
        {
            gLog4Net.Info("HeartBeat");

            Timestamp = _getSecsFromTicks(DateTime.Now.Ticks);

            if (OnHeartBeat != null)
                OnHeartBeat();
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
