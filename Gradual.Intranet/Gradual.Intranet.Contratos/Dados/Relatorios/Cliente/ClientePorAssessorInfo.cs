using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Relatorios.Cliente
{
    public class ClientePorAssessorInfo : ICodigoEntidade
    {
        public int? ConsultaCdAssessor { get; set; }

        public int CdAssessor { get; set; }

        public DateTime DtNascimentoFundacao { get; set; }

        public DateTime DtPasso1 { get; set; }

        public char TpPessoa { get; set; }

        public string DsCpfCnpj { get; set; }

        public char CdSexo { get; set; }

        public int IdCliente { get; set; }

        public int StPasso { get; set; }

        public string DsNome { get; set; }

        public int CdCodigoBovespa { get; set; }

        public int CdCodigoBMF { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
