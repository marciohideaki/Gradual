using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Intranet.Contratos.Dados.Portal
{
    [Serializable]
    public class SolicitacaoNovaSenhaInfo : Gradual.OMS.Library.ICodigoEntidade
    {
        public String   DsEmail                 { get; set; }
        public String   DsCpfCnpj               { get; set; }
        public String   Login                   { get; set; }
        public DateTime DtNascimentoFundacao    { get; set; }
        public String   CdSenha                 { get; set; }
        public Int32    ValidadeToken           { get; set; }
        public DateTime DataHora                { get; set; }

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }
    }
}
