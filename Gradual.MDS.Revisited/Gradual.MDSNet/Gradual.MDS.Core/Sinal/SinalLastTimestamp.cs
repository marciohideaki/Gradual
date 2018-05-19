using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.MDS.Core.Lib;
using System.Configuration;
using System.Globalization;
using log4net;
using System.Collections.Concurrent;

namespace Gradual.MDS.Core.Sinal
{
    public class SinalTimestampInfo
    {
        public string Instrumento { get; set; }
        public long LastTick { get; set; }
        public string TipoSinal { get; set; }

        public SinalTimestampInfo(string instrumento, string tiposinal)
        {
            Instrumento = instrumento;
            LastTick = DateTime.UtcNow.Ticks;
            TipoSinal = tiposinal;
        }
    }

    public class SinalLastTimestamp
    {
        private ConcurrentDictionary<string, SinalTimestampInfo> dctTimestampLOF = new ConcurrentDictionary<string, SinalTimestampInfo>();
        private ConcurrentDictionary<string, SinalTimestampInfo> dctTimestampNEGStreamer = new ConcurrentDictionary<string, SinalTimestampInfo>();
        private ConcurrentDictionary<string, SinalTimestampInfo> dctTimestampNEGHB = new ConcurrentDictionary<string, SinalTimestampInfo>();
        private ConcurrentDictionary<string, SinalTimestampInfo> dctTimestampLNGHB = new ConcurrentDictionary<string, SinalTimestampInfo>();
        private ILog logger;

        public long LOFInterval { get; set; }
        public long LOFIntervaloNaoEnviadosHB { get; set; }

        public long NEGIntervalHB { get; set; }
        public long NEGIntervaloNaoEnviados { get; set; }
        public long NEGIntervalStreamer { get; set; }
        public long LivroNegociosIntervalHB { get; set; }

        public DateTime DataHoraInicioPregao
        {
            get
            {
                string horainicio = "1000";
                if (ConfigurationManager.AppSettings["HorarioInicioPregao"] != null)
                {
                    horainicio = ConfigurationManager.AppSettings["HorarioInicioPregao"].ToString();
                }

                return DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd") + horainicio, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
            }
        }

        public DateTime DataHoraFinalPregao
        {
            get
            {
                string horainicio = "1650";
                if (ConfigurationManager.AppSettings["HorarioFinalPregao"] != null)
                {
                    horainicio = ConfigurationManager.AppSettings["HorarioFinalPregao"].ToString();
                }

                return DateTime.ParseExact(DateTime.Now.ToString("yyyyMMdd") + horainicio, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
            }
        }

        public SinalLastTimestamp()
        {
            LOFInterval = 250;
            LOFIntervaloNaoEnviadosHB = 1000;


            NEGIntervalHB = 250;
            NEGIntervaloNaoEnviados = 1000;
            NEGIntervalStreamer= 100;
            LivroNegociosIntervalHB = 500;

            if (ConfigurationManager.AppSettings["IntervaloLOFHB"] != null)
            {
                LOFInterval = Convert.ToInt64(ConfigurationManager.AppSettings["IntervaloLOFHB"].ToString());
            }

            if (ConfigurationManager.AppSettings["LOFIntervaloNaoEnviadosHB"] != null)
            {
                LOFInterval = Convert.ToInt64(ConfigurationManager.AppSettings["LOFIntervaloNaoEnviadosHB"].ToString());
            }

            if (ConfigurationManager.AppSettings["IntervaloNEGHB"] != null)
            {
                NEGIntervalHB = Convert.ToInt64(ConfigurationManager.AppSettings["IntervaloNEGHB"].ToString());
            }

            if (ConfigurationManager.AppSettings["NEGIntervaloNaoEnviados"] != null)
            {
                NEGIntervaloNaoEnviados = Convert.ToInt64(ConfigurationManager.AppSettings["NEGIntervaloNaoEnviados"].ToString());
            }

            if (ConfigurationManager.AppSettings["IntervalNEGStreamer"] != null)
            {
                NEGIntervalStreamer = Convert.ToInt64(ConfigurationManager.AppSettings["IntervalNEGStreamer"].ToString());
            }

            if (ConfigurationManager.AppSettings["IntervaloLivroNegociosHB"] != null)
            {
                LivroNegociosIntervalHB = Convert.ToInt64(ConfigurationManager.AppSettings["IntervaloLivroNegociosHB"].ToString());
            }

            logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public bool ShouldSendLOF(string instrumento)
        {
            if (String.IsNullOrEmpty(instrumento))
                return true;

            SinalTimestampInfo info = null;
            // Envia o sinal se for o 1o envio
            if (!dctTimestampLOF.TryGetValue(instrumento, out info))
            {
                info = new SinalTimestampInfo(instrumento, ConstantesMDS.TIPO_REQUISICAO_LIVRO_OFERTAS);
                dctTimestampLOF.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                return true;
            }

            // Envia o sinal se nao estiver no horario de pregao
            if (DateTime.Compare(DateTime.Now, DataHoraInicioPregao) <= 0 || DateTime.Compare(DateTime.Now, DataHoraFinalPregao) >= 0)
                return true;

            // Testa se o ultimo envio esta fora do intervalo
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks-info.LastTick);
            if (ts.TotalMilliseconds >= LOFInterval)
            {
                dctTimestampLOF[instrumento].LastTick = DateTime.UtcNow.Ticks;
                return true;
            }

            // Garante mandar um sinal por segundo, no minimo
            DateTime x = new DateTime(info.LastTick);
            if (String.CompareOrdinal(x.ToString("yyyyMMddHHmmss"), DateTime.UtcNow.ToString("yyyyMMddHHmmss")) < 0)
            {
                dctTimestampLOF[instrumento].LastTick = DateTime.UtcNow.Ticks;
                return true;
            }

            //TODO: incluir um calculo para casos de papeis pouco negociados,
            // que podem sofrer 2 atualizacoes dentro do intervalo, e nao serem atualizados depois

            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ObterLOFNaoEnviados()
        {
            List<string> lstNotSent = new List<string>();

            string [] keys = dctTimestampLOF.Keys.ToArray();

            //foreach (KeyValuePair<string, SinalTimestampInfo> entry in dctTimestampLOF)
            foreach (string instrumento in keys)
            {
                SinalTimestampInfo entry;

                if (dctTimestampLOF.TryGetValue(instrumento, out entry))
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - entry.LastTick);
                    DateTime corte = DateTime.UtcNow.Subtract(new TimeSpan(TimeSpan.TicksPerMinute / 2));
                    if (ts.TotalMilliseconds > LOFIntervaloNaoEnviadosHB && entry.LastTick > corte.Ticks)
                    {
                        lstNotSent.Add(instrumento);

                        SinalTimestampInfo newEntry = new SinalTimestampInfo(instrumento, entry.TipoSinal);
                        newEntry.LastTick = DateTime.UtcNow.Ticks;

                        //dctTimestampLOF.TryUpdate(instrumento, newEntry, entry);
                    }
                }
            }

            return lstNotSent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public bool ShouldSendNEGHB(string instrumento)
        {
            if (String.IsNullOrEmpty(instrumento))
                return true;

            SinalTimestampInfo info = null;
            // Envia o sinal se for o 1o envio
            if (!dctTimestampNEGHB.TryGetValue(instrumento, out info))
            {
                info = new SinalTimestampInfo(instrumento, ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS);
                dctTimestampNEGHB.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                return true;
            }

            // Envia o sinal se nao estiver no horario de pregao
            if (DateTime.Compare(DateTime.Now, DataHoraInicioPregao) <= 0 || DateTime.Compare(DateTime.Now, DataHoraFinalPregao) >= 0)
                return true;

            // Testa se o ultimo envio esta fora do intervalo
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - info.LastTick);
            if (ts.TotalMilliseconds >= NEGIntervalHB)
            {
                info.LastTick = DateTime.UtcNow.Ticks;
                dctTimestampNEGHB.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                return true;
            }

            // Garante mandar um sinal por segundo, no minimo
            DateTime x = new DateTime(info.LastTick);
            if (String.CompareOrdinal(x.ToString("yyyyMMddHHmmss"), DateTime.UtcNow.ToString("yyyyMMddHHmmss")) < 0)
            {
                info.LastTick = DateTime.UtcNow.Ticks;
                dctTimestampNEGHB.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public bool ShouldSendLivroNegociosHB(string instrumento)
        {
            if (String.IsNullOrEmpty(instrumento))
                return true;

            SinalTimestampInfo info = null;

            // Envia o sinal se for o 1o envio
            if (!dctTimestampLNGHB.TryGetValue(instrumento, out info))
            {
                info = new SinalTimestampInfo(instrumento, ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS);
                dctTimestampLNGHB.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                return true;
            }

            // Envia o sinal se nao estiver no horario de pregao
            if (DateTime.Compare(DateTime.Now, DataHoraInicioPregao) <= 0 || DateTime.Compare(DateTime.Now, DataHoraFinalPregao) >= 0)
                return true;

            // Testa se o ultimo envio esta fora do intervalo
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - info.LastTick);
            if (ts.TotalMilliseconds >= LivroNegociosIntervalHB)
            {
                info.LastTick = DateTime.UtcNow.Ticks;
                return true;
            }

            // Garante mandar um sinal por segundo, no minimo
            DateTime x = new DateTime(info.LastTick);
            if (String.CompareOrdinal(x.ToString("yyyyMMddHHmmss"), DateTime.UtcNow.ToString("yyyyMMddHHmmss")) < 0)
            {
                info.LastTick = DateTime.UtcNow.Ticks;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instrumento"></param>
        /// <returns></returns>
        public bool ShouldSendNEGStreamer(string instrumento)
        {
            try
            {
                if (String.IsNullOrEmpty(instrumento))
                    return true;

                SinalTimestampInfo info = null;
                // Envia o sinal se for o 1o envio
                if (!dctTimestampNEGStreamer.TryGetValue(instrumento, out info))
                {
                    info = new SinalTimestampInfo(instrumento, ConstantesMDS.TIPO_REQUISICAO_NEGOCIOS);
                    dctTimestampNEGStreamer.AddOrUpdate(instrumento, info, (key, oldValue) => info);
                    return true;
                }

                // Envia o sinal se nao estiver no horario de pregao
                if (DateTime.Compare(DateTime.Now, DataHoraInicioPregao) <= 0 || DateTime.Compare(DateTime.Now, DataHoraFinalPregao) >= 0)
                    return true;

                // Testa se o ultimo envio esta fora do intervalo
                TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - info.LastTick);
                if (ts.TotalMilliseconds >= NEGIntervalStreamer)
                {
                    info.LastTick = DateTime.UtcNow.Ticks;
                    return true;
                }

                // Garante mandar um sinal por segundo, no minimo
                DateTime x = new DateTime(info.LastTick);
                if (String.CompareOrdinal(x.ToString("yyyyMMddHHmmss"), DateTime.UtcNow.ToString("yyyyMMddHHmmss")) < 0)
                {
                    info.LastTick = DateTime.UtcNow.Ticks;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.Error("ShouldSendNEGStreamer(): " + ex.Message, ex);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ObterNEGNaoEnviadosHB(long intervalo)
        {
            List<string> lstNotSent = new List<string>();

            string[] keys = dctTimestampNEGHB.Keys.ToArray();

            foreach (string instrumento in keys)
            {
                SinalTimestampInfo entry;
                if (dctTimestampNEGHB.TryGetValue(instrumento, out entry))
                {
                    if (entry != null)
                    {
                        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - entry.LastTick);
                        DateTime corte = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, Convert.ToInt32(intervalo * 2 / 1000)));
                        if (ts.TotalMilliseconds > NEGIntervaloNaoEnviados && entry.LastTick > corte.Ticks)
                        {
                            lstNotSent.Add(instrumento);

                            SinalTimestampInfo newEntry = new SinalTimestampInfo(instrumento, entry.TipoSinal);
                            newEntry.LastTick = DateTime.UtcNow.Ticks;

                            //dctTimestampNEGHB.TryUpdate(instrumento, newEntry, entry);
                        }
                    }
                    else
                        logger.Error("Caralho de asa");
                }
            }

            return lstNotSent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ObterNEGNaoEnviadosStreamer(long intervalo)
        {
            List<string> lstNotSent = new List<string>();

            string[] keys = dctTimestampNEGStreamer.Keys.ToArray();

            foreach (string instrumento in keys)
            {
                SinalTimestampInfo entry;

                if (dctTimestampNEGStreamer.TryGetValue(instrumento, out entry))
                {
                    if (entry != null)
                    {
                        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - entry.LastTick);
                        DateTime corte = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, Convert.ToInt32(intervalo * 2 / 1000)));
                        if (ts.TotalMilliseconds > NEGIntervaloNaoEnviados && entry.LastTick > corte.Ticks)
                        {
                            lstNotSent.Add(instrumento);
                            
                            SinalTimestampInfo newEntry = new SinalTimestampInfo(instrumento, entry.TipoSinal);
                            newEntry.LastTick = DateTime.UtcNow.Ticks;

                            //dctTimestampNEGStreamer.TryUpdate(instrumento, newEntry, entry);
                        }
                    }
                    else
                        logger.Error("Caralho");
                }
            }

            return lstNotSent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ObterLNGNaoEnviadosHB(long intervalo)
        {
            List<string> lstNotSent = new List<string>();

            string[] keys = dctTimestampLNGHB.Keys.ToArray();

            foreach (string instrumento in keys)
            {
                SinalTimestampInfo entry;
                if (dctTimestampLNGHB.TryGetValue(instrumento, out entry))
                {
                    if (entry != null)
                    {
                        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - entry.LastTick);
                        DateTime corte = DateTime.UtcNow.Subtract(new TimeSpan(0, 0, Convert.ToInt32(intervalo * 2 / 1000)));
                        if (ts.TotalMilliseconds > NEGIntervaloNaoEnviados && entry.LastTick > corte.Ticks)
                        {
                            lstNotSent.Add(instrumento);

                            SinalTimestampInfo newEntry = new SinalTimestampInfo(instrumento, entry.TipoSinal);
                            newEntry.LastTick = DateTime.UtcNow.Ticks;

                            //dctTimestampLNGHB.TryUpdate(instrumento, newEntry, entry);
                        }
                    }
                    else
                        logger.Error("Caralho de asa LNG");
                }
            }

            return lstNotSent;
        }
    }
}
