using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Integracao.Sinacor.Dados
{
    /// <summary>
    /// Classe para retornar informações dos códigos cblc de cliente
    /// </summary>
    [Serializable]
    public class ClienteCBLCInfo
    {
        /// <summary>
        /// Código CBLC
        /// </summary>
        public string CodigoCBLC { get; set; }

        /// <summary>
        /// Tipo da conta (corrente ou investimento)
        /// </summary>
        public ClienteCBLCTipoContaEnum TipoConta { get; set; }
    }
}
