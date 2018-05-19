using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orbite.RV.Contratos.MarketData.Dados;

namespace Orbite.RV.Contratos.MarketData.Mensagens
{
    public class ReceberListaInstrumentosRequest
    {
        public ReceberListaInstrumentoTipoListaEnum TipoLista { get; set; }
        public DateTime? DataReferencia { get; set; }
        public InstrumentoTipoEnum? InstrumentoTipo { get; set; }

        public ReceberListaInstrumentosRequest()
        {
            this.TipoLista = ReceberListaInstrumentoTipoListaEnum.Padrao;
        }
    }
}
