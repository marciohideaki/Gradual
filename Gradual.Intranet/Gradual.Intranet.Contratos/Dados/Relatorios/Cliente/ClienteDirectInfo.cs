using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteDirectInfo : ICodigoEntidade
    {
        public DateTime De { get; set; }

        public DateTime Ate { get; set; }

        public int StClienteCompleto { get; set; }

        public string NomeCliente { get; set; }

        public Nullable<int> IdProdutoPlano { get; set; }

        public char StSituacao { get; set; }

        public string DsCpfCnpj { get; set; }

        public string NomeProduto { get; set; }

        public DateTime DtOperacao { get; set; }

        public Nullable<int> CdCblc { get; set; }

        public Nullable<DateTime> DtAdesao { get; set; }

        public string DsEmail { get; set; }

        public int CdAssessor { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}


