using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R004 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion

        #region | Propriedades

        private Nullable<DateTime> GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private Nullable<DateTime> GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }
        
        private IEnumerable<TransporteRelatorio_004> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_004>)this.Session["ListaDeResultados_Relatorio_004"];
            }

            set
            {
                this.Session["ListaDeResultados_Relatorio_004"] = value;
            }
        }

        private string GetTipoPessoa
        {
            get
            {
                string lRetorno = string.Empty;

                if (!string.IsNullOrWhiteSpace(this.Request.Form["TipoPessoa"]))
                    lRetorno = this.Request.Form["TipoPessoa"];

                return lRetorno;
            }
        }

        private int? GetAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return lRetorno;

                return null;
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteRenovacaoCadastralInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteRenovacaoCadastralInfo>();

            var lInfo = new ClienteRenovacaoCadastralInfo()
            {
                CdAssessor    = this.GetAssessor == null ? null : this.GetAssessor.ToString(),
                DtPesquisa    = this.GetDataInicial,
                DtPesquisaFim = this.GetDataFinal.Value.AddDays(1D),
                TipoPessoa    = this.GetTipoPessoa,
                DsDesejaAplicar = String.Empty
            };

            lRequest.EntidadeCadastro = lInfo;

            lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteRenovacaoCadastralInfo>(lRequest);

            if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
            {
                if (lResponse.Resultado.Count > 0)
                {
                    var lLista = new List<TransporteRelatorio_004>();

                    lResponse.Resultado.ForEach(delegate(ClienteRenovacaoCadastralInfo cliInfo) { lLista.Add(new TransporteRelatorio_004(cliInfo)); });

                    if (lLista.Count >= gTamanhoDaParte)
                    {
                        this.ListaDeResultados = lLista;

                        this.rptRelatorio.DataSource = BuscarParte(1);

                        rowLinhaCarregandoMais.Visible = true;
                    }
                    else
                    {
                        this.rptRelatorio.DataSource = lLista;
                    }

                    this.rptRelatorio.DataBind();

                    this.rowLinhaDeNenhumItem.Visible = false;
                }
                else
                {
                    this.rowLinhaDeNenhumItem.Visible = true;
                }
            }
        }

        private List<TransporteRelatorio_004> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_004> lRetorno = new List<TransporteRelatorio_004>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                for (int a = lIndiceInicial; a < this.ListaDeResultados.Count(); a++)
                {
                    lRetorno.Add(this.ListaDeResultados.ElementAt(a));

                    if (a == lIndiceFinal) break;
                }
            }

            return lRetorno;
        }

        private string ResponderBuscarMaisDados()
        {
            string lRetorno;

            int lParte;

            if (int.TryParse(Request.Form["Parte"], out lParte))
            {
                string lMensagemFim;

                if (null == this.ListaDeResultados || (lParte * gTamanhoDaParte) > this.ListaDeResultados.Count())
                {
                    lMensagemFim = "Fim";
                }
                else
                {
                    lMensagemFim = string.Format("TemMais:Parte {0} de {1}", lParte, Math.Ceiling((double)(this.ListaDeResultados.Count() / gTamanhoDaParte)));
                }


                lRetorno = RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = RetornarSucessoAjax("Fim");
            }

            return lRetorno;
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
            else if (this.Acao == "BuscarParte")
            {
                this.Response.Clear();

                string lResponse = this.ResponderBuscarMaisDados();

                this.Response.Write(lResponse);

                this.Response.End();
            }
        }

        #endregion
    }
}
