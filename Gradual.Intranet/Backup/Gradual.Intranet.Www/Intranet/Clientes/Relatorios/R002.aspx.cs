using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios;
using Gradual.OMS.Library;
using Gradual.OMS.Seguranca.Lib;
using Gradual.Intranet.Contratos.Dados.Enumeradores;

namespace Gradual.Intranet.Www.Intranet.Clientes.Relatorios
{
    public partial class R002 : PaginaBaseAutenticada
    {
        #region | Atributos

        public int gQtClientesEncontrador = default(int);

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

        private Nullable<int> GetAssessor
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

        private string GetBolsa
        {
            get 
            {
                var lRetorno = Request.Form["Bolsa"];

                if (string.IsNullOrEmpty(lRetorno))
                    return string.Empty;

                return lRetorno;
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

        private Nullable<int> GetTipoPendenciaCadastral
        {
            get 
            {
                var lRetorno = default(int);

                if(int.TryParse(this.Request.Form["TipoDePendencia"], out lRetorno))
                    return lRetorno;

                return null;
            }
        }

        private Nullable<bool> GetPendenciaResolvida
        {
            get
            {
                if(null == this.Request.Form["PendenciaResolvida"] || "0".Equals(this.Request.Form["PendenciaResolvida"]))
                    return false;

                else if (null != this.Request.Form["PendenciaResolvida"] && "1".Equals(this.Request.Form["PendenciaResolvida"]))
                    return true;

                return null;
            }
        }

        private IEnumerable<TransporteRelatorio_002> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_002>)Session["ListaDeResultados_Relatorio_002"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_002"] = value;
            }
        }

        #endregion

        #region | Métodos

        private List<TransporteRelatorio_002> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_002> lRetorno = new List<TransporteRelatorio_002>();

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

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<ClientePendenciaCadastralRelInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<ClientePendenciaCadastralRelInfo>();

            try
            {                
                var lInfo = new ClientePendenciaCadastralRelInfo()
                {
                    DtDe                     = this.GetDataInicial,
                    DtAte                    = this.GetDataFinal.AddDays(1D), //--> 1D - Numeral 1 já convertido para o tamanho Double.
                    CodigoAssessor           = this.GetAssessor,
                    IdTipoPendenciaCadastral = this.GetTipoPendenciaCadastral,
                    StResolvido              = this.GetPendenciaResolvida,
                    TipoPessoa               = this.GetTipoPessoa
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<ClientePendenciaCadastralRelInfo>(lRequest);

                if (MensagemResponseStatusEnum.OK.Equals(lResponse.StatusResposta))
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        this.gQtClientesEncontrador = this.ContarClientesFiltradosNoRelatorio(lResponse.Resultado);

                        IEnumerable<TransporteRelatorio_002> lLista = 
                            from ClientePendenciaCadastralRelInfo i in lResponse.Resultado 
                            select new TransporteRelatorio_002(i);

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

        private int ContarClientesFiltradosNoRelatorio(List<ClientePendenciaCadastralRelInfo> pParametro)
        {
            var lRetorno = new List<string>();

            if (null != pParametro && pParametro.Count > 0)
            {
                pParametro.ForEach(cpc => 
                {
                    if (!lRetorno.Contains(cpc.DsCpfCnpj))
                         lRetorno.Add(cpc.DsCpfCnpj);
                });
            }

            return lRetorno.Count;
        }

        #endregion

        #region | Eventos

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
