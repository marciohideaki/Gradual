using Gradual.OMS.Library;
using System;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteClubesInfo : ICodigoEntidade
    {
        public int IdCliente { get; set; }

        public string NomeClube { get; set; }

        public decimal Cota { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorBruto { get; set; }

        public decimal IR { get; set; }

        public decimal IOF { get; set; }

        public decimal ValorLiquido { get; set; }

        public DateTime DataInicioPesquisa { get; set; }

        public DateTime DataFimPesquisa { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string TipoMercado { get { return "FUN"; } }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
