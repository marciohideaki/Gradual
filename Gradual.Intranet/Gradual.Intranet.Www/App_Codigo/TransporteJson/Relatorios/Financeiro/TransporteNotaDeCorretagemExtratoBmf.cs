using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro
{
    public class TransporteNotaDeCorretagemExtratoBmf
    {
        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        public string Sentido { get; set; }

        public string Mercadoria { get; set; }

        public string Mercadoria_Serie { get; set; }

        public string Vencimento { get; set; }

        public string Quantidade { get; set; }

        public string PrecoAjuste { get; set; }

        public string TipoNegocio { get; set; }

        public string ValorOperacao { get; set; }
         
        public string DC { get; set; }

        public string TaxaOperacional { get; set; }
         
        public string Observacao { get; set; }

        public List<TransporteNotaDeCorretagemExtratoBmf> TraduzirLista(List<NotaDeCorretagemExtratoBmfInfo> pParametros)
        {
            var lRetorno = new List<TransporteNotaDeCorretagemExtratoBmf>();

            if (pParametros != null)
            {
                pParametros.ForEach(nc =>
                    {
                        lRetorno.Add(new TransporteNotaDeCorretagemExtratoBmf()
                            {
                                DC               = nc.DC,
                                Mercadoria       = nc.Mercadoria,
                                Mercadoria_Serie = nc.Mercadoria_Serie,
                                Observacao       = nc.Observacao,
                                PrecoAjuste      = nc.PrecoAjuste.ToString("N4"),
                                Quantidade       = Math.Abs(nc.Quantidade).ToString(),
                                Sentido          = nc.Sentido,
                                TaxaOperacional  = Math.Abs(nc.TaxaOperacional).ToString("N2"),
                                TipoNegocio      = nc.TipoNegocio,
                                ValorOperacao    = Math.Abs(nc.ValorOperacao).ToString("N2"),
                                Vencimento       = nc.Vencimento.ToString("dd/MM/yyyy")

                            });
                    });
            }

            return lRetorno;
        }
    }
}