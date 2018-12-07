using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados
{
    public class CustodiaTermo
    {
        public Int32    CodigoCliente           { get; set; }
        public String   NomeCliente             { get; set; }
        public String   CodigoNegocio           { get; set; }
        public Int32    QuantidadeDisponivel    { get; set; }
        public Int32    QuantidadeOriginal      { get; set; }
        public decimal  PrecoTermo              { get; set; }
        public decimal  PrecoMadioD1            { get; set; }
        public decimal  FinanceiroATermo        { get; set; }
        public decimal  FinanceiroD1            { get; set; }
        public decimal  CustoTermo              { get; set; }
        public decimal  ResultadoTermoD1        { get; set; }
        public DateTime DataAbertura            { get; set; }
        public DateTime DataVencimento          { get; set; }
        public DateTime DataRolagem             { get; set; }
        public decimal  Financeiro              { get; set; }
        public decimal  ResultadoTermo          { get; set; }

        public CustodiaTermo() { }

        public CustodiaTermo(System.Data.DataRow pRow)
        {
            CodigoCliente           = pRow["CodigoCliente"].DBToInt32();
            NomeCliente             = pRow["NomeCliente"].ToString();
            CodigoNegocio           = pRow["CodigoNegocio"].ToString();
            QuantidadeDisponivel    = pRow["QuantidadeDisponivel"].DBToInt32();
            QuantidadeOriginal      = pRow["QuantidadeOriginal"].DBToInt32();
            PrecoTermo              = pRow["PrecoTermo"].DBToDecimal();
            PrecoMadioD1            = pRow["PrecoMadioD1"].DBToDecimal();
            FinanceiroATermo        = pRow["FinanceiroATermo"].DBToDecimal();
            FinanceiroD1            = pRow["FinanceiroD1"].DBToDecimal();
            CustoTermo              = pRow["CustoTermo"].DBToDecimal();
            ResultadoTermoD1        = pRow["ResultadoTermoD1"].DBToDecimal();
            DataAbertura            = pRow["DataAbertura"].DBToDateTime();
            DataVencimento          = pRow["DataVencimento"].DBToDateTime();
            DataRolagem             = CalcularDataUtil(DataVencimento, -3);
            Financeiro              = pRow["Financeiro"].DBToDecimal();
            ResultadoTermo          = pRow["ResultadoTermo"].DBToDecimal();

        }

        public void CustodiaTermoALiquidar(System.Data.DataRow pRow)
        {
            this.CodigoCliente         = pRow["CodigoCliente"].DBToInt32();
            this.CodigoNegocio         = pRow["CodigoNegocio"].ToString();
            this.QuantidadeDisponivel  = pRow["QuantidadeALiquidar"].DBToInt32();
        }

        private DateTime CalcularDataUtil(DateTime pData, Int32 pDias)
        {
            pData = pData.AddDays(pDias);

            switch (pData.DayOfWeek)
            { 
                case DayOfWeek.Saturday:
                    if (pDias > 0)
                    {
                        pData = pData.AddDays(2);
                    }
                    else
                    {
                        pData = pData.AddDays(-1);
                    }
                    break;

                case DayOfWeek.Sunday:
                    if (pDias > 0)
                    {
                        pData = pData.AddDays(1);
                    }
                    else
                    {
                        pData = pData.AddDays(-2);
                    }

                    break;
            }

            return pData;
        }
    }
}
