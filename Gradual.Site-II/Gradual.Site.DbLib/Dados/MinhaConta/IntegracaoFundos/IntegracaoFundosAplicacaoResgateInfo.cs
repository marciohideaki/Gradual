using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.IntegracaoFundos
{
    public class IntegracaoFundosAplicacaoResgateInfo
    {
        public int CodigoCliente        { get; set; }
        public int CodigoInternoFundo   { get; set; }
        public string StAprovado        { get; set; }
        public int StatusMovimento      { get; set; }
        public int CodigoCarteira       { get; set; }
        public DateTime DtInclusao      { get; set; }
        public string DsMotivoStatus    { get; set; }
        public string ContaCredito      { get; set; }
        public int IdOperacaoFinancial  { get; set; }

        public string CodigoGestor                  { get; set; }
        public string CodigoFundo                   { get; set; }
        public string CodigoBancoCliente            { get; set; }
        public string CodigoAgencia                 { get; set; }
        public string CodigoConta                   { get; set; }
        public string DigitoConferencia             { get; set; }
        public string CodigoSubConta                { get; set; }
        public string DataInclusao                  { get; set; }
        public string DataAplicacao                 { get; set; }
        public string CodigoSequenciaAplicacao      { get; set; }
        public string TipoRegistro                  { get; set; }
        public string DataLancamento                { get; set; }
        public string TipoMovimento                 { get; set; }
        public string CodigoSeqLancamento           { get; set; }
        public string DataProcessamento             { get; set; }
        public string QtdeCotasMovimento            { get; set; }
        public string QtdeUfirMovimento             { get; set; }
        public decimal VlrCotacaoMovimento          { get; set; }
        public decimal VlrBrutoMovimento            { get; set; }
        public decimal VlrCustoMovimento            { get; set; }
        public decimal VlrCustoAplicacao            { get; set; }
        public decimal VlrCustoMedioResgate         { get; set; }
        public decimal VlrIRMovimento               { get; set; }
        public decimal VlrIOFMovimento              { get; set; }
        public decimal VlrTaxaPerfomance            { get; set; }
        public decimal VlrTaxaResgateAntecipado     { get; set; }
        public decimal VlrLiquidoCalculadoMovimento { get; set; }
        public decimal VlrLiquidoSolicitado         { get; set; }
    }
}
