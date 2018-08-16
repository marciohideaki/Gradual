using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R009 : PaginaBaseAutenticada
    {

        #region Globais

        private int gTamanhoDaParte = 50;

        #endregion
         
        #region | Propriedades

        private DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataInicial"], out lRetorno);

                return lRetorno;
            }
        }

        private DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno);

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

                if (!int.TryParse(this.Request.Form["Assessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private IEnumerable<TransporteRelatorio_009> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_009>)Session["ListaDeResultados_Relatorio_009"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_009"] = value;
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
        #endregion

        #region | Métodos Private

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteSemLoginInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteSemLoginInfo>();

            try
            {
                ClienteSemLoginInfo lInfo = new ClienteSemLoginInfo()
                {
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal.AddDays(1D),
                    CodigoAssessor = this.GetAssessor,
                    TipoPessoa     = this.GetTipoPessoa
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSemLoginInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_009> lLista = from ClienteSemLoginInfo i in lResponse.Resultado select new TransporteRelatorio_009(i);

                        if (lLista.Count() >= gTamanhoDaParte)
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

                        rowLinhaDeNenhumItem.Visible = false;
                    }
                    else
                    {
                        rowLinhaDeNenhumItem.Visible = true;
                    }
                }
            }
            catch (Exception exBusca)
            {
                throw exBusca;
            }
        }

        private List<TransporteRelatorio_009> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_009> lRetorno = new List<TransporteRelatorio_009>();

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

        #region Event Handlers

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (this.Acao == "BuscarItensParaListagemSimples")
            {
                ResponderBuscarItensParaListagemSimples();
            }
            else if (this.Acao == "BuscarParte")
            {
                Response.Clear();

                string lResponse = ResponderBuscarMaisDados();

                Response.Write(lResponse);

                Response.End();
            }
        }

        #endregion
    }
}
