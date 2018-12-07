using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gradual.Site.DbLib.Dados.MinhaConta.Suitability
{
    public class ClienteInfo
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string PerfilSuitability { get; set; }

        #region | Propriedades Suitability

        public Nullable<int> IdClienteSuitability { get; set; }

        public Nullable<int> IdCliente { get; set; }

        public Nullable<int> CdCblc { get; set; }

        public string ds_perfil { get; set; }

        public string ds_status { get; set; }

        public DateTime dt_realizacao { get; set; }

        public bool st_preenchidopelocliente { get; set; }

        public string ds_loginrealizado { get; set; }

        public string ds_fonte { get; set; }

        public string ds_respostas { get; set; }

        public bool MudouSuitability { get; set; }
        #endregion

        #region | ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
