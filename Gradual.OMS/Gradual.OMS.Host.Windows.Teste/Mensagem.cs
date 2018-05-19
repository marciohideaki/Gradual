using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Host.Windows.Teste
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Mensagem
    {
        public DateTime Data { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MensagemRequestBase Request { get; set; }
        
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MensagemResponseBase Response { get; set; }
        
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MensagemSinalizacaoBase Sinalizacao { get; set; }

        public Mensagem()
        {
            this.Data = DateTime.Now;
        }
    }
}
