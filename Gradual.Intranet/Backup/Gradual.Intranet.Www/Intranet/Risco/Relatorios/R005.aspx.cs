using System;
using System.Web.UI.WebControls;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Risco;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Risco;

namespace Gradual.Intranet.Www.Intranet.Risco.Relatorios
{
    public partial class R005 : PaginaBaseAutenticada
    {
        #region | Atributos

        ConsultarEntidadeCadastroResponse<RiscoClienteLimiteMovimentoRelInfo> gConsultaMovimento;

        #endregion

        #region | Propriedades

        private int GetTipoBusca
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["TipoBusca"], out lRetorno);

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

        private int? GetIdClienteParametroSelecionado
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["IdClienteParametroSelecionado"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime? GetDataAtualizacao
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataValidadeAtualizada"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime GetDataInicio
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicio"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFim
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFim"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetHistorico
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.Form["Historico"]))
                    return null;

                return this.Request.Form["Historico"];
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);

                if ("BuscarItensParaListagemSimples".Equals(this.Acao))
                {
                    this.ResponderBuscarItensParaListagemSimples();
                }
                else if ("AtualizarDataVencimento".Equals(this.Acao))
                {
                    string lRespostaJson;

                    lRespostaJson = this.ResponderAtualizarDataDeVencimento();

                    Response.Clear();

                    Response.Write(lRespostaJson);

                    Response.End();
                }
            }
            catch (Exception ex)
            {
                base.RetornarErroAjax(ex.ToString());
            }
        }

        protected void rptRelatorio_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (null != gConsultaMovimento)
            {
                int lIdClienteParametro = ((TransporteRelatorio_005)(e.Item.DataItem)).IdClienteParametro;
                var lDataSource = gConsultaMovimento.Resultado.FindAll(delegate(RiscoClienteLimiteMovimentoRelInfo clm) { return clm.IdClienteParametro == lIdClienteParametro; });

                ((Repeater)e.Item.Controls[1]).DataSource = new TransporteRelatorio_005().TraduzirListaMovimento(lDataSource);
                ((Repeater)e.Item.Controls[1]).DataBind();
            }
        }

        #endregion

        #region | Métodos de apoio

        private string ResponderBuscarItensParaListagemSimples()
        {
            var lConsultaSaldo = 
                base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteLimiteRelInfo>(
                    new ConsultarEntidadeCadastroRequest<RiscoClienteLimiteRelInfo>(
                        new RiscoClienteLimiteRelInfo()
                        {
                            ConsultaClienteParametro = this.GetParametroBusca,
                            ConsultaClienteTipo = (OpcoesBuscarPor)this.GetTipoBusca,
                            ConsultaFim = this.GetDataFim,
                            ConsultaHistorico = this.GetHistorico,
                            ConsultaIdParametro = this.GetParametro,
                            ConsultaInicio = this.GetDataInicio,
                        })
                        {
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome, 
                            IdUsuarioLogado = base.UsuarioLogado.Id 
                        });

            gConsultaMovimento = 
                base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<RiscoClienteLimiteMovimentoRelInfo>(
                    new ConsultarEntidadeCadastroRequest<RiscoClienteLimiteMovimentoRelInfo>(
                        new RiscoClienteLimiteMovimentoRelInfo()
                        {
                            ConsultaClienteParametro = this.GetParametroBusca,
                            ConsultaClienteTipo = (OpcoesBuscarPor)this.GetTipoBusca,
                        })
                        {
                            DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                            IdUsuarioLogado = base.UsuarioLogado.Id 
                        });

            if (null != lConsultaSaldo.Resultado && !0.Equals(lConsultaSaldo.Resultado.Count))
            {
                var listaTransporte = new TransporteRelatorio_005().TraduzirListaSaldo(lConsultaSaldo.Resultado);
                base.PopularComboComListaGenerica<TransporteRelatorio_005>(listaTransporte, this.rptRelatorio);
                this.rowLinhaDeNenhumItem.Visible = false;
            }
            else
                this.rowLinhaDeNenhumItem.Visible = true;

            return string.Empty;
        }

        private string ResponderAtualizarDataDeVencimento()
        {
            try
            {
                var lResultado = base.ServicoPersistencia.SalvarObjeto<RiscoClienteLimiteRelInfo>(
                    new OMS.Persistencia.SalvarObjetoRequest<RiscoClienteLimiteRelInfo>()
                    {
                        Objeto = new RiscoClienteLimiteRelInfo()
                        {
                            DtValidade = this.GetDataAtualizacao,
                            IdClienteParametro = this.GetIdClienteParametroSelecionado,
                        },
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    });

                return base.RetornarSucessoAjax("Data do parâmetro atualizada com sucesso!", this.GetIdClienteParametroSelecionado, this.GetDataAtualizacao);
            }
            catch
            {
                return base.RetornarSucessoAjax("Não foi possível atualizar a data de validade.");
            }
        }

        #endregion
    }
}