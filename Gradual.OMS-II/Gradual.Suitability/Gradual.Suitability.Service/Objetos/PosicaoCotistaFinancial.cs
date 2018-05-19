using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Suitability.Service.Objetos
{
    public class PosicaoCotistaFinancial
    {
        public string IdPosicao               { get; set; }
        public string IdOperacao              { get; set; }
        public string IdCotista               { get; set; }
        public string IdCarteira              { get; set; }
        public string ValorAplicacao          { get; set; }
        public string DataAplicacao           { get; set; }
        public string DataConversao           { get; set; }
        public string CotaAplicacao           { get; set; }
        public string CotaDia                 { get; set; }
        public string ValorBruto              { get; set; }
        public string ValorLiquido            { get; set; }
        public string QuantidadeInicial       { get; set; }
        public string Quantidade              { get; set; }
        public string QuantidadeBloqueada     { get; set; }
        public string DataUltimaCobrancaIR    { get; set; }
        public string ValorIR                 { get; set; }
        public string ValorIOF                { get; set; }
        public string ValorPerformance        { get; set; }
        public string ValorIOFVirtual         { get; set; }
        public string QuantidadeAntesCortes   { get; set; }
        public string ValorRendimento         { get; set; }
        public string DataUltimoCortePfee     { get; set; }
        public string PosicaoIncorporada      { get; set; }
        public string CodigoAnbima            { get; set; }
    }
}
