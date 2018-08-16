using System;
using System.Globalization;
using System.Text;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.Intranet.Www.App_Codigo.TransporteJson.Relatorios.Financeiro;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Info.Enum;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class ExtratoDeConta : PaginaBase
    {
        #region | Atributos

        public string gSaldoDisponivelPosNeg = "";

        public string gSaldoDisponivel = "";

        public string gTotalClientePosNeg = "";

        public string gTotalCliente = "";

        public string gSaldoAnterior = "";

        private CultureInfo gCultureInfo = new CultureInfo("pt-BR");

        #endregion

        #region | Propriedades

        public DateTime GetDataInicial
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataInicial"], gCultureInfo, DateTimeStyles.AdjustToUniversal, out lRetorno);

                return lRetorno;
            }
        }

        public DateTime GetDataFinal
        {
            get
            {
                var lRetorno = default(DateTime);

                DateTime.TryParse(this.Request["DataFinal"], gCultureInfo, DateTimeStyles.AdjustToUniversal, out lRetorno);

                return lRetorno;
            }
        }

        private int GetCodCliente
        {
            get
            {
                var lRetorno = default(int);

                int.TryParse(this.Request["CdBovespaCliente"], out lRetorno);

                if (lRetorno == default(int))
                    int.TryParse(this.Request["CdBmfCliente"], out lRetorno);

                return lRetorno;
            }
        }

        private string GetNomeCliente
        {
            get
            {
                return this.Request["NomeCliente"];
            }
        }

        private EnumTipoExtradoDeConta GetTipoDeExtrato
        {
            get
            {
                if ("liq".Equals(this.Request["TipoExtrato"]))
                    return EnumTipoExtradoDeConta.Liquidacao;
                else
                    return EnumTipoExtradoDeConta.Movimento;
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if ("CarregarComoCSV".Equals(this.Request["Acao"]))
            {
                this.ResponderArquivoCSV();
            }
            else
            {
                this.ResponderCarregarHtml();
            }
        }

        #endregion

        #region | Métodos

        private ContaCorrenteExtratoInfo CarregarRelatorio()
        {
            //--> Setando o padrão de exibição do negatico para currency (R$ -xxx.xxx,xx)
            this.gCultureInfo.NumberFormat.CurrencyNegativePattern = 2;

            var lServicoAtivador = Ativador.Get<IServicoExtratos>();

            var lRespostaBusca = lServicoAtivador.ConsultarExtratoContaCorrente(
                new ContaCorrenteExtratoRequest()
                {
                    ConsultaTipoExtratoDeConta = this.GetTipoDeExtrato,
                    ConsultaCodigoCliente = this.GetCodCliente,
                    ConsultaNomeCliente = this.GetNomeCliente,
                    ConsultaDataInicio = this.GetDataInicial,
                    ConsultaDataFim = this.GetDataFinal,
                });

            if (lRespostaBusca.StatusResposta == CriticaMensagemEnum.OK)
            {
                base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                return lRespostaBusca.Relatorio;
            }
            else
                throw new Exception(string.Format("{0}-{1}", lRespostaBusca.StatusResposta, lRespostaBusca.StackTrace));
        }

        private void ResponderArquivoCSV()
        {
            var lRelatorio = this.CarregarRelatorio();

            StringBuilder lBuilder = new StringBuilder();

            /*
             //Exemplo de arquivo csv para esse relatorio:
             //RelatorioContaCorrente.csv:
             
                Relatório: Extrato de Conta Corrente
                Cliente:;0000031940 - RAFAEL SANCHES GARCIA
                Saldo Anterior:;45,50
                Movimentação;Liq;Histórico;Débito;Crédito;Saldo;
             
             */

            lBuilder.AppendLine("Relatório: Extrato de Conta Corrente");

            lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", lRelatorio.CodigoCliente.ToCodigoClienteFormatado(), lRelatorio.NomeCliente);

            lBuilder.AppendFormat("Saldo Anterior:;{0}\r\n", lRelatorio.SaldoAnterior.ToString("C2", gCultureInfo));

            lBuilder.AppendLine("Movimentação;Liq;Histórico;Débito(R$);Crédito(R$);Saldo(R$)");

            var lListaContaCorrenteMovimento = new TransporteExtratoContaCorrente().TraduzirLista(lRelatorio.ListaContaCorrenteMovimento);

            if (lListaContaCorrenteMovimento.Count > 0)
            {
                foreach (TransporteExtratoContaCorrente lItem in lListaContaCorrenteMovimento)
                {
                    lBuilder.AppendFormat("{0};{1};{2};{3};{4};{5}\r\n", lItem.Mov, lItem.Liq, lItem.Historico, lItem.Debito, lItem.Credito, lItem.Saldo);
                }
            }
            else
            {
                lBuilder.AppendLine("(0 lançamentos encontrados)");
            }

            lBuilder.AppendLine("");

            lBuilder.AppendFormat(";;;;Disponível:;{0}\r\n", lRelatorio.SaldoDisponivel.ToString("N2", gCultureInfo));

            lBuilder.AppendFormat(";;;;Total Cliente:;{0}\r\n", lRelatorio.SaldoTotal.ToString("N2", gCultureInfo));

            Response.Clear();

            Response.ContentType = "text/csv";

            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            Response.Charset = "iso-8859-1";

            Response.AddHeader("content-disposition", "attachment;filename=ExtratoDeConta.csv");

            Response.Write(lBuilder.ToString());

            Response.End();
        }

        private void ResponderCarregarHtml()
        {
            try
            {
                var lRelatorio = this.CarregarRelatorio();

                this.lblCodigoCliente.Text = lRelatorio.CodigoCliente.ToString().PadLeft(10, '0');
                this.lblNomeCliente.Text = lRelatorio.NomeCliente.ToStringFormatoNome();

                if (lRelatorio.ListaContaCorrenteMovimento.Count > 0)
                {
                    this.tbTotaisCliente.Visible = true;
                    this.lblRelatorioVazio.Visible = false;
                    this.divExtratoDeConta.Visible = true;
                    this.rptLinhasDoRelatorio.Visible = true;

                    this.rptLinhasDoRelatorio.DataSource = new TransporteExtratoContaCorrente().TraduzirLista(lRelatorio.ListaContaCorrenteMovimento);
                    this.rptLinhasDoRelatorio.DataBind();
                }
                else
                {
                    this.tbTotaisCliente.Visible = false;
                    this.lblRelatorioVazio.Visible = true;
                    this.divExtratoDeConta.Visible = false;
                    this.rptLinhasDoRelatorio.Visible = false;
                }

                this.gTotalCliente = lRelatorio.SaldoTotal.ToString("C2", gCultureInfo);
                this.gSaldoDisponivel = lRelatorio.SaldoDisponivel.ToString("C2", gCultureInfo);
                this.gSaldoAnterior = lRelatorio.SaldoAnterior.ToString("C2", gCultureInfo);

                this.gSaldoDisponivelPosNeg =
                   this.gTotalClientePosNeg = lRelatorio.SaldoTotal > 0 ? "ValorPositivo" : "ValorNegativo";
            }
            catch (Exception ex)
            {
                this.Response.Clear();

                this.Response.Write(string.Concat("Erro: ", ex.ToString()));

                this.Response.End();
            }
        }

        #endregion
    }
}