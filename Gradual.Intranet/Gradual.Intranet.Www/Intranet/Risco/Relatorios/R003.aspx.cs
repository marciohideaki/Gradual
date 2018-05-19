using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R003 : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int? GetBolsa
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Bolsa"], out lRetorno) || 0.Equals(lRetorno))
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
            var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoParametrosRelInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoParametrosRelInfo>()
                {
                    EntidadeCadastro = new RiscoParametrosRelInfo() 
                    {
                        ConsultaIdBolsa = this.GetBolsa,
                        ConsultaIdParametro = this.GetParametro,
                    },
                    DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                    IdUsuarioLogado = base.UsuarioLogado.Id 
                });

            var lLista = new TransporteRelatorio_003().TraduzirLista(lConsulta.Resultado);

            if(null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
            {
                base.PopularComboComListaGenerica<TransporteRelatorio_003>(lLista, this.rptRelatorio);
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;
        }

        #endregion
    }
}