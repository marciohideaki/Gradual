using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Site.Www
{
    public static class PerfMon
    {
        private static Dictionary<string, List<PerfMonData>> _Dados = new Dictionary<string, List<PerfMonData>>();

        public static Dictionary<string, List<PerfMonData>> Dados
        {
            get
            {
                return _Dados;
            }

            set
            {
                _Dados = value;
            }
        }

        #region Métodos Públicos

        public static void Marcar(string pChave, string pDescricao)
        {
            if (!PerfMon.Dados.ContainsKey(pChave))
                PerfMon.Dados.Add(pChave, new List<PerfMonData>());

            PerfMon.Dados[pChave].Add(new PerfMonData(pDescricao));
        }

        public static void Limpar(string pChave)
        {
            if (PerfMon.Dados.ContainsKey(pChave))
            {
                PerfMon.Dados.Remove(pChave);
            }
        }

        public static string RenderizarRelatorio(string pChave, bool pLimparPosteriormente)
        {
            string lRetorno = "";

            TimeSpan lSpan;

            if (PerfMon.Dados.ContainsKey(pChave))
            {
                for (var a = 0; a < PerfMon.Dados[pChave].Count; a++)
                {
                    if(a == 0)
                    {
                        lSpan = new TimeSpan();
                    }
                    else
                    {
                        lSpan = new TimeSpan(PerfMon.Dados[pChave][a].DataRegistrada.Ticks - PerfMon.Dados[pChave][a - 1].DataRegistrada.Ticks);
                    }

                    lRetorno += string.Format("[{0}][{1}]ms > {2} <br/>"
                                              , PerfMon.Dados[pChave][a].DataRegistrada.ToString("HH:mm:ss:fff")
                                              , lSpan.TotalMilliseconds.ToString("n0").PadLeft(9, ' ').Replace(" ", "&nbsp;")
                                              , PerfMon.Dados[pChave][a].Descricao);
                }

                lSpan = new TimeSpan(PerfMon.Dados[pChave][PerfMon.Dados[pChave].Count - 1].DataRegistrada.Ticks - PerfMon.Dados[pChave][0].DataRegistrada.Ticks);

                lRetorno += string.Format("[{0}][{1}]ms Total."
                                            , PerfMon.Dados[pChave][PerfMon.Dados[pChave].Count - 1].DataRegistrada.ToString("HH:mm:ss:fff")
                                            , lSpan.TotalMilliseconds.ToString("n0").PadLeft(9, ' ').Replace(" ", "&nbsp;"));

                if (pLimparPosteriormente)
                {
                    PerfMon.Dados.Remove(pChave);
                }
            }

            return lRetorno;
        }

        #endregion
    }

    public class PerfMonData
    {
        public DateTime DataRegistrada { get; set; }

        public string Descricao { get; set; }

        public PerfMonData(string pDescricao)
        {
            this.DataRegistrada = DateTime.Now;

            this.Descricao = pDescricao;
        }
    }
}