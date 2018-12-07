using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Monitores.Risco.Lib;
using System.Globalization;

namespace Gradual.Site.Www.Transporte
{
    public class Transporte_Termo
    {
        private CultureInfo gCultura = new CultureInfo("pt-BR");

        public string DataExecucao { get; set; }

        public string DataVencimento { get; set; }

        public string CodigoCliente { get; set; }

        public string Instrumento { get; set; }

        public string LucroPrejuizo { get; set; }

        public string PrecoExecucao { get; set; }

        public string PrecoMercado { get; set; }

        public string Quantidade { get; set; }

        public string SubtotalLucroPrejuizo { get; set; }

        public string SubtotalQuantidade { get; set; }

        public string SubtotalValor { get; set; }

        public string PrecoMedio { get; set; }

        public string lCount { get; set; }

        public string SubtotalContrato { get; set; }

        public List<Transporte_Termo> TraduzirLista(List<PosicaoTermoInfo> pParametros)
        {
            var lRetorno = new List<Transporte_Termo>();
            var lRetornoTemp = new List<Transporte_Termo>();

            int lCount = 0;

            if (null != pParametros && pParametros.Count > 0)
                pParametros.ForEach(termo =>
                {
                    var lTemp = lRetornoTemp.Find(ordem => ordem.Instrumento == termo.Instrumento);

                    if (lTemp != null && !string.IsNullOrEmpty(lTemp.Instrumento))
                    {
                        lRetornoTemp.Remove(lTemp);

                        decimal lTempDecimal = 0.0M;

                        int lTempInt = 0;

                        lCount = int.Parse(lTemp.lCount) + 1;

                        lTemp.DataExecucao            = termo.DataExecucao.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.DataVencimento          = termo.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lTemp.CodigoCliente           = termo.IDCliente.ToString();
                        lTemp.Instrumento             = termo.Instrumento;
                        lTemp.PrecoExecucao           = termo.PrecoExecucao.ToString();
                        lTemp.PrecoMercado            = termo.PrecoMercado.ToString();
                        lTempInt                      = int.Parse(lTemp.SubtotalQuantidade.Replace(".", string.Empty)) + termo.Quantidade;
                        lTemp.SubtotalQuantidade      = string.Format("{0:#,0}", lTempInt);
                        lTempDecimal                  = 0.0M;
                        lTempDecimal                  = decimal.Parse(lTemp.SubtotalValor) + termo.PrecoExecucao;
                        lTemp.SubtotalValor           = lTempDecimal.ToString("N2");
                        lTempDecimal                  = 0.0M;
                        lTempDecimal                  = Convert.ToDecimal(lTemp.SubtotalLucroPrejuizo) + termo.LucroPrejuizo;
                        lTemp.SubtotalLucroPrejuizo   = lTempDecimal.ToString("N2");
                        decimal ltempSubTotalContrato = (termo.Quantidade * termo.PrecoExecucao);
                        lTemp.SubtotalContrato        = (Convert.ToDecimal(lTemp.SubtotalContrato, gCultura) + ltempSubTotalContrato).ToString("N2");
                        lTemp.lCount                  = lCount.ToString();
                        
                        lRetornoTemp.Add(lTemp);
                    }
                    else
                    {
                        Transporte_Termo lInfo = new Transporte_Termo();

                        lCount = 1;

                        lInfo.DataExecucao          = termo.DataExecucao.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.DataVencimento        = termo.DataVencimento.ToString("dd/MM/yyyy hh:mm:ss");
                        lInfo.CodigoCliente         = termo.IDCliente.ToString();
                        lInfo.Instrumento           = termo.Instrumento;
                        lInfo.LucroPrejuizo         = termo.LucroPrejuizo.ToString("N2");
                        lInfo.PrecoExecucao         = termo.PrecoExecucao.ToString();
                        lInfo.PrecoMercado          = termo.PrecoMercado.ToString();
                        lInfo.Quantidade            = termo.Quantidade.ToString();
                        lInfo.SubtotalValor         = termo.PrecoExecucao.ToString("N2");
                        lInfo.SubtotalLucroPrejuizo = termo.LucroPrejuizo.ToString("N2");
                        lInfo.SubtotalQuantidade    = termo.Quantidade.ToString();
                        lInfo.lCount                = lCount.ToString();
                        lInfo.SubtotalContrato      = (termo.Quantidade * termo.PrecoExecucao).ToString("N2");
                        lRetornoTemp.Add(lInfo);
                    }
                });

            for (int i = 0; i < lRetornoTemp.Count; i++)
            {
                Transporte_Termo termo = new Transporte_Termo();

                termo = lRetornoTemp[i];

                termo.PrecoMedio = (Math.Round(Convert.ToDecimal(lRetornoTemp[i].SubtotalValor) / Convert.ToInt32(lRetornoTemp[i].lCount), 2)).ToString();

                lRetorno.Add(termo);
            }

            return lRetorno;
        }
    }
}