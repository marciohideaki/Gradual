using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Site.Www
{
    public class TransporteTelefone
    {
        #region Propriedades

        public string DDD       { get; set; }
        public string Numero    { get; set; }
        public string Ramal     { get; set; }
        public int IdTipo       { get; set; }
        public string TipoDesc  { get; set; }
        public bool Principal   { get; set; }

        public string PrincipalDesc { get; set; }

        #endregion

        #region Construtores

        public TransporteTelefone(){}

        public TransporteTelefone(ClienteTelefoneInfo pTelefoneBase)
        {
            this.DDD    = pTelefoneBase.DsDdd;
            this.Numero = pTelefoneBase.DsNumero;
            this.Ramal  = pTelefoneBase.DsRamal;
            this.IdTipo = pTelefoneBase.IdTipoTelefone;


            //TODO: isso é uma patch pro telefone aparecer na textbox porque a máscara js sobrescreve o valor, mas está vindo formatado e nao pode, tem que vir sem format do banco
            if (this.Numero[4] == '-')
            {
                this.Numero = "0" + this.Numero;
            }

            this.Principal = pTelefoneBase.StPrincipal;

            this.PrincipalDesc = this.Principal ? "Sim" : "Não";

            switch (pTelefoneBase.IdTipoTelefone)
            {
                case 2:
                    this.TipoDesc = "Comercial";
                    break;
                case 1:
                    this.TipoDesc = "Residêncial";
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

        #endregion

        #region Métodos Públicos

        public override string ToString()
        {
            return string.Format("({0}) {1}{2}", this.DDD, this.Numero, string.IsNullOrEmpty(this.Ramal) ? "" : (" r." + this.Ramal));
        }

        #endregion
    }
}