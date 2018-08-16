using System;
using Gradual.Intranet.Contratos.Dados.Enumeradores;
using Gradual.Intranet.Contratos.Dados.Relatorios.Monitoramento;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Monitoramento;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.Intranet.Monitoramento.Relatorios
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

        private Nullable<DateTime> GetDataVencimento
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["DataVencimento"]))
                    return null;

                return Convert.ToDateTime(this.Request.Form["DataVencimento"]);
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

        private int? GetCodigoAssessor
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetPapel
        {
            get
            {
                var lRetorno = this.Request.Form["Papel"];

                if (string.IsNullOrWhiteSpace(lRetorno))
                    return null;

                return lRetorno.Substring(0, lRetorno.Length <= 4 ? lRetorno.Length : 4).ToUpper().Trim();
            }
        }

        private int? GetCodigoCarteira
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["Carteira"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private string GetSerie
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["Serie"]))
                    return null;

                return this.Request.Form["Serie"];
            }
        }

        private int? GetStrike
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["strike"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetSentidoCompradoLancado
        {
            get
            {
                if ("C".Equals(this.Request.Form["Sentido"]))
                    return 1;
                else if ("L".Equals(this.Request.Form["Sentido"]))
                    return -1;
                else
                    return null;
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

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            try
            {
                int? lCodigoAssessor = base.CodigoAssessor.HasValue ? base.CodigoAssessor.Value : (GetCodigoAssessor.HasValue ? GetCodigoAssessor.Value : new Nullable<int>() );

                List<int> lListaAssessores = base.ConsultarCodigoAssessoresVinculadosListInt(lCodigoAssessor);
                
                string sListaAssessores = string.Empty;

                var lConsulta = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePosicaoDeOpcaoRelInfo>(
                    new ConsultarEntidadeCadastroRequest<ClientePosicaoDeOpcaoRelInfo>()
                    {
                        EntidadeCadastro = new ClientePosicaoDeOpcaoRelInfo()
                        {
                            ConsultaClienteParametro       = this.GetClienteParametro,
                            ConsultaSentidoCompradoLancado = this.GetSentidoCompradoLancado,
                            ConsultaCodigoAssessor         = sListaAssessores,
                            //ConsultaCodigoAssessor         =  lCodigoAssessor.ToString(),
                            ConsultaCodigoCarteira         = this.GetCodigoCarteira,
                            ConsultaClienteTipo            = this.GetBuscarPor,
                            ConsultarStrike                = this.GetStrike,
                            ConsultaSerie                  = this.GetSerie,
                            ConsultaPapel                  = this.GetPapel,
                            ConsultaDtVencimento           = this.GetDataVencimento
                        },
                        DescricaoUsuarioLogado = base.UsuarioLogado.Nome,
                        IdUsuarioLogado = base.UsuarioLogado.Id
                    });

                var lLista = new List<TransporteRelatorio_001>();

                if (lListaAssessores != null && lListaAssessores.Count > 0)
                {
                    lLista = new TransporteRelatorio_001().TraduzirLista(lConsulta.Resultado, lListaAssessores);
                }
                else
                {
                    lLista = new TransporteRelatorio_001().TraduzirLista(lConsulta.Resultado);
                }

                if (null != lConsulta.Resultado && !0.Equals(lConsulta.Resultado.Count))
                {
                    base.PopularComboComListaGenerica<TransporteRelatorio_001>(lLista, this.rptRelatorio);
                    this.rowLinhaDeNenhumItem.Visible = false;

                    base.RegistrarLogConsulta();
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
