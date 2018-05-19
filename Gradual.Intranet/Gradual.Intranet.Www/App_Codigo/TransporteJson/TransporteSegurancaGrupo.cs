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
    public class TransporteSegurancaGrupo
    {

        public TransporteSegurancaGrupo() { }

        public TransporteSegurancaGrupo(UsuarioGrupoInfo pUsuarioGrupoInfo)
        {
            this.Id = pUsuarioGrupoInfo.CodigoUsuarioGrupo;
            this.Nome = pUsuarioGrupoInfo.NomeUsuarioGrupo;
        }

        #region Propriedades

        public string Id { get; set; }

        public string Nome { get; set; }
        
        /// <summary>
        /// (get) Descrição completa para aparecer na listagem
        /// </summary>
        public string Descricao
        {
            get
            {
                return string.Format("{0}", this.Nome);
            }
        }

        public UsuarioGrupoInfo ToUsuarioGrupoInfo()
        {
            UsuarioGrupoInfo lRetorno = new UsuarioGrupoInfo();

            lRetorno.NomeUsuarioGrupo = this.Nome;
            lRetorno.CodigoUsuarioGrupo = this.Id;

            return lRetorno;
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Grupo"; } }

        #endregion
    }
}
