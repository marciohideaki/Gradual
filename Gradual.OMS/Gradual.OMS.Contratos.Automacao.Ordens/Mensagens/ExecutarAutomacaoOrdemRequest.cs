using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Contratos.Automacao.Ordens.Dados;
using Gradual.OMS.Contratos.Comum.Mensagens;
using Gradual.OMS.Contratos.Ordens;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Gradual.OMS.Contratos.Automacao.Ordens.Mensagens
{
    public class ExecutarAutomacaoOrdemRequest : MensagemRequestBase
    {
        public ItemAutomacaoTipoEnum AutomacaoTipo { get; set; }
        public string CodigoBolsa { get; set; }
        public string CodigoCliente { get; set; }
        public string Instrumento { get; set; }
        /*public decimal PrecoEnvioGain { get; set; }
        public decimal PrecoEnvioLoss { get; set; }
        public decimal PrecoGain { get; set; }
        public decimal PrecoLoss { get; set; }
        public decimal PrecoStartGain { get; set; }
        public decimal PrecoStartLoss { get; set; }
        public decimal TaxaInicio { get; set; }
        public decimal TaxaMovel { get; set; }*/
        [DataMember]
        public List<AutomacaoPrecosTaxasInfo> PrecosTaxas { get; set; }

        public decimal Quantidade { get; set; }
        public ItemPrazoExecucaoEnum PrazoExecucao { get; set; }
        //public DateTime DataValidade { get; set; }
        public int IdSistema { get; set; }

        public ExecutarAutomacaoOrdemRequest()
        {
            this.PrecosTaxas = new List<AutomacaoPrecosTaxasInfo>();
        }
    }
}
