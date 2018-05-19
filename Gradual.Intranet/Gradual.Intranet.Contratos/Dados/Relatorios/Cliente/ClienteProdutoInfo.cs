using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Cliente
{
    public class ClienteProdutoInfo : ICodigoEntidade
    {
        public DateTime De { get; set; }

        public DateTime Ate { get; set; }

        public Nullable<int> CodigoAssessor { get; set; }

        public int StClienteCompleto { get; set; }

        public string NomeCliente { get; set; }

        public Nullable<int> IdProdutoPlano { get; set; }

        public char StSituacao { get; set; }

        public string DsCpfCnpj { get; set; }

        public string NomeProduto { get; set; }

        public DateTime DtOperacao { get; set; }

        public Nullable<int> CdCblc { get; set; }

        public Nullable<DateTime> DtAdesao { get; set; }

        public Nullable<DateTime> DtFimAdesao { get; set; }

        public string DsEmail { get; set; }

        public int CdAssessor { get; set; }

        public Nullable<DateTime> DtUltima_cobranca { get; set; }

        public decimal VlCobrado { get; set; }

        public string Origem { get; set; }

        public string UsuarioLogado { get; set; }

        public int CodigoUsuarioLogado { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
