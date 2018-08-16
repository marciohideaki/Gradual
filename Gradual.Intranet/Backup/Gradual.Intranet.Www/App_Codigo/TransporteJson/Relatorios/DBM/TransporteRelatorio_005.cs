using System.Collections.Generic;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM
{
    public class TransporteRelatorio_005
    {
        public string Operador { get; set; }

        public string Cliente { get; set; }

        public string Bolsa { get; set; }

        public string CB { get; set; }

        public string DVDesconto { get; set; }

        public string DVPercentual { get; set; }

        public string CL { get; set; }

        public string FG { get; set; }

        public string PC { get; set; }

        public string VC { get; set; }

        public string TotalCB { get; set; }

        public string TotalDV { get; set; }

        public string TotalCL { get; set; }

        public string TotalFG { get; set; }

        public string TotalPC { get; set; }

        public string TotalVC { get; set; }

        public List<TransporteRelatorio_005> TraduzirLista(List<TotalClientePorAssessorInfo> pParametros)
        {
            var lRetorno = new List<TransporteRelatorio_005>();
            var lTotalCB = default(decimal);
            var lTotalDV = default(decimal);
            var lTotalCL = default(decimal);
            var lTotalFG = default(decimal);
            var lTotalPC = default(decimal);
            var lTotalVC = default(decimal);

            if (null != pParametros && pParametros.Count > 0)
            {
                pParametros.ForEach(tca =>
                {
                    lRetorno.Add(new TransporteRelatorio_005()
                    {
                        Bolsa = tca.DsBolsa.ToUpper(),
                        CB = tca.VlCorretagemBruta.ToString("N2"),
                        CL = tca.VlCorretagemLiquida.ToString("N2"),
                        Cliente = string.Concat(tca.CdCliente.ToString(), " - ", tca.NmCliente.ToStringFormatoNome()),
                        Operador = string.Concat(tca.CdAssessor.ToString(), " - ", tca.NmAssessor.ToStringFormatoNome()),
                        DVDesconto = tca.VlDescontoDv.ToString("N2"),
                        DVPercentual = tca.PcDescontoDv.ToString("N2"),
                        FG = tca.VlFg.ToString("N2"),
                        PC = tca.VlPc.ToString("N2"),
                        VC = tca.VlVc.ToString("N2"),
                    });

                    lTotalCB += tca.VlCorretagemBruta;
                    lTotalDV += tca.PcDescontoDv;
                    lTotalCL += tca.VlCorretagemLiquida;
                    lTotalFG += tca.VlFg;
                    lTotalPC += tca.VlPc;
                    lTotalVC += tca.VlVc;
                });

                lRetorno[0].TotalCB = lTotalCB.ToString("N2");
                lRetorno[0].TotalDV = lTotalDV.ToString("N2");
                lRetorno[0].TotalCL = lTotalCL.ToString("N2");
                lRetorno[0].TotalFG = lTotalFG.ToString("N2");
                lRetorno[0].TotalPC = lTotalPC.ToString("N2");
                lRetorno[0].TotalVC = lTotalVC.ToString("N2");
            }

            return lRetorno;
        }
    }
}