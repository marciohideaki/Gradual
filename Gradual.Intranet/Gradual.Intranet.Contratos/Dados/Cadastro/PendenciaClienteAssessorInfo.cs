using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class PendenciaClienteAssessorInfo : ICodigoEntidade
    {
        public string NomeCliente { get; set; }
        public string NomeAssessor { get; set; }
        public string TpPendenciaDescricao { get; set; }
        public string DescricaoPendencia { get; set; }
        public int IdAssessor { get; set; }
        public int? CdBmfBovespa { get; set; }
        public string EmailAssessor { get; set; }
        public string EmailCliente { get; set; }
        public string CpfCnpjCliente { get; set; }
        public int? IdCliente { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
