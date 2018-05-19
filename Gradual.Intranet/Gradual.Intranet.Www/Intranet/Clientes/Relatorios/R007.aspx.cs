using System;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R007 : PaginaBaseAutenticada
    {
        #region | Atributos

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

        private string GetPais
        {
            get 
            {
                if (string.IsNullOrEmpty(this.Request.Form["Pais"]))
                    return string.Empty;

                return this.Request.Form["Pais"];
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

        private int GetCodigoAtividade
        {
            get 
            {
                var lRetorno = default(int);

                int.TryParse(this.Request.Form["AtividadeIlicita"], out lRetorno);

                return lRetorno;
            }
        }

        private IEnumerable<TransporteRelatorio_007> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_007>)this.Session["ListaDeResultados_Relatorio_007"];
            }

            set
            {
                this.Session["ListaDeResultados_Relatorio_007"] = value;
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

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClienteSuspeitoInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClienteSuspeitoInfo>();

            try
            {
                var lInfo = new ClienteSuspeitoInfo()
                {
                    DtDe        = this.GetDataInicial,
                    DtAte       = this.GetDataFinal.AddDays(1D),
                    CdPais      = this.GetPais,
                    CdAtividade = this.GetCodigoAtividade,
                    TipoPessoa  = this.GetTipoPessoa,
                    CdAssessor  = null == this.GetAssessor ? null : this.GetAssessor.ToString(),
                };
                
                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSuspeitoInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        var lLista = new List<TransporteRelatorio_007>();

                        lResponse.Resultado.ForEach(delegate(ClienteSuspeitoInfo csi) { lLista.Add(new TransporteRelatorio_007(csi)); });

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
                base.RetornarErroAjax("Houve um erro ao tentar carregar relatório.", exBusca);
            }
        }

        private List<TransporteRelatorio_007> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_007> lRetorno = new List<TransporteRelatorio_007>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = (pParte - 1) * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados && lIndiceInicial >= 0)
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

                lRetorno = base.RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = base.RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            try
            {
                if (this.Acao == "BuscarItensParaListagemSimples")
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
            catch (Exception ex)
            {
                base.RetornarErroAjax("Houve um erro ao tentar carregar o relatório", ex);
            }
        }

        #endregion
    }
}
