using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Comum.Mensagens;

namespace Gradual.OMS.Library.Web
{
    public class PaginaBase : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            // Realiza cadeia do load
            base.OnLoad(e);

            // Verifica se tem acao
            string acao = Request["acao"];
            if (acao != null)
            {
                // Pega o tipo da mensagem
                Type tipoMensagem = ResolutorTipos.Resolver(Request["TipoMensagem"]);

                // Desserializa a mensagem json para o tipo correto
                MensagemRequestBase mensagemRequest = null;

                // Apenas se descobriu o tipo da mensagem
                if (tipoMensagem != null && Request["Mensagem"] != null)
                {
                    // Desserializa a mensagem
                    mensagemRequest =
                        (MensagemRequestBase)
                            JsonConvert.DeserializeObject(Request["Mensagem"], tipoMensagem);

                    // Se tem sessao, coloca
                    if (Session["CodigoSessao"] != null)
                        mensagemRequest.CodigoSessao = (string)Session["CodigoSessao"];
                }

                // Tem objeto para extrair?
                //object objeto = null;
                //string jsonObjeto = Request["mensagem"];
                //string tipoObjeto = Request["tipoMensagem"];
                //if (tipoObjeto != null && jsonObjeto != null)
                //    objeto = JsonConvert.DeserializeObject(jsonObjeto, Type.GetType(tipoObjeto));

                // Pede execução da ação
                object retorno = OnExecutarAcao(acao, mensagemRequest);

                // Serializa a resposta
                Response.Clear();
                Response.Write(JsonConvert.SerializeObject(retorno));
                Response.End();
            }
        }

        protected virtual object OnExecutarAcao(string acao, object parametros)
        {
            return null;
        }
    }
}
