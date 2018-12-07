using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;

namespace Gradual.Site.Www
{
    public class TransporteCadastroTelefone
    {
        public int IdCliente { get; set; }
        public int? IdTelefone { get; set; }

        public string DDD       { get; set; }
        public string Numero    { get; set; }
        public string Ramal     { get; set; }
        public int IdTipo       { get; set; }
        public string TipoDesc  { get; set; }
        public bool Principal   { get; set; }

        public string PrincipalDesc { get; set; }

        public TransporteCadastroTelefone() { }

        public TransporteCadastroTelefone(ClienteTelefoneInfo pTelefoneBase)
        {
            if(pTelefoneBase.IdTelefone.HasValue)
                this.IdTelefone = pTelefoneBase.IdTelefone.Value;

            this.IdCliente = pTelefoneBase.IdCliente;

            this.DDD    = pTelefoneBase.DsDdd;
            this.Numero = pTelefoneBase.DsNumero;
            this.Ramal  = pTelefoneBase.DsRamal;
            this.IdTipo = pTelefoneBase.IdTipoTelefone;

            if (this.Numero.Length == 10 && !this.Numero.Contains("-"))
            {
                this.Numero = this.Numero.Substring(0, 9);
            }

            if (this.Numero.Length == 9)
            {
                this.Numero = this.Numero.Insert(5, "-");
            }
            
            if (this.Numero.Length == 8)
            {
                this.Numero = this.Numero.Insert(4, "-");
            }

            this.Principal = pTelefoneBase.StPrincipal;

            this.PrincipalDesc = this.Principal ? "Sim" : "Não";

            switch (pTelefoneBase.IdTipoTelefone)
            {
                case 2:
                    this.TipoDesc = "Comercial";
                    break;
                case 1:
                    this.TipoDesc = "Residencial";
                    break;
                case 3:
                    this.TipoDesc = "Celular";
                    break;
                case 4:
                    this.TipoDesc = "Fax";
                    break;
                default:
                    this.TipoDesc = pTelefoneBase.IdTipoTelefone.ToString();
                    break;
            }
        }

        public ClienteTelefoneInfo ToClienteTelefoneInfo()
        {
            ClienteTelefoneInfo lRetorno = new ClienteTelefoneInfo();

            if (this.Numero.Length == 10 && !this.Numero.Contains("-"))
            {
                this.Numero = this.Numero.Substring(0, 9);
            }

            lRetorno.IdCliente = this.IdCliente;
            lRetorno.IdTelefone = this.IdTelefone;
            lRetorno.DsDdd = this.DDD;
            lRetorno.DsNumero = this.Numero.Replace("-", "");

            if(!string.IsNullOrEmpty(this.Ramal))
                lRetorno.DsRamal = this.Ramal.ToUpper();

            lRetorno.IdTipoTelefone = this.IdTipo;

            if (this.IdTipo == 3)
            {
                lRetorno.StPrincipal = true;    //padrão o celular é principal pra todos
            }

            return lRetorno;
        }
    }
}