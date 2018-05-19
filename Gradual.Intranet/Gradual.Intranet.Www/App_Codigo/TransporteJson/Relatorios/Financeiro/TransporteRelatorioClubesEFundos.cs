using System.Collections.Generic;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRelatorioClubesEFundos
    {
        public List<TransporteRelatorioClubes> ListaClubes { get; set; }
        public List<TransporteRelatorioFundos> ListaFundos { get; set; }
        public List<TransporteRelatorioRendaFixa> ListaRendaFixa { get; set; }

        public TransporteRelatorioClubesEFundos()
        {
            this.ListaClubes = new List<TransporteRelatorioClubes>();

            this.ListaFundos = new List<TransporteRelatorioFundos>();

            this.ListaRendaFixa = new List<TransporteRelatorioRendaFixa>();
        }
    }
}