using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Risco.Regras
{
    /// <summary>
    /// Classe de configurações para a regra de caracteristicas de operação
    /// </summary>
    [Serializable]
    public class RegraCaracteristicaOperacaoConfig
    {
        public bool PermiteOperarOpcao { get; set; }

        public bool PermiteOperarFuturo { get; set; }

        public bool PermiteOperarTermo { get; set; }

        public bool PermiteOperarAcao { get; set; }

        public bool PermiteBolsaBovespa { get; set; }

        public bool PermiteBolsaBMF { get; set; }
    }
}
