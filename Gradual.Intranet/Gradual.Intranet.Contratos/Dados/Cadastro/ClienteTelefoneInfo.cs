using System;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Contratos.Dados
{
    public class ClienteTelefoneInfo : ICodigoEntidade
    {
        #region | Propriedades

        public Nullable<int> IdTelefone { get; set; }
        public Int32 IdCliente { get; set; }
        public Int32 IdTipoTelefone { get; set; }
        public Boolean StPrincipal { get; set; }
        public string DsDdd { get; set; }
        public string DsRamal { get; set; }
        public string DsNumero { get; set; }

        #endregion

        #region Construtor

        public ClienteTelefoneInfo() { }
        
        public ClienteTelefoneInfo(string pIdCliente)
        {
            this.IdCliente = int.Parse(pIdCliente);
        }

        #endregion

        #region | ICodigoEntidade Members

        public string ReceberCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
