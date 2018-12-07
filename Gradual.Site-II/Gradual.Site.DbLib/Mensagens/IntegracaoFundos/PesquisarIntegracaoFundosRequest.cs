using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarIntegracaoFundosRequest :MensagemRequestBase
    {
        public string Indexadores       { get; set; }
        
        public int IdProduto            { get; set; }
        
        public string Cnpj              { get; set; }
        
        public int IdCategoria          { get; set; }
        
        public string NomeProduto       { get; set; }
        
        public string Perfis            { get; set; }

        public int IdPerfilSuitability  { get; set; }
    }
}
