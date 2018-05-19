using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R001 : PaginaBaseAutenticada
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

        private int? GetPermissao
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Permissao"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                this.ResponderBuscarItensParaListagemSimples();
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoPermissaoRelInfo>(
                    new ConsultarEntidadeCadastroRequest<RiscoPermissaoRelInfo>()
                    {
                        EntidadeCadastro = new RiscoPermissaoRelInfo()
                        {
                            ConsultaBolsa = this.GetBolsa,
                            ConsultaPermissao = this.GetPermissao,
                        },  DescricaoUsuarioLogado = base.UsuarioLogado.Nome , IdUsuarioLogado = base.UsuarioLogado.Id 
                    });

                var lLista = new TransporteRelatorio_001().TraduzirLista(lConsulta.Resultado);

                if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
                {
                    base.PopularComboComListaGenerica<TransporteRelatorio_001>(lLista, this.rptRelatorio);
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