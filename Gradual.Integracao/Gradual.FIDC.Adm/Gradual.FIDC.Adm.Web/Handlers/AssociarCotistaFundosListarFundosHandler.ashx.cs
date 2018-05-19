using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.Handlers
{
    public class AssociarCotistaFundosListarFundosHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string lNomeParcialFundo = "";

            if (context.Request["NomeFundo"] != null)
            {
                lNomeParcialFundo = context.Request["NomeFundo"].ToString().ToUpper();
            }

            try
            {
                var lServico = new DbLib.Persistencia.CadastroFundoDB();

                var lResponse = lServico.Buscar(new DbLib.Mensagem.CadastroFundoRequest());

                lResponse.ListaFundos = lResponse.ListaFundos.OrderBy(p => p.NomeFundo).ToList();

                if (lResponse.StatusResposta.Equals(OMS.Library.MensagemResponseStatusEnum.OK))
                {
                    var lListaFundosFiltrada = lResponse.ListaFundos.Where(f => f.NomeFundo.ToUpper().Contains(lNomeParcialFundo.ToUpper()));

                    context.Response.ContentType = "application/json";
                    context.Response.Write(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lListaFundosFiltrada));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}