using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    [Serializable]
    public class ListarItensAutomacaoOrdemRequest : MensagemRequestBase
    {
        public ItemAutomacaoTipoEnum? TipoAutomacaoOrdem { get; set; }
        public ItemAutomacaoStatusEnum? StatusAutomacaoOrdem { get; set; }
        public int? CodigoCliente { get; set; }
        public int? CodigoItemAutomacaoOrdem { get; set; }
        public DateTime DataDeOrdemEnvio{ get; set; }
        public string CodigoNegocio { get; set; }
        public string Login { get; set; }
    }
}
