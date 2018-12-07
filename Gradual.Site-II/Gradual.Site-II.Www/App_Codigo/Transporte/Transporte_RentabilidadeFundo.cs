using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;

namespace Gradual.Site.Www.Transporte
{
    public class Transporte_RentabilidadeFundo
    {   
        public string Ano           { get ;set; }
        public string PercentualJan { get ;set; }
        public string PercentualFev { get ;set; }
        public string PercentualMar { get ;set; }
        public string PercentualAbr { get ;set; }
        public string PercentualMai { get ;set; }
        public string PercentualJun { get ;set; }
        public string PercentualJul { get ;set; }
        public string PercentualAgo { get ;set; }
        public string PercentualSet { get ;set; }
        public string PercentualOut { get ;set; }
        public string PercentualNov { get ;set; }
        public string PercentualDez { get ;set; }
        public string PercentualAno { get ;set; }

        public List<Transporte_RentabilidadeFundo> TraduzirLista(List<IntegracaoFundosSimulacaoInfo> pInfo)
        {
            var lRetorno = new List<Transporte_RentabilidadeFundo>();

            Transporte_RentabilidadeFundo lTrans = null;

            var lLista = from a in pInfo
                         group a by a.Data.Year into g
                         select new
                         {
                             Ano = g.Key,
                             ListaMeses = g.ToList(),
                         };

            foreach (var a in lLista)
            {
                lTrans = new Transporte_RentabilidadeFundo();

                lTrans.Ano           = a.Ano.ToString();

                lTrans.PercentualJan = "-";
                lTrans.PercentualFev = "-";
                lTrans.PercentualMar = "-";
                lTrans.PercentualAbr = "-";
                lTrans.PercentualMai = "-";
                lTrans.PercentualJun = "-";
                lTrans.PercentualJul = "-";
                lTrans.PercentualAgo = "-";
                lTrans.PercentualSet = "-";
                lTrans.PercentualOut = "-";
                lTrans.PercentualNov = "-";
                lTrans.PercentualDez = "-";
                lTrans.PercentualAno = "-";
                
                //decimal lVariacaoAno = 0.0M;
                if (a.ListaMeses != null && a.ListaMeses.Count > 0)
                {
                    lTrans.PercentualAno = a.ListaMeses[0].VariacaoAno.ToString("N2");
                }

                foreach (var b in a.ListaMeses)
                {
                    lTrans.PercentualJan = (b.Data.Month == 1 && lTrans.PercentualJan  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualJan;
                    lTrans.PercentualFev = (b.Data.Month == 2 && lTrans.PercentualFev  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualFev;
                    lTrans.PercentualMar = (b.Data.Month == 3 && lTrans.PercentualMar  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualMar;
                    lTrans.PercentualAbr = (b.Data.Month == 4 && lTrans.PercentualAbr  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualAbr;
                    lTrans.PercentualMai = (b.Data.Month == 5 && lTrans.PercentualMai  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualMai;
                    lTrans.PercentualJun = (b.Data.Month == 6 && lTrans.PercentualJun  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualJun;
                    lTrans.PercentualJul = (b.Data.Month == 7 && lTrans.PercentualJul  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualJul;
                    lTrans.PercentualAgo = (b.Data.Month == 8 && lTrans.PercentualAgo  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualAgo;
                    lTrans.PercentualSet = (b.Data.Month == 9 && lTrans.PercentualSet  == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualSet;
                    lTrans.PercentualOut = (b.Data.Month == 10 && lTrans.PercentualOut == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualOut;
                    lTrans.PercentualNov = (b.Data.Month == 11 && lTrans.PercentualNov == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualNov;
                    lTrans.PercentualDez = (b.Data.Month == 12 && lTrans.PercentualDez == "-") ? b.Variacao.ToString("N2") : lTrans.PercentualDez;
                    //lVariacaoAno += b.Variacao;
                    // lVariacaoAno.ToString("N2");
                }

                
                lRetorno.Add(lTrans);
            }
            return lRetorno;
        }
    }
}