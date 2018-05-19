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
    public class FaxBovespaDbLib
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

        #region | Métodos de Bovespa Portugues
        public FaxResponse ObterFaxBovespa(FaxRequest lRequest)
        {
            var lRetorno = new FaxResponse();

            var lAcessaDados = new AcessaDados();

            log4net.Config.XmlConfigurator.Configure();
            
            try
            {
                lAcessaDados.ConnectionStringName = "CORRWIN";

                this.BuscarFax_Cabecalho(ref lRequest, ref lAcessaDados, ref lRetorno);

                this.BuscarFax_DataLiquidacao(ref lRequest, ref lAcessaDados, ref lRetorno);
                
                this.BuscarFax_Papel(ref lRequest, ref lAcessaDados, ref lRetorno);
                
                this.BuscarFax_SOMA(ref lRequest, ref lAcessaDados, ref lRetorno);
                
                this.BuscarFax_TOTAL(ref lRequest, ref lAcessaDados, ref lRetorno);
                
                this.BuscarFax_RODAPE(ref lRequest, ref lAcessaDados, ref lRetorno);
                
            }
            catch(Exception ex)
            {
                gLogger.Error("ObterFaxBovespa", ex);
                lRetorno.DescricaoResposta = ex.ToString();
                lRetorno.StatusResposta    = MensagemResponseStatusEnum.ErroNegocio;
            }

            return lRetorno;
        }

        private void BuscarFax_Cabecalho(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_CAB_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData",          DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.Relatorio.CodigoCliente = lDataTable.Rows[0]["CD_CLIENTE"].DBToInt32();
                        plRetorno.Relatorio.DigitoCliente = lDataTable.Rows[0]["DV_CLIENTE"].DBToInt32();
                        plRetorno.Relatorio.NomeCliente   = lDataTable.Rows[0]["NM_CLIENTE"].DBToString();
                        plRetorno.Relatorio.Empresa       = lDataTable.Rows[0]["EMPRESA"].DBToString();
                        plRetorno.Relatorio.Telefone      = lDataTable.Rows[0]["TELEFONE"].DBToString();
                        plRetorno.Relatorio.Fax           = lDataTable.Rows[0]["FAX"].DBToString();
                        plRetorno.Relatorio.DataPregao    = lDataTable.Rows[0]["DATA"].DBToDateTime();
                    }
                }
            }
        }
        
        private void BuscarFax_DataLiquidacao(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_DTLIQ_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.Relatorio.DataLiquidacaoOpcao = lDataTable.Rows[0]["DT_LIQUIDACAO_OPC"].DBToDateTime();
                        plRetorno.Relatorio.DataLiquidacaoVista = lDataTable.Rows[0]["DT_LIQUIDACAO_VISTA"].DBToDateTime();
                    }
                }
            }
        }

        private void BuscarFax_Papel(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_PAPEL_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        plRetorno.Relatorio.DetalhesBovespa = new List<FaxBovespaDetalheInfo>();

                        for (int i =0; i < lDataTable.Rows.Count; i++ )
                        {

                            var lDetBovespa = new FaxBovespaDetalheInfo();

                            lDetBovespa.PapelCodigoIsin    = lDataTable.Rows[i]["CD_CODISI"].DBToString();
                            lDetBovespa.PapelCodigoNegocio = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                            lDetBovespa.PapelCorretagem    = lDataTable.Rows[i]["CORRETAGEM"].DBToDecimal();
                            lDetBovespa.PapelNomeRes       = lDataTable.Rows[i]["NM_NOMRES"].DBToString();
                            lDetBovespa.PapelPreco         = lDataTable.Rows[i]["PRECO"].DBToDecimal();
                            lDetBovespa.PapelQuantidade    = lDataTable.Rows[i]["QUANT"].DBToInt32();
                            lDetBovespa.PapelSentido       = lDataTable.Rows[i]["SENTIDO"].DBToString();
                            lDetBovespa.PapelTipoMercado   = lDataTable.Rows[i]["TP_MERCADO"].DBToString();
                            lDetBovespa.PapelTotal         = lDataTable.Rows[i]["TOTAL"].DBToDecimal();

                            plRetorno.Relatorio.DetalhesBovespa.Add(lDetBovespa);
                        }
                        
                    }
                }
            }
        }
        
        private void BuscarFax_SOMA(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_SOMA_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lDetBovespa = plRetorno.Relatorio.DetalhesBovespa.FindAll(papel => 
                            {
                                return papel.PapelCodigoNegocio == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString() &&
                                    papel.PapelSentido == lDataTable.Rows[i]["SENTIDO"].DBToString();
                            });

                            for (int j = 0; j < lDetBovespa.Count; j++)
                            {
                                lDetBovespa[j].SomaCodigoNegocio   = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                                lDetBovespa[j].SomaMedio           = lDataTable.Rows[i]["MEDIO"].DBToDecimal(); ;
                                lDetBovespa[j].SomaQtdeTotal       = lDataTable.Rows[i]["TOTAL_QTD"].DBToDecimal();
                                lDetBovespa[j].SomaTotal           = lDataTable.Rows[i]["TOTAL"].DBToDecimal();
                                lDetBovespa[j].SomaTotalCorretagem = lDataTable.Rows[i]["TOTAL_CORR"].DBToDecimal();
                            }

                            //plRetorno.Relatorio.DetalhesBovespa.Add(lDetBovespa);
                        }

                    }
                }
            }
        }
        
        private void BuscarFax_TOTAL(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_TOTAL_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            var lDetBovespa = plRetorno.Relatorio.DetalhesBovespa.FindAll(papel =>
                            {
                                return papel.PapelCodigoNegocio == lDataTable.Rows[i]["CD_NEGOCIO"].DBToString() &&
                                    papel.PapelSentido == lDataTable.Rows[i]["SENTIDO"].DBToString();
                            });

                            for (int j = 0; j < lDetBovespa.Count; j++)
                            {
                                lDetBovespa[j].TotalCodigoNegocio = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                                lDetBovespa[j].TotalNet           = lDataTable.Rows[i]["TOTAL_NET"].DBToDecimal();
                                lDetBovespa[j].TotalSentido       = lDataTable.Rows[i]["SENTIDO"].DBToString();
                                lDetBovespa[j].TotalPrecoNet      = lDataTable.Rows[i]["PRECO_NET"].DBToDecimal();
                            }

                            //plRetorno.Relatorio.DetalhesBovespa.Add(lDetBovespa);
                        }

                    }
                }
            }
        }

        private void BuscarFax_RODAPE(ref FaxRequest pParametros, ref AcessaDados pAcessoDados, ref FaxResponse plRetorno)
        {
            using (DbCommand lDbCommand = pAcessoDados.CreateCommand(CommandType.StoredProcedure, "PRC_FAX_RODAPE_BOV_BR"))
            {
                pAcessoDados.AddInParameter(lDbCommand, "pClienteCodigo", DbType.Int32, pParametros.ConsultaCodigoCliente);
                pAcessoDados.AddInParameter(lDbCommand, "pData", DbType.Date, pParametros.ConsultaDataMovimento);

                using (var lDataTable = pAcessoDados.ExecuteOracleDataTable(lDbCommand))
                {
                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < lDataTable.Rows.Count; i++)
                        {
                            if (lDataTable.Rows[i]["TP_NEGOCIO"].DBToString() == "FUT")
                            {
                                plRetorno.Relatorio.RodapeTotalComprasOpcao      = lDataTable.Rows[i]["TOTAL_COMPRAS_OPCAO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalVendasOpcao       = lDataTable.Rows[i]["TOTAL_VENDAS_OPCAO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalTermoOpcao        = lDataTable.Rows[i]["TOTAL_TERMO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalAjusteOpcao       = lDataTable.Rows[i]["TOTAL_AJUSTE"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalNegociosOpcao     = lDataTable.Rows[i]["TOTAL_NEGOCIOS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalCorretagemOpcao   = lDataTable.Rows[i]["TOTAL_CORRETAGEM"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaCblcOpcao          = lDataTable.Rows[i]["TAXAS_CBLC"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaBovespaOpcao       = lDataTable.Rows[i]["TAXAS_BOVESPA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaOperacionaisOpcao  = lDataTable.Rows[i]["TAXAS_OPERACIONAIS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeOutrasDepesasOpcao     = lDataTable.Rows[i]["OUTRAS_DESPESAS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeIRDayTradeOpcao        = lDataTable.Rows[i]["IR_DAY_TRADE"].DBToDecimal();
                                plRetorno.Relatorio.RodapeIrOperacoesOpcao       = lDataTable.Rows[i]["IR_OPERACOES"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalLiquidoOpcao      = lDataTable.Rows[i]["TOTAL_LIQUIDO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeBaseIRDTOpcao          = lDataTable.Rows[i]["BASE_IR_DT"].DBToDecimal();
                                plRetorno.Relatorio.RodapeBaseIROperacoesOpcao   = lDataTable.Rows[i]["BASE_IR_OPER"].DBToDecimal();
                                plRetorno.Relatorio.RodapeEmolumentoBolsaOpcao   = lDataTable.Rows[i]["EMOLUMENTO_BOLSA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeEmolumentoTotalOpcao   = lDataTable.Rows[i]["EMOLUMENTO_TOTAL"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaLiquidacaoOpcao    = lDataTable.Rows[i]["TAXA_LIQUIDACAO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroBolsaOpcao = lDataTable.Rows[i]["TAXA_REGISTRO_BOLSA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroTotalOpcao = lDataTable.Rows[i]["TAXA_REGISTRO_TOTAL"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroOpcao      = lDataTable.Rows[i]["TAXA_REGISTRO"].DBToDecimal();
                            }
                            else
                            {
                                plRetorno.Relatorio.RodapeTotalComprasVista      = lDataTable.Rows[i]["TOTAL_COMPRAS_VISTA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalVendasVista       = lDataTable.Rows[i]["TOTAL_VENDAS_VISTA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalTermoVista        = lDataTable.Rows[i]["TOTAL_TERMO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalAjusteVista       = lDataTable.Rows[i]["TOTAL_AJUSTE"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalNegociosVista     = lDataTable.Rows[i]["TOTAL_NEGOCIOS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalCorretagemVista   = lDataTable.Rows[i]["TOTAL_CORRETAGEM"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaCblcVista          = lDataTable.Rows[i]["TAXAS_CBLC"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaBovespaVista       = lDataTable.Rows[i]["TAXAS_BOVESPA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaOperacionaisVista  = lDataTable.Rows[i]["TAXAS_OPERACIONAIS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeOutrasDepesasVista     = lDataTable.Rows[i]["OUTRAS_DESPESAS"].DBToDecimal();
                                plRetorno.Relatorio.RodapeIRDayTradeVista        = lDataTable.Rows[i]["IR_DAY_TRADE"].DBToDecimal();
                                plRetorno.Relatorio.RodapeIrOperacoesVista       = lDataTable.Rows[i]["IR_OPERACOES"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTotalLiquidoVista      = lDataTable.Rows[i]["TOTAL_LIQUIDO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeBaseIRDTVista          = lDataTable.Rows[i]["BASE_IR_DT"].DBToDecimal();
                                plRetorno.Relatorio.RodapeBaseIROperacoesVista   = lDataTable.Rows[i]["BASE_IR_OPER"].DBToDecimal();
                                plRetorno.Relatorio.RodapeEmolumentoBolsaVista   = lDataTable.Rows[i]["EMOLUMENTO_BOLSA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeEmolumentoTotalVista   = lDataTable.Rows[i]["EMOLUMENTO_TOTAL"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaLiquidacaoVista    = lDataTable.Rows[i]["TAXA_LIQUIDACAO"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroBolsaVista = lDataTable.Rows[i]["TAXA_REGISTRO_BOLSA"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroTotalVista = lDataTable.Rows[i]["TAXA_REGISTRO_TOTAL"].DBToDecimal();
                                plRetorno.Relatorio.RodapeTaxaRegistroVista      = lDataTable.Rows[i]["TAXA_REGISTRO"].DBToDecimal();
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
