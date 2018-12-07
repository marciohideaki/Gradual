using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PerfilSuitabilityIntegracaoFundosResponse
    {
        public string PerfilSuitability     { get; set; }

        public int idPerfilSuitability      { get; set; }
        
        public int IdClienteSuitability     { get; set; }
        
        public int IdCliente                { get; set; }
        
        public string Status                { get; set; }
        
        public DateTime dtRealizacao        { get; set; }
        
        public string PreenchidoPeloCliente { get; set; }
        
        public string LoginRealizado        { get; set; }
        
        public string Fonte                 { get; set; }
        
        public string Respostas             { get; set; }
    }
}
