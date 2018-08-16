using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSegurancaPermissaoSeguranca
    {
        #region Propriedades

        public string Nome { get; set; }

        public string DescricaoPermissao { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// (get) Descrição completa para aparecer na listagem
        /// </summary>
        public string Descricao
        {
            get
            {
                return string.Format("{0}: {1}", this.Nome, this.DescricaoPermissao);
            }
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "PermissaoSeguranca"; } }

        #endregion

        #region Construtores

        public TransporteSegurancaPermissaoSeguranca() { }

        public TransporteSegurancaPermissaoSeguranca(PermissaoInfo pPermissaoInfo)
        {
            this.Id = pPermissaoInfo.CodigoPermissao;
            this.Nome = pPermissaoInfo.NomePermissao;
            this.DescricaoPermissao = pPermissaoInfo.DescricaoPermissao;
        }

        #endregion

        public PermissaoInfo ToPermissaoInfo()
        {
            PermissaoInfo lRetorno = new PermissaoInfo();

            lRetorno.DescricaoPermissao = this.DescricaoPermissao;
            lRetorno.NomePermissao = this.Nome;
            lRetorno.CodigoPermissao = this.Id;
            
            return lRetorno;
        }
    }
}
