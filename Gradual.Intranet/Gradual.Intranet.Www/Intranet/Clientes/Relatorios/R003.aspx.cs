using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R003 : PaginaBaseAutenticada
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

                return lRetorno.AddDays(1D);
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

        private Nullable<bool> GetPendenciaResolvida
        {
            get
            {
                if (null == this.Request.Form["PendenciaResolvida"] || "0".Equals(this.Request.Form["PendenciaResolvida"]))
                    return false;

                else if (null != this.Request.Form["PendenciaResolvida"] && "1".Equals(this.Request.Form["PendenciaResolvida"]))
                    return true;

                return null;
            }
        }

        private string GetCpfCnpj
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["CpfCnpj"]))
                    return null;

                return this.Request.Form["CpfCnpj"];
            }
        }

        #endregion

        private string GetTipoPessoa
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Request.Form["TipoPessoa"]))
                    return null;

                return this.Request.Form["TipoPessoa"];
            }
        }

        private IEnumerable<TransporteRelatorio_003> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_003>)Session["ListaDeResultados_Relatorio_003"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_003"] = value;
            }
        }

        private void ResponderBuscarItensParaListagemSimples()
        {
            ConsultarEntidadeCadastroRequest<ClienteSolicitacaoAlteracaoCadastralInfo> lRequest = new ConsultarEntidadeCadastroRequest<ClienteSolicitacaoAlteracaoCadastralInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            ConsultarEntidadeCadastroResponse<ClienteSolicitacaoAlteracaoCadastralInfo> lResponse = new ConsultarEntidadeCadastroResponse<ClienteSolicitacaoAlteracaoCadastralInfo>();

            try
            {
                ClienteSolicitacaoAlteracaoCadastralInfo lInfo = new ClienteSolicitacaoAlteracaoCadastralInfo()
                {
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal,
                    CodigoAssessor = this.GetAssessor,
                    StResolvido    = this.GetPendenciaResolvida,
                    DsCpfCnpj      = this.GetCpfCnpj,
                    TipoPessoa     = this.GetTipoPessoa
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClienteSolicitacaoAlteracaoCadastralInfo>(lRequest);
                
                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_003> lLista = from ClienteSolicitacaoAlteracaoCadastralInfo i in lResponse.Resultado select new TransporteRelatorio_003(i);

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

        private List<TransporteRelatorio_003> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_003> lRetorno = new List<TransporteRelatorio_003>();

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
    }
}
