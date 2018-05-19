using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.ContaCorrente.Lib;
using Gradual.OMS.ContaCorrente.Lib.Enum;
using Gradual.OMS.ContaCorrente.Lib.Info;
using Gradual.OMS.ContaCorrente.Lib.Info.Enum;
using Gradual.OMS.ContaCorrente.Lib.Mensageria;
using Gradual.OMS.Risco.Persistencia.Lib;
using log4net;

namespace Gradual.OMS.ContaCorrente
{
    public class ServicoExtratos : IServicoExtratos
    {
        #region | Atributos

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Métodos de relatórios

        /// <summary>
        /// Realiza a consulta do extrato de conta corrente de um cliente.
        /// </summary>
        /// <param name="pParametros">Data início/fim da pesquisa, código do cliente no Sinacor e nome do cliente.</param>
        /// <returns>Lista com dados do extrato do cliente.</returns>
        public ContaCorrenteExtratoResponse ConsultarExtratoContaCorrente(ContaCorrenteExtratoRequest pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lDataTable = new DataTable();
            var lRetorno = new ContaCorrenteExtratoResponse();
            var lContaCorrenteMovimentoRelInfo = new ContaCorrenteMovimentoInfo();

            decimal lSaldoDAnterior, lSaldoDisponivel, lSaldoTotal, lLancamento;

            try
            {
                gLogger.Debug(String.Format("Inicio ConsultarExtratoContaCorrente. Cliente: {0}", pParametros.ConsultaCodigoCliente));

                if (pParametros.ConsultaCodigoCliente == 0)
                    return null;

                lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "SINACOR";

                var lProcedure = EnumTipoExtradoDeConta.Liquidacao.Equals(pParametros.ConsultaTipoExtratoDeConta) ? "PRC_EXTRATO_CC_LIQUI_SEL" : "PRC_EXTRATO_CC_MOVTO_SEL";

                using (DbCommand lComando = lAcessaDados.CreateCommand(CommandType.StoredProcedure, lProcedure))
                {
                    lAcessaDados.AddInParameter(lComando, "pcd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                    lAcessaDados.AddInParameter(lComando, "pdt_inicio", DbType.DateTime, pParametros.ConsultaDataInicio);
                    lAcessaDados.AddInParameter(lComando, "pdt_fim", DbType.DateTime, pParametros.ConsultaDataFim);

                    lAcessaDados.AddOutParameter(lComando, "psaldo_d_anterior", DbType.VarNumeric, 4);
                    lAcessaDados.AddOutParameter(lComando, "psaldo_disponivel", DbType.VarNumeric, 4);
                    lAcessaDados.AddOutParameter(lComando, "psaldo_total", DbType.VarNumeric, 4);

                    lDataTable = lAcessaDados.ExecuteOracleDataTable(lComando);

                    lSaldoDAnterior = Convert.ToDecimal(lAcessaDados.GetParameterValue(lComando, "psaldo_d_anterior"));
                    lSaldoDisponivel = Convert.ToDecimal(lAcessaDados.GetParameterValue(lComando, "psaldo_disponivel"));
                    lSaldoTotal = Convert.ToDecimal(lAcessaDados.GetParameterValue(lComando, "psaldo_total"));
                }

                lLancamento = 0;

                lRetorno.Relatorio.CodigoCliente = pParametros.ConsultaCodigoCliente.Value;
                lRetorno.Relatorio.NomeCliente = BuscarNomeDoClienteSeNecessario(pParametros.ConsultaCodigoCliente, pParametros.ConsultaNomeCliente); // pParametros.CodigoCliente.ToString();     //TODO: De onde vem o nome do cliente?

                lRetorno.Relatorio.SaldoAnterior = lSaldoDAnterior;
                lRetorno.Relatorio.SaldoDisponivel = lSaldoDisponivel;
                lRetorno.Relatorio.SaldoTotal = lSaldoTotal;

                foreach (DataRow lRow in lDataTable.Rows)
                {
                    lContaCorrenteMovimentoRelInfo = new ContaCorrenteMovimentoInfo();

                    if (!Convert.IsDBNull(lRow["Credito"]))
                    {
                        lLancamento = Convert.ToDecimal(lRow["Credito"]);

                        lContaCorrenteMovimentoRelInfo.ValorCredito = lLancamento;

                        lSaldoDAnterior += lLancamento;
                    }
                    else
                    {
                        lLancamento = (Math.Abs(Convert.ToDecimal(lRow["Debito"])) * -1);

                        lContaCorrenteMovimentoRelInfo.ValorDebito = lLancamento;

                        lSaldoDAnterior += lLancamento;
                    }

                    lContaCorrenteMovimentoRelInfo.DataMovimento = lRow["dt_lancamento"].DBToDateTime();

                    lContaCorrenteMovimentoRelInfo.DataLiquidacao = lRow["dt_liquidacao"].DBToDateTime();

                    lContaCorrenteMovimentoRelInfo.ValorSaldo = lSaldoDAnterior;

                    lContaCorrenteMovimentoRelInfo.Historico = lRow["ds_lancamento"].ToString();

                    //lSaldoDAnterior += lLancamento;

                    lRetorno.Relatorio.ListaContaCorrenteMovimento.Add(lContaCorrenteMovimentoRelInfo);
                }

                lRetorno.StatusResposta = CriticaMensagemEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarExtratoContaCorrente", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
            }
            finally
            {
                lAcessaDados = null;
                lDataTable.Dispose();
                lDataTable = null;
            }

            gLogger.Debug(String.Format("Fim ConsultarExtratoContaCorrente. Cliente: {0}. Descrição Resposta: {1}, count: {2}, consulta de {3} até {4}", pParametros.ConsultaCodigoCliente, lRetorno.StatusResposta, lRetorno.Relatorio.ListaContaCorrenteMovimento.Count.ToString(), pParametros.ConsultaDataInicio.ToString(), pParametros.ConsultaDataFim.ToString()));

            return lRetorno;
        }

        /// <summary>
        /// Realiza a consulta do extrado de custódia de um cliente.
        /// </summary>
        /// <param name="pParametros">Código do Cliente, Mercado, código de negócio.</param>
        /// <returns>Lista com dados do extrato do cliente.</returns>
        public CustodiaExtratoResponse ConsultarExtratoCustodia(CustodiaExtratoRequest pParametros)
        {
            var lRetorno = new CustodiaExtratoResponse();
            var lCustodiaExtratoInfo = new CustodiaExtratoInfo();
            var lAcessaDados = new AcessaDados();
            var lAtivos = string.Empty;

            try
            {
                lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "SINACOR";

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CUSTODIA_ANALITICA"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCD_ASSESOR", DbType.Int32, System.DBNull.Value);

                    lAcessaDados.AddInParameter(lDbCommand, "pCD_CLIENTE", DbType.Int32, pParametros.ConsultaCodigoCliente);

                    lAcessaDados.AddInParameter(lDbCommand, "pTP_MERCADO", DbType.AnsiString, pParametros.ConsultaTipoMercado);

                    lAcessaDados.AddInParameter(lDbCommand, "pCD_NEGOCIO", DbType.AnsiString, pParametros.ConsultaBolsa);

                    using (DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand))
                    {
                        lRetorno.Relatorio.CodigoCliente = pParametros.ConsultaCodigoCliente;
                        lRetorno.Relatorio.NomeCliente = BuscarNomeDoClienteSeNecessario(pParametros.ConsultaCodigoCliente, pParametros.ConsultaNomeCliente);

                        foreach (DataRow lRow in lDataTable.Rows)
                        {
                            lCustodiaExtratoInfo = new CustodiaExtratoInfo();

                            lCustodiaExtratoInfo.Ativo = lRow["cod_neg"].DBToString();
                            lCustodiaExtratoInfo.CodigoCarteira = lRow["cod_cart"].DBToInt32();
                            lCustodiaExtratoInfo.DescricaoCarteira = lRow["DESC_CART"].DBToString();
                            lCustodiaExtratoInfo.NomeEmpresa = lRow["NOME_EMP_EMI"].DBToString();
                            lCustodiaExtratoInfo.Compra = lRow["qtde_exec_cpa"].DBToDecimal();
                            lCustodiaExtratoInfo.Mercado = lRow["tipo_merc"].DBToString();
                            lCustodiaExtratoInfo.Quantidade = lRow["qtde_disp"].DBToInt32();
                            lCustodiaExtratoInfo.QuantidadeAtual = lRow["qtde_atual"].DBToInt32();
                            lCustodiaExtratoInfo.Venda = lRow["qtde_exec_vda"].DBToDecimal();
                            lCustodiaExtratoInfo.FechamentoAnterior = lRow["prec_med"].DBToDecimal();

                            if (!string.IsNullOrEmpty(lCustodiaExtratoInfo.Ativo))
                                lAtivos += string.Format("'{0}',", lCustodiaExtratoInfo.Ativo.Trim());

                            lRetorno.Relatorio.ListaCustodiaExtratoInfo.Add(lCustodiaExtratoInfo);
                        }

                        lRetorno.Relatorio.ListaCustodiaExtratoInfo.Sort(delegate(CustodiaExtratoInfo cc1, CustodiaExtratoInfo cc2) { return Comparer<string>.Default.Compare(cc1.DescricaoCarteira, cc2.DescricaoCarteira); });
                    }

                    lAtivos = lAtivos.TrimEnd(',').Trim();
                }

                if (!string.IsNullOrEmpty(lAtivos))
                {
                    lAcessaDados = new AcessaDados();
                    lAcessaDados.ConnectionStringName = "OMS";

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TB_ATIVO_BUSCA_MULTIPLA"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "@WhereCd_NegociacaoIN", DbType.String, lAtivos);

                        using (DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand))
                        {
                            foreach (DataRow lRow in lDataTable.Rows)
                            {
                                lRetorno.Relatorio.ListaCustodiaExtratoInfo.ForEach(delegate(CustodiaExtratoInfo cei)
                                {
                                    if (cei.Ativo.ToUpper().Trim() == lRow["cd_negociacao"].DBToString().ToUpper().Trim())
                                    {
                                        cei.Abertura = lRow["vl_abertura"].DBToDecimal();
                                        cei.ValorAtual = cei.Quantidade * cei.FechamentoAnterior;
                                    }
                                });
                            }
                        }
                    }
                }

                lRetorno.Relatorio.ListaCustodiaExtratoInfo.ForEach(delegate(CustodiaExtratoInfo cei)
                {
                    switch (cei.Mercado)
                    {
                        case "VIS": lRetorno.Relatorio.SubTotalMercadoAVista += cei.ValorAtual; break;
                        case "TER": lRetorno.Relatorio.SubTotalMercadoTermo += cei.ValorAtual; break;
                        case "OPC": lRetorno.Relatorio.SubTotalMercadoDeOpcoes += cei.ValorAtual; break;
                        case "OPV": lRetorno.Relatorio.SubTotalMercadoDeOpcoes += cei.ValorAtual; break;
                        case "FUT": lRetorno.Relatorio.SubTotalMercadoFuturo += cei.ValorAtual; break;
                    }

                    lRetorno.Relatorio.Total = lRetorno.Relatorio.SubTotalMercadoAVista
                                             + lRetorno.Relatorio.SubTotalMercadoTermo
                                             + lRetorno.Relatorio.SubTotalMercadoDeOpcoes
                                             + lRetorno.Relatorio.SubTotalMercadoFuturo;
                });

                lRetorno.StatusResposta = CriticaMensagemEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarExtratoCustodia", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
            }
            finally
            {
                lAcessaDados = null;
            }

            return lRetorno;
        }

        /// <summary>
        /// Raliza a consulta de Nota de Corretagem de um cliente.
        /// </summary>
        /// <param name="pParametros">Código do cliente no Sinacor.</param>
        /// <returns>Lista com dados do extrato do cliente.</returns>
        public NotaDeCorretagemExtratoResponse ConsultarNotaDeCorretagem(NotaDeCorretagemExtratoRequest pParametros)
        {
            var lRetorno = new NotaDeCorretagemExtratoResponse();
            var lAcessaDados = new AcessaDados();
            
            log4net.Config.XmlConfigurator.Configure();
            try
            {
                lRetorno.Relatorio.CodigoCliente = pParametros.ConsultaCodigoCliente;

                lAcessaDados.ConnectionStringName = "SINACOR";

                BuscarNotaDeCorretagem_CabecalhoDoCliente(ref pParametros, ref lAcessaDados, ref lRetorno);

                BuscarNotaDeCorretagem_CabecalhoDaCorretora(ref pParametros, ref lAcessaDados, ref lRetorno);

                BuscarNotaDeCorretagem_ListaDeNegocios(ref pParametros, ref lAcessaDados, ref lRetorno);

                BuscarNotaDeCorretagem_Rodape(ref pParametros, ref lAcessaDados, ref lRetorno);

                lRetorno.StatusResposta = CriticaMensagemEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarNotaDeCorretagem", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
            }
            finally
            {
                lAcessaDados = null;
            }

            return lRetorno;
        }

        /// <summary>
        /// Realiza a consulta da movimentação financeira de um cliente.
        /// </summary>
        /// <param name="pParametros">Código do cliente no Sinacor.</param>
        /// <returns>Lista com dados do extrato do cliente.</returns>
        public FinanceiroExtratoResponse ConsultarExtratoFinanceiro(FinanceiroExtratoRequest pParametros)
        {
            var lAcessaDados = new AcessaDados();
            var lRetorno = new FinanceiroExtratoResponse();
            string lTipoMercado, lTipoOperacao;

            try
            {
                var lDadosDeSaldo = this.BuscarSaldoEmContaNoServico(pParametros.ConsultaCodigoCliente);
                var lContaMargem = (null != lDadosDeSaldo.SaldoContaMargem && lDadosDeSaldo.SaldoContaMargem.HasValue) ? lDadosDeSaldo.SaldoContaMargem.Value : 0;

                { //--> Saldo Disponível (R$)
                    lRetorno.Relatorio.SaldoDisponivel_EmConta =
                        lRetorno.Relatorio.SaldoDisponivel_Total =
                        lRetorno.Relatorio.SaldoDisponivel_ValorParaDia = lDadosDeSaldo.SaldoD0;


                    lRetorno.Relatorio.SaldoDisponivel_ResgateParaDia = lDadosDeSaldo.SaldoD0 > 0 ? lDadosDeSaldo.SaldoD0 : 0;

                    lRetorno.Relatorio.SaldoTotalEmContaCorrente = lDadosDeSaldo.SaldoD0
                                                                 + lDadosDeSaldo.SaldoD1
                                                                 + lDadosDeSaldo.SaldoD2
                                                                 + lDadosDeSaldo.SaldoD3
                                                                 + lContaMargem
                                                                 + lDadosDeSaldo.LimiteOperacioalDisponivelOpcao
                                                                 + lDadosDeSaldo.LimiteOperacioalDisponivelAVista;
                }

                { //--> Limite
                    lRetorno.Relatorio.Limite_DeCreditoParaOpcoes = lDadosDeSaldo.LimiteOperacioalTotalOpcao;
                    lRetorno.Relatorio.Limite_DeCreditoAVista = lDadosDeSaldo.LimiteOperacioalTotalAVista;

                    lRetorno.Relatorio.Limite_TotalDisponivelParaOpcoes = lDadosDeSaldo.LimiteOperacioalDisponivelOpcao;

                    lRetorno.Relatorio.Limite_TotalDisponivelAVista = lDadosDeSaldo.LimiteOperacioalDisponivelAVista;
                }

                {   //--> Saldo Projetado:
                    lRetorno.Relatorio.SaldoD0 = lDadosDeSaldo.SaldoD0;
                    lRetorno.Relatorio.SaldoD1 = lDadosDeSaldo.SaldoD1;
                    lRetorno.Relatorio.SaldoD2 = lDadosDeSaldo.SaldoD2;
                    lRetorno.Relatorio.SaldoD3 = lDadosDeSaldo.SaldoD3;
                    lRetorno.Relatorio.SaldoContaMargem = lContaMargem;
                    lRetorno.Relatorio.SaldoProjetado = lRetorno.Relatorio.SaldoD1
                                                      + lRetorno.Relatorio.SaldoD2
                                                      + lRetorno.Relatorio.SaldoD3
                                                      + lContaMargem;
                }

                lAcessaDados = new AcessaDados();
                lAcessaDados.ConnectionStringName = "SINACOR";

                {   //--> Saldo do dia:
                    //lComando = lDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_DISPONIVEL_DIA_SEL");

                    //lDados.AddInParameter(lComando, "cd_cliente", DbType.Int32, pParametros.CodigoCliente);

                    //lTabela = lDados.ExecuteOracleDataTable(lComando);

                    //if (lTabela.Rows.Count > 0)
                    //{
                    //    lRetorno.Relatorio.SaldoDisponivel_EmConta = lTabela.Rows[0]["SALDO"].DBToDecimal();
                    //    lRetorno.Relatorio.SaldoDisponivel_ResgateParaDia = lTabela.Rows[0]["VALORRESGATE"].DBToDecimal();
                    //    lRetorno.Relatorio.SaldoDisponivel_ValorParaDia = lTabela.Rows[0]["VALORDIA"].DBToDecimal();
                    //}
                }

                {   //--> Operações realizadas no dia:
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OPERACOES_EXEC_DIA_SEL"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);

                        using (var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand))
                        {
                            var lValor = default(decimal);

                            foreach (DataRow lRow in lDataTable.Rows)
                            {
                                lTipoMercado = lRow["TIPOMERCADO"].DBToString().ToUpper();
                                lTipoOperacao = lRow["TIPOOPERACAO"].DBToString().ToUpper();

                                lValor = lRow["TOTAL"].DBToDecimal();

                                switch (lTipoMercado)
                                {
                                    case "FRA":
                                    case "VIS":
                                        if (lTipoOperacao == "C") lRetorno.Relatorio.OperacoesRealizadasDoDia_ComprasExecutadas = lValor;
                                        if (lTipoOperacao == "V") lRetorno.Relatorio.OperacoesRealizadasDoDia_VendasExecutadas = lValor;
                                        break;
                                    case "OPC":
                                        if (lTipoOperacao == "C") lRetorno.Relatorio.OperacoesRealizadasDoDia_ComprasDeOpcoes = lValor;
                                        if (lTipoOperacao == "V") lRetorno.Relatorio.OperacoesRealizadasDoDia_VendasDeOpcoes = lValor;
                                        break;
                                    case "OPV":
                                        if (lTipoOperacao == "C") lRetorno.Relatorio.OperacoesRealizadasDoDia_ComprasDeOpcoes = lValor;
                                        if (lTipoOperacao == "V") lRetorno.Relatorio.OperacoesRealizadasDoDia_VendasDeOpcoes = lValor;
                                        break;
                                }
                            }
                        }
                    }

                    lRetorno.Relatorio.OperacoesRealizadasDoDia_TotalAVista   = lRetorno.Relatorio.OperacoesRealizadasDoDia_VendasExecutadas - lRetorno.Relatorio.OperacoesRealizadasDoDia_ComprasExecutadas;
                    lRetorno.Relatorio.OperacoesRealizadasDoDia_TotalDeOpcoes = lRetorno.Relatorio.OperacoesRealizadasDoDia_VendasDeOpcoes   - lRetorno.Relatorio.OperacoesRealizadasDoDia_ComprasDeOpcoes;
                }

                {   //--> Operações não realizadas no dia:
                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OPERACOES_ABERT_DIA_SEL"))
                    {
                        lAcessaDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);

                        using (var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand))
                        {
                            if (null != lDataTable && lDataTable.Rows.Count > 0)
                            {
                                lTipoOperacao = lDataTable.Rows[0]["qtdVenda"].DBToString().ToUpper();
                                lRetorno.Relatorio.OperacoesNaoRealizadasDoDia_ComprasEmAberto = lDataTable.Rows[0]["qtdCompra"].DBToDecimal();
                                lRetorno.Relatorio.OperacoesNaoRealizadasDoDia_VendasEmAberto = lDataTable.Rows[0]["qtdVenda"].DBToDecimal();
                                lRetorno.Relatorio.OperacoesNaoRealizadasDoDia_TotalEmAberto = lDataTable.Rows[0]["Total"].DBToDecimal();
                            }
                        }
                    }
                }

                lRetorno.Relatorio.NomeCliente = BuscarNomeDoClienteSeNecessario(pParametros.ConsultaCodigoCliente, pParametros.ConsultaNomeCliente);
                lRetorno.Relatorio.CodigoCliente = pParametros.ConsultaCodigoCliente;

                lRetorno.StatusResposta = CriticaMensagemEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarExtratoFinanceiro", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StackTrace = ex.StackTrace;
                lRetorno.StatusResposta = CriticaMensagemEnum.Exception;
            }
            finally
            {
                lAcessaDados = null;
            }

             return lRetorno;
        }

        #endregion

        #region | Métodos de apoio

        /// <summary>
        /// Realiza a consulta do nome do cliente com base no seu código cliente do sinacor.
        /// </summary>
        private string BuscarNomeDoClienteSeNecessario(int? pCodigoCliente, string pNomeCliente)
        {
            if (string.IsNullOrEmpty(pNomeCliente))
            {
                return string.Empty;
                //ServicoCliente lServico = new ServicoCliente();

                //return lServico.BuscarDadosDoCliente(new BuscarDadosDoClienteRequest() { CodigoCliente = pRequest.CodigoCliente }).DadosDoCliente.Nomecliente;
            }
            else
            {
                return pNomeCliente;
            }
        }

        private void BuscarNotaDeCorretagem_CabecalhoDoCliente(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse lRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "prc_NC_DadosFinanceiro_Sel"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "dt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);
                pAcessoDados.AddInParameter(lDbCommand, "cd_empresa", DbType.Int32, pParametros.ConsultaCodigoCorretora);
                pAcessoDados.AddInParameter(lDbCommand, "tp_mercado", DbType.String, pParametros.ConsultaTipoDeMercado);

                if (pParametros.ConsultaProvisorio)
                    pAcessoDados.AddInParameter(lDbCommand, "in_broker", DbType.String, "C");

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.Relatorio.CabecalhoCliente.Agencia            = lDataTable.Rows[0]["DS_AGENCIA"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.ContaCorrente      = lDataTable.Rows[0]["NR_CTACORR"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.NrBanco            = lDataTable.Rows[0]["CD_BANCO"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.NrNota             = lDataTable.Rows[0]["NR_NOTA"].DBToInt32();
                        lRetorno.Relatorio.CabecalhoCliente.EnderecoCliente    = lDataTable.Rows[0]["endereco_cli"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.CepCidadeUFCliente = lDataTable.Rows[0]["cep_cidd_uf_cli"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.CodCliente         = lDataTable.Rows[0]["CD_CLIENTE_CC"].DBToInt32();
                        lRetorno.Relatorio.CabecalhoCliente.CpfCnpj            = lDataTable.Rows[0]["CD_CPFCGC"].DBToString();
                        lRetorno.Relatorio.CabecalhoCliente.DvCliente          = lDataTable.Rows[0]["DV_CLIENTE_ORI"].DBToInt32();
                        lRetorno.Relatorio.CabecalhoCliente.NomeCliente        = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                    }
                }
            }
        }

        private void BuscarNotaDeCorretagem_CabecalhoDaCorretora(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse lRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "prc_NC_DadosFinanceiro_Sel"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "dt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);
                pAcessoDados.AddInParameter(lDbCommand, "cd_empresa", DbType.Int32, pParametros.ConsultaCodigoCorretora);
                pAcessoDados.AddInParameter(lDbCommand, "tp_mercado", DbType.String, pParametros.ConsultaTipoDeMercado);

                if (pParametros.ConsultaProvisorio)
                    pAcessoDados.AddInParameter(lDbCommand, "in_broker", DbType.String, "C");

                using (DataTable lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.Relatorio.CabecalhoCorretora.CpfCnpj = lDataTable.Rows[0]["CD_CGC"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.DataPregao = lDataTable.Rows[0]["DT_NEGOCIO"].DBToDateTime();
                        lRetorno.Relatorio.CabecalhoCorretora.EmailCorretora = lDataTable.Rows[0]["CD_EMAIL"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.EnderecoCorretora = lDataTable.Rows[0]["ENDERECO_CORR"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.NomeCorretora = lDataTable.Rows[0]["DS_RSOC_LOC"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.NumeroNota = lDataTable.Rows[0]["NR_NOTA"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.SiteCorretora = lDataTable.Rows[0]["CD_INTERNET"].DBToString();
                        lRetorno.Relatorio.CabecalhoCorretora.TelefonesCorretora = lDataTable.Rows[0]["TELEFONES_CORR"].DBToString();
                    }
                }
            }
        }

        private void BuscarNotaDeCorretagem_ListaDeNegocios(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse lRetorno)
        {
            DataTable lTabela = null;
            DbCommand lComando = null;

            bool lDadosHistoricos = DateTime.Today.Subtract(pParametros.ConsultaDataMovimento).Days > 14;
            var lProc_NotaCorretagem = lDadosHistoricos ? "prc_NC_NegociosRealizados_Sel" : "prc_NC_Negociacao_Sel";
            var lNotaDeCorretagemExtratoInfo = new NotaDeCorretagemExtratoInfo();
            
            lComando = pAcessoDados.CreateCommand(CommandType.StoredProcedure, lProc_NotaCorretagem);

            pAcessoDados.AddInParameter(lComando, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
            pAcessoDados.AddInParameter(lComando, "dt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);
            pAcessoDados.AddInParameter(lComando, "dt_negocio", DbType.Date, pParametros.ConsultaDataMovimento);
            pAcessoDados.AddInParameter(lComando, "tp_mercado", DbType.String, pParametros.ConsultaTipoDeMercado);

            if (pParametros.ConsultaProvisorio && !lDadosHistoricos)
                pAcessoDados.AddInParameter(lComando, "in_broker", DbType.String, "C");

            lTabela = pAcessoDados.ExecuteOracleDataTable(lComando);

            foreach (DataRow dr in lTabela.Rows)
            {
                lNotaDeCorretagemExtratoInfo = new NotaDeCorretagemExtratoInfo();

                lNotaDeCorretagemExtratoInfo.EspecificacaoTitulo = dr["cd_especif"].DBToString();
                lNotaDeCorretagemExtratoInfo.NomeBolsa = dr["nm_resu_bolsa"].DBToString();
                lNotaDeCorretagemExtratoInfo.NomeEmpresa = dr["nm_nomres"].ToString();
                lNotaDeCorretagemExtratoInfo.Observacao = dr["in_negocio"].ToString();
                lNotaDeCorretagemExtratoInfo.Quantidade = dr["qt_qtdesp"].DBToInt32();
                lNotaDeCorretagemExtratoInfo.TipoMercado = dr["ds_mercado"].ToString();
                lNotaDeCorretagemExtratoInfo.TipoOperacao = dr["cd_natope"].ToString();
                lNotaDeCorretagemExtratoInfo.ValorNegocio = dr["vl_negocio"].DBToDecimal();
                lNotaDeCorretagemExtratoInfo.ValorTotal = dr["vl_totneg"].DBToDecimal();
                lNotaDeCorretagemExtratoInfo.CodigoCliente = pParametros.ConsultaCodigoCliente;
                lNotaDeCorretagemExtratoInfo.CodigoNegocio = dr["cd_negocio"].DBToString();
                lNotaDeCorretagemExtratoInfo.DC = dr["cd_natope"].DBToString().Equals("C") ? "D" : "C";

                lRetorno.Relatorio.ListaNotaDeCorretagemExtratoInfo.Add(lNotaDeCorretagemExtratoInfo);
            }
        }

        private void BuscarNotaDeCorretagem_Rodape(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse lRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "prc_NC_DadosFinanceiro_Sel"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "dt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);
                pAcessoDados.AddInParameter(lDbCommand, "cd_empresa", DbType.Int32, pParametros.ConsultaCodigoCorretora);
                pAcessoDados.AddInParameter(lDbCommand, "tp_mercado", DbType.String, pParametros.ConsultaTipoDeMercado);

                if (pParametros.ConsultaProvisorio)
                    pAcessoDados.AddInParameter(lDbCommand, "in_broker", DbType.String, "C");

                using (DataTable lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        lRetorno.Relatorio.Rodape.CompraOpcoes = lDataTable.Rows[0]["VL_CPAOPC"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.CompraVista = lDataTable.Rows[0]["VL_CPAVIS"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.VendaOpcoes = lDataTable.Rows[0]["VL_VDAOPC"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.VendaVista = lDataTable.Rows[0]["VL_VDAVIS"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.Debentures = lDataTable.Rows[0]["VL_DEBENT"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.OperacoesTermo = lDataTable.Rows[0]["VL_TERMO"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.OperacoesFuturo = lDataTable.Rows[0]["VL_FUTURO"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.OperacoesTitulosPublicos = lDataTable.Rows[0]["VL_TITPUB"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.ValorDasOperacoes = lDataTable.Rows[0]["VL_TOTNEG"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.ValorAjusteFuturo = lDataTable.Rows[0]["VL_FUTURO"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.IRSobreCorretagem = lDataTable.Rows[0]["VL_IRCORR"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.IRRFSobreDayTrade = lDataTable.Rows[0]["VL_IRRF_DESPESA"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.ValorLiquidoOperacoes = lDataTable.Rows[0]["VL_LIQ_OPERACOES"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.TaxaLiquidacao = lDataTable.Rows[0]["VL_EMOLUM_CB"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.TaxaLiquidacao_DC = lDataTable.Rows[0]["VL_EMOLUM_CB_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.TaxaDeRegistro = lDataTable.Rows[0]["VL_TAXREG_CB"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.TaxaDeRegistro_DC = lDataTable.Rows[0]["VL_TAXREG_CB_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.Total_123_A = lDataTable.Rows[0]["VL_LIQNOT_CB"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.Total_123_A_DC = lDataTable.Rows[0]["VL_LIQNOT_CB_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.TaxaTerOpcFut = lDataTable.Rows[0]["VL_TAXPTA"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.TaxaTerOpcFut_DC = lDataTable.Rows[0]["vl_taxpta_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.TaxaANA = lDataTable.Rows[0]["VL_TAXANA"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.TaxaANA_DC = lDataTable.Rows[0]["VL_TAXANA_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.Emolumentos = lDataTable.Rows[0]["VL_EMOLUM_BV"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.Emolumentos_DC = lDataTable.Rows[0]["VL_EMOLUM_BV_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.Corretagem = lDataTable.Rows[0]["VL_VALCOR"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.Corretagem_DC = (lDataTable.Rows[0]["VL_VALCOR"].DBToDecimal() > 0 ? "C" : "D");
                        lRetorno.Relatorio.Rodape.ISS = lDataTable.Rows[0]["VL_ISS"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.ISS_DC = "D"; //lDataTable.Rows[0]["VL_ISS_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.IRRFOperacoes = lDataTable.Rows[0]["VL_IROPER"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.IRRFOperacoes_DC = (lDataTable.Rows[0]["VL_IROPER"].DBToDecimal() > 0 ? "C" : "D");
                        lRetorno.Relatorio.Rodape.VLBaseOperacoesIRRF = lDataTable.Rows[0]["VL_BASEIROPER"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.VLBaseOperacoesIRRF_DC = lDataTable.Rows[0]["VL_IROPER_CD"].DBToString();
                        lRetorno.Relatorio.Rodape.ValorLiquidoNota = lDataTable.Rows[0]["VL_LIQNOT"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.Outras = lDataTable.Rows[0]["VL_DESP_NOTA"].DBToDecimal();
                        lRetorno.Relatorio.Rodape.DescOutras = lDataTable.Rows[0]["DS_DESP_NOTA"].DBToString();
                    }
                }
            }
        }

        private ContaCorrenteInfo BuscarSaldoEmContaNoServico(int pCodigoCliente)
        {
            var lRetorno = new SaldoContaCorrenteResponse<ContaCorrenteInfo>();

            lRetorno = new ServicoContaCorrente().ObterSaldoContaCorrente(new SaldoContaCorrenteRequest() { IdCliente = pCodigoCliente });

            if (CriticaMensagemEnum.OK.Equals(lRetorno.StatusResposta))
                return lRetorno.Objeto;
            else
                return new ContaCorrenteInfo();
        }
        
        #endregion
    }
}
