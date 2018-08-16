using System;
using System.Globalization;
using System.Text;
using Gradual.Intranet.Www.App_Codigo;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Library.Servicos;

namespace Gradual.Intranet.Www.Intranet.Posicao
{
    public partial class Financeiro : PaginaBase
    {
        #region | Propriedades

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
                if (string.IsNullOrWhiteSpace(this.Request["NomeCliente"]))
                    return null;

                return this.Request["NomeCliente"];
            }
        }

        public string GetAcao
        {
            get 
            {
                if (string.IsNullOrEmpty(this.Request["Acao"]))
                    return string.Empty;

                return this.Request["Acao"];
            }
        }

        #endregion

        #region | Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if ("CarregarComoCSV".Equals(this.GetAcao))
            {
                this.ResponderArquivoCSV();
            }
            else
            {
                this.CarregarHtml();
            }
        }

        #endregion

        #region | Métodos

        private FinanceiroExtratoInfo CarregarRelatorio()
        {
            var lServicoAtivador = Ativador.Get<IServicoExtratos>();

            var lRespostaBusca = lServicoAtivador.ConsultarExtratoFinanceiro(new FinanceiroExtratoRequest()
            {
                ConsultaCodigoCliente = this.GetCodCliente,
                ConsultaNomeCliente = this.GetNomeCliente,
            });

            if (CriticaMensagemEnum.OK.Equals(lRespostaBusca.StatusResposta))
            {
                base.RegistrarLogConsulta(new Contratos.Dados.Cadastro.LogIntranetInfo() { CdBovespaClienteAfetado = this.GetCodCliente, DsObservacao = string.Concat("Consulta realizada para o cliente: cd_codigo = ", this.GetCodCliente) });

                return lRespostaBusca.Relatorio;
            }
            else
            {
                throw new Exception(string.Format("{0}-{1}", lRespostaBusca.StatusResposta, lRespostaBusca.StackTrace));
            }
        }

        private void ResponderArquivoCSV()
        {
            var lCultura = new CultureInfo("pt-BR");

            var lRelatorio = this.CarregarRelatorio();

            var lBuilder = new StringBuilder();

            lBuilder.AppendLine("Relatório: Saldo em Conta Corrente");

            lBuilder.AppendFormat("Cliente:;{0}-{1}\r\n", lRelatorio.CodigoCliente.ToCodigoClienteFormatado(), lRelatorio.NomeCliente.ToStringFormatoNome());

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Saldo Disponível;Saldo (R$)"); //--> Saldo Disponível

            lBuilder.AppendFormat("Valor para o Dia:;{0}\r\n", lRelatorio.SaldoDisponivel_ValorParaDia.ToString("N2"));

            lBuilder.AppendFormat("Resgate para o Dia:;{0}\r\n", lRelatorio.SaldoDisponivel_ResgateParaDia.ToString("N2"));

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Limite;Saldo (R$)"); //--> Limite

            lBuilder.AppendFormat("Limite Total de Crédito à Vista:;{0}\r\n", (null != lRelatorio.Limite_DeCreditoAVista && lRelatorio.Limite_DeCreditoAVista.HasValue) ? lRelatorio.Limite_DeCreditoAVista.Value.ToString("N", lCultura) : "0,00");

            lBuilder.AppendFormat("Limite Total de Crédito para Opções:;{0}\r\n", lRelatorio.Limite_DeCreditoParaOpcoes.ToString("N2"));

            lBuilder.AppendFormat("Total Disponível à Vista:;{0}\r\n", (null != lRelatorio.Limite_TotalDisponivelAVista && lRelatorio.Limite_TotalDisponivelAVista.HasValue) ? lRelatorio.Limite_TotalDisponivelAVista.Value.ToString("N", lCultura) : "0,00");

            lBuilder.AppendFormat("Total Disponível para Opções:;{0}\r\n", lRelatorio.Limite_TotalDisponivelParaOpcoes.ToString("N2"));

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Lançamentos Projetados para;Saldo (R$)"); //--> Lançamentos Projetados para

            lBuilder.AppendFormat("Projetado D + 1:;{0}\r\n", lRelatorio.SaldoD1.ToString("N2"));

            lBuilder.AppendFormat("Projetado D + 2:;{0}\r\n", lRelatorio.SaldoD2.ToString("N2"));

            lBuilder.AppendFormat("Projetado D + 3:;{0}\r\n", lRelatorio.SaldoD3.ToString("N2"));

            lBuilder.AppendFormat("Saldo Conta Margem:;{0}\r\n", (null != lRelatorio.SaldoContaMargem && lRelatorio.SaldoContaMargem.HasValue) ? lRelatorio.SaldoContaMargem.Value.ToString("N", lCultura) : "0,00");

            lBuilder.AppendFormat("Total Projetado:;{0}\r\n", (null != lRelatorio.SaldoProjetado && lRelatorio.SaldoProjetado.HasValue) ? lRelatorio.SaldoProjetado.Value.ToString("N", lCultura) : "0,00");

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Operações Realizadas do Dia;Saldo (R$)"); //--> Operações Realizadas do Dia

            lBuilder.AppendFormat("Compras Executadas à Vista:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_ComprasExecutadas.ToString("N", lCultura));

            lBuilder.AppendFormat("Vendas Executadas à Vista:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_VendasExecutadas.ToString("N", lCultura));

            lBuilder.AppendFormat("Total à Vista:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_TotalAVista.ToString("N", lCultura));

            lBuilder.AppendFormat("Compras Executadas de Opções:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_ComprasDeOpcoes.ToString("N", lCultura));

            lBuilder.AppendFormat("Vendas Executadas de Opções:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_VendasDeOpcoes.ToString("N", lCultura));

            lBuilder.AppendFormat("Total de Opções:;{0}\r\n", lRelatorio.OperacoesRealizadasDoDia_TotalDeOpcoes.ToString("N", lCultura));

            lBuilder.AppendLine("");

            lBuilder.AppendLine("Operações Não Realizadas do Dia;Quantidade"); //--> Operações Não Realizadas do Dia

            lBuilder.AppendFormat("Compras em Aberto:;{0}\r\n", lRelatorio.OperacoesNaoRealizadasDoDia_ComprasEmAberto.ToString("N0", lCultura));

            lBuilder.AppendFormat("Vendas em Aberto:;{0}\r\n", lRelatorio.OperacoesNaoRealizadasDoDia_VendasEmAberto.ToString("N0", lCultura));

            lBuilder.AppendFormat("Total em Aberto:;{0}\r\n", lRelatorio.OperacoesNaoRealizadasDoDia_TotalEmAberto.ToString("N0", lCultura));

            lBuilder.AppendLine("");

            lBuilder.AppendFormat("Saldo Projetado:;{0}\r\n", (null != lRelatorio.SaldoProjetado && lRelatorio.SaldoProjetado.HasValue) ? lRelatorio.SaldoProjetado.Value.ToString("C2", lCultura) : "0,00");

            lBuilder.AppendFormat("Saldo Total em Conta corrente:;{0}\r\n", lRelatorio.SaldoTotalEmContaCorrente.ToString("C2", lCultura));

            Response.Clear();

            Response.ContentType = "text/csv";

            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");

            Response.Charset = "iso-8859-1";

            Response.AddHeader("content-disposition", "attachment;filename=RelatorioFinanceiro.csv");

            Response.Write(lBuilder.ToString());

            Response.End();

        }

        private void CarregarHtml()
        {
            CultureInfo lCultura = new CultureInfo("pt-BR");
            lCultura.NumberFormat.CurrencyNegativePattern = 2; //--> Setando o padrão de exibição do negatico para currency (R$ -xxx.xxx,xx)

            var lFinanceiroExtratoInfo = this.CarregarRelatorio();

            this.lblCodigoCliente.Text = lFinanceiroExtratoInfo.CodigoCliente.ToCodigoClienteFormatado();
            this.lblNomeCliente.Text = lFinanceiroExtratoInfo.NomeCliente.ToStringFormatoNome();
            //this.UsuarioLogado.Nome = lFinanceiroExtratoInfo.NomeCliente.ToStringFormatoNome();

            {   //-> Saldo Disponível (R$)
                this.lblSaldoDisponivel_ValorParaDia.Text = lFinanceiroExtratoInfo.SaldoDisponivel_ValorParaDia.ToString("N", lCultura);
                this.lblSaldoDisponivel_ValorParaDia_PosNeg.Text = lFinanceiroExtratoInfo.SaldoDisponivel_ValorParaDia >= 0 ? "ValorPositivo" : "ValorNegativo"; ;

                this.lblSaldoDisponivel_ResgateParaDia.Text = lFinanceiroExtratoInfo.SaldoDisponivel_ResgateParaDia.ToString("N", lCultura);
                this.lblSaldoDisponivel_ResgateParaDia_PosNeg.Text = lFinanceiroExtratoInfo.SaldoDisponivel_ResgateParaDia >= 0 ? "ValorPositivo" : "ValorNegativo";
            }

            {   //--> Limite
                this.lblLimite_TotalDisponivelAVista.Text = (null != lFinanceiroExtratoInfo.Limite_TotalDisponivelAVista && lFinanceiroExtratoInfo.Limite_TotalDisponivelAVista.HasValue) ? lFinanceiroExtratoInfo.Limite_TotalDisponivelAVista.Value.ToString("N", lCultura) : "0,00";
                this.lblLimite_TotalDisponivelAVista_PosNeg.Text = lFinanceiroExtratoInfo.Limite_TotalDisponivelAVista >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLimite_TotalDisponivelParaOpcoes.Text = lFinanceiroExtratoInfo.Limite_TotalDisponivelParaOpcoes.ToString("N", lCultura);
                this.lblLimite_TotalDisponivelParaOpcoes_PosNeg.Text = lFinanceiroExtratoInfo.Limite_TotalDisponivelParaOpcoes >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLimite_DeCreditoAVista.Text = (null != lFinanceiroExtratoInfo.Limite_DeCreditoAVista && lFinanceiroExtratoInfo.Limite_DeCreditoAVista.HasValue) ? lFinanceiroExtratoInfo.Limite_DeCreditoAVista.Value.ToString("N", lCultura) : "0,00";
                this.lblLimite_DeCreditoParaOpcoes.Text = lFinanceiroExtratoInfo.Limite_DeCreditoParaOpcoes.ToString("N", lCultura);
            }

            {   //-> Lançamentos projetados para                        
                this.lblLancamento_AcoesProjetadoD1.Text = lFinanceiroExtratoInfo.SaldoD1.ToString("N", lCultura);
                this.lblLancamento_AcoesProjetadoD1_PosNeg.Text = lFinanceiroExtratoInfo.SaldoD1 >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLancamento_AcoesProjetadoD2.Text = lFinanceiroExtratoInfo.SaldoD2.ToString("N", lCultura);
                this.lblLancamento_AcoesProjetadoD2_PosNeg.Text = lFinanceiroExtratoInfo.SaldoD2 >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLancamento_AcoesProjetadoD3.Text = lFinanceiroExtratoInfo.SaldoD3.ToString("N", lCultura);
                this.lblLancamento_AcoesProjetadoD3_PosNeg.Text = lFinanceiroExtratoInfo.SaldoD3 >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLancamento_AcoesProjetadoContaMargem.Text = (null != lFinanceiroExtratoInfo.SaldoContaMargem && lFinanceiroExtratoInfo.SaldoContaMargem.HasValue) ? lFinanceiroExtratoInfo.SaldoContaMargem.Value.ToString("N", lCultura) : "0,00";
                this.lblLancamento_AcoesProjetadoContaMargem_PosNeg.Text = lFinanceiroExtratoInfo.SaldoContaMargem >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblLancamento_AcoesProjetadoTotal.Text = (null != lFinanceiroExtratoInfo.SaldoProjetado && lFinanceiroExtratoInfo.SaldoProjetado.HasValue) ? lFinanceiroExtratoInfo.SaldoProjetado.Value.ToString("N", lCultura) : "0,00";
                this.lblLancamento_AcoesProjetadoTotal_PosNeg.Text = lFinanceiroExtratoInfo.SaldoProjetado >= 0 ? "ValorPositivo" : "ValorNegativo";
            }

            {   //--> Operações Realizadas do Dia
                this.lblOperacoesRealizadasDoDia_ComprasExecutadas.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_ComprasExecutadas.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_VendasExecutadas.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_VendasExecutadas.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_TotalAVista.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_TotalAVista.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_TotalAVista_PosNeg.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_TotalAVista >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblOperacoesRealizadasDoDia_ComprasDeOpcoes.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_ComprasDeOpcoes.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_VendasDeOpcoes.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_VendasDeOpcoes.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_TotalDeOpcoes.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_TotalDeOpcoes.ToString("N", lCultura);
                this.lblOperacoesRealizadasDoDia_TotalDeOpcoes_PosNeg.Text = lFinanceiroExtratoInfo.OperacoesRealizadasDoDia_TotalDeOpcoes >= 0 ? "ValorPositivo" : "ValorNegativo";
            }

            {   //--> Operações Não Realizadas do Dia
                this.lblOperacoesNaoRealizadasDoDia_ComprasEmAberto.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_ComprasEmAberto.ToString("N0", lCultura);
                this.lblOperacoesNaoRealizadasDoDia_ComprasEmAberto_PosNeg.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_ComprasEmAberto >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblOperacoesNaoRealizadasDoDia_VendasEmAberto.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_VendasEmAberto.ToString("N0", lCultura);
                this.lblOperacoesNaoRealizadasDoDia_VendasEmAberto_PosNeg.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_VendasEmAberto >= 0 ? "ValorPositivo" : "ValorNegativo";

                this.lblOperacoesNaoRealizadasDoDia_TotalEmAberto.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_TotalEmAberto.ToString("N0", lCultura);
                this.lblOperacoesNaoRealizadasDoDia_TotalEmAberto_PosNeg.Text = lFinanceiroExtratoInfo.OperacoesNaoRealizadasDoDia_TotalEmAberto >= 0 ? "ValorPositivo" : "ValorNegativo";
            }

            this.lblSaldoTotalEmContaCorrente.Text = lFinanceiroExtratoInfo.SaldoTotalEmContaCorrente.ToString("C2", lCultura);
            this.lblSaldoTotalEmContaCorrente_PosNeg.Text = lFinanceiroExtratoInfo.SaldoTotalEmContaCorrente >= 0 ? "ValorPositivo" : "ValorNegativo";

            this.lblSaldoProjetado.Text = (null != lFinanceiroExtratoInfo.SaldoProjetado && lFinanceiroExtratoInfo.SaldoProjetado.HasValue) ? lFinanceiroExtratoInfo.SaldoProjetado.Value.ToString("C2", lCultura) : "0,00";
            this.lblSaldoProjetado_PosNeg.Text = lFinanceiroExtratoInfo.SaldoProjetado >= 0 ? "ValorPositivo" : "ValorNegativo";
        }

        #endregion
    }
}