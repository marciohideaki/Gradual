using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Collections;
using Gradual.OMS.Comunicacao.Automacao.Ordens;
using Gradual.OMS.Ordens.StartStop.Lib;
using System.Globalization;
using Gradual.OMS.Comunicacao.Automacao.Ordens.Mensagens.Enviadas;

namespace Gradual.OMS.StopStart
{
    public static class FormatadorUMDF
    {
        public static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Hashtable hstOrdensEnviadasMds = new Hashtable();

        private static MDSPackageSocket [] _umdfsockets;

        public static void SetMDSPackageSocketArray(MDSPackageSocket [] umdfsockets)
        {
            _umdfsockets = umdfsockets;
        }

        public static void ArmarStopSimples(AutomacaoOrdensInfo _TOrder)
        {
            try
            {
                CultureInfo _ciPtBR = new CultureInfo("pt-BR");

                SS_StopStart _SS_StopSimples = new SS_StopStart("BV");

                _SS_StopSimples.CodigoInstrumento = _TOrder.Symbol;

                _SS_StopSimples.IdStopStart = Convert.ToString(_TOrder.StopStartID);
                _SS_StopSimples.IdTipoOrdem = Convert.ToString((int)_TOrder.IdStopStartTipo);

                _SS_StopSimples.PrecoGain = string.Format(_ciPtBR, "{0:F2}", _TOrder.StopGainValuePrice);
                _SS_StopSimples.PrecoLoss = string.Format(_ciPtBR, "{0:F2}", _TOrder.StopLossValuePrice);
                _SS_StopSimples.PrecoStart = string.Format(_ciPtBR, "{0:F2}", _TOrder.StartPriceValue);
                _SS_StopSimples.AjusteMovel = string.Format(_ciPtBR, "{0:F2}", _TOrder.AdjustmentMovelPrice);
                _SS_StopSimples.InicioMovel = string.Format(_ciPtBR, "{0:F2}", _TOrder.InitialMovelPrice);

                string msg = _SS_StopSimples.getMessageSS();

                logger.Debug("Mensagem pro MDS [" + msg + "]");

                if (_umdfsockets != null)
                {
                    foreach (MDSPackageSocket mdssocket in _umdfsockets)
                    {
                        mdssocket.SendData(msg, true);
                    }
                }

                lock (hstOrdensEnviadasMds)
                {
                    hstOrdensEnviadasMds.Add(_SS_StopSimples.IdStopStart, _SS_StopSimples);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar o método ArmarStopLoss. Erro: {0}.\n StackTrace {1}:", ex.Message, ex.StackTrace));
            }
        }

        public static void CancelarOrdemStop(string Instrument, int id_stopstart)
        {
            try
            {
                CS_CancelamentoStop _CS_CancelamentoStop = new CS_CancelamentoStop();

                _CS_CancelamentoStop.IdStopStart = id_stopstart.ToString();
                _CS_CancelamentoStop.CodigoInstrumento = Instrument;

                if (_umdfsockets != null)
                {
                    foreach (MDSPackageSocket mdssocket in _umdfsockets)
                    {
                        mdssocket.SendData(_CS_CancelamentoStop.getMessageCE(), true);
                    }
                }

                lock (hstOrdensEnviadasMds)
                {
                    hstOrdensEnviadasMds.Remove(_CS_CancelamentoStop.IdStopStart);
                }
            }
            catch (Exception ex)
            {
                logger.Error("CancelarOrdemStop(): " + ex.Message, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static void ReenviarStops()
        {
            lock (hstOrdensEnviadasMds)
            {
                foreach (SS_StopStart stop in hstOrdensEnviadasMds.Values)
                {
                    try
                    {
                        if (_umdfsockets != null)
                        {
                            foreach (MDSPackageSocket mdssocket in _umdfsockets)
                            {
                                if (mdssocket != null && mdssocket.IsConectado())
                                {
                                    mdssocket.SendData(stop.getMessageSS(), true);
                                }
                            }
                        }

                        logger.Warn("Ordem Stop [" + stop.IdStopStart + "] Reenviada");
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Erro ao reenviar stop [" + stop.IdStopStart + "]: " + ex.Message, ex);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void RemoverStopExecutado(string idstopstart)
        {
            lock (hstOrdensEnviadasMds)
            {
                if (hstOrdensEnviadasMds.ContainsKey(idstopstart))
                    hstOrdensEnviadasMds.Remove(idstopstart);
                else
                    logger.Error("RemoverStopExecutado(" + idstopstart + ") Ordem nao estava na hash");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>objeto List[SS_StopStart]</returns>
        public static List<SS_StopStart> ListarStopsArmados()
        {
            List<SS_StopStart> ret = new List<SS_StopStart>();

            lock (hstOrdensEnviadasMds)
            {
                foreach (SS_StopStart stop in hstOrdensEnviadasMds.Values)
                {
                    ret.Add(stop);
                }
            }

            return ret;
        }
    }
}
