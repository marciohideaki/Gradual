using System;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R008 : PaginaBaseAutenticada
    {
        #region | Propriedades

        public string GetBuscaParametro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["ParametroBusca"]))
                    return null;

                return this.Request.Form["ParametroBusca"];
            }
        }

        public int? GetBuscaPor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["TipoBusca"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        public int? GetIdGrupo
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Grupo"], out lRetorno) || 0.Equals(lRetorno))
                    return null;

                return lRetorno;
            }
        }

        public int? GetIdParametro
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

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lEntidadeCadastro = new RiscoClienteBloqueioGrupoRelInfo();
            {   //--> Configurar parâmetros de pesquisa.
                lEntidadeCadastro.IdGrupo = this.GetIdGrupo;

                lEntidadeCadastro.IdParametro = this.GetIdParametro;

                if (((int)OpcoesBuscarClientePor.CodBovespa).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.CdCodigo = this.GetBuscaParametro;

                else if (((int)OpcoesBuscarClientePor.NomeCliente).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.DsNome = this.GetBuscaParametro;

                else if (((int)OpcoesBuscarClientePor.CpfCnpj).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.DsCpfCnpj = this.GetBuscaParametro;
            }

            var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteBloqueioGrupoRelInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoClienteBloqueioGrupoRelInfo>() 
                {
                     EntidadeCadastro = lEntidadeCadastro,
                });

            if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
            {
                var lListaTransporte = new TransporteRelatorio_008().TraduzirLista(lConsulta.Resultado);
                base.PopularComboComListaGenerica<TransporteRelatorio_008>(lListaTransporte, this.rptRelatorio);
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;
        }

        #endregion
    }
}