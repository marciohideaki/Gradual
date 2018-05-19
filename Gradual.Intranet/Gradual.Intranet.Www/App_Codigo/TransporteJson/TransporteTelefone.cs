using System;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteTelefone
    {
        #region Propriedades

        public Nullable<Int32> Id { get; set; }

        public Int32 ParentId { get; set; }

        public string Tipo { get; set; }

        public string DDD { get; set; }

        public string Numero { get; set; }

        public string Ramal { get; set; }

        public bool Principal { get; set; }

        public string TipoDeItem { get { return "Telefone"; } }

        public bool Exclusao { get; set; }
        #endregion

        #region Construtor

        public TransporteTelefone() { }

        public TransporteTelefone(ClienteTelefoneInfo pInfo, bool pExcluir) 
        {
            this.Id        = pInfo.IdTelefone;
            this.ParentId  = pInfo.IdCliente;
            this.Tipo      = pInfo.IdTipoTelefone.DBToString();
            this.DDD       = pInfo.DsDdd.DBToString();
            this.Numero    = (pInfo.DsNumero.Length > 4) ? pInfo.DsNumero.Insert(4, "-") : string.Empty;
            this.Ramal     = pInfo.DsRamal;
            this.Principal = pInfo.StPrincipal;
            this.Exclusao  = pExcluir;
        }

        #endregion

        #region Métodos Públicos

        public ClienteTelefoneInfo ToClienteTelefoneInfo()
        {
            ClienteTelefoneInfo lRetorno = new ClienteTelefoneInfo();

            lRetorno.IdTelefone = this.Id;
            lRetorno.IdCliente = this.ParentId;

            lRetorno.IdTipoTelefone = this.Tipo.DBToInt32();
            lRetorno.DsDdd = this.DDD;
            lRetorno.DsNumero = this.Numero.Replace("-", "");
            lRetorno.DsRamal = this.Ramal;
            lRetorno.StPrincipal = this.Principal;
            
            return lRetorno;
        }

        #endregion
    }
}
