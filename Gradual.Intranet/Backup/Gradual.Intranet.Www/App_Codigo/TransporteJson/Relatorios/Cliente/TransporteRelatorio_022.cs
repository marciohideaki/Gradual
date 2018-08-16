using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios
{
    public class TransporteRelatorio_022
    {
        #region Propriedades
        public string CodigoCliente    { get; set; }
        public string DataPregao       { get; set; }
        public string Papel            { get; set; }
        public string QtdeCompras      { get; set; }
        public string QtdeVendas       { get; set; }
        public string QtdeLiquida      { get; set; }
        public string Preco            { get; set; }
        public string VolCompras       { get; set; }
        public string VolVendas        { get; set; }
        public string VolLiquido       { get; set; }
        public string CodigoAssessor   { get; set; }
        public string VlNegocio        { get; set; }

        public string MostraTotal      { get; set; }

        public string TotalQtdeCompras { get; set; }
        public string TotalQtdeVendas  { get; set; }
        public string TotalQtdeLiquida { get; set; }
        public string TotalVolCompras  { get; set; }
        public string TotalVolVendas   { get; set; }
        public string TotalVolLiquida  { get; set; }

        public string Ordem { get; set; }
        #endregion

        #region Construtores
        public TransporteRelatorio_022( PapelPorClienteInfo info )
        {
            this.CodigoCliente  = info.CodigoCliente.ToString();
            this.DataPregao     = info.DataPregao   .ToString("dd/MM/yyyy");
            this.Papel          = info.Papel        .ToString();
            this.QtdeCompras    = info.QtdeCompras  .ToString("N2");
            this.QtdeVendas     = info.QtdeVendas   .ToString("N2");
            this.QtdeLiquida    = info.QtdeLiquida  .ToString("N2");
            this.Preco          = info.Preco        .ToString("N6");
            this.VolCompras     = info.VolCompras   .ToString("N2");
            this.VolVendas      = info.VolVendas    .ToString("N2");
            this.VolLiquido     = info.VolLiquido.ToString("N2");
            this.CodigoAssessor = info.CodigoAssessor.ToString();
            this.VlNegocio      = info.VlNegocio.ToString("N2");
            this.MostraTotal    = "sim";

            
        }

        public TransporteRelatorio_022()
        {}
        #endregion

        #region Métodos
        public List<TransporteRelatorio_022> TraduzirLista(List<PapelPorClienteInfo> pParametros, string pPapel = "")
        {
            var lRetorno = new List<TransporteRelatorio_022>();

            TransporteRelatorio_022 lPapelCliente = null;

            List<PapelPorClienteInfo> lLista =  IncluiTotal(pParametros);

            for (int i = 0; i < lLista.Count; i++ )
            {
                PapelPorClienteInfo papel = lLista[i];

                if (pPapel != string.Empty)
                {
                    if (papel.Papel.IndexOf(pPapel) == -1)
                    {
                        continue;
                    }
                }

                lPapelCliente = new TransporteRelatorio_022();

                lPapelCliente.CodigoCliente  = papel.CodigoCliente.ToString();
                lPapelCliente.CodigoAssessor = papel.CodigoAssessor.ToString();
                lPapelCliente.DataPregao     = papel.DataPregao.ToString("dd/MM/yyyy");
                lPapelCliente.Papel          = papel.Papel.ToString();
                lPapelCliente.QtdeCompras    = papel.QtdeCompras.ToString("N0");
                lPapelCliente.QtdeVendas     = papel.QtdeVendas.ToString("N0");
                lPapelCliente.QtdeLiquida    = papel.QtdeLiquida.ToString("N0");
                lPapelCliente.Preco          = papel.Preco.ToString("N6");
                lPapelCliente.VolCompras     = papel.VolCompras.ToString("N2");
                lPapelCliente.VolVendas      = papel.VolVendas.ToString("N2");
                lPapelCliente.VolLiquido     = papel.VolLiquido.ToString("N2");
                lPapelCliente.VlNegocio      = papel.VlNegocio.ToString("N2");
                lPapelCliente.MostraTotal    = papel.MostraTotal;

                lPapelCliente.TotalQtdeCompras = papel.TotalQtdeCompras.ToString("N0");
                lPapelCliente.TotalQtdeVendas  = papel.TotalQtdeVendas.ToString("N0");
                lPapelCliente.TotalVolCompras  = papel.TotalVolCompras.ToString("N2");
                lPapelCliente.TotalVolVendas   = papel.TotalVolVendas.ToString("N2");
                lPapelCliente.Ordem            = papel.Ordem.ToString();
                
                lRetorno.Add(lPapelCliente);
            }

            return lRetorno;
        }

        public List<PapelPorClienteInfo> IncluiTotal(List<PapelPorClienteInfo> pParametros)
        {
            var lRetornoTemp = new List<PapelPorClienteInfo>();
            
            var lInsere = pParametros;

            var lLista = from a in pParametros
                         group a by new
                         {
                             a.Papel,
                             a.CodigoCliente
                         } into g
                         select new PapelPorClienteInfo()
                         {
                             Papel = g.Key.Papel,
                             CodigoCliente = g.Key.CodigoCliente,
                             Resultado = g.ToList()
                         };

            int TotalQtdeCompras    = 0;
            int TotalQtdeVendas     = 0;
            decimal TotalVolVendas  = 0.0M;
            decimal TotalVolCompras = 0.0M;

            foreach (var a in lLista)
            {
                PapelPorClienteInfo lLinhaTotal = new PapelPorClienteInfo();
                lLinhaTotal.CodigoCliente = a.Resultado[0].CodigoCliente;
                lLinhaTotal.Papel         = a.Resultado[0].Papel;
                lLinhaTotal.DataPregao    = a.Resultado[0].DataPregao.AddHours(1.0);
                lLinhaTotal.VolLiquido    = a.Resultado[0].VolLiquido;
                lLinhaTotal.QtdeLiquida   = a.Resultado[0].QtdeLiquida;
                lLinhaTotal.Preco         = a.Resultado[0].Preco;
                lLinhaTotal.Ordem         = 1;

                TotalQtdeCompras = 0;
                TotalQtdeVendas  = 0;
                TotalVolVendas   = 0.0M;
                TotalVolCompras  = 0.0M;
                

                foreach (PapelPorClienteInfo b in a.Resultado)
                {
                    TotalQtdeCompras += b.QtdeCompras;
                    TotalQtdeVendas  += b.QtdeVendas;

                    TotalVolVendas   += b.VolVendas;
                    TotalVolCompras  += b.VolCompras;
                }

                lLinhaTotal.TotalVolCompras  = TotalVolCompras;
                lLinhaTotal.TotalVolVendas   = TotalVolVendas;
                
                lLinhaTotal.TotalQtdeCompras = TotalQtdeCompras;
                lLinhaTotal.TotalQtdeVendas  = TotalQtdeVendas;
                
                lLinhaTotal.MostraTotal      = "sim";

                lInsere.Add(lLinhaTotal);
            }

            lRetornoTemp.AddRange(lInsere);

            var lRetorno = (from a in lRetornoTemp orderby a.CodigoCliente, a.Papel, a.Ordem ascending select a).ToList();

            return lRetorno;
        }
        #endregion
    }
}