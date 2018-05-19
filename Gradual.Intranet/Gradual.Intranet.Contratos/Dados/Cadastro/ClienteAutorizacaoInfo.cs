using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados.Cadastro
{
    public class ClienteAutorizacaoInfo : ICodigoEntidade
    {
        #region Propriedades

        public Nullable<int> IdAutorizacao { get; set; }

        public int IdCliente { get; set; }
        public string CodigoBov { get; set; }

        public string NomeCliente { get; set; }
        public DateTime DataExportacao { get; set; }
        public string CPF { get; set; }
        public string Passo { get; set; }

        public Nullable<int>      IdLogin_D1 { get; set; }
        public Nullable<DateTime> DataAutorizacao_D1 { get; set; }
        public string Nome_D1 { get; set; }
        public string Email_D1 { get; set; }

        public Nullable<int>      IdLogin_D2 { get; set; }
        public Nullable<DateTime> DataAutorizacao_D2 { get; set; }
        public string Nome_D2 { get; set; }
        public string Email_D2 { get; set; }

        public Nullable<int>      IdLogin_P1 { get; set; }
        public Nullable<DateTime> DataAutorizacao_P1 { get; set; }
        public string Nome_P1 { get; set; }
        public string Email_P1 { get; set; }

        public Nullable<int>      IdLogin_P2 { get; set; }
        public Nullable<DateTime> DataAutorizacao_P2 { get; set; }
        public string Nome_P2 { get; set; }
        public string Email_P2 { get; set; }

        public Nullable<int>      IdLogin_T1 { get; set; }
        public Nullable<DateTime> DataAutorizacao_T1 { get; set; }
        public string Nome_T1 { get; set; }
        public string Email_T1 { get; set; }

        public Nullable<int>      IdLogin_T2 { get; set; }
        public Nullable<DateTime> DataAutorizacao_T2 { get; set; }
        public string Nome_T2 { get; set; }
        public string Email_T2 { get; set; }

        public string StAutorizado { get; set; }

        #endregion

        #region ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
