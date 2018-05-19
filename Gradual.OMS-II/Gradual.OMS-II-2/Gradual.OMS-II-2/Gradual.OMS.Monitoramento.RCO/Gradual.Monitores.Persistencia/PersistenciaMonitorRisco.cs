using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using log4net;
using Gradual.Monitores.Risco.Lib;
using System.Collections;

namespace Gradual.Monitores.Persistencia
{
    public class PersistenciaMonitorRisco
    {
        private const string gNomeConexaoSinacor     = "SINACOR";
        private const string gNomeConexaoSQL         = "Risco";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";
        private const string gNomeConexaoCadastro    = "Cadastro";
        private const string gNomeConexaoGradualOMS  = "GradualOMS";

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Construtor vazio
        /// </summary>
        public PersistenciaMonitorRisco()
        {
            //log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Método que insere um resumo da exposição do cliente no sql. 
        /// Esse dados são usados para relatório de Compliance na intranet.
        /// Procedure: prc_cliente_exposicao_intradiario_ins 
        /// </summary>
        /// <param name="pRequest">Dados com o um resumo da Exposição intradiário do cliente</param>
        public void InserirPosicaoExposicaoCliente(MonitoramentoIntradiarioInfo pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                logger.Info("Gravando EXPOSIÇÃO do Cliente " + pRequest.CodigoCliente);
                    //.InfoFormat("Inserindo dados de exposição do Cliente {0} - {1}", 
                //    pRequest.CodigoCliente,
                //    pRequest.NomeCliente);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_exposicao_intradiario_ins"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente",         DbType.String,  pRequest.NomeCliente            );
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",       DbType.Int32,   pRequest.CodigoCliente.Value    );
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeAssessor",        DbType.String,  pRequest.NomeAssessor           );
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor",      DbType.Int32,   pRequest.CodigoAssessor.Value   );
                    lAcessaDados.AddInParameter(lDbCommand, "@NETxSFP",             DbType.Decimal, pRequest.NETxSFP                );
                    lAcessaDados.AddInParameter(lDbCommand, "@EXPxPosicao",         DbType.Decimal, pRequest.EXPxPosicao            );
                    lAcessaDados.AddInParameter(lDbCommand, "@Net",                 DbType.Decimal, pRequest.Net                    );
                    lAcessaDados.AddInParameter(lDbCommand, "@SFP",                 DbType.Decimal, pRequest.SFP                    );
                    lAcessaDados.AddInParameter(lDbCommand, "@LucroPrejuizo",       DbType.Decimal, pRequest.LucroPrejuizo          );
                    lAcessaDados.AddInParameter(lDbCommand, "@PercentualPrejuizo",  DbType.Decimal, pRequest.PercentualPrejuizo     );
                    lAcessaDados.AddInParameter(lDbCommand, "@Posicao",             DbType.Decimal, pRequest.Posicao                );
                    lAcessaDados.AddInParameter(lDbCommand, "@Exposicao",           DbType.Decimal, pRequest.Exposicao              );
                    lAcessaDados.AddInParameter(lDbCommand, "@VolumeBovespa",       DbType.Decimal, pRequest.VolumeBovespa          );
                    
                    if (pRequest.CodigoClienteBmf.HasValue)
                        lAcessaDados.AddInParameter(lDbCommand, "@CodigoClienteBmf",    DbType.Int32, pRequest.CodigoClienteBmf.Value);
                    
                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Inserir Posição e Exposição-: ", ex);
            }
        }

        /// <summary>
        /// Método que atualiza um resumo da exposição do cliente no sql.
        /// Esses dados são usados para relatório de compliance na intranet
        /// Procedure: prc_cliente_exposicao_intradiario_upd
        /// </summary>
        /// <param name="pRequest">Dados com o um resumo da Exposição intraday do cliente</param>
        public void AtualizarPosicaoExposicaoCliente(MonitoramentoIntradiarioInfo pRequest)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                logger.Info("Atualizado EXPOSIÇÃO do Cliente " + pRequest.CodigoCliente);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cliente_exposicao_intradiario_upd"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoAssessor",      DbType.Int32,   pRequest.CodigoAssessor.Value);
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeAssessor",        DbType.String,  pRequest.NomeAssessor        );
                    lAcessaDados.AddInParameter(lDbCommand, "@NomeCliente",         DbType.String,  pRequest.NomeCliente         );
                    lAcessaDados.AddInParameter(lDbCommand, "@CodigoCliente",       DbType.Int32,   pRequest.CodigoCliente.Value );
                    lAcessaDados.AddInParameter(lDbCommand, "@NETxSFP",             DbType.Decimal, pRequest.NETxSFP             );
                    lAcessaDados.AddInParameter(lDbCommand, "@EXPxPosicao",         DbType.Decimal, pRequest.EXPxPosicao         );
                    lAcessaDados.AddInParameter(lDbCommand, "@Net",                 DbType.Decimal, pRequest.Net                 );
                    lAcessaDados.AddInParameter(lDbCommand, "@SFP",                 DbType.Decimal, pRequest.SFP                 );
                    lAcessaDados.AddInParameter(lDbCommand, "@LucroPrejuizo",       DbType.Decimal, pRequest.LucroPrejuizo       );
                    lAcessaDados.AddInParameter(lDbCommand, "@PercentualPrejuizo",  DbType.Decimal, pRequest.PercentualPrejuizo  );
                    lAcessaDados.AddInParameter(lDbCommand, "@Posicao",             DbType.Decimal, pRequest.Posicao             );
                    lAcessaDados.AddInParameter(lDbCommand, "@Exposicao",           DbType.Decimal, pRequest.Exposicao           );
                    lAcessaDados.AddInParameter(lDbCommand, "@VolumeBovespa",       DbType.Decimal, pRequest.VolumeBovespa       );

                    if (pRequest.CodigoClienteBmf.HasValue)
                        lAcessaDados.AddInParameter(lDbCommand, "@CodigoClienteBmf", DbType.Int32, pRequest.CodigoClienteBmf.Value);

                    lAcessaDados.ExecuteNonQuery(lDbCommand);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro ao Atualizar Posição e Exposição-: ", ex);
            }
        }

        /// <summary>
        /// Método que busca no banco de dados do sinacor e gera uma lista
        /// com a Posição de termo do cliente.
        /// Procedure: prc_movimento_termo
        /// </summary>
        /// <param name="idCliente">Código principal bovespa do cliente</param>
        /// <returns>Retorna uma Lista de Posição do termo do cliente</returns>
        public List<PosicaoTermoInfo> ObterPosicaoTermo(int idCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoTermoInfo _PosicaoTermo;
            List<PosicaoTermoInfo> lstTermo = new List<PosicaoTermoInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_movimento_termo"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "CodigoCliente", DbType.Int32, idCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {

                            _PosicaoTermo = new PosicaoTermoInfo();

                            _PosicaoTermo.IDCliente = (lDataTable.Rows[i]["cod_cli"]).DBToInt32();
                            _PosicaoTermo.Instrumento = (lDataTable.Rows[i]["cod_neg"]).DBToString();
                            _PosicaoTermo.DataExecucao = (lDataTable.Rows[i]["data_exec"]).DBToDateTime();
                            _PosicaoTermo.DataVencimento = (lDataTable.Rows[i]["data_venc"]).DBToDateTime();
                            _PosicaoTermo.Quantidade = (lDataTable.Rows[i]["qtde_disp"]).DBToInt32();
                            _PosicaoTermo.PrecoExecucao = (lDataTable.Rows[i]["val_nego"]).DBToDecimal();

                            lstTermo.Add(_PosicaoTermo);
                        }
                    }
                }

                return lstTermo;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

        }

        /// <summary>
        /// Método que busca o valor de garantia bmf do cliente.
        /// Procedure: PRC_SALDOCLIENTE_BMF
        /// </summary>
        /// <param name="IdCliente">Código bmf do cliente</param>
        /// <returns>Retorna o valor de garantia do cliente</returns>
        public decimal ObterPosicaoGarantiaBMF(int IdCliente)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            decimal lRetorno = 0.0M;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, IdCliente);

                DataTable lDataTable =lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable.Rows.Count > 0)
                {
                    lRetorno = (lDataTable.Rows[0]["VL_TOTGAR"]).DBToDecimal();
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que busca margem Requerida Bmf do Cliente.
        /// Procedure: PRC_SALDOCLIENTE_BMF
        /// </summary>
        /// <param name="IdCliente">Código BMF do Cliente</param>
        /// <returns>Retorna o valor da margem da requerida BMF</returns>
        public decimal ObterPosicaoMargemRequeridaBMF(int IdCliente)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            decimal lRetorno = 0.0M;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, IdCliente);

                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (lDataTable.Rows.Count > 0)
                {
                    lRetorno = (lDataTable.Rows[0]["VL_TOTMAR"]).DBToDecimal();
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que busca no sinacor o valor de de garantia bovespa
        /// Procedure: PRC_SEL_GARANTIA_BOV
        /// </summary>
        /// <param name="IdCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o valor de garantia bovespa do cliente</returns>
        public decimal ObterPosicaoGarantiaBovespa(int IdCliente)
        {
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

            decimal lRetorno = 0.0M;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_GARANTIA_BOV"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, IdCliente);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno = lDataTable.Rows[i]["VALO_GARN_DEPO"].DBToDecimal();
                    }
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que busca no sinacor a Situação financeira patrimonial do cliente
        /// Procedure: prc_sfp_monitor
        /// </summary>
        /// <param name="CodigoCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o valor da situação financeira patrimonial do cliente</returns>
        public decimal ObterSituacaoFinanceiraPatrimonial(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sfp_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    decimal ValorItem = 0;

                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorItem = (lDataTable.Rows[0]["VL_ben"]).DBToDecimal();
                        SaldoTotal += ValorItem;
                    }
                }

                return SaldoTotal;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a situacao financeira patrimonial: " + CodigoCliente.ToString(), ex);

            }

            return SaldoTotal;

        }

        /// <summary>
        /// Método que busca no sinacor a Situação financeira patrimonial L1 do cliente.
        /// L1 está na tscclibol.
        /// Procedure: prc_Limite_operacional_monitor
        /// </summary>
        /// <param name="CodigoCliente">Código Bovespa do cliente</param>
        /// <returns>Retorno a somatória (Saldo total) de SFP L1 do cliente</returns>
        public decimal ObterSituacaoFinanceiraPatrimonialL1(int CodigoCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_Limite_operacional_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    decimal ValorItem = 0;

                    if (lDataTable.Rows.Count > 0)
                    {
                        ValorItem = (lDataTable.Rows[0]["VL_ben"]).DBToDecimal();
                        SaldoTotal += ValorItem;
                    }
                }

                return SaldoTotal;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a situacao financeira patrimonial: " + CodigoCliente.ToString(), ex);

            }

            return SaldoTotal;

        }

        /// <summary>
        /// Método que busca no sinacor a listagem de posição BTC do cliente
        /// Procedure: prc_posicaoclient_btc
        /// </summary>
        /// <param name="idCliente">Código bovespa do cliente</param>
        /// <returns>Retorna listagem da posição BTC do cliente</returns>
        public List<BTCInfo> ObterPosicaoBTC(int idCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            BTCInfo _PosicaoBTC;
            List<BTCInfo> lstBTC = new List<BTCInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_posicaoclient_btc"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, idCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBTC = new BTCInfo();

                            _PosicaoBTC.CodigoCliente = (lDataTable.Rows[i]["COD_CLI"]).DBToInt32();
                            _PosicaoBTC.Carteira = (lDataTable.Rows[i]["COD_CART"]).DBToInt32();
                            _PosicaoBTC.Instrumento = (lDataTable.Rows[i]["COD_NEG"]).DBToString();

                            _PosicaoBTC.DataAbertura = (lDataTable.Rows[i]["DATA_ABER"]).DBToDateTime();
                            _PosicaoBTC.DataVencimento = (lDataTable.Rows[i]["DATA_VENC"]).DBToDateTime();

                            _PosicaoBTC.PrecoMedio = (lDataTable.Rows[i]["PREC_MED"]).DBToDecimal();
                            _PosicaoBTC.Quantidade = (lDataTable.Rows[i]["QTDE_ACOE"]).DBToInt32();
                            _PosicaoBTC.Remuneracao = (lDataTable.Rows[i]["VAL_LIQ"]).DBToDecimal();
                            _PosicaoBTC.Taxa = (lDataTable.Rows[i]["TAXA_REMU"]).DBToDecimal();
                            _PosicaoBTC.TipoContrato = (lDataTable.Rows[i]["TIPO_COTR"]).DBToString();

                            lstBTC.Add(_PosicaoBTC);
                        }
                    }
                }

                return lstBTC;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

        }

        /// <summary>
        /// Método que obtem o valor de cotação da taxa PTAX do sinacor
        /// A TAXA PTAX é usada pelo sistema para efetuar o calculo de o fator multiplcador de DI
        /// Ptax é uma taxa de cambio calculada durenta o dia pelo Banco Central do Brasil. 
        /// Consiste na média das taxas informadas pelos dealers de dolar durante 4 janelas do dia. 
        /// </summary>
        /// <returns>Retorna o valor da cotação da taxa PTAX</returns>
        public double ObterCotacaoPtax()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            double Saldo = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_COTACAO_PTAX"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            Saldo = (lDataTable.Rows[i]["VL_DOLVDA_ATU"]).DBToDouble();
                        }
                    }

                }

                return Saldo;
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }

            return Saldo;
        }

        /// <summary>
        /// Método que busca no sinacor o valor de Saldo de Abertura.
        /// Obtem o valor da dbmfinan33.
        /// Procedure: prc_obter_saldo_abertura
        /// </summary>
        /// <param name="Account">Código do bovespa do cliente</param>
        /// <returns>Retorna o valor do obter saldo de abertura da dbmfinan33</returns>
        public decimal ObterSaldoAbertura(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal Saldo = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_saldo_abertura"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            //vl_total
                            Saldo = (lDataTable.Rows[i]["vl_total"]).DBToDecimal();
                        }
                    }

                }

                return Saldo;
            }
            catch (Exception ex)
            {
                string message = ex.Message;

            }

            return Saldo;

        }
        
        /// <summary>
        /// Método que busca no sinacor o valor de garantia de bovespa do cliente.
        /// Procedure: PRC_GARANTIAS_BOV_DIN_SEL
        /// </summary>
        /// <param name="Account">Código bovespa do cliente</param>
        /// <returns>Retorna o valor da garantia de bovespa do cliente.</returns>
        public decimal ObterSaldoGarantiaBovespa(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BOV_DIN_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoTotal = (lDataTable.Rows[0]["VL_DINHEIRO"]).DBToDecimal();
                    }
                }

                return SaldoTotal;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a posicao de BOVESPA do cliente: " + Account.ToString(), ex);
            }

            return SaldoTotal;
        }

        /// <summary>
        /// Método que que busca no sinacor o saldo em garantia de bmf do cliente
        /// Procedure: PRC_GARANTIAS_BMF_DET_SEL
        /// </summary>
        /// <param name="Account">Código bmf do cliente</param>
        /// <returns>Retorna o valor de garantias bmf do cliente</returns>
        public decimal ObterSaldoGarantiaBmfSemMargem(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                decimal Bloqueios = 0.0M;
                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DIN_SEL"))
                //{
                //    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, Account);

                //    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                //    if (lDataTable.Rows.Count > 0)
                //    {
                //        SaldoTotal = (lDataTable.Rows[0]["VL_DINHEIRO"]).DBToDecimal();
                //    }

                //}

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DET_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoTotal += (lDataTable.Rows[0]["VL_GARANTIA"]).DBToDecimal();
                    }
                }

                return SaldoTotal - Bloqueios;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a posicao de BMF do cliente: " + Account.ToString() ,ex);

            }

            return SaldoTotal;
        }

        /// <summary>
        /// Método que busca no sinacor o saldo de Bmf do cliente.
        /// Primeiro busca o valor bloqueado e depois busca o valor de garantias.
        /// Procedure: PRC_SALDOCLIENTE_BMF - Busca bloqueio
        /// Procedure: PRC_GARANTIAS_BMF_DET_SEL - Busca o valor de garantias
        /// </summary>
        /// <param name="Account">Código bmf do cliente</param>
        /// <returns>Retorna o valor de garantias bmf menos o valor bloquado de bmf</returns>
        public decimal ObterSaldoBMF(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoTotal = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                decimal Bloqueios = 0.0M;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOCLIENTE_BMF"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pId_Cliente", DbType.Int32, Account);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        Bloqueios = (lDataTable.Rows[0]["VL_TOTMAR"]).DBToDecimal();
                    }
                }

                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DIN_SEL"))
                //{
                //    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, Account);

                //    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                //    if (lDataTable.Rows.Count > 0)
                //    {
                //        //decimal Garantias = (lDataTable.Rows[0]["VL_TOTGAR"]).DBToDecimal();
                //        //decimal Bloqueios = (lDataTable.Rows[0]["VL_TOTMAR"]).DBToDecimal();

                //        SaldoTotal = (lDataTable.Rows[0]["VL_DINHEIRO"]).DBToDecimal();
                //    }

                //}

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DET_SEL"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, Account);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoTotal += (lDataTable.Rows[0]["VL_GARANTIA"]).DBToDecimal();
                    }
                }

                return SaldoTotal - Bloqueios;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao calcular a posicao de BMF do cliente: " + Account.ToString() ,ex);

            }

            return SaldoTotal;

        }

        /// <summary>
        /// Método que busca o valor de contamargem do cliente no sinacor
        /// Procedure: prc_sel_cliente_contamargem
        /// </summary>
        /// <param name="IdCliente">Código Bovespa do cliente </param>
        /// <returns>Retorna o valor de conta margem do cliente</returns>
        public decimal ObterSaldoContaMargem(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoContaMargem = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoContaMargem;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_cliente_contamargem"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    lDbCommand.Connection.Close();

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        SaldoContaMargem = (lDataTable.Rows[0]["VL_LIMITE"]).DBToDecimal();
                    }
                    else
                    {
                        SaldoContaMargem = 0;
                    }
                }
                
                return SaldoContaMargem;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta margem do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        /// <summary>
        /// Método que busca no sinacor o valor de tesouro direto do cliente 
        /// Procedure: PRC_SEL_POSICAO_TESOURO
        /// </summary>
        /// <param name="CodigoCliente">Código bovespa do cliente</param>
        /// <returns>Retorna a somatória dos valores de tesouro direto do cliente</returns>
        public decimal ObterPosicaoTesouroDiretoCliente(int CodigoCliente)
        {
            AcessaDados acesso = new AcessaDados();

            decimal ValorTesouro = 0;

            try
            {
                acesso.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_POSICAO_TESOURO"))
                {

                    acesso.AddInParameter(cmd, "IdCliente", DbType.Int32, CodigoCliente);

                    DataTable table = acesso.ExecuteOracleDataTable(cmd);

                    foreach (DataRow dr in table.Rows)
                    {
                        ValorTesouro += dr["VAL_POSI"].DBToDecimal();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

            return ValorTesouro;
        }

        /// <summary>
        /// Método que busca no sql a listagem de posição de fundos do cliente.
        /// (IMPORTANTE) A Listagem é gerada somente a partir do SQL da posição só de ITAU.
        /// Futuramente é necessário efetuar a listagem também da Financial 
        /// para obter a posição completa dos clientes
        /// Procedure: cpfcnpj_sel_sp - Busca o cnpf/cnpj do cliente para posteriormente buscar a posição de cotista pelo cpf/cnpj
        /// Procedure: prc_sel_posicao_cotista - Busca o posição de cotista
        /// </summary>
        /// <param name="idCliente">Código principal do cliente</param>
        /// <returns>Retorna uma Listagem do Tipo ClienteFundoInfo (Fundos) </returns>
        public List<ClienteFundoInfo> ObterPosicaoFundoCliente(int CodigoCliente)
        {
            AcessaDados acesso = new AcessaDados();
            List<ClienteFundoInfo> lstFundos = new List<ClienteFundoInfo>();
            ClienteFundoInfo _ClienteFundoInfo = null;

            decimal ValorBruto = 0;

            try
            {
                acesso.ConnectionStringName = "Cadastro";
                string cpfcnpj = "";

                using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "cpfcnpj_sel_sp"))
                {
                    acesso.AddInParameter(cmd, "@cd_codigo", DbType.Int32, CodigoCliente);

                    DataTable table = acesso.ExecuteDbDataTable(cmd);

                    if (table.Rows.Count > 0)
                    {
                        DataRow dr = table.Rows[0];
                        cpfcnpj = dr["ds_cpfcnpj"].DBToString().PadLeft(15, '0');
                    }
                }

                acesso = new AcessaDados();

                acesso.ConnectionStringName = "ClubesFundos";

                if (!string.IsNullOrEmpty(cpfcnpj))
                {
                    using (DbCommand cmd = acesso.CreateCommand(CommandType.StoredProcedure, "prc_sel_posicao_cotista"))
                    {
                        acesso.AddInParameter(cmd, "@dsCpfCnpj", DbType.String, cpfcnpj);

                        DataTable table = acesso.ExecuteDbDataTable(cmd);

                        foreach (DataRow dr in table.Rows)
                        {
                            _ClienteFundoInfo = new ClienteFundoInfo();
                            _ClienteFundoInfo.NomeFundo = dr["dsRazaoSocial"].DBToString();
                            _ClienteFundoInfo.Saldo = dr["valorBruto"].DBToDecimal();
                            lstFundos.Add(_ClienteFundoInfo);
                        }
                    }
                }

                return lstFundos;

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// Método que busca no sinacor a relação de ordens bmf do cliente no after
        /// Procedure: PRC_RELACAO_ORDENS_AFTER
        /// </summary>
        /// <param name="IdCliente">Código bmf de cliente</param>
        /// <returns>Retorna a lista de objetos PosicaoBmfInfo de ordens bmf no after </returns>
        public List<PosicaoBmfInfo> ObtemPosicaoIntradayBMFAFTER(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoBmfInfo _PosicaoBmfInfo;
            List<PosicaoBmfInfo> PosicaoBMF = new List<PosicaoBmfInfo>();   

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_ORDENS_AFTER"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBmfInfo = new PosicaoBmfInfo();

                            _PosicaoBmfInfo.Cliente                 = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _PosicaoBmfInfo.Contrato                = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _PosicaoBmfInfo.QuantidadeContato       = (lDataTable.Rows[i]["qt_negocio"]).DBToInt32();
                            _PosicaoBmfInfo.PrecoAquisicaoContrato  = (lDataTable.Rows[i]["pr_negocio"]).DBToDecimal();
                            _PosicaoBmfInfo.DataOperacao            = (lDataTable.Rows[i]["dt_datmov"]).DBToDateTime();
                            _PosicaoBmfInfo.Sentido = (lDataTable.Rows[i]["cd_natope"]).DBToString();

                            PosicaoBMF.Add(_PosicaoBmfInfo);
                        }
                    }
                }

                return PosicaoBMF;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return PosicaoBMF;


        }

        /// <summary>
        /// Método que busca no sinacor a relação de ordens bmf do cliente 
        /// Procedure: PRC_RELACAO_ORDENS_BMF
        /// </summary>
        /// <param name="IdCliente">Código bmf do cliente</param>
        /// <returns>Retorna a lista de objetos PosicaoBmfInfo de ordens</returns>
        public List<PosicaoBmfInfo> ObtemPosicaoIntradayBMF(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            PosicaoBmfInfo _PosicaoBmfInfo;
            List<PosicaoBmfInfo> PosicaoBMF = new List<PosicaoBmfInfo>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_ORDENS_BMF"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _PosicaoBmfInfo = new PosicaoBmfInfo();

                            _PosicaoBmfInfo.Cliente                = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _PosicaoBmfInfo.Contrato               = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _PosicaoBmfInfo.QuantidadeContato      = (lDataTable.Rows[i]["qt_negocio"]).DBToInt32();
                            _PosicaoBmfInfo.PrecoAquisicaoContrato = (lDataTable.Rows[i]["pr_negocio"]).DBToDecimal();
                            _PosicaoBmfInfo.DataOperacao           = (lDataTable.Rows[i]["dt_datmov"]).DBToDateTime();
                            _PosicaoBmfInfo.Sentido                = (lDataTable.Rows[i]["cd_natope"]).DBToString();
                            _PosicaoBmfInfo.CodigoSerie            = (lDataTable.Rows[i]["cod_seri"]).DBToString();
                            _PosicaoBmfInfo.Contraparte            = (lDataTable.Rows[i]["cd_contrapar"]).DBToInt32();
                            
                            PosicaoBMF.Add(_PosicaoBmfInfo);
                        }
                    }
                }

                return PosicaoBMF;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return PosicaoBMF;


        }

        /// <summary>
        /// Método que busca no sinacor a relação de custódia de bovespa de abertura do cliente
        /// Procedure: prc_obter_custo_abertura_dia
        /// </summary>
        /// <param name="CodigoCliente">Código bovespa do cliente</param>
        /// <returns>Retorna Lista de objeto de CustodiaAberturaInfo do cliente</returns>
        public List<CustodiaAberturaInfo> ObterCustodiaAbertura(int CodigoCliente)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaAberturaInfo> _ListaCustodiaInfo = new List<CustodiaAberturaInfo>();
            CustodiaAberturaInfo _CustodiaInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_custo_abertura_dia"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, CodigoCliente); ;                 

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _CustodiaInfo = new CustodiaAberturaInfo();

                            _CustodiaInfo.Instrumento = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            _CustodiaInfo.Quantidade = (lDataTable.Rows[i]["qtde_atual"]).DBToInt32();
                            _CustodiaInfo.TipoMercado = (lDataTable.Rows[i]["tipo_merc"]).DBToString();
                            _CustodiaInfo.LoteNegociacao = (lDataTable.Rows[i]["nr_lotneg"]).DBToInt32();
                            _CustodiaInfo.CodigoCarteira = (lDataTable.Rows[i]["cod_cart"]).DBToInt32();
                            
                            _CustodiaInfo.CodigoCliente = CodigoCliente;    
                           
                            if (_CustodiaInfo.Quantidade != 0)
                            {
                                _ListaCustodiaInfo.Add(_CustodiaInfo);
                            }
                        }
                    }
                }      

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemCustodiaAbertura");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCustodiaInfo;

        }

        /// <summary>
        /// Método que busca no sinacor a relação de custódia de bmf do cliente
        /// Procedure: prc_custodia_bmf_monitor2
        /// </summary>
        /// <param name="CodigoCliente">Código bmf do cliente</param>
        /// <returns>Retorna a lista de objetos CustodiaAberturaInfo do cliente</returns>
        public List<CustodiaAberturaInfo> ObterCustodiaAberturaBMF(int CodigoCliente)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<CustodiaAberturaInfo> _ListaCustodiaInfo = new List<CustodiaAberturaInfo>();
            CustodiaAberturaInfo _CustodiaInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_custodia_bmf_monitor2"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, CodigoCliente); ;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _CustodiaInfo = new CustodiaAberturaInfo();

                            string Instrumento = string.Empty;

                            if ((lDataTable.Rows[i]["tipo_merc"]).DBToString().Equals("OPF"))
                            {
                                Instrumento = (lDataTable.Rows[i]["cod_neg"]).DBToString();
                            }
                            else
                            {
                                Instrumento = (lDataTable.Rows[i]["cod_comm"]).DBToString() + "" + (lDataTable.Rows[i]["cod_seri"]).DBToString();
                            }

                            _CustodiaInfo.Instrumento    = Instrumento;
                            _CustodiaInfo.Quantidade     = (lDataTable.Rows[i]["qtde_disp"]).DBToInt32();                          
                            _CustodiaInfo.TipoMercado    = "BMF";
                            _CustodiaInfo.LoteNegociacao = 1;
                            _CustodiaInfo.CodigoCarteira = 0;
                            _CustodiaInfo.CodigoSerie    = (lDataTable.Rows[i]["cod_seri"]).DBToString();
                            _CustodiaInfo.CodigoCliente  = CodigoCliente;

                            if (_CustodiaInfo.Quantidade != 0)
                            {
                                _ListaCustodiaInfo.Add(_CustodiaInfo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemCustodiaAbertura");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCustodiaInfo;

        }

        /// <summary>
        /// Método que busca no sinacor opção exercida do operador zero.
        /// Esse procedimento é necesspario pois na consulta que busca as operações não consegue pegar as 
        /// opperações de opções execídas do operador zero.
        /// E o setor de risco estava tendo carência de informções sobre este tipo de operação.
        /// Procedure: prc_obter_opcao_exercida_op0
        /// </summary>
        /// <param name="ClientID">Código bovespa do cliente</param>
        /// <returns>Retorna as operações de opções exercidas com o código do operador zero 0</returns>
        public List<PosicaoClienteIntradayInfo> ObtemPosicaoOpcaoExercidaOperadorZero(int ClientID)
        {
            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<PosicaoClienteIntradayInfo> _ListaCliente = new List<PosicaoClienteIntradayInfo>();
            PosicaoClienteIntradayInfo _itemCliente;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                logger.InfoFormat("Verificando Opção exercida do cliente [{0}] ", ClientID);

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_opcao_exercida_op0"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, ClientID);

                    DateTime dtPosi = DateTime.Now;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    TimeSpan dtFinal = (DateTime.Now - dtPosi);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _itemCliente = new PosicaoClienteIntradayInfo();

                            _itemCliente.CodigoCliente     = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _itemCliente.NomeCliente       = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            _itemCliente.Assessor          = (lDataTable.Rows[i]["cd_assessor"]).DBToString();
                            _itemCliente.PrecoMedioNegocio = (lDataTable.Rows[i]["VL_NEGOCIO"]).DBToDecimal();
                            _itemCliente.Quantidade        = (lDataTable.Rows[i]["QT_NEGOCIO"]).DBToInt32();
                            _itemCliente.Instrumento       = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            _itemCliente.SentidoOperacao   = (lDataTable.Rows[i]["CD_NATOPE"]).DBToChar();
                            _itemCliente.VolumeOperacao    = (lDataTable.Rows[i]["VALOR"]).DBToDecimal();
                            _itemCliente.Porta             = (lDataTable.Rows[i]["Porta"]).DBToString();
                            _itemCliente.InstrumentoOpcao  = (lDataTable.Rows[i]["CD_TITOBJ"]).DBToString();


                            string InstrumentoAux = _itemCliente.Instrumento;

                            if (_itemCliente.Instrumento.Substring(_itemCliente.Instrumento.Length - 1, 1) == "F")
                            {
                                InstrumentoAux = _itemCliente.Instrumento.Remove(_itemCliente.Instrumento.Length - 1);
                            }

                            _itemCliente.Instrumento = InstrumentoAux;

                            _ListaCliente.Add(_itemCliente);
                        }
                    }
                }

                TimeSpan stamp = (InitialDate - DateTime.Now);

            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObtemPosicaoOpcaoExercidaOperadorZero");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCliente;
        }

        /// <summary>
        /// Método que busca no sinacor as ordens de bovespa do cliente no dia(Intraday)
        /// Método importante para listagem das ordens com as informações de porta de operação, valor, Tipo de objeto.
        /// Procedure: prc_obter_movimento_operacao
        /// </summary>
        /// <param name="ClientID">Código bovespa do cliente</param>
        /// <returns>Retorna a listagem de operações do cliente no dia intranday</returns>
        public List<PosicaoClienteIntradayInfo> ObtemPosicaoIntraday(string ClientID)
        {

            DateTime InitialDate = DateTime.Now;

            AcessaDados lAcessaDados = new AcessaDados();

            List<PosicaoClienteIntradayInfo> _ListaCliente = new List<PosicaoClienteIntradayInfo>();
            PosicaoClienteIntradayInfo _itemCliente;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;
                //prc_obter_movimento_operacao
                //using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_movimento_risco"))
                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_obter_movimento_operacao"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodigoCliente", DbType.Int32, ClientID.DBToInt32());

                    DateTime dtPosi = DateTime.Now;

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    TimeSpan dtFinal = (DateTime.Now - dtPosi);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            _itemCliente = new PosicaoClienteIntradayInfo();

                            _itemCliente.CodigoCliente = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            _itemCliente.NomeCliente       = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            _itemCliente.Assessor          = (lDataTable.Rows[i]["cd_assessor"]).DBToString();
                            _itemCliente.PrecoMedioNegocio = (lDataTable.Rows[i]["VL_NEGOCIO"]).DBToDecimal();
                            _itemCliente.Quantidade        = (lDataTable.Rows[i]["QT_NEGOCIO"]).DBToInt32();
                            _itemCliente.Instrumento       = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            _itemCliente.SentidoOperacao   = (lDataTable.Rows[i]["CD_NATOPE"]).DBToChar();
                            _itemCliente.VolumeOperacao    = (lDataTable.Rows[i]["VALOR"]).DBToDecimal();
                            _itemCliente.Porta             = (lDataTable.Rows[i]["Porta"]).DBToString();
                            _itemCliente.InstrumentoOpcao = (lDataTable.Rows[i]["CD_TITOBJ"]).DBToString();

                            string InstrumentoAux = _itemCliente.Instrumento;

                            if (_itemCliente.Instrumento.Substring(_itemCliente.Instrumento.Length - 1, 1) == "F")
                            {
                                InstrumentoAux = _itemCliente.Instrumento.Remove(_itemCliente.Instrumento.Length - 1);
                            }

                            _itemCliente.Instrumento = InstrumentoAux;

                            _ListaCliente.Add(_itemCliente);
                        }
                    }
                }

                var lListaClienteOpcao = new List<PosicaoClienteIntradayInfo>();

                lListaClienteOpcao = ObtemPosicaoOpcaoExercidaOperadorZero(ClientID.DBToInt32());

                if (lListaClienteOpcao.Count > 0)
                {
                    logger.InfoFormat("Encontrado opção exercida no operador zero do cliente [{0}]", ClientID);
                    _ListaCliente.AddRange(lListaClienteOpcao);
                }

                TimeSpan stamp = (InitialDate - DateTime.Now);                
           
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método GetTradesIntraday");
                logger.Info("Descricao do erro: " + ex.Message);
                logger.Info("StackTrace do erro: " + ex.StackTrace);
                throw (ex);
            }

            return _ListaCliente;

        }

        /// <summary>
        /// Método que busca no sql o c limite operacional 
        /// Procedure: prc_sel_limite_agrupado_preco
        /// </summary>
        /// <param name="Account">Código bovespa do cliente</param>
        /// <returns>Lista de Limites de bovespa do cliente</returns>
        public LimitesInfo ObterLimiteOperacional(int Account)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            LimitesInfo _LimitsInfo = new LimitesInfo();
         

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSQL;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_sel_limite_agrupado_preco"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@idClient", DbType.Int32, Account);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {

                        decimal AllocatedBuyAvista = 0;
                        decimal AllocatedBuyOptions = 0;
                        decimal BuyAVista = 0;
                        decimal BuyOptions = 0;

                        decimal AllocatedSellAvista = 0;
                        decimal AllocatedSellOptions = 0;
                        decimal SellAVista = 0;
                        decimal SellOptions = 0;


                        BuyAVista = (lDataTable.Rows[0]["valor"]).DBToDecimal();
                        AllocatedBuyAvista = (lDataTable.Rows[0]["alocado"]).DBToDecimal();

                        BuyOptions = (lDataTable.Rows[1]["valor"]).DBToDecimal();
                        AllocatedBuyOptions = (lDataTable.Rows[1]["alocado"]).DBToDecimal();

                        SellAVista = (lDataTable.Rows[2]["valor"]).DBToDecimal();
                        AllocatedSellAvista = (lDataTable.Rows[2]["alocado"]).DBToDecimal();

                        SellOptions = (lDataTable.Rows[3]["valor"]).DBToDecimal();
                        AllocatedSellOptions = (lDataTable.Rows[3]["alocado"]).DBToDecimal();

                        _LimitsInfo.LimiteAVista = (BuyAVista + SellAVista);
                        _LimitsInfo.LimiteOpcoes = (BuyOptions + SellOptions);
                        _LimitsInfo.LimiteTotal = (_LimitsInfo.LimiteAVista + _LimitsInfo.LimiteOpcoes);
                        _LimitsInfo.LimiteDisponivel = (_LimitsInfo.LimiteTotal - (AllocatedBuyAvista + AllocatedSellAvista + AllocatedSellOptions));




                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return _LimitsInfo;

        }

        /// <summary>
        /// Método que busca no sinacor relação de clientes que operaram no dia
        /// Procedure: PRC_RELACAO_CLIENTE_ORDENS
        /// </summary>
        /// <returns>REtorna uma listagem com os clientes que operaram no dia</returns>
        public List<int> ObterClientesPosicaoDia()
        {      
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RELACAO_CLIENTE_ORDENS"))
                {                         
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);              

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int IdClient = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            lock (lstClientes)
                            {
                                lstClientes.Add(IdClient);
                            }
                        }
                    }

                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }

        /// <summary>
        /// Método que busca no sinacor relação de cliente que operaram nos ultimos 2 minutos
        /// Procedure: PRC_REL_CLIENTE_ORDENS_2MIN
        /// </summary>
        /// <returns>Retorna uma listagem da relação de clientes que operaram nos ultimos 2 minutos</returns>
        public List<int> ObterClientesPosicaoDiaLimitada()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_REL_CLIENTE_ORDENS_2MIN"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int IdClient = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            lock (lstClientes)
                            {
                                lstClientes.Add(IdClient);
                            }
                        }
                    }

                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }

        /// <summary>
        /// Método que busca no sinacor a relação de clientes que operaram bmf durante o after
        /// Procedure: prc_relacao_bmfafter
        /// </summary>
        /// <returns>Relação de clientes que operaram bmf durante o after</returns>
        public List<int> ObterClientesPosicaoBMFAfter()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<int> lstClientes = new List<int>();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_relacao_bmfafter"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            int IdClient = (lDataTable.Rows[i]["cd_cliente"]).DBToInt32();
                            lock (lstClientes)
                            {
                                lstClientes.Add(IdClient);
                            }
                        }
                    }

                }

                return lstClientes;
            }
            catch (Exception ex)
            {
                logger.Info("Ocorreu um erro ao acessar o método ObterClientesPosicaoDia.", ex);
            }

            return lstClientes;
        }

        /// <summary>
        /// Método que busca no sinacor os dados do cliente como nome, assessor, nome do assessor, e
        /// Código de bovespa e código de bmf
        /// Procedure: prc_obtem_cliente_asse_monitor
        /// </summary>
        /// <param name="CodigoCliente">Código Principal do cliente</param>
        /// <returns>Retorna o ovjeto ClienteInfo preenchido com os dados de cliente</returns>
        public ClienteInfo ObterDadosCliente(int CodigoCliente)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            ClienteInfo _ClienteInfo = new ClienteInfo();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cliente_asse_monitor"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0){
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string TipoCliente        = (lDataTable.Rows[i]["Tipo"]).DBToString();
                            _ClienteInfo.Assessor     = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            _ClienteInfo.NomeCliente  = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            _ClienteInfo.NomeAssessor = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();

                            if (TipoCliente == "BOVESPA"){
                                _ClienteInfo.CodigoBovespa = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            }
                            else{
                                _ClienteInfo.CodigoBMF = (lDataTable.Rows[i]["Codigo"]).DBToString();
                            }

                            _ClienteInfo.Assessor    = (lDataTable.Rows[i]["CD_ASSESSOR"]).DBToString();
                            _ClienteInfo.NomeCliente = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();                     

                        }
                    }

                }

                return _ClienteInfo;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// Método que busca no sinacor o código bmf do cliente
        /// Procedure: prc_obtem_cod_bmf_monitor
        /// </summary>
        /// <param name="CodigoCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o código bmf do cliente</returns>
        public int ObterContaBMF(int CodigoCliente)
        {

            AcessaDados lAcessaDados = new AcessaDados();
            int ContaBMF = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_obtem_cod_bmf_monitor")){
                    lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.AnsiString, CodigoCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0) {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++){
                            ContaBMF = (lDataTable.Rows[i]["Codigo"]).DBToInt32();             
                        }
                    }

                }

                return ContaBMF;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        /// <summary>
        /// Método que busca no sql no banco de dados de risco a cotação atual do instrumento
        /// Procedure: prc_cotacoes_monitor_item
        /// </summary>
        /// <param name="pInstrumento">Instrumento para buscar a cotação atualizada</param>
        /// <returns>Retorna o objeto Cotação valor preenchido com os valores de cotação atualizados.</returns>
        public CotacaoValor ObterCotacaoAtual(string pInstrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();
            
            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            CotacaoValor lRetorno = new CotacaoValor();

            lRetorno.ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;// gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor_item"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pAtivo", DbType.AnsiString, pInstrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);


                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            IdAtivo      = (lDataTable.Rows[i]["id_ativo"]).DBToString();
                            ValorCotacao = (lDataTable.Rows[i]["vl_ultima"]).DBToDecimal();

                            lRetorno.Ativo           = IdAtivo;
                            lRetorno.ValorCotacao    = ValorCotacao;
                            lRetorno.ValorFechamento = (lDataTable.Rows[i]["vl_fechamento"]).DBToDecimal();
                            lRetorno.ValorAbertura   = (lDataTable.Rows[i]["VL_abertura"]).DBToDecimal();
                            lRetorno.Variacao        = (lDataTable.Rows[i]["vl_oscilacao"]).DBToDecimal();
                            lRetorno.ValorAjuste     = (lDataTable.Rows[i]["vl_ajuste"].DBToDecimal());
                            lRetorno.ValorPU         = (lDataTable.Rows[i]["vl_pu"].DBToDecimal());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;

        }

        /// <summary>
        /// Método que busca no sql no banco de dados de risco a cotação atualizada do instrumento usado 
        /// para métodos estaticos
        /// Procedure: prc_cotacoes_monitor_item
        /// </summary>
        /// <param name="pInstrumento">Instrumento para buscar a cotação atualizada</param>
        /// <returns>Retorna o objeto cotação valor preenchido com os valores de cotação atualizados.</returns>
        public static CotacaoValor ObterCotacaoAtualStatic(string pInstrumento)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            //Hashtable htQuote = new Hashtable();

            decimal ValorCotacao = 0;
            string IdAtivo = string.Empty;

            CotacaoValor lRetorno = new CotacaoValor();

            lRetorno.ValorCotacao = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;// gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor_item"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@pAtivo", DbType.AnsiString, pInstrumento);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            IdAtivo      = (lDataTable.Rows[i]["id_ativo"]).DBToString();
                            ValorCotacao = (lDataTable.Rows[i]["vl_ultima"]).DBToDecimal();

                            lRetorno.Ativo = IdAtivo;
                            lRetorno.ValorCotacao = ValorCotacao;

                            //lock (htQuote)
                            //{
                            //    htQuote.Add(IdAtivo.Trim(), ValorCotacao);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return lRetorno;

        }

        /// <summary>
        /// Struct com os a cotação atual trazida do banco de dados
        /// </summary>
        public struct CotacaoValor
        {
            /// <summary>
            /// Ativo para buscar a cotação
            /// </summary>
            public string Ativo { get; set; }

            /// <summary>
            /// Valor da cotação retornada do banco de dados
            /// </summary>
            public decimal ValorCotacao { get; set; }

            /// <summary>
            /// Valor de Abertura retornada do banco de dados
            /// </summary>
            public decimal ValorAbertura { get; set; }

            /// <summary>
            /// Valor de fechamento retornado do banco de dados
            /// </summary>
            public decimal ValorFechamento { get; set; }

            /// <summary>
            /// Variação retornada do banco de dados
            /// </summary>
            public decimal Variacao { get; set; }

            /// <summary>
            /// HOra sda ultima cotação
            /// </summary>
            public DateTime HoraCotacao { get; set; }

            /// <summary>
            /// Valor do Ajuste para intrumentos BMF, retornado do banco de dados.
            /// </summary>
            public decimal ValorAjuste { get; set; }

            /// <summary>
            /// Valor de Preço Unitário para instrumentos BMF, retornado do banco de dados.
            /// </summary>
            public decimal ValorPU { get; set; }
        }

        /// <summary>
        /// Método que busca no sinacor as cotações de fechamento de todos os instrumentos 
        /// Procedure: prc_cotacoes_fech_monitor
        /// </summary>
        /// <returns>Retorna uma hashtable com os valores de cotação de fechamento de todos os instrumentos</returns>
        public Hashtable ObtemCotacaoFechamento()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_cotacoes_fech_monitor"))
                {
                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    lDbCommand.Connection.Close();

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            decimal ValorCotacao = (lDataTable.Rows[i]["vl_fechamento"]).DBToDecimal();

                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo, ValorCotacao);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return htQuote;

        }

        /// <summary>
        /// Método que busca no sinacor as cotações de abertura de todos os intrumentos
        /// Procedure: prc_cotacoes_abert_monitor
        /// </summary>
        /// <returns>Retorna uma hashtable com os valores de cotação de abertura de todos os instrumentos</returns>
        public Hashtable ObtemCotacaoAbertura()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_cotacoes_abert_monitor"))
                {
                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    lDbCommand.Connection.Close();

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["ds_ativo"]).DBToString();
                            decimal ValorCotacao = (lDataTable.Rows[i]["vl_abertura"]).DBToDecimal();

                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo, ValorCotacao);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return htQuote;

        }

        /// <summary>
        /// Método que busca no sinacor as cotações de Ajustes bmf de todos os intrumentos
        /// Procedure: prc_cotacoes_monitor_ajuste
        /// </summary>
        /// <returns>Retorna uma hashtable com os valores de cotação de abertura de todos os instrumentos</returns>
        public Hashtable ObtemCotacaoAjusteBmf()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_cotacoes_monitor_ajuste"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    lDbCommand.Connection.Close();

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["id_ativo"]).DBToString();
                            decimal ValorAjuste = (lDataTable.Rows[i]["vl_ajuste"]).DBToDecimal();

                            lock (htQuote)
                            {
                                htQuote.Add(IdAtivo, ValorAjuste);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return htQuote;
        }

        /// <summary>
        /// Método que busca no sinacor o tipo de mercado de todos os instrumentos
        /// Procedure: prc_tipo_mercado_papel
        /// </summary>
        /// <returns>Retorna uma hashtable com os tipos de mercado de cada instrumento.</returns>
        public Hashtable ObtemTipoMercadoInstrumento()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            Hashtable htQuote = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_tipo_mercado_papel"))
                {


                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {

                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string IdAtivo = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            string TipoMercado = (lDataTable.Rows[i]["cd_tpmerc"]).DBToString();

                            lock (htQuote)
                            {
                                if (!htQuote.Contains(IdAtivo))
                                {
                                    htQuote.Add(IdAtivo, TipoMercado);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return htQuote;

        }

        /// <summary>
        /// Método que busca no sql no banco de dados do risco a custodia de bovespa e bmf do cliente
        /// Essa listagem de custódia está sendo usada para efetuar uma calculo no método TotalCustodiaMonitorIntradiario.
        /// Procedure: prc_vcfposicao_risco_sel
        /// </summary>
        /// <param name="CodigoBovespa">Código bovespa do cliente</param>
        /// <param name="CodigoBmf">Código Bmf do cliente</param>
        /// <returns>Retorna uma listagem com a custódia intradiária do cliente</returns>
        public static List<MonitorCustodiaInfo.CustodiaPosicao> ConsultarCustodiaNormalSql(int CodigoBovespa, int CodigoBmf)
        {
            MonitorCustodiaInfo lRetorno = new MonitorCustodiaInfo();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gNomeConexaoGradualOMS;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_vcfposicao_risco_sel"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@Id_Cliente", DbType.Int32, CodigoBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "@Id_ClienteBMF", DbType.Int32, CodigoBmf);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        lRetorno.ListaCustodia.Add(new MonitorCustodiaInfo.CustodiaPosicao()
                        {
                            CodigoInstrumento = lDataTable.Rows[i]["COD_NEG"].DBToString(),
                            CodigoCarteira    = lDataTable.Rows[i]["COD_CART"].DBToInt32(),
                            DescricaoCarteira = lDataTable.Rows[i]["DESC_CART"].DBToString().Trim(),
                            TipoMercado       = lDataTable.Rows[i]["TIPO_MERC"].DBToString(),
                            TipoGrupo         = lDataTable.Rows[i]["TIPO_GRUP"].DBToString(),
                            IdCliente         = lDataTable.Rows[i]["COD_CLI"].DBToInt32(),
                            QtdeAtual         = lDataTable.Rows[i]["QTDE_ATUAL"].DBToDecimal(),
                            QtdeLiquidar      = lDataTable.Rows[i]["QTDE_LIQUID"].DBToDecimal(),
                            QtdeDisponivel    = lDataTable.Rows[i]["QTDE_DISP"].DBToDecimal(),
                            QtdeAExecVenda    = lDataTable.Rows[i]["QTDE_AEXE_VDA"].DBToDecimal(),
                            QtdeAExecCompra   = lDataTable.Rows[i]["QTDE_AEXE_CPA"].DBToDecimal(),
                            NomeEmpresa       = lDataTable.Rows[i]["NOME_EMP_EMI"].DBToString(),
                            ValorPosicao      = lDataTable.Rows[i]["VAL_POSI"].DBToDecimal(),
                            DtVencimento      = lDataTable.Rows[i]["DATA_VENC"].DBToDateTime(),
                            QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"].DBToDecimal(),
                            QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"].DBToDecimal(),
                            QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"].DBToDecimal(),
                            CodigoSerie       = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                            FatorCotacao      = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                            QtdeDATotal       = lDataTable.Rows[i]["QTDE_TOTAL"].DBToDecimal(),
                            Cotacao           = ObterCotacaoAtualStatic(lDataTable.Rows[i]["COD_NEG"].DBToString()).ValorCotacao
                        });
                    }
            }
            lAcessaDados.Dispose();
            lAcessaDados = null;
            return lRetorno.ListaCustodia;
        }

        /// <summary>
        /// Método que busca no sinacor o saldo de conta corrente D3 do cliente
        /// Procedure: PRC_SALDO_DIA_OMS
        /// </summary>
        /// <param name="IdCliente">Código bovespa do cliente </param>
        /// <returns>Retorna o saldo de conta corrente D3 </returns>
        private decimal ObterSaldoD3(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_DIA_OMS"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["vl_projet3"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
            finally
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
            }
        }

        /// <summary>
        /// Método que busca no sinacor o saldo de conta corrente D0 do cliente.
        /// Procedure: prc_saldo_negociavel_oms
        /// </summary>
        /// <param name="IdCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o saldo D0 de conta corrente do cliente</returns>
        public decimal ObterSaldoD0(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_saldo_negociavel_oms"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);

                    DataTable lDataTable =
                        lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        return (lDataTable.Rows[0]["vl_disponivel"]).DBToDecimal();
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
            finally
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
            }
        }

        /// <summary>
        /// Método que busca nos sinacor o Saldo de conta corrente D+1.
        /// Procedure: PRC_SALDOD1_NOTA
        /// </summary>
        /// <param name="IdCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o saldo D1 de conta corrente do cliente</returns>
        private decimal ObterSaldoD1(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoD1 = 0;
            decimal SaldoAberturaOpcoes = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOD1_NOTA"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);


                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoD1 = (lDataTable.Rows[0]["Opecacoes_D1"]).DBToDecimal();
                    }

                    SaldoAberturaOpcoes = this.AberturaDiaOpcoes(IdCliente);
                }

                return (SaldoAberturaOpcoes + SaldoD1);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
            finally
            {
                lAcessaDados.Dispose();
                lAcessaDados = null;
            }
        }

        /// <summary>
        /// Método que busca no sinacor o saldo de conta corrente em D1 da dbmfinan33.
        /// Procedure: PRC_SALDOD1_ABERTURA
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        private decimal AberturaDiaOpcoes(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            decimal SaldoAberturaD1 = 0;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDOD1_ABERTURA"))
                {

                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);
                    

                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        SaldoAberturaD1 = (lDataTable.Rows[0]["Abertura_D1"]).DBToDecimal();
                    }
                }

                return SaldoAberturaD1;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro ao acessar a Conta Corrente (D3)do cliente: {0} Erro: {1}", IdCliente.ToString(), ex.StackTrace));
            }
        }

        /// <summary>
        /// Método que busca no sinacor o saldo de conta corrente do cliente
        /// Procedure: PRC_SALDO_ABERTURA_OMS
        /// </summary>
        /// <param name="IdCliente">Código bovespa do cliente</param>
        /// <returns>Retorna o objeto ContaCorrentoInfo preenchido com os saldos de conta corrente do cliente</returns>
        public ContaCorrenteInfo ObterSaldoContaCorrente(int IdCliente)
        {
            AcessaDados lAcessaDados = new AcessaDados();
            ContaCorrenteInfo lRetorno = new ContaCorrenteInfo();

            try
            {

                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SALDO_ABERTURA_OMS"))
                {     
                    lAcessaDados.AddInParameter(lDbCommand, "pCodCliente", DbType.Int32, IdCliente);
                    
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (lDataTable.Rows.Count > 0)
                    {
                        lRetorno.SaldoD2 = (lDataTable.Rows[0]["VL_PROJET2"]).DBToDecimal();
                    }

                    lRetorno.SaldoD0 = this.ObterSaldoD0(IdCliente);
                    lRetorno.SaldoD1 = this.ObterSaldoD1(IdCliente);
                    lRetorno.SaldoD3 = this.ObterSaldoD3(IdCliente);                                              
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


            return lRetorno;
        }

        /// <summary>
        /// Método que busca no sql na base de dados do cadastro os dados de assessores vinculados.
        /// Procedure: ListarAssessoresVinculados_lst_sp
        /// </summary>
        /// <param name="CodigoAssessor">Código do assessor</param>
        /// <param name="CodigoLogin">Código de login do assessor</param>
        /// <returns>Retorna os Listagem de assessores vinculados com o assessor passado por parametro</returns>
        public List<string> ListarAssessoresVinculados(int? CodigoAssessor, int ?CodigoLogin)
        {
            var lRetorno = new List<string>();
            var lAcessaDados = new AcessaDados();
            
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "ListarAssessoresVinculados_lst_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_assessor", DbType.Int32, CodigoAssessor.Value);

                if (CodigoLogin.HasValue)
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@cd_login", DbType.Int32, CodigoLogin.Value);
                }

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(lLinha["cd_assessor"].DBToString());
                    }
                }
            }

            return lRetorno;
        }

        /// <summary>
        /// Método que atualiza a base de dados do gradualOMS com os dados de lucro prejuízo e patrimônio Liquido de cada cliente.
        /// Procedure: prc_atualiza_lucro_prejuizo_cliente
        /// </summary>
        /// <param name="pIdCliente">Código Bovespa do cliente</param>
        /// <param name="pLucroPrejuizo">Valoe de Lucro prejuizo do cliente</param>
        /// <param name="pPatrimonioLiquido">PAtrimonio liquido do cliente</param>
        public void AtualizaPosicaoRiscoLucroPrejuizo(int pIdCliente, decimal pLucroPrejuizo, decimal pPatrimonioLiquido )
        {
            AcessaDados lDados = new AcessaDados();
            
            try
            {
                lDados.ConnectionStringName = gNomeConexaoSQL;

                DbCommand lCommand = lDados.CreateCommand(CommandType.StoredProcedure, "prc_atualiza_lucro_prejuizo_cliente");

                lDados.AddInParameter(lCommand, "@cd_cliente"           , DbType.Int32,  pIdCliente);
                lDados.AddInParameter(lCommand, "@vl_lucro_prejuizo"    , DbType.Decimal, pLucroPrejuizo);
                lDados.AddInParameter(lCommand, "@vl_patrimonio_liquido", DbType.Decimal, pPatrimonioLiquido);

                // Executa a operação no banco.
                lDados.ExecuteNonQuery(lCommand);

                //lRetorno.IdFiltro = Convert.ToInt32(lDados.GetParameterValue(lCommand, "@id_filtro"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

    }
}
