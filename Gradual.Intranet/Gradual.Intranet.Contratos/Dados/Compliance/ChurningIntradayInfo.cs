using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Compliance
{
    public enum enumPercentualTR
    {
        TODOS = 0,
        ABAIXO_2 = 1,
        ENTRE_2_E_8 = 2,
        ACIMA_8 = 3
    }

    public enum enumPercentualCE
    {
        TODOS = 0,
        ABAIXO_10 = 1,
        ENTRE_10_E_15 = 2,
        ENTRE_15_E_20 = 3,
        ACIMA_20 =4
    }
    public enum enumTotalCompras
    {
        TODOS              = 0,
        ABAIXO_500M        = 1,
        ENTRE_500M_E_1000M = 2,
        ACIMA_1000M        = 3
    }

    public enum enumTotalVendas
    {
        TODOS = 0,
        ABAIXO_500M = 1,
        ENTRE_500M_E_1000M = 2,
        ACIMA_1000M = 3
    }

    public enum enumCarteiraMedia
    {
        TODOS              = 0,
        ABAIXO_500M        = 1,
        ENTRE_500M_E_1000M = 2,
        ACIMA_1000M        = 3
    }
    public class ChurningIntradayInfo
    {
        public DateTime Data                               { get; set; }
        public DateTime DataDe                             { get; set; }
        public DateTime DataAte                            { get; set; }
        public List<DateTime> ListaFeriados                { get; set; }
        public Nullable<int> CodigoCliente                 { get; set; }
        public Nullable<int> CodigoAssessor                { get; set; }
        public Nullable<int> CodigoLogin                   { get; set; }
        public string NomeCliente                          { get; set; }
        public string NomeAssessor                         { get; set; }
        public decimal ValorVendas                         { get; set; }
        public decimal ValorCompras                        { get; set; }
        public decimal ValorCarteira                       { get; set; }
        public decimal ValorVendasDia                      { get; set; }
        public decimal ValorComprasDia                     { get; set; }
        public string Porta                                { get; set; }
        public string TipoPessoa                           { get; set; }
        public decimal ValorCorretagem                     { get; set; }
        public decimal ValorCorretagemDia                  { get; set; }
                                                       
        public enumPercentualCE  enumPercentualCE          { get; set; }
        public enumPercentualTR  enumPercentualTR          { get; set; }
        public enumTotalCompras  enumTotalCompras          { get; set; }
        public enumTotalVendas   enumTotalVendas           { get; set; }
        public enumCarteiraMedia enumCarteiraMedia         { get; set; }
                                                           
        public decimal PercentualTRnoDia                   { get; set; }
        public decimal PercentualTRnoPeriodo               { get; set; }
        public decimal PercentualCEnoDia                   { get; set; }
        public decimal PercentualCEnoPeriodo               { get; set; }
                                                           
        public decimal ValorCarteiraMedia                  { get; set; }
        public decimal ValorCarteiraDia                    { get; set; }

        public decimal ValorL1                             { get; set; }

        public List<ChurningIntradayInfo> Resultado = null;

        public CarteiraChurningDia CarteiraDia { get; set; }
        public ChurningIntradayInfo ()
        {
            Resultado = new List<ChurningIntradayInfo>();
        }
    }

    public class CarteiraChurningDia
    {
        public DateTime Data                { get; set; }
        public decimal ValorCarteiraDia     { get; set; }
        public int CodigoCliente            { get; set; }
        public decimal PercentualTRnoDia    { get; set; }
        public decimal PercentualCEnoDia    { get; set; }
        public decimal ValorComprasDia      { get; set; }
        public decimal ValorVendasDia       { get; set; }
        public decimal ValorCorretagemDia   { get; set; }
        public CarteiraChurningDia()
        {

        }
    }
}
