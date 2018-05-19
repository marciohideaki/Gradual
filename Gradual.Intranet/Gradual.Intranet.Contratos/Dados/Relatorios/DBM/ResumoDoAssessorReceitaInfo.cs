using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.DBM
{
    public class ResumoDoAssessorReceitaInfo : ICodigoEntidade
    {
        public int? ConsultaCodigoAssessor { get; set; }

        public DateTime ConsultaDataInicial { get; set; }

        public DateTime ConsultaDataFinal { get; set; }

        public decimal QtBovespaClientes { get; set; }

        public decimal VlBovespaCorretagem { get; set; }

        public decimal QtBmfClientes { get; set; }

        public decimal VlBmfCorretagem { get; set; }

        public decimal VlTesouroCorretagem { get; set; }

        public decimal QtTesouroClientes { get; set; }

        public decimal VlOutrosCorretagem { get; set; }

        public decimal QtOutrosClientes { get; set; }

        public decimal VlBtcCorretagem { get; set; }

        public decimal QtBtcClientes { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
