using Gradual.OMS.Library;
using System;

namespace Gradual.Intranet.Contratos.Dados
{
    public class LancamentoTEDInfo : ICodigoEntidade
    {
        public int? CodigoCliente           { get; set; }
        public string NomeCliente           { get; set; } 
        public Nullable<DateTime> DtDe      { get; set; }
        public Nullable<DateTime> DtAte     { get; set; }
        public DateTime DataMovimento       { get; set; }
        public string   NumeroLancamento    { get; set; }
        public string   Descricao           { get; set; }
        public decimal  Valor               { get; set; }
        
        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
