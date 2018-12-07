using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Gradual.Site.Www
{
    public class TransporteISIN
    {
        #region Propriedades

        public string Titulo { get; set; }

        public string CodigoISIN { get; set; }

        public string CodigoIPO { get; set; }

        public string DataCadastro { get; set; }

        public string DataInicial { get; set; }

        public string DataFinal { get; set; }

        public string HoraInicio { get; set; }

        //public DateTime DataFinalFormatada { get; set; }

        //public DateTime HoraInicioFormatada { get; set; }

        public string HoraFinal { get; set; }

        public DateTime DataHoraInicial
        {
            get
            {
                CultureInfo lCultura = new CultureInfo("pt-BR");

                DateTime lData;

                string lDataTexto = string.Format("{0} {1}", this.DataInicial, this.HoraInicio).Trim();
                
                if (!DateTime.TryParseExact(lDataTexto, "dd/MM/yyyy HH:mm", lCultura, DateTimeStyles.None, out lData))
                {
                    if (!DateTime.TryParseExact(lDataTexto, "dd/MM/yyyy", lCultura, DateTimeStyles.None, out lData))
                    {
                        lData = DateTime.Now;
                    }
                }

                return lData;
            }
        }
        
        public DateTime DataHoraFinal
        {
            get
            {
                CultureInfo lCultura = new CultureInfo("pt-BR");

                DateTime lData;

                string lDataTexto = string.Format("{0} {1}", this.DataFinal, this.HoraFinal).Trim();
                
                if (!DateTime.TryParseExact(lDataTexto, "dd/MM/yyyy HH:mm", lCultura, DateTimeStyles.None, out lData))
                {
                    if (!DateTime.TryParseExact(lDataTexto, "dd/MM/yyyy", lCultura, DateTimeStyles.None, out lData))
                    {
                        lData = DateTime.Now;
                    }
                }

                return lData;
            }
        }

        public string TipoOferta { get; set; }

        public string Tag { get; set; }
        
        #endregion

        public static void FiltrarLista(ref List<TransporteISIN> pLista, int pCodigoOfertaPublica)
        {
            DateTime lHoraAgora = DateTime.Now;

            List<int> lIDsParaRemover = new List<int>();

            for (int a = pLista.Count - 1; a >= 0; a--)
            {
                if (pLista[a].CodigoIPO.DBToInt32() != pCodigoOfertaPublica
                || pLista[a].DataHoraInicial > lHoraAgora 
                || pLista[a].DataHoraFinal   < lHoraAgora)
                    lIDsParaRemover.Add(a);
            }

            for (int a = 0; a < lIDsParaRemover.Count; a++)
            {
                pLista.RemoveAt(lIDsParaRemover[a]);
            }
        }
    }
}