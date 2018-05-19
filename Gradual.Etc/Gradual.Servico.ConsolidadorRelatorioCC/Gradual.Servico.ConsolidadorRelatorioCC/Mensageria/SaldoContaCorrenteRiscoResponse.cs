using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servico.Contratos.ConsolidadorRelatorioCC.Mensageria
{
    public class SaldoContaCorrenteRiscoResponse<T> : MensagemBase
    {
        public T Objeto { set; get; }

        public List<T> ObjetoLista { get; set; }

        public SaldoContaCorrenteRiscoResponse()
        {
            this.ObjetoLista = new List<T>();
        }
    }
}
