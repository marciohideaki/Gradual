using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.BackOffice.BrokerageProcessor.Lib.IMBARQ
{
    [Serializable]
    public class ItemRelatorioPosicao
    {
        public string ISIN { get; set; }
        public string PrecoExercicio { get; set; }
        public string DataVencimento { get; set; }
        public string Distribuicao { get; set; }
        public string Serie { get; set; }
        public string CodigoNegociacao { get; set; }
        public string PosicaoCompradaBloqueadaExercicio { get; set; }
        public string PosicaoAtual { get; set; }
        public string PosicaoInicial { get; set; }
        public string CompradaDia { get; set; }
        public string VendidaDia { get; set; }
        public string CompradaTransferencia { get; set; }
        public string VendidaTransferencia { get; set; }
        public string PosicaoCobertaVendida { get; set; }
        public string PosicaoDescobertaVendida { get; set; }
    }


}
