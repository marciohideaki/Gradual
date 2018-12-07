using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos;
using Gradual.OMS.Library;
using Gradual.Site.DbLib.Dados.MinhaConta.Suitability;

namespace Gradual.Site.DbLib.Mensagens.IntegracaoFundos
{
    public class PesquisarIntegracaoFundosResponse : MensagemResponseBase
    {
        public List<IntegracaoFundosInfo> ListaFundos   { get; set; }

        public int IdListaProduto                       { get; set; }

        public PerfilSuitabilityInfo PerfilSuitability  { get; set; }

        public bool UtilizarRentabilidade               { get; set; }

        public bool UtilizarPrioridade                  { get; set; }

        public int TopPrioridade                        { get; set; }

        public PesquisarIntegracaoFundosResponse()
        {
            ListaFundos = new List<IntegracaoFundosInfo>();

            PerfilSuitability = new PerfilSuitabilityInfo();
        }
    }
}
