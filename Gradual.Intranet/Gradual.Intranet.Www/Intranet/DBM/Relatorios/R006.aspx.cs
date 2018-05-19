using System;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Contratos.Mensagens;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.DBM;
using Gradual.OMS.Library;
using System.Collections.Generic;

namespace Gradual.Intranet.Www.Intranet.DBM.Relatorios
{
    public partial class R006 : PaginaBaseAutenticada
    {
        #region | Atributos

        private int gTamanhoDaParte = 150;

        public string gTotalClientesOperaram;

        public string gDataConsulta;

        #endregion

        #region | Propriedades

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

        private DateTime GetDataOperacao
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataOperacao"], out lRetorno);

                return lRetorno;
            }
        }

        private List<TransporteRelatorio_006> ListaDeResultados
        {
            get
            {
                return (List<TransporteRelatorio_006>)Session["ListaDeResultados_Relatorio_006"];
            }

            set
            {
                Session["ListaDeResultados_Relatorio_006"] = value;
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

        private List<TransporteRelatorio_006> BuscarParte(int pParte)
        {
            List<TransporteRelatorio_006> lRetorno = new List<TransporteRelatorio_006>();

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

        private void ResponderBuscarItensParaListagemSimples()
        {
            var lListaAssessores = base.ConsultarCodigoAssessoresVinculadosString(this.GetCdAssessor);
            var lRetorno = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SaldoProjecoesEmContaCorrenteInfo>(
                new ConsultarEntidadeCadastroRequest<SaldoProjecoesEmContaCorrenteInfo>()
                {
                    EntidadeCadastro = new SaldoProjecoesEmContaCorrenteInfo()
                    {
                        ConsultaCdAssessor = lListaAssessores,
                        ConsultaDataOperacao = this.GetDataOperacao
                    }
                });

            if (lRetorno.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                if (lRetorno.Resultado.Count > 0)
                {
                    var lTransporte = new TransporteRelatorio_006().TraduzirLista(lRetorno.Resultado);

                    this.gTotalClientesOperaram = lTransporte.Count.ToString();
                    this.gDataConsulta = this.GetDataOperacao.ToString("dd/MM/yyyy");

                    if (lTransporte.Count >= gTamanhoDaParte)
                    {
                        this.ListaDeResultados = lTransporte;
                        this.rptSaldoProjecoesEmContaCorrente.DataSource = BuscarParte(0);
                    }
                    else
                    {
                        this.rptSaldoProjecoesEmContaCorrente.DataSource = lTransporte;
                        this.ListaDeResultados = null;
                    }

                    this.rptSaldoProjecoesEmContaCorrente.DataBind();

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

            var lResponse = base.ServicoPersistenciaCadastro.ConsultarEntidadeCadastro<SaldoProjecoesEmContaCorrenteInfo>(
                new ConsultarEntidadeCadastroRequest<SaldoProjecoesEmContaCorrenteInfo>()
                {
                    EntidadeCadastro = new SaldoProjecoesEmContaCorrenteInfo()
                    {
                        ConsultaCdAssessor = lListaAssessores,
                        ConsultaDataOperacao = this.GetDataOperacao
                    }
                });

            if (lResponse.StatusResposta == MensagemResponseStatusEnum.OK)
            {
                var lTransporte = new TransporteRelatorio_006().TraduzirLista(lResponse.Resultado);

                lConteudoArquivo.AppendLine("Saldo e projeções em conta corrente\t\r\n\r\n");

                lConteudoArquivo.Append("Código\tNome\tTotal\tA Liquidar\tDisponível\tD + 1\tD + 2\n");

                if (null != lTransporte && lTransporte.Count > 0)
                    lTransporte.ForEach(spc =>
                    {
                        lConteudoArquivo.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", spc.CodigoCliente, spc.NomeCliente, spc.Total, spc.ALiquidar, spc.Disponivel, spc.D1, spc.D2);
                    });

                this.Response.Clear();

                this.Response.ContentType = "text/xls";

                this.Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

                this.Response.Charset = "iso-8859-1";

                this.Response.AddHeader("content-disposition", "attachment;filename=SaldoProjeçõesEmContaCorrente.xls");

                this.Response.Write(lConteudoArquivo.ToString());

                this.Response.End();
            }
        }

        #endregion
    }
}