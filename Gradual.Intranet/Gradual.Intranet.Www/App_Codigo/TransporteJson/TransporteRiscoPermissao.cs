using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Risco.Regra.Lib.Dados;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteRiscoPermissao
    {

        public TransporteRiscoPermissao() { }

        public TransporteRiscoPermissao(PermissaoRiscoInfo pPermissaoRiscoInfo)
        {
            this.Id = pPermissaoRiscoInfo.CodigoPermissao.ToString();
            this.Descricao = pPermissaoRiscoInfo.NomePermissao;
            this.Bolsa = (int)pPermissaoRiscoInfo.Bolsa;
        }

        #region Propriedades

        public string Id { get; set; }

        public string Descricao { get; set; }

        public int Bolsa { get; set; }


        public PermissaoRiscoInfo ToPermissaoRiscoInfo()
        {
            PermissaoRiscoInfo lRetorno = new PermissaoRiscoInfo();

            lRetorno.NomePermissao = this.Descricao;
            int id = 0;
            if (int.TryParse(this.Id, out id))
                lRetorno.CodigoPermissao = id;
            lRetorno.Bolsa = (BolsaInfo)Bolsa;
            return lRetorno;
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Permissao"; } }

        #endregion
    }

}