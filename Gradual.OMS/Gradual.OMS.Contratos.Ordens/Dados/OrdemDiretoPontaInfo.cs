using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Estrutura para representar uma ponta de uma ordem de direto
    /// </summary>
    [Serializable]
    public class OrdemDiretoPontaInfo
    {
        /// <summary>
        /// Ponta da oferta. Valores aceitos: 1 = compra; 2 = venda
        /// </summary>
        [Description("Ponta da oferta. Valores aceitos: 1 = compra; 2 = venda")]
        public OrdemDirecaoEnum Side { get; set; }

        /// <summary>
        /// Identificador único da ordem, conforme atribuído pelo cliente
        /// </summary>
        [Description("Identificador único da ordem, conforme atribuído pelo cliente")]
        public string ClOrdID { get; set; }

        /// <summary>
        /// Número de ações ou contratos da oferta
        /// </summary>
        [Description("Número de ações ou contratos da oferta")]
        public double OrderQty { get; set; }

        /// <summary>
        /// Indica se a posição resultante de uma operação deve ser de fechamento. 
        /// Valor aceito: C = fechamento
        /// </summary>
        [Description("Indica se a posição resultante de uma operação deve ser de fechamento. Valor aceito: C = fechamento")]
        public char? PositionEffect { get; set; }

        /// <summary>
        /// Mnemônico de subconta. Obrigatório se NoAllocs > 0. Deve ser sermpre o primeiro campo do grupo de repetição. 
        /// </summary>
        [Description("Mnemônico de subconta. Obrigatório se NoAllocs > 0. Deve ser sermpre o primeiro campo do grupo de repetição.")]
        public string AllocAccount { get; set; }

        /// <summary>
        /// Identifica a fonte do campo AllocAccount. 
        /// Valor aceito: 99 = outros (código do cliente ou proprietário)
        /// </summary>
        [Description("Identifica a fonte do campo AllocAccount. Valor aceito: 99 = outros (código do cliente ou proprietário)")]
        public int? AllocAcctIDSource { get; set; }

        /// <summary>
        /// Identifica como a operação deve ser especificada. Valor aceito: 1 = especificação obrigatória 
        /// (operação de repasse); informações sobre especificação não fornecidas (incompletas). A ausência 
        /// desse campo indica que a especificação não é obrigatória ou foi fornecida com a operação.
        /// </summary>
        [Description("Identifica como a operação deve ser especificada. Valor aceito: 1 = especificação obrigatória (operação de repasse); informações sobre especificação não fornecidas (incompletas). A ausência desse campo indica que a especificação não é obrigatória ou foi fornecida com a operação.")]
        public string TradeAllocIndicator { get; set; }

        /// <summary>
        /// Lista de envolvidos na operação.
        /// </summary>
        public List<PartyInfo> Parties { get; set; }

        /// <summary>
        /// Construtor. Inicializa a lista de Parties
        /// </summary>
        public OrdemDiretoPontaInfo()
        {
            this.Parties = new List<PartyInfo>();
        }
    }
}
