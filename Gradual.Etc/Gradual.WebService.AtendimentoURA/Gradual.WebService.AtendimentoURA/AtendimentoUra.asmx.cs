using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Gradual.WebService.AtendimentoUra.Lib.Mensagem;
using Gradual.WebService.AtendimentoUra.DbLib;

namespace Gradual.WebService.AtendimentoURA
{
    /// <summary>
    /// Summary description for AtendimentoUra
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AtendimentoUra : System.Web.Services.WebService
    {
        [WebMethod]
        public LoginClienteUraResponse ConsultarLoginClienteTeste(string pCodigoIdentificador)
        {
            return this.ConsultarLoginCliente(new LoginClienteUraRequest() 
            {
                CodigoIdentificador = pCodigoIdentificador,
            });
        }

        [WebMethod]
        public LoginClienteUraResponse ConsultarLoginCliente(LoginClienteUraRequest pParametro)
        {
            var lRetorno = new LoginClienteDbLib().ConsultarCliente(pParametro);

            return lRetorno;
        }
    }
}
