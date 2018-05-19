using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R004 : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int? GetBolsa
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Bolsa"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetGrupo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Grupo"], out lRetorno) || 0.Equals(lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetParametro
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Parametro"], out lRetorno) || 0.Equals(lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetTipoBusca
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["TipoBusca"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private bool? GetEstado
        {
            get 
            {
                var lRetorno = default(bool);

                if (!bool.TryParse(this.Request.Form["EstadoExpirado"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetParametroBusca
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["ParametroBusca"]))
                    return string.Empty;

                return this.Server.HtmlDecode(this.Request.Form["ParametroBusca"]);
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if (this.Acao == "BuscarItensParaListagemSimples")
                {
                    this.ResponderBuscarItensParaListagemSimples();
                    base.Response.Clear();
                    base.Response.End();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos de apoio

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteParametroRelInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoClienteParametroRelInfo>(
                    new RiscoClienteParametroRelInfo()
                    {
                        ConsultaClienteParametro = this.GetParametroBusca,
                        ConsultaClienteTipo = (OpcoesBuscarPor)this.GetTipoBusca,
                        ConsultaIdBolsa = this.GetBolsa,
                        ConsultaIdGrupo = this.GetGrupo,
                        ConsultaIdParametro = this.GetParametro,
                        ConsultaEstado = this.GetEstado,
                    })
                    {
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome , 
                        IdUsuarioLogado = base.UsuarioLogado.Id 
                    });

            if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
            {
                var listaTransporte = new TransporteRelatorio_004().TraduzirLista(lConsulta.Resultado);
                base.PopularComboComListaGenerica<TransporteRelatorio_004>(listaTransporte, this.rptRelatorio);
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;
        }

        #endregion
    }
}