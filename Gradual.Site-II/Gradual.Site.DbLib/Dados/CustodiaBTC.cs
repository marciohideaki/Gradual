using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gradual.Site.DbLib.Dados
{
    public class CustodiaBTC
    {
        public String   NumeroContrato          { get; set; }
        public Int32    CodigoCliente           { get; set; }
        public String   NomeCliente             { get; set; }
        public Int32    CodigoAssessor          { get; set; }
        public String   NomeAssessor            { get; set; }
        public String   TipoContrato            { get; set; }
        public String   CodigoNegocio           { get; set; }
        public Int32    Quantidade              { get; set; }
        public DateTime Abertura                { get; set; }
        public DateTime Vencimento              { get; set; }
        public Int32    NumeroDias              { get; set; }
        public decimal  Cotacao                 { get; set; }
        public decimal  Remuneracao             { get; set; }
        public decimal  Comissao                { get; set; }
        public decimal  TaxaFinal               { get; set; }
        public Int32    Contraparte             { get; set; }
        public decimal  Financeiro              { get; set; }
        public decimal  ValorComissaoAno        { get; set; }
        public decimal  ValorComissaoDia        { get; set; }
        public decimal  ValorComissaoMes        { get; set; }
        public decimal  ValorComissaoALiquidar  { get; set; }
        public decimal  Custo                   { get; set; }

        public CustodiaBTC() { }
        
        public CustodiaBTC(DataRow lRow)
        {
            NumeroContrato          = lRow["NumeroContrato"].ToString();
            CodigoCliente           = lRow["CodigoCliente"].DBToInt32();
            NomeCliente             = lRow["NomeCliente"].ToString();
            CodigoAssessor          = lRow["CodigoAssessor"].DBToInt32();
            NomeAssessor            = lRow["NomeAssessor"].ToString();
            TipoContrato            = lRow["TipoContrato"].ToString();
            CodigoNegocio           = lRow["CodigoNegocio"].ToString();
            Quantidade              = lRow["Quantidade"].DBToInt32();
            Abertura                = lRow["Abertura"].DBToDateTime();
            Vencimento              = lRow["Vencimento"].DBToDateTime();
            NumeroDias              = lRow["NumeroDias"].DBToInt32();
            Cotacao                 = lRow["Cotacao"].DBToDecimal();
            Remuneracao             = lRow["Remuneracao"].DBToDecimal();
            Comissao                = lRow["Comissao"].DBToDecimal();
            TaxaFinal               = lRow["TaxaFinal"].DBToDecimal();
            Contraparte             = lRow["Contraparte"].DBToInt32();
            Financeiro              = TipoContrato.Equals("T") ? lRow["Financeiro"].DBToDecimal()*(-1) : lRow["Financeiro"].DBToDecimal();
            ValorComissaoAno        = lRow["ValorComissaoAno"].DBToDecimal();
            ValorComissaoDia        = lRow["ValorComissaoDia"].DBToDecimal();
            ValorComissaoMes        = lRow["ValorComissaoMes"].DBToDecimal();
            ValorComissaoALiquidar  = lRow["ValorComissaoALiquidar"].DBToDecimal();
            Custo                   = lRow["Custo"].DBToDecimal();
        }

    }
}
