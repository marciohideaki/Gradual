using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.Spider.PostTradingClientEngine.Objects
{
    public class VendaDescoberta
    {
        public System.Int32     Account             { get; set; }
        public System.String    Ativo               { get; set; }
        public System.String    SegmentoMercado     { get; set; }
        public System.String    Variacao            { get; set; }
        public System.String    UltimoPreco         { get; set; }
        public System.String    PrecoFechamento     { get; set; }
        public System.String    QtdAbertura         { get; set; }
        public System.String    QtdD1               { get; set; }
        public System.String    QtdD2               { get; set; }
        public System.String    QtdD3               { get; set; }
        public System.String    QtdExecC            { get; set; }
        public System.String    QtdExecV            { get; set; }
        public System.Int32     NetExec             { get; set; }
        public System.String    QtdAbC              { get; set; }
        public System.String    QtdAbV              { get; set; }
        public System.String    NetAb               { get; set; }
        public System.String    PcMedC              { get; set; }
        public System.String    PcMedV              { get; set; }
        public System.String    FinancNet           { get; set; }
        public System.Decimal   LucroPrej           { get; set; }
        public System.String    DtPosicao           { get; set; }
        public System.String    DtMovimento         { get; set; }
        public System.String    Bolsa               { get; set; }
        public System.String    TipoMercado         { get; set; }
        public System.String    DtVencimento        { get; set; }
        public System.String    ExecBroker          { get; set; }
        public System.Int32     QtdTotal            { get; set; }
        public System.Int32     QtdDisponivel       { get; set; }
        public System.String    VolCompra           { get; set; }
        public System.String    VolVenda            { get; set; }
        public System.String    VolTotal            { get; set; }
    }
}