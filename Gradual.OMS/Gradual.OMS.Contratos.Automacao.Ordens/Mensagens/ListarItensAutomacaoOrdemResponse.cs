using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class ListarItensAutomacaoOrdemResponse : MensagemResponseBase
    {

        /// <summary>
        /// Código do cliente
        /// </summary>
        public string CodigoCliente { get; set; }

        /// <summary>
        /// Código do Sistema Cliente, por exemplo: HB (HomeBroker), Plataforma, Robo, etc.
        /// </summary>
        public string CodigoSistemaCliente { get; set; }

        public List<ItemAutomacaoOrdemInfo> ListaDeAutomacaoOrdens { get; set; }

        public ListarItensAutomacaoOrdemResponse()
        {
            ListaDeAutomacaoOrdens = new List<ItemAutomacaoOrdemInfo>();
        }
    }   
}
