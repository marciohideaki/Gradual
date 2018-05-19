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
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.App_Codigo.TransporteJson
{
    public class TransporteSegurancaUsuario
    {
        #region Propriedades

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }

        public string Senha { get; set; }

        public string CodAssessor { get; set; }

        public string CodAssessorAssociado { get; set; }

        public string TipoAcesso { get; set; }

        /// <summary>
        /// (get) Descrição completa para aparecer na listagem
        /// </summary>
        public string Descricao
        {
            get
            {
                return string.Format("Nome: {0}, Email: {1}", this.Nome, this.Email);
            }
        }

        /// <summary>
        /// (get) Tipo de objeto, para o javascript
        /// </summary>
        public string TipoDeObjeto { get { return "Usuario"; } }

        #endregion

        #region Construtores

        public TransporteSegurancaUsuario() { }

        public TransporteSegurancaUsuario(UsuarioInfo pUsuarioInfo)
        {
            this.Id = pUsuarioInfo.CodigoUsuario;
            this.Nome = pUsuarioInfo.Nome;
            this.Email = pUsuarioInfo.Email;
            this.Senha = pUsuarioInfo.Senha;
            this.TipoAcesso = pUsuarioInfo.CodigoTipoAcesso.ToString();

            if ((eTipoAcesso)pUsuarioInfo.CodigoTipoAcesso == eTipoAcesso.Assessor)
            {
                this.CodAssessor = pUsuarioInfo.CodigoAssessor.ToString();
                this.CodAssessorAssociado = pUsuarioInfo.CodigosFilhoAssessor;
            }
        }

        #endregion

        public UsuarioInfo ToUsuarioInfo()
        {
            UsuarioInfo lRetorno = new UsuarioInfo();

            lRetorno.CodigosFilhoAssessor = this.CodAssessorAssociado;
            lRetorno.Nome = this.Nome;
            lRetorno.Email = this.Email;
            lRetorno.CodigoUsuario = this.Id;
            lRetorno.Senha = Criptografia.CalculateMD5Hash(this.Senha);
            int lCodAssessor = -1;
            if(int.TryParse(this.CodAssessor, out lCodAssessor))
                lRetorno.CodigoAssessor = lCodAssessor;
            int lTipoAcesso = -1;
            if(int.TryParse(this.TipoAcesso, out lTipoAcesso))
                lRetorno.CodigoTipoAcesso = lTipoAcesso;
            return lRetorno;
        }
    }
}
