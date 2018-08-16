using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R002 : PaginaBaseAutenticada
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
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                ResponderBuscarItensParaListagemSimples();
            }
        }

        #endregion

        #region | Métodos de apoio

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClientePermissaoRelInfo>(
                    new ConsultarEntidadeCadastroRequest<RiscoClientePermissaoRelInfo>(
                        new RiscoClientePermissaoRelInfo()
                        {
                            ConsultaClienteParametro = this.GetParametroBusca,
                            ConsultaClienteTipo = (OpcoesBuscarPor)this.GetTipoBusca,
                            ConsultaIdBolsa = this.GetBolsa,
                            ConsultaIdGrupo = this.GetGrupo

                        }) {    DescricaoUsuarioLogado = base.UsuarioLogado.Nome , IdUsuarioLogado = base.UsuarioLogado.Id });

                if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
                {
                    var listaTransporte = new TransporteRelatorio_002().TraduzirLista(lConsulta.Resultado);
                    base.PopularComboComListaGenerica<TransporteRelatorio_002>(listaTransporte, this.rptRelatorio);
                    this.rowLinhaDeNenhumItem.Visible = false;
                }
                else
                    this.rowLinhaDeNenhumItem.Visible = true;
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion
    }
}