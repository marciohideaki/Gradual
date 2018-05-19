using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Spider.LimiteRestricao.Test.App_Codigo
{
    public class TransporteLimiteBovespa
    {
        #region | Propriedades

        public int IdParametroLimiteDescobertoMercadoAVista { get; set; }
        public int IdParametroLimiteCompraMercadoAVista     { get; set; }
        public int IdParametroLimiteDescobertoMercadoOpcoes { get; set; }
        public int IdParametroLimiteCompraMercadoOpcoes     { get; set; }
        public int IdParametroLimiteMaximoDaOrdem           { get; set; }
        public int IdParametroLimitePerdaMaximaOpcao        { get; set; }
        public int IdParametroLimitePerdaMaximaVista        { get; set; }


        public int CodBovespa                       { get; set; }

        public decimal LimiteAVista                 { get; set; }

        public string VencimentoAVista              { get; set; }

        public decimal LimiteAVistaDescoberto       { get; set; }

        public string VencimentoAVistaDescoberto    { get; set; }

        public decimal LimiteOpcao                  { get; set; }

        public string VencimentoOpcao               { get; set; }

        public decimal LimiteOpcaoDescoberto        { get; set; }

        public string VencimentoOpcaoDescoberto     { get; set; }

        public decimal ValorMaximoDaOrdem           { get; set; }

        public string VencimentoMaximoDaOrdem       { get; set; }

        public decimal LimitePerdaMaximaOpcao       { get; set; }

        public string VencimentoPerdaMaximaOpcao    { get; set; }

        public decimal LimitePerdaMaximaVista       { get; set; }

        public string VencimentoPerdaMaximaVista    { get; set; }

        public List<string> Permissoes              { get; set; }

        #endregion
    }
}
