using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class EsqueciSenhaInfo : ICodigoEntidade
    {
        public string   DsEmail                 { get; set; }
        public string   DsCpfCnpj               { get; set; }
        public DateTime DtNascimentoFundacao    { get; set; }
        public string   CdSenha                 { get; set; }
        public bool     StAlteracaoFuncionario  { get; set; }

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
