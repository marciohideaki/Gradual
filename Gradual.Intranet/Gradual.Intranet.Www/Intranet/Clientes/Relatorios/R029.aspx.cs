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
    public partial class R029 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 50;

        #endregion
        
        #region | Propriedades

        private DateTime? GetDataInicial
        {
            get 
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(Request.Form["DataInicial"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private DateTime? GetDataFinal
        {
            get 
            {
                var lRetorno = default(DateTime);

                if (!DateTime.TryParse(this.Request.Form["DataFinal"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private int? GetIdAssessor
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

        private int? GetCodCliente    
        {
            get
            {
                var lRetorno = default(int);

                if (!int.TryParse(this.Request.Form["CodCliente"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private IEnumerable<TransporteRelatorio_029> ListaDeResultados
        {
            get
            {
                return (IEnumerable<TransporteRelatorio_029>)Session["ListaDeResultados_Relatorio_029"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_029"] = value;
            }
        }

        #endregion

        #region | Eventos

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

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
            else if (this.Acao == "CarregarComoCSV")
            {
                this.ResponderArquivoCSV();
            }
        }

        #endregion

        #region | Métodos

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lRequest = new ConsultarEntidadeCadastroRequest<LancamentoTEDInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<LancamentoTEDInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {
                var lInfo = new LancamentoTEDInfo()
                {
                    CodigoCliente = this.GetCodCliente,
                    DtDe           = this.GetDataInicial,
                    DtAte          = this.GetDataFinal.Value.AddDays(1D),
                };

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<LancamentoTEDInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_029> lLista = from LancamentoTEDInfo i in lResponse.Resultado select new TransporteRelatorio_029(i);

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

        private List<TransporteRelatorio_029> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_029> lRetorno = new List<TransporteRelatorio_029>();

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


                lRetorno = base.RetornarSucessoAjax(BuscarParte(lParte), lMensagemFim);
            }
            else
            {
                lRetorno = base.RetornarSucessoAjax("Fim");
            }

            return lRetorno;
        }

        private LancamentoTEDInfo GerarRequest()
        {
            DateTime lDataInicial           = default(DateTime);
            DateTime lDataFinal             = default(DateTime);

            DateTime.TryParse(this.Request["DataInicial"], out lDataInicial);
            DateTime.TryParse(this.Request["DataFinal"], out lDataFinal);

            var lCodAssessor = default(int);

            if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                lCodAssessor = base.CodigoAssessor.Value;

            if (!int.TryParse(this.Request["CodAssessor"], out lCodAssessor))
                lCodAssessor = 0;

            var lCodCliente = default(int);

            if (!int.TryParse(this.Request["CodCliente"], out lCodCliente)) 
            {
                lCodCliente = 0;
            }

            return new LancamentoTEDInfo()
            {
                DtDe            = lDataInicial,
                DtAte           = lDataFinal,
                CodigoCliente   = lCodCliente
            };
        }


        private void ResponderArquivoCSV()
        {
            System.Text.StringBuilder lBuilder = new System.Text.StringBuilder();

            var lRequest = new ConsultarEntidadeCadastroRequest<LancamentoTEDInfo>() { IdUsuarioLogado = base.UsuarioLogado.Id, DescricaoUsuarioLogado = base.UsuarioLogado.Nome };
            var lResponse = new ConsultarEntidadeCadastroResponse<LancamentoTEDInfo>();

            string lPrefixo = this.PrefixoDaRaiz;

            try
            {
                var lInfo = GerarRequest();

                lRequest.EntidadeCadastro = lInfo;

                lResponse = this.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<LancamentoTEDInfo>(lRequest);

                if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
                {
                    if (lResponse.Resultado.Count > 0)
                    {
                        IEnumerable<TransporteRelatorio_029> lLista = from LancamentoTEDInfo i in lResponse.Resultado select new TransporteRelatorio_029(i);

                        if (lLista.Count() > 0)
                        {
                            lBuilder.AppendLine("DataMovimento\tCodigoCliente\tNomeCliente\tNumeroLancamento\tDescricao\tValor\t");

                            foreach (TransporteRelatorio_029 lOcorrencia in lLista)
                            {
                                lBuilder.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n"
                                , lOcorrencia.DataMovimento
                                , lOcorrencia.CodigoCliente
                                , lOcorrencia.NomeCliente
                                , lOcorrencia.NumeroLancamento
                                , lOcorrencia.Descricao
                                , lOcorrencia.Valor);
                            }

                            Response.Clear();

                            Response.ContentType = "text/xls";

                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("iso-8859-1");

                            Response.Charset = "iso-8859-1";

                            Response.AddHeader("content-disposition", "attachment;filename=MonitoramentoDeTeds.xls");

                            Response.Write(lBuilder.ToString());

                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
