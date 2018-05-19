using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum.Dados;

namespace Gradual.OMS.Contratos.Comum.Mensagens
{
    /// <summary>
    /// Mensagem de resposta a uma solicitação de geração de 
    /// metadados em banco de dados
    /// </summary>
    public class GerarDbMetadadoResponse : MensagemResponseBase
    {
        /// <summary>
        /// Resultado das listas geradas
        /// </summary>
        public List<ListaInfo> Listas { get; set; }

        /// <summary>
        /// Construtor default
        /// </summary>
        public GerarDbMetadadoResponse()
        {
            this.Listas = new List<ListaInfo>();
        }
    }
}
