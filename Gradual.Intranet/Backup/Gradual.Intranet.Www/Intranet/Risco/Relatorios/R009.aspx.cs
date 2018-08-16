using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;
using Gradual.Intranet.Www.App_Codigo;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R009 : PaginaBaseAutenticada
    {
        #region | Propriedades

        private int GetIdCanalBovespa
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ChannelIdBovespa"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetBuscaParametro
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["ParametroBusca"]))
                    return null;

                return this.Request.Form["ParametroBusca"].Replace(".", "").Replace(".", "").Replace(".", "").Replace("-", "").Replace("/", "");
            }
        }

        private int? GetBuscaPor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["TipoBusca"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetTipoDeOrdem
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["TipoDeOrdem"]))
                    return null;

                return this.Request.Form["TipoDeOrdem"];
            }
        }

        private string GetAtivo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Papel"]))
                    return null;

                return this.Request.Form["Papel"];
            }
        }

        private DateTime? GetDataTransacaoDe
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataInicio"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime? GetDataTransacaoAte
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataFim"], out lRetorno))
                    return null;

                return lRetorno.AddDays(1D);
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
            var lEntidadeCadastro = new RiscoClienteSaldoBloqueadoRelInfo();
            {
                lEntidadeCadastro.ConsultaIdCanalBovespa = this.GetIdCanalBovespa;
                lEntidadeCadastro.ConsultaDsAtivo = this.GetAtivo;
                lEntidadeCadastro.ConsultaDtTransacaoAte = this.GetDataTransacaoAte;
                lEntidadeCadastro.ConsultaDtTransacaoDe = this.GetDataTransacaoDe;
                lEntidadeCadastro.ConsultaTpOrdem = this.GetTipoDeOrdem;

                if (((int)OpcoesBuscarClientePor.CodBovespa).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.ConsultaIdCliente = string.IsNullOrWhiteSpace(this.GetBuscaParametro) ? new Nullable<int>() : this.GetBuscaParametro.DBToInt32();

                else if (((int)OpcoesBuscarClientePor.NomeCliente).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.ConsultaDsNome = this.GetBuscaParametro;

                else if (((int)OpcoesBuscarClientePor.CpfCnpj).Equals(this.GetBuscaPor))
                    lEntidadeCadastro.ConsultaDsCpfCnpj = this.GetBuscaParametro;
            };

            var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteSaldoBloqueadoRelInfo>(
                new ConsultarEntidadeCadastroRequest<RiscoClienteSaldoBloqueadoRelInfo>()
                {
                    EntidadeCadastro = lEntidadeCadastro
                });

            if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
            {
                var lListaTransporte = new TransporteRelatorio_009().TraduzirLista(lConsulta.Resultado);
                base.PopularComboComListaGenerica<TransporteRelatorio_009>(lListaTransporte, this.rptRelatorio);
                this.lblTotalBloqueado.Text = (lListaTransporte != null && lListaTransporte.Count > 0) ? lListaTransporte[0].BloqueadoTotalGeral : "0,00";
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;
        }

        #endregion
    }
}