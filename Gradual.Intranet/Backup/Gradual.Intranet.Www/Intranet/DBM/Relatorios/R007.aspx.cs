using System;
using System.Collections.Generic;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R007 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 150;

        #endregion

        #region | Propriedades

        private DateTime GetDataLancamento
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataOperacao"], out lRetorno);

                return lRetorno;
            }
        }

        private int? GetCdAssessor
        {
            get
            {
                if (null != base.CodigoAssessor && base.CodigoAssessor >= 0 && base.ExibirApenasAssessorAtual)
                    return base.CodigoAssessor.Value;

                var lRetorno = default(int);

                if (!int.TryParse(this.Request["CodAssessor"], out lRetorno))
                    return null;

                return lRetorno;
            }
        }

        private List<TransporteRelatorio_007> ListaDeResultados
        {
            get
            {
                return (List<TransporteRelatorio_007>)Session["ListaDeResultados_Relatorio_007"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_007"] = value;
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
                else if (this.Acao == "CarregarComoCSV")
                {
                    this.ResponderArquivoCSV();
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
                base.RetornarErroAjax(ex.ToString());
            }
        }

        #endregion

        #region | Métodos

        private List<TransporteRelatorio_007> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_007> lRetorno = new List<TransporteRelatorio_007>();

            int lIndiceInicial, lIndiceFinal;

            lIndiceInicial = pParte * gTamanhoDaParte;
            lIndiceFinal = lIndiceInicial + gTamanhoDaParte;

            if (null != this.ListaDeResultados)
            {
                for (int i = lIndiceInicial; i < this.ListaDeResultados.Count; i++)
                {
                    lRetorno.Add(this.ListaDeResultados[i]);

                    if (i == lIndiceFinal) break;
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

                if (null == this.ListaDeResultados || (lParte * gTamanhoDaParte) > this.ListaDeResultados.Count)
                {
                    lMensagemFim = "Fim";
                }
                else
                {
                    lMensagemFim = string.Format("TemMais:Parte {0} de {1}", lParte, Math.Ceiling((double)(this.ListaDeResultados.Count / gTamanhoDaParte)));
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
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);
            var lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<MovimentoDeContaCorrenteInfo>(
                new ConsultarEntidadeCadastroRequest<MovimentoDeContaCorrenteInfo>()
                {
                    EntidadeCadastro = new MovimentoDeContaCorrenteInfo()
                    {
                        ConsultaCodigoAssessor = lListaAssessores,
                        ConsultaDataLancamento = this.GetDataLancamento,
                    }
                });

            if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lRetorno.Resultado.Count > 0)
                {
                    var lTransporte = new TransporteRelatorio_007().TraduzirLista(lRetorno.Resultado);


                    if (lTransporte.Count >= gTamanhoDaParte)
                    {
                        this.ListaDeResultados = lTransporte;

                        this.rptMvtoContaCorrente.DataSource = this.BuscarParte(0);
                    }
                    else
                    {
                        this.rptMvtoContaCorrente.DataSource = lTransporte;
                    }
                    
                    this.rptMvtoContaCorrente.DataBind();

                    this.rowLinhaDeNenhumItem.Visible = false;
                    this.rowLinhaCarregandoMais.Visible = true;
                }
                else
                {
                    this.rowLinhaCarregandoMais.Visible = false;
                    this.rowLinhaDeNenhumItem.Visible = true;
                }
            }
        }

        private void ResponderArquivoCSV()
        {
            var lConteudoArquivo = new StringBuilder();
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);

            var lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<MovimentoDeContaCorrenteInfo>(
                new ConsultarEntidadeCadastroRequest<MovimentoDeContaCorrenteInfo>()
                {
                    EntidadeCadastro = new MovimentoDeContaCorrenteInfo()
                    {
                        ConsultaCodigoAssessor = lListaAssessores,
                        ConsultaDataLancamento = this.GetDataLancamento,
                    }
                });

            if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_007().TraduzirLista(lRetorno.Resultado);

                lConteudoArquivo.AppendLine("Movimento de Conta Corrente\t\r\n");
                lConteudoArquivo.AppendFormat("Retirado em {0}\t\r\n\r\n", DateTime.Today.ToString("dd/MM/yyyy"));

                lConteudoArquivo.Append("Cliente\tData de Lançamento\tData de Referência\tData de Liquidação\tLançamento\tValor (R$)\n\r");
                
                if (null != lTransporte && lTransporte.Count > 0)
                lTransporte.ForEach(tsp=>
                {
                    lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r", tsp.Cliente, tsp.DataLanc, tsp.DataRef, tsp.DataLiq, tsp.Lancamento, tsp.Valor);
                });

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=MovimentoDeContaCorrente.xls");

                this.Response.Write(lConteudoArquivo.ToString());

                this.Response.End();
            }
        }

        #endregion
    }
}