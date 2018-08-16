using System.Collections.Generic;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteLimiteBovespa
    {
        #region | Propriedades

        public int CodBovespa { get; set; }

        public bool FlagOperaAVista { get; set; }

        public decimal LimiteAVista { get; set; }

        public string VencimentoAVista { get; set; }

        public bool FlagOperaAVistaDescoberto { get; set; }

        public decimal LimiteAVistaDescoberto { get; set; }

        public string VencimentoAVistaDescoberto { get; set; }

        public bool FlagOperaOpcao { get; set; }

        public decimal LimiteOpcao { get; set; }

        public string VencimentoOpcao { get; set; }

        public bool FlagOperaOpcaoDescoberto { get; set; }

        public decimal LimiteOpcaoDescoberto { get; set; }

        public string VencimentoOpcaoDescoberto { get; set; }

        public decimal ValorMaximoDaOrdem { get; set; }

        public string VencimentoMaximoDaOrdem { get; set; }

        public bool FlagOperaOrdemStop { get; set; }

        public bool FlagOperaContaMargem { get; set; }

        public bool FlagOperaBovespa { get; set; }

        public bool FlagOperaBMF { get; set; }

        public bool FlagOperaAVistaExpirarLimite { get; set; }

        public bool FlagOperaAVistaDescobertoExpirarLimite { get; set; }

        public bool FlagOperaOpcaoExpirarLimite { get; set; }

        public bool FlagOperaOpcaoDescobertoExpirarLimite { get; set; }

        public bool FlagOperaAVistaIncluirLimite { get; set; }

        public bool FlagOperaAVistaDescobertoIncluirLimite { get; set; }

        public bool FlagOperaOpcaoIncluirLimite { get; set; }

        public bool FlagOperaOpcaoDescobertoIncluirLimite { get; set; }

        public bool FlagOperaAVistaRenovarLimite { get; set; }

        public bool FlagOperaAVistaDescobertoRenovarLimite { get; set; }

        public bool FlagOperaOpcaoRenovarLimite { get; set; }

        public bool FlagOperaOpcaoDescobertoRenovarLimite { get; set; }

        public bool FlagMaximoDaOrdem { get; set; }

        public bool FlagMaximoDaOrdemExpirarLimite { get; set; }

        public bool FlagMaximoDaOrdemIncluirLimite { get; set; }

        public bool FlagMaximoDaOrdemRenovarLimite { get; set; }

        public List<string> Permissoes { get; set; }

        #endregion
    }
}