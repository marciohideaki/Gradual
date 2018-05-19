using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Custodia.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.Risco
{
    /// <summary>
    /// Classe de auxilio para montar o dicionário do cache de custódia no serviço de risco
    /// </summary>
    public class CustodiaHelper
    {
        /// <summary>
        /// Codigo da custódia
        /// </summary>
        public string CodigoCustodia { get; set; }

        /// <summary>
        /// Arvore com a custódia montada
        /// </summary>
        public Arvore<string, CustodiaPosicaoInfo> ArvoreCustodia { get; set; }
    }
}
