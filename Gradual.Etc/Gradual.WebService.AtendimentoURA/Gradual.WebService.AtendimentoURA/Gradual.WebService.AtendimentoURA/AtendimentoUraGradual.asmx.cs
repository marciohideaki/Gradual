using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Gradual.WebService.AtendimentoUra.Lib.Mensagem;
using Gradual.WebService.AtendimentoUra.DbLib;
using Gradual.WebService.AtendimentoUra.Lib;

namespace Gradual.WebService.AtendimentoURA
{
    /// <summary>
    /// Summary description for AtendimentoUra
    /// </summary>
    [WebService(Namespace = "http://ura.gradualinvestimentos.com.br/", Description = "WebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AtendimentoUra : System.Web.Services.WebService
    {
        [WebMethod]
        public LoginAssessorUraResponse ConsultarAssessorViaString(string pSenhaAssessor)
        {
            var lRetorno = this.ConsultarAssessor(new LoginAssessorUraRequest() { ObjetoDeConsulta = new AtendimentoUraAssessorInfo() { Senha = pSenhaAssessor } });

            return lRetorno;
        }

        [WebMethod]
        public LoginAssessorUraResponse ConsultarAssessor(LoginAssessorUraRequest pParametro)
        {
            var lRetorno = new LoginAssessorDbLib().ConsultarAssessor(pParametro);

            return lRetorno;
        }

        [WebMethod]
        public LoginClienteUraResponse ConsultarLoginClienteViaString(string pCodigoIdentificador)
        {
            return this.ConsultarLoginCliente(new LoginClienteUraRequest()
            {
                ObjetoDeConsulta = new AtendimentoUraClienteInfo() { CodigoIdentificador = pCodigoIdentificador },
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
