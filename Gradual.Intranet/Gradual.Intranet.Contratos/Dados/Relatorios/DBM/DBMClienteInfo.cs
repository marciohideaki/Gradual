using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class DBMClienteInfo : ICodigoEntidade
    {
        public string NomeAssessor { get; set; }
        public int CodigoAssessor { get; set; }
        public string CPF_CNPJ { get; set; }
        public decimal Corretagem { get; set; }
        public decimal Volume { get; set; }
        public decimal Custodia { get; set; }
        public Int32 CodigoFilial { get; set; }

        public TipoMercado Mercado { get; set; }

        public enum TipoMercado
        {
            BMF = 0,
            BVSP = 1,
            BMF_BVSP = 2,
        }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
