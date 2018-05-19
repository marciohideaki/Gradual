using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Intranet.Www.App_Codigo
{
    public class PosicaoFundo
    {
        public int IdPosicao                        { get; set; }
        public System.Nullable<int> IdOperacao      { get; set; }
        public String NomeFundo                     { get; set; }
        public int IdCotista                        { get; set; }
        public int IdCarteira                       { get; set; }
        public decimal ValorAplicacao               { get; set; }
        public System.DateTime DataAplicacao        { get; set; }
        public System.DateTime DataConversao        { get; set; }
        public decimal CotaAplicacao                { get; set; }
        public decimal CotaDia                      { get; set; }
        public decimal ValorBruto                   { get; set; }
        public decimal ValorLiquido                 { get; set; }
        public decimal QuantidadeInicial            { get; set; }
        public decimal Quantidade                   { get; set; }
        public decimal QuantidadeBloqueada          { get; set; }
        public System.DateTime DataUltimaCobrancaIR { get; set; }
        public decimal ValorIR                      { get; set; }
        public decimal ValorIOF                     { get; set; }
        public decimal ValorPerformance             { get; set; }
        public decimal ValorIOFVirtual              { get; set; }
        public decimal QuantidadeAntesCortes        { get; set; }
        public decimal ValorRendimento              { get; set; }
        public string PosicaoIncorporada            { get; set; }
        public string CodigoAnbima                  { get; set; }
        public System.Nullable<System.DateTime> DataUltimoCortePfee { get; set; }
        public DateTime DataProcessamento           { get; set; }

        public PosicaoFundo() { }

        public PosicaoFundo(System.Data.DataRow pRow)
        {
            this.IdPosicao              = pRow["IdPosicao"].DBToInt32();          
            this.IdOperacao             = pRow["IdOperacao"].DBToInt32();
            this.NomeFundo              = pRow["Nome"].DBToString();
            this.IdCotista              = pRow["IdCotista"].DBToInt32();
            this.IdCarteira             = pRow["IdCarteira"].DBToInt32();
            this.ValorAplicacao         = pRow["ValorAplicacao"].DBToDecimal();      
            this.DataAplicacao          = pRow["DataAplicacao"].DBToDateTime();
            this.DataConversao          = pRow["DataConversao"].DBToDateTime();
            this.CotaAplicacao          = pRow["CotaAplicacao"].DBToDecimal();
            this.CotaDia                = pRow["CotaDia"].DBToDecimal();
            this.ValorBruto             = pRow["ValorBruto"].DBToDecimal();
            this.ValorLiquido           = pRow["ValorLiquido"].DBToDecimal();
            this.QuantidadeInicial      = pRow["QuantidadeInicial"].DBToDecimal();
            this.Quantidade             = pRow["Quantidade"].DBToDecimal();
            this.QuantidadeBloqueada    = pRow["QuantidadeBloqueada"].DBToDecimal();
            this.DataUltimaCobrancaIR   = pRow["DataUltimaCobrancaIR"].DBToDateTime();
            this.ValorIR                = pRow["ValorIR"].DBToDecimal();
            this.ValorIOF               = pRow["ValorIOF"].DBToDecimal();
            this.ValorPerformance       = pRow["ValorPerformance"].DBToDecimal();
            this.ValorIOFVirtual        = pRow["ValorIOFVirtual"].DBToDecimal();
            this.QuantidadeAntesCortes  = pRow["QuantidadeAntesCortes"].DBToDecimal();
            this.ValorRendimento        = pRow["ValorRendimento"].DBToDecimal();
            this.PosicaoIncorporada     = pRow["PosicaoIncorporada"].DBToString();
            this.CodigoAnbima           = pRow["CodigoAnbid"].Equals(null) ? String.Empty : pRow["CodigoAnbid"].DBToString();
            this.DataUltimoCortePfee    = pRow["DataUltimoCortePfee"].DBToDateTime();
            this.DataProcessamento      = pRow["DataDia"].DBToDateTime();
        }
    }
}