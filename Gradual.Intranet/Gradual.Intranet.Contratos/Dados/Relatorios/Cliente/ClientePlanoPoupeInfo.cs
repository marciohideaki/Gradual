using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Cliente
{
    public class ClientePlanoPoupeInfo : ICodigoEntidade
    {
        public int? ConsultaCdCliente { get; set; }

        public int? ConsultaIdProduto { get; set; }

        public DateTime ConsultaDtInicio { get; set; }

        public DateTime ConsultaDtFim { get; set; }

        public int CdCliente { get; set; }

        public string DsCpfCnpj { get; set; }

        public string DsNome { get; set; }

        public int IdProduto { get; set; }

        public string DsProduto { get; set; }

        public string DsAtivo { get; set; }

        public DateTime DtVencimento { get; set;  }

        public DateTime DtRetroTrocaAtivo { get; set; }

        public DateTime DtAdesao { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
