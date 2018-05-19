using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    public class ClienteInfo
    {
        /// <summary>
        /// Código interno do cliente
        /// </summary>
        public string CodigoClienteInterno { get; set; }
        
        /// <summary>
        /// Lista de contas que o cliente tem nas origens.
        /// </summary>
        public List<ContaInfo> Contas { get; set; }
        
        /// <summary>
        /// Lista com os códigos de clientes nas origens externas. Os parâmetros são origem e lista de códigos externos.
        /// Esta lista pode ser montada automaticamente quando a lista de contas estiver sendo preenchida.
        /// </summary>
        public Dictionary<string, List<string>> CodigosClienteExterno { get; set; }
    }
}
