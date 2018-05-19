using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradual.Spider.PostTradingClientEngine.Objects
{
    public class Ordem
    {
        public System.Int32     Cliente             { get; set; }
        public System.Int32     CodigoOrdem         { get; set; }
        public System.String    Exchange            { get; set; }
        public System.String    Ativo               { get; set; }
        public System.String    Sentido             { get; set; }
        public System.String    Status              { get; set; }
        public System.Int32     QuantidadeOrdem     { get; set; }
        public System.Int32     QuantidadeExecutada { get; set; }
        public System.Int32     QuantidadeAberta    { get; set; }
        public System.Decimal   Preco               { get; set; }
        public System.String    Horario             { get; set; }
        public System.Decimal   PrecoStop           { get; set; }
        public System.String    TipoOrdem           { get; set; }
        public System.String    DataValidade        { get; set; }
        public System.String    Plataforma          { get; set; }
        public System.String    ExecBroker          { get; set; }
    }
}
