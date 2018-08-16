using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteMonitoramentoRiscoVolumes
    {
        public TransporteMonitoramentoRiscoVolumes(){}

        public string VolumeTotalBovespa { get; set; }

        public string VolumeTotalBmf { get; set; }

        public TransporteMonitoramentoRiscoVolumes(decimal pVolumeBmf, decimal pVolumeBovespa)
        {
            VolumeTotalBovespa = pVolumeBovespa.ToString("N2");

            VolumeTotalBmf = pVolumeBmf.ToString("N2");
        }
    }
    public class TransporteParametrosMonitoramentoRisco
    {
        public TransporteParametrosMonitoramentoRisco() { }

        public TransporteParametrosMonitoramentoRisco(MonitoramentoRiscoLucroPrejuizoParametrosInfo info )
        {
            idUsuario  = info.IdUsuario.ToString();
            IdJanela   = info.IdJanela.ToString();
            NomePagina = info.NomeJanela;
            Colunas    = info.Colunas;
            Consulta   = info.Consulta;
        }

        public string idUsuario  { get; set; }

        public string IdJanela   { get; set; }

        public string NomePagina { get; set; }

        public string Colunas    { get; set; }
        
        public string Consulta   { get; set; }

    }

    public class TransporteParametrosMonitoramentoRiscoColunas
    {
        public string Colunas { get; set; }

        public TransporteParametrosMonitoramentoRiscoColunas(){}

        public TransporteParametrosMonitoramentoRiscoColunas(MonitoramentoRiscoLucroPrejuizoParametrosInfo info)
        {
            if (info.Colunas.IndexOf("|") > -1)
            {
                Colunas = info.Colunas.Remove(info.Colunas.LastIndexOf('|'));
            }
            else
            {
                Colunas = info.Colunas;
            }
        }
    }
}