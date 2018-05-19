using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Newtonsoft.Json;


namespace Gradual.Intranet.Www.Intranet.Seguranca.Formularios.Dados
{
    [ValidarSegurancaAttribute("9C5DA26B-8C30-4c1d-AA7A-B7A22CF2CA8F", "1", "1")]
    public partial class Desbloqueio : PaginaBaseAutenticada
    {
        private int GetCodigoCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(base.Request.Form["CodigoCliente"], out lRetorno);

                return lRetorno;
            }
        }


        public string ResponderSalvar()
        {
            string lretorno = string.Empty;

            try
            {
                AlterarPermissaoAcessoRequest lRequest                = new AlterarPermissaoAcessoRequest();
                
                lRequest.PermissaoAcessoUsuarioInfo.IdUsuario         = this.GetCodigoCliente;
                
                lRequest.PermissaoAcessoUsuarioInfo.UsuarioAcessoAcao = UsuarioAcessoEnum.Desbloqueio;
                
                AlterarPermissaoAcessoResponse lResponse              = ServicoSeguranca.AlterarPermissaoAcesso(lRequest);
             
                if (lResponse.Resposta == MensagemResponseStatusEnum.OK)
                {
                    base.RegistrarLogInclusao();

                    lretorno = RetornarSucessoAjax("Desbloqueio efetuado com sucesso!!!");
                }
                else
                {
                    lretorno = RetornarErroAjax(lResponse.Resposta.ToString());
                }
            }
            catch (Exception ex)
            {
                lretorno = RetornarErroAjax(ex.Message);
            }

            return lretorno;
        }

        public string ResponderCarregarHtmlComDados()
        {
            return string.Empty;
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            RegistrarRespostasAjax(new string[] { 
                                                    "Salvar",
                                                    "CarregarHtmlComDados"
                                                },
                new ResponderAcaoAjaxDelegate[] { 
                                                    ResponderSalvar,
                                                    ResponderCarregarHtmlComDados

                                                   });

        }
    }
}