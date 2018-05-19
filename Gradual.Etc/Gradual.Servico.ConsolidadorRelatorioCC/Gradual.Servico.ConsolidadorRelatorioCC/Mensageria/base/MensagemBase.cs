using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Servico.Contratos.ConsolidadorRelatorioCC.Enum;

namespace Servico.Contratos.ConsolidadorRelatorioCC.Mensageria
{
	 public abstract class MensagemBase
    {
        [Category("MensagemBase")]
        public DateTime DataResposta { get; set; }
        [Description("Descrição da mensagem retornada pela validação de risco")]
        public string DescricaoResposta { get; set; }
        [Description("Descrição do trace de uma mensagem de erro")]
        public string StackTrace { get; set; }
        [Description("Tipo de de critica retornada pela validação de risco")]
        public CriticaMensagemEnum StatusResposta { get; set; }

        public MensagemBase()
        {
            DataResposta = DateTime.Now;
        }
    }
}
