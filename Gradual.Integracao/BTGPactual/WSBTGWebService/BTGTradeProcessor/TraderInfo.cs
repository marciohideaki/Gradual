using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BTGTradeProcessor
{
    public class TraderInfo
    {
        public string CpfCnpj { get; set; }
        public int CdCorretoraBmfDestino { get; set; }
        public string  NomeCorretoraBmfDestino { get; set; }
        public string ContaCliente { get; set; }
        public string NomeEmitente { get; set; }
    }
}
