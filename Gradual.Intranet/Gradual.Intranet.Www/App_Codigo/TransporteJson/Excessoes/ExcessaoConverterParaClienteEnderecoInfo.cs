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

namespace Gradual.Intranet.Www.App_Codigo.Excessoes
{
    public class ExcessaoConverterParaClienteEnderecoInfo : System.Exception
    {
        private const string MENSAGEM = "Erro durante o envio do request para salvar o ClienteEnderecoInfo";
        public ExcessaoConverterParaClienteEnderecoInfo() : base(MENSAGEM) { }
        public ExcessaoConverterParaClienteEnderecoInfo(Exception ex):base(MENSAGEM, ex){}
    }
}
