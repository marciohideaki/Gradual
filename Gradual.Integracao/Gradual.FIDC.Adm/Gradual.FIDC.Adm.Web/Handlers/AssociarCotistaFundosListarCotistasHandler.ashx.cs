using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradual.FIDC.Adm.Web.Handlers
{
    public class AssociarCotistaFundosListarCotistasHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string lNomeParcialFundo = "";

            if (context.Request["NomeCotista"] != null)
            {
                lNomeParcialFundo = context.Request["NomeCotista"].ToString().ToUpper();
            }

            try
            {
                var lServico = new DbLib.Persistencia.CotistasFidcDb();

                var lResponse = lServico.SelecionarLista(new DbLib.Mensagem.CadastroCotistasFidcRequest());

                lResponse.ListaCotistaFidc = lResponse.ListaCotistaFidc.OrderBy(p => p.NomeCotista).ToList();

                if (lResponse.StatusResposta.Equals(OMS.Library.MensagemResponseStatusEnum.OK))
                {
                    var lListaFundosFiltrada = lResponse.ListaCotistaFidc.Where(f => f.NomeCotista.ToUpper().Contains(lNomeParcialFundo.ToUpper()));

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