using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www
{
    /// <summary>
    /// Summary description for RetornarArquivo
    /// </summary>
    public class RetornarArquivo : IHttpHandler
    {


        private IServicoPersistenciaCadastro RetornarServicoPersistencia(HttpContext context)
        {
            if (context.Application["ServicoPersistencia"] == null)
                context.Application["ServicoPersistencia"] = BuscarServicoDoAtivador<IServicoPersistenciaCadastro>(context);

            return (IServicoPersistenciaCadastro)context.Application["ServicoPersistencia"];
        }

        private T BuscarServicoDoAtivador<T>(HttpContext context)
        {
            if (context.Application["ServicosCarregados"] == null)
            {
                if (!ServicoHostColecao.Default.Servicos.ContainsKey(string.Format("{0}-", typeof(T))))
                    ServicoHostColecao.Default.CarregarConfig(ConfiguracoesValidadas.TipoDeObjetoAtivador);

                context.Application["ServicosCarregados"] = true;
            }
            return Ativador.Get<T>();
        }

        public void ProcessRequest(HttpContext context)
        {
            int id = int.Parse(context.Request.QueryString["ID"]);

            ReceberEntidadeCadastroRequest<ArquivoContratoInfo> lRequest = new ReceberEntidadeCadastroRequest<ArquivoContratoInfo>()
            {
                EntidadeCadastro = new ArquivoContratoInfo()
                {
                    IdArquivoContrato = id
                }
            };

            ReceberEntidadeCadastroResponse<ArquivoContratoInfo> lResponse = new ReceberEntidadeCadastroResponse<ArquivoContratoInfo>();
            IServicoPersistenciaCadastro iServicoPersistencia = RetornarServicoPersistencia(context);
            lResponse = iServicoPersistencia.ReceberEntidadeCadastro<ArquivoContratoInfo>(lRequest);
            context.Response.ContentType = lResponse.EntidadeCadastro.MIMEType;
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + lResponse.EntidadeCadastro.Nome);
            context.Response.AddHeader("Content-Length", lResponse.EntidadeCadastro.Tamanho.ToString());
            //context.Response.OutputStream.
            context.Response.OutputStream.Write(lResponse.EntidadeCadastro.Arquivo, 0, lResponse.EntidadeCadastro.Tamanho);
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