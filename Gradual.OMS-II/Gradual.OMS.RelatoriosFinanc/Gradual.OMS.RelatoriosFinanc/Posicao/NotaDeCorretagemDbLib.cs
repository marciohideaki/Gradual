using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.OMS.Library;
using Gradual.OMS.RelatoriosFinanc.Lib.Dados;
using Gradual.OMS.RelatoriosFinanc.Lib.Mensagens;
using log4net;

namespace Gradual.OMS.RelatoriosFinanc.Posicao
{
    public class NotaDeCorretagemDbLib
    {
        #region | Atributos

        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static List<DateTime> gDataFeriados = new List<DateTime>();

        #endregion

        #region | Propriedades

        private List<DateTime> GetDataFeriados
        {
            get
            {
                if (gDataFeriados.Count == 0 || gDataFeriados[gDataFeriados.Count - 1].Year != DateTime.Today.Year)
                {
                    var lAcessaDados = new AcessaDados();
                    lAcessaDados.ConnectionStringName = "Sinacor";

                    using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_feriado_por_periodo"))
                    {
                        gDataFeriados = new List<DateTime>();

                        lAcessaDados.AddInParameter(lDbCommand, "pDtInicio", DbType.Date, new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1));
                        lAcessaDados.AddInParameter(lDbCommand, "pDtFim", DbType.Date, new DateTime(DateTime.Now.AddYears(1).Year, 1, 1));

                        var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                        gDataFeriados.Clear();

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                            foreach (DataRow lLinha in lDataTable.Rows)
                                gDataFeriados.Add(lLinha["DT_FERIADO"].DBToDateTime());
                    }
                }

                return gDataFeriados;
            }
        }

        #endregion

        #region | Métodos

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

                this.BuscarNotaDeCorretagem_CabecalhoDoCliente(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_CabecalhoDaCorretora(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_ListaDeNegocios(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_Rodape(ref pParametros, ref lAcessaDados, ref lRetorno);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarNotaDeCorretagem", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }
            finally
            {
                lAcessaDados = null;
            }

            return lRetorno;
        }

        public NotaDeCorretagemExtratoResponse ConsultarNotaDeCorretagemBmf(NotaDeCorretagemExtratoRequest pParametros)
        {
            var lRetorno = new NotaDeCorretagemExtratoResponse();
            var lAcessaDados = new AcessaDados();

            log4net.Config.XmlConfigurator.Configure();
            try
            {
                lRetorno.RelatorioBmf.CodigoClienteBmf = pParametros.ConsultaCodigoCliente;

                lAcessaDados.ConnectionStringName = "SINACOR";

                this.BuscarNotaDeCorretagem_CabecalhoDoCliente(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_CabecalhoDaCorretora(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_ListaDeNegociosBmf(ref pParametros, ref lAcessaDados, ref lRetorno);

                this.BuscarNotaDeCorretagem_RodapeBmf(ref pParametros, ref lAcessaDados, ref lRetorno);

                lRetorno.StatusResposta = MensagemResponseStatusEnum.OK;
            }
            catch (Exception ex)
            {
                gLogger.Error("ConsultarNotaDeCorretagem", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta = MensagemResponseStatusEnum.ErroNegocio;
            }
            finally
            {
                lAcessaDados = null;
            }

            return lRetorno;
        }

        private void BuscarNotaDeCorretagem_CabecalhoDoCliente(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse plRetorno)
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
                        plRetorno.Relatorio.CabecalhoCliente.Agencia = lDataTable.Rows[0]["DS_AGENCIA"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.ContaCorrente = lDataTable.Rows[0]["NR_CTACORR"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.NrBanco = lDataTable.Rows[0]["CD_BANCO"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.NrNota = lDataTable.Rows[0]["NR_NOTA"].DBToInt32();
                        plRetorno.Relatorio.CabecalhoCliente.EnderecoCliente = lDataTable.Rows[0]["endereco_cli"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.CepCidadeUFCliente = lDataTable.Rows[0]["cep_cidd_uf_cli"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.CodCliente = lDataTable.Rows[0]["CD_CLIENTE_CC"].DBToInt32();
                        plRetorno.Relatorio.CabecalhoCliente.CpfCnpj = lDataTable.Rows[0]["CD_CPFCGC"].DBToString();
                        plRetorno.Relatorio.CabecalhoCliente.DvCliente = lDataTable.Rows[0]["DV_CLIENTE_ORI"].DBToInt32();
                        plRetorno.Relatorio.CabecalhoCliente.NomeCliente = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                    }
                }
            }
        }

        private void BuscarNotaDeCorretagem_CabecalhoDaCorretora(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse pRetorno)
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
                        pRetorno.Relatorio.CabecalhoCorretora.CpfCnpj = lDataTable.Rows[0]["CD_CGC"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.DataPregao = lDataTable.Rows[0]["DT_NEGOCIO"].DBToDateTime();
                        pRetorno.Relatorio.CabecalhoCorretora.EmailCorretora = lDataTable.Rows[0]["CD_EMAIL"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.EnderecoCorretora = lDataTable.Rows[0]["ENDERECO_CORR"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.NomeCorretora = lDataTable.Rows[0]["DS_RSOC_LOC"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.NumeroNota = lDataTable.Rows[0]["NR_NOTA"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.SiteCorretora = lDataTable.Rows[0]["CD_INTERNET"].DBToString();
                        pRetorno.Relatorio.CabecalhoCorretora.TelefonesCorretora = lDataTable.Rows[0]["TELEFONES_CORR"].DBToString();
                    }
                }
            }
        }

        private void BuscarNotaDeCorretagem_ListaDeNegociosBmf(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse pRetorno)
        {
            var lNotaDeCorretagemExtratoInfo = new NotaDeCorretagemExtratoBmfInfo();

            using (DbCommand lComando = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_NC_NEGOCIACAO_BMF_SEL"))
            {
                pAcessoDados.AddInParameter(lComando, "pCd_cliente", DbType.Int32, pParametros.ConsultaCodigoClienteBmf);
                pAcessoDados.AddInParameter(lComando, "pDt_negocio", DbType.Date, pParametros.ConsultaDataMovimento);

                var lTabela = pAcessoDados.ExecuteOracleDataTable(lComando);

                foreach (DataRow dr in lTabela.Rows)
                {
                    lNotaDeCorretagemExtratoInfo = new NotaDeCorretagemExtratoBmfInfo();

                    lNotaDeCorretagemExtratoInfo.Sentido             = dr["C_V"].DBToString();
                    lNotaDeCorretagemExtratoInfo.Mercadoria          = dr["Mercadoria"].DBToString();
                    lNotaDeCorretagemExtratoInfo.Mercadoria_Serie    = dr["Mercadoria_Serie"].ToString();
                    lNotaDeCorretagemExtratoInfo.Vencimento          = dr["Vencimento"].DBToDateTime();
                    lNotaDeCorretagemExtratoInfo.Quantidade          = dr["Quantidade"].DBToInt32();
                    lNotaDeCorretagemExtratoInfo.PrecoAjuste         = dr["Preco_Ajuste"].DBToDecimal();
                    lNotaDeCorretagemExtratoInfo.TipoNegocio         = dr["Tipo_do_Negocio"].ToString();
                    lNotaDeCorretagemExtratoInfo.ValorOperacao       = dr["Vlr_Operacao_Ajuste"].DBToDecimal();
                    lNotaDeCorretagemExtratoInfo.DC                  = dr["Vlr_Operacao_Ajuste"].DBToDecimal() < 0 ? "D" : "C";
                    lNotaDeCorretagemExtratoInfo.Observacao          = dr["OBS"].ToString();
                    lNotaDeCorretagemExtratoInfo.TaxaOperacional     = dr["Taxa_Operacional"].DBToDecimal();

                    pRetorno.RelatorioBmf.ListaNotaDeCorretagemExtratoBmfInfo.Add(lNotaDeCorretagemExtratoInfo);
                }
            }
        }

        private void BuscarNotaDeCorretagem_ListaDeNegocios(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse pRetorno)
        {
            bool lNotaProvisoria = DateTime.Today.Equals(pParametros.ConsultaDataMovimento);
            var lProc_NotaCorretagem = lNotaProvisoria ? "prc_NC_Negociacao_Sel" : "prc_NC_NegociosRealizados_Sel";
            var lNotaDeCorretagemExtratoInfo = new NotaDeCorretagemExtratoInfo();

            using (var lComando = pAcessoDados.CreateCommand(CommandType.StoredProcedure, lProc_NotaCorretagem))
            {
                pAcessoDados.AddInParameter(lComando, "cd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lComando, "dt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);
                pAcessoDados.AddInParameter(lComando, "dt_negocio", DbType.Date, pParametros.ConsultaDataMovimento);
                pAcessoDados.AddInParameter(lComando, "tp_mercado", DbType.String, pParametros.ConsultaTipoDeMercado);

                if (pParametros.ConsultaProvisorio && !lNotaProvisoria)
                    pAcessoDados.AddInParameter(lComando, "in_broker", DbType.String, "C");

                var lTabela = pAcessoDados.ExecuteOracleDataTable(lComando);

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

                    pRetorno.Relatorio.ListaNotaDeCorretagemExtratoInfo.Add(lNotaDeCorretagemExtratoInfo);
                }
            }
        }

        private void BuscarNotaDeCorretagem_RodapeBmf(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse pRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_NC_RODAPE_BMF_SEL"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametros.ConsultaCodigoClienteBmf);
                pAcessoDados.AddInParameter(lDbCommand, "pdt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);

                if (pParametros.ConsultaProvisorio)
                {
                    pAcessoDados.AddInParameter(lDbCommand, "in_broker", DbType.String, "C");
                }

                using (DataTable lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (lDataTable.Rows.Count > 0)
                    {
                        pRetorno.RelatorioBmf.Rodape.DsCpfCnpj        = lDataTable.Rows[0]["DsCpfCnpj"].ToString();
                        pRetorno.RelatorioBmf.Rodape.NumeroDaNota     = lDataTable.Rows[0]["Nr_Nota"].ToString();
                        pRetorno.RelatorioBmf.Rodape.DataPregao       = lDataTable.Rows[0]["Data_Pregao"].DBToDateTime();
                        pRetorno.RelatorioBmf.Rodape.VendaDisponivel  = lDataTable.Rows[0]["VendaDiposnivel"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.CompraDisponivel = lDataTable.Rows[0]["CompraDiposnivel"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.VendaOpcoes      = lDataTable.Rows[0]["VendaOpcoes"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.CompraOpcoes     = lDataTable.Rows[0]["CompraOpcoes"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.ValorNegocios    = lDataTable.Rows[0]["ValorNegocios"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.IRRF             = lDataTable.Rows[0]["IRRF"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.IRRFDayTrade     = lDataTable.Rows[0]["IRRFDayTrade"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TaxaOperacional  = lDataTable.Rows[0]["TaxaOperacional"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TaxaRegistroBmf  = lDataTable.Rows[0]["TaxaRegistroBmf"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TaxaBmf          = lDataTable.Rows[0]["TaxaBmf"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.ISS              = lDataTable.Rows[0]["ISS"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.AjustePosicao    = lDataTable.Rows[0]["AjustePosicao"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.AjusteDayTrade   = lDataTable.Rows[0]["AjusteDayTrade"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TotalDespesas    = lDataTable.Rows[0]["TotalDespesas"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.IrrfCorretagem   = lDataTable.Rows[0]["IrrfCorretagem"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TotalContaNormal = lDataTable.Rows[0]["TotalContaNormal"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TotalLiquido     = lDataTable.Rows[0]["TotalLiquido"].DBToDecimal();
                        pRetorno.RelatorioBmf.Rodape.TotalLiquidoNota = lDataTable.Rows[0]["TotalLiquidoNota"].DBToDecimal();
                    }
                }
            }
        }

        private void BuscarNotaDeCorretagem_Rodape(ref NotaDeCorretagemExtratoRequest pParametros, ref AcessaDados pAcessoDados, ref NotaDeCorretagemExtratoResponse pRetorno)
        {                                                                                       /*"prc_NC_DadosFinanceiro_Sel"))*/
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "prc_notacorretagem_finan_hist"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pcd_cliente", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "ptp_negocio", DbType.String, pParametros.ConsultaTipoDeMercado == "VIS" ? "NOR" : "FUT");
                pAcessoDados.AddInParameter(lDbCommand, "pdt_datmov", DbType.Date, pParametros.ConsultaDataMovimento);

                if (pParametros.ConsultaProvisorio)
                    pAcessoDados.AddInParameter(lDbCommand, "in_broker", DbType.String, "C");

                using (DataTable lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        pRetorno.Relatorio.Rodape.Debentures = lDataTable.Rows[0]["VL_DEBENT"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.VendaVista = lDataTable.Rows[0]["VL_VDAVIS"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.CompraVista = lDataTable.Rows[0]["VL_CPAVIS"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.CompraOpcoes = lDataTable.Rows[0]["VL_CPAOPC"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.VendaOpcoes = lDataTable.Rows[0]["VL_VDAOPC"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.OperacoesTermo = lDataTable.Rows[0]["VL_TERMO"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.OperacoesTitulosPublicos = lDataTable.Rows[0]["VL_TITPUB"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.ValorDasOperacoes = lDataTable.Rows[0]["VL_TOTNEG"].DBToDecimal();

                        pRetorno.Relatorio.Rodape.ValorLiquidoOperacoes = pParametros.ConsultaTipoDeMercado == "VIS" ? lDataTable.Rows[0]["VL_LIQ_OPERACOES_VIS"].DBToDecimal() : lDataTable.Rows[0]["VL_LIQ_OPERACOES_OPC"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.TaxaLiquidacao        = lDataTable.Rows[0]["VL_EMOLUM_CB"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.TaxaLiquidacao_DC     = "D";
                        pRetorno.Relatorio.Rodape.TaxaDeRegistro        = lDataTable.Rows[0]["VL_TAXREG_CB"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.TaxaDeRegistro_DC     = (lDataTable.Rows[0]["VL_TAXREG_CB"].DBToDecimal() > 0 ? "C" : "D"); 
                        pRetorno.Relatorio.Rodape.TaxaANA               = lDataTable.Rows[0]["VL_TAXANA"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.TaxaANA_DC            = (lDataTable.Rows[0]["VL_TAXANA"].DBToDecimal() > 0 ? "C" : "D");
                        pRetorno.Relatorio.Rodape.Emolumentos           = lDataTable.Rows[0]["VL_EMOLUM_BV"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.Emolumentos_DC        = (lDataTable.Rows[0]["VL_EMOLUM_BV"].DBToDecimal() > 0 ? "C" : "D");
                        pRetorno.Relatorio.Rodape.Corretagem            = lDataTable.Rows[0]["VL_VALCOR"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.Corretagem_DC         = (lDataTable.Rows[0]["VL_VALCOR"].DBToDecimal() > 0 ? "C" : "D");
                        pRetorno.Relatorio.Rodape.ISS                   = lDataTable.Rows[0]["VL_ISS"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.ISS_DC                = "D"; 
                        pRetorno.Relatorio.Rodape.Outras                = lDataTable.Rows[0]["VL_DESP_NOTA"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.ValorLiquidoNota      = lDataTable.Rows[0]["VL_LIQNOT"].DBToDecimal(); //--> Total Bovespa/Soma
                        pRetorno.Relatorio.Rodape.DataLiquidoPara       = lDataTable.Rows[0]["PZ_PROJ_CC"].DBToDateTime(); // this.RecuperarDataLiquidoPara(pRetorno.Relatorio.CabecalhoCorretora.DataPregao, pParametros.ConsultaTipoDeMercado);

                        pRetorno.Relatorio.Rodape.OperacoesFuturo        = lDataTable.Rows[0]["VL_FUTURO"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.ValorAjusteFuturo      = lDataTable.Rows[0]["VL_FUTURO"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRSobreCorretagem      = lDataTable.Rows[0]["VL_IRCORR"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRRFSobreDayTrade      = lDataTable.Rows[0]["VL_IRRF_DESPESA"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.Total_123_A            = lDataTable.Rows[0]["VL_LIQNOT_CB"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.Total_123_A_DC         = (lDataTable.Rows[0]["VL_LIQNOT_CB"].DBToDecimal() > 0 ? "C" : "D");
                        pRetorno.Relatorio.Rodape.TaxaTerOpcFut          = lDataTable.Rows[0]["VL_TAXPTA"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.TaxaTerOpcFut_DC       = (lDataTable.Rows[0]["VL_TAXPTA"].DBToDecimal() > 0 ? "C" : "D"); 
                        pRetorno.Relatorio.Rodape.IRRFOperacoes          = lDataTable.Rows[0]["VL_IROPER"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRRFOperacoes_DC       = (lDataTable.Rows[0]["VL_IROPER"].DBToDecimal() > 0 ? "C" : "D");
                        pRetorno.Relatorio.Rodape.VLBaseOperacoesIRRF    = lDataTable.Rows[0]["VL_BASEIROPER"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.VLBaseOperacoesIRRF_DC = (lDataTable.Rows[0]["VL_BASEIROPER"].DBToDecimal() > 0 ? "C" : "D");

                        //pRetorno.Relatorio.Rodape.DescOutras = lDataTable.Rows[0]["DS_DESP_NOTA"].DBToString();
                        pRetorno.Relatorio.Rodape.IRRFSobreDayTradeBase     = lDataTable.Rows[0]["VL_BASEIRDT_DAYTRADE"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRRFSobreDayTradeProjecao = lDataTable.Rows[0]["VL_IRRETIDO_DAYTRADE"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRRFSemOperacoesBase      = lDataTable.Rows[0]["VL_BASEIRDT_RESUMO"].DBToDecimal();
                        pRetorno.Relatorio.Rodape.IRRFSemOperacoesValor     = lDataTable.Rows[0]["VL_IRRETIDO_RESUMO"].DBToDecimal();
                    }
                }
            }
        }

        //private void ValidarDataAposFeridado(DateTime pDataPregao, ref DateTime pDataLiquidoPara)
        //{
        //    var lIncrementaFeriado = default(bool);
        //    var lIncrementoDias = default(int);

        //    while (!lIncrementaFeriado && pDataLiquidoPara >= pDataPregao.AddDays(lIncrementoDias))
        //    {
        //        lIncrementaFeriado = this.GetDataFeriados.Contains(pDataPregao.AddDays(lIncrementoDias));
        //        lIncrementoDias++;
        //    }

        //    if (lIncrementaFeriado)
        //    {   //--> Verificando o feriado
        //        pDataLiquidoPara = pDataLiquidoPara.AddDays(1);

        //        if (DayOfWeek.Saturday.Equals(pDataLiquidoPara.DayOfWeek))
        //            pDataLiquidoPara = pDataLiquidoPara.AddDays(2);
        //        else if (DayOfWeek.Sunday.Equals(pDataLiquidoPara.DayOfWeek))
        //            pDataLiquidoPara = pDataLiquidoPara.AddDays(1);

        //        if (this.GetDataFeriados.Contains(pDataLiquidoPara))
        //            this.ValidarDataAposFeridado(pDataPregao, ref pDataLiquidoPara); //--> Verifica a existência de feriados consecutivos
        //    }
        //}

        //private DateTime RecuperarDataLiquidoPara(DateTime pDataPregao, string pTipoMercado)
        //{
        //    var lRetorno = new DateTime();

        //    if ("OPC".Equals(pTipoMercado))
        //    {
        //        lRetorno = pDataPregao.AddDays(1);

        //        if (DayOfWeek.Friday.Equals(pDataPregao.DayOfWeek))
        //            lRetorno = lRetorno.AddDays(2); //--> Adicionando o pez'ríodo do final de semana.
        //    }
        //    else
        //    {
        //        lRetorno = pDataPregao.AddDays(3);

        //        //--> Adicionando o período do final de semana.
        //        if (DayOfWeek.Wednesday.Equals(pDataPregao.DayOfWeek)
        //        || (DayOfWeek.Thursday.Equals(pDataPregao.DayOfWeek))
        //        || (DayOfWeek.Friday.Equals(pDataPregao.DayOfWeek)))
        //            lRetorno = lRetorno.AddDays(2); //--> Adicionando o período do final de semana.
        //    }

        //    this.ValidarDataAposFeridado(pDataPregao, ref lRetorno);

        //    return lRetorno;
        //}

        #endregion
    }
}
