using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Spider.Lib.Dados;

namespace Gradual.Spider.Www.App_Codigo.Transporte
{
    public class Transporte_GerenciadorPlataforma
    {
        #region Propriedades

        public string CodigoTraderPlataforma { get; set; }

        public string CodigoTrader { get; set; }

        public string CodigoPlataforma { get; set; }

        public string NomePlataforma { get; set; }

        public string CodigoAcesso { get; set; }

        public string NomeAcesso { get; set; }

        public string CodigoSessao { get; set; }

        public string NomeSessao { get; set; }

        public string DataAtualizacao { get; set; }

        public List<int> CodigoPlataformas { get; set; }

        #endregion

        #region Métodos

        public List<Transporte_GerenciadorPlataforma> TraduzirLista(List<GerenciadorPlataformaInfo> pLista)
        {
            var lRetorno = new List<Transporte_GerenciadorPlataforma>();

            foreach (var a in pLista)
            {
                var lTrans = new Transporte_GerenciadorPlataforma();
                
                lTrans.CodigoTrader     = a.CodigoTrader.ToString();
                lTrans.CodigoAcesso     = a.CodigoAcesso.ToString();
                lTrans.CodigoPlataforma = a.CodigoPlataforma.ToString();
                lTrans.DataAtualizacao  = a.DataUltimoEvento.ToString("dd/MM/yyyy");
                lTrans.CodigoSessao     = a.CodigoSessao.ToString();
                lTrans.NomePlataforma   = a.NomePlataforma;
                lTrans.NomeSessao       = a.NomeSessao;

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        public Transporte_GerenciadorPlataforma TraduzirLog(List<Transporte_GerenciadorPlataforma> pLista)
        {
            var lRetorno = new Transporte_GerenciadorPlataforma();

            lRetorno.CodigoPlataformas = new List<int>();

            lRetorno.CodigoAcesso = pLista[0].CodigoAcesso;
            lRetorno.CodigoSessao = pLista[0].CodigoSessao;
            lRetorno.CodigoTrader = pLista[0].CodigoTrader;

            for (int i = 0; i < pLista.Count; i++)
            {
                lRetorno.CodigoPlataformas.Add(pLista[i].CodigoPlataforma.DBToInt32());
            }

            return lRetorno;
        }

        public List<GerenciadorPlataformaInfo> ToPlataformaSessaoInfo(Transporte_GerenciadorPlataforma pPlataforma)
        {
            var lRetorno = new List<GerenciadorPlataformaInfo>();

            var lTrans = new GerenciadorPlataformaInfo();

            for (int i=0; i < pPlataforma.CodigoPlataformas.Count; i++)
            {
                var p = pPlataforma;

                lTrans = new GerenciadorPlataformaInfo();

                lTrans.DataUltimoEvento = DateTime.Now;
                lTrans.CodigoTrader     = int.Parse(p.CodigoTrader);
                lTrans.CodigoSessao     = !string.IsNullOrEmpty(p.CodigoSessao)? int.Parse(p.CodigoSessao) : new Nullable<int>()  ;
                lTrans.CodigoPlataforma = pPlataforma.CodigoPlataformas[i];
                lTrans.CodigoAcesso     = int.Parse(p.CodigoAcesso);

                if (!string.IsNullOrEmpty(p.CodigoTraderPlataforma))
                {
                    lTrans.CodigoTraderPlataforma = int.Parse(p.CodigoTraderPlataforma);
                }

                lRetorno.Add(lTrans);
            }

            return lRetorno;
        }

        public Transporte_GerenciadorPlataforma Traduzir(GerenciadorPlataformaInfo pParametros)
        {
            var lRetorno = new Transporte_GerenciadorPlataforma();

            return lRetorno;
        }
        #endregion
    }
}