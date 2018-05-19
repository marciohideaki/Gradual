using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Sistema;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Sistema;

namespace Gradual.Intranet.Www.Intranet.Sistema.Relatorios
{
    public partial class R001 : PaginaBaseAutenticada
    {
        #region | Propriedades

        private OpcoesBuscarPor GetBuscarPor
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["TipoBusca"], out lRetorno);

                return (OpcoesBuscarPor)lRetorno;
            }
        }

        private string GetClienteParametro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["ParametroBusca"]))
                    return null;

                return this.Request.Form["ParametroBusca"].Trim();
            }
        }

        private string GetEmailUsuario
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["EmailUsuario"]))
                    return null;

                return this.Request.Form["EmailUsuario"];
            }
        }

        private int? GetTipoAcao
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["TipoAcao"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetTela
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Tela"]))
                    return null;

                return this.Request.Form["Tela"];
            }
        }

        private DateTime GetDataDe
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicio"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataAte
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFim"], out lRetorno);

                return lRetorno.AddDays(1);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (base.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
        }

        #endregion

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SistemaControleLogIntranetRelInfo>(
                    new ConsultarEntidadeCadastroRequest<SistemaControleLogIntranetRelInfo>()
                    {
                        EntidadeCadastro = new SistemaControleLogIntranetRelInfo()
                        {
                            ConsultaClienteParametro = this.GetClienteParametro,
                            ConsultaDataAte = this.GetDataAte,
                            ConsultaDataDe = this.GetDataDe,
                            ConsultaEmailUsuario = this.GetEmailUsuario,
                            ConsultaOpcoesBusca = this.GetBuscarPor,
                            ConsultaTela = this.GetTela,
                            ConsultaTipoAcao = this.GetTipoAcao,
                        }
                    });

                if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
                {
                    var lLista = new TransporteRelatorio_001().TraduzirLista(lConsulta.Resultado);

                    base.PopularComboComListaGenerica<TransporteRelatorio_001>(lLista, this.rptRelatorio);
                    this.rowLinhaDeNenhumItem.Visible = false;

                    //base.RegistrarLogConsulta();
                }
                else
                {
                    this.rowLinhaDeNenhumItem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax("Ocorreu um erro durante a execução", ex);
            }
        }
    }
}