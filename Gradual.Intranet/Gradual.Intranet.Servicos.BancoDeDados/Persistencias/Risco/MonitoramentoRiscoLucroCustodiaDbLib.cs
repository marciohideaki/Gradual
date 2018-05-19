using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Intranet.Contratos.Dados.Risco;
using Gradual.OMS.Persistencia;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Collections;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Risco
{
    public static class MonitoramentoRiscoLucroCustodiaDbLib
    {
        private static string gNomeConexaoOracle = "Sinacor";

        public static ReceberObjetoResponse<MonitoramentoRiscoLucroCustodiaInfo> 
            ConsultarCustodiaNormal(ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<MonitoramentoRiscoLucroCustodiaInfo>();// new List<MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento>();
            var lAcessaDados = new AcessaDados();

            lRetorno.Objeto = new MonitoramentoRiscoLucroCustodiaInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET2"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.Objeto.ConsultaCdClienteBovespa);
                lAcessaDados.AddInParameter(lDbCommand, "IdClienteBMF", DbType.Int32, pParametros.Objeto.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Objeto.ListaMovimento.Add(new MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento()
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
                            NomeEmpresa       = lDataTable.Rows[i]["NOME_EMP_EMI"] .DBToString(),
                            ValorPosicao      = lDataTable.Rows[i]["VAL_POSI"].DBToDecimal(),
                            DtVencimento      = lDataTable.Rows[i]["DATA_VENC"]    .DBToDateTime(),
                            QtdeD1            = lDataTable.Rows[i]["QTDE_DA1"]     .DBToDecimal(),
                            QtdeD2            = lDataTable.Rows[i]["QTDE_DA2"]     .DBToDecimal(),
                            QtdeD3            = lDataTable.Rows[i]["QTDE_DA3"]     .DBToDecimal(),
                            CodigoSerie       = lDataTable.Rows[i]["COD_SERI"].DBToString(),
                            FatorCotacao      = lDataTable.Rows[i]["FAT_COT"].DBToDecimal(),
                            QtdeDATotal       = lDataTable.Rows[i]["QTDE_DATOTAL"].DBToDecimal(),
                        });
            }

            return lRetorno;
        }

        public static ReceberObjetoResponse<MonitoramentoRiscoLucroTaxaPTAXInfo> ObteCotacaoPtax()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            

            ReceberObjetoResponse<MonitoramentoRiscoLucroTaxaPTAXInfo> lSaldo = new ReceberObjetoResponse<MonitoramentoRiscoLucroTaxaPTAXInfo>();

            lSaldo.Objeto = new MonitoramentoRiscoLucroTaxaPTAXInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_OBTER_COTACAO_PTAX"))
            {
                DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                    {
                        lSaldo.Objeto.ValorPtax = (lDataTable.Rows[i]["VL_DOLVDA_ATU"]).DBToDecimal();
                    }
                }

            }

            return lSaldo;
        }


        public static ReceberObjetoResponse<MonitoramentoRiscoLucroVencimentosDI> ObterVencimentosDI()
        {
            AcessaDados lAcessaDados = new AcessaDados();

            string procedure = "prc_obter_relacao_DI";

            ReceberObjetoResponse<MonitoramentoRiscoLucroVencimentosDI> lRetorno = 
                new ReceberObjetoResponse<MonitoramentoRiscoLucroVencimentosDI>();

            lRetorno.Objeto = new MonitoramentoRiscoLucroVencimentosDI();

            Hashtable htVencimentoDI = new Hashtable();

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, procedure))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            string Instrumento = (lDataTable.Rows[i]["cd_codneg"]).DBToString();
                            DateTime Vencimento = (lDataTable.Rows[i]["DT_VENC"]).DBToDateTime();

                            htVencimentoDI.Add(Instrumento, Vencimento);
                        }
                    }

                }

                lRetorno.Objeto.VencimentosDI = htVencimentoDI;

                return lRetorno;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }


        public static ReceberObjetoResponse<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo>
            ConsultarCustodiaPosicao(ReceberEntidadeRequest<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo>();// new List<MonitoramentoRiscoLucroCustodiaInfo.CustodiaMovimento>();
            var lAcessaDados = new AcessaDados();

            lRetorno.Objeto = new MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo();

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CUSTODIA_INTRANET_DIA"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "IdCliente", DbType.Int32, pParametros.Objeto.ConsultaCdClienteBMF);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao lPosicao =
                            new MonitoramentoRiscoLucroCustodiaPosicaoDiaBmfInfo.CustodiaPosicao();

                        lPosicao.CodigoInstrumento = lDataTable.Rows[i]["CD_NEGOCIO"].DBToString();
                        lPosicao.TipoMercado       = "FUT";
                        lPosicao.TipoGrupo         = "FUT";
                        lPosicao.IdCliente         = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32();
                        lPosicao.QtdeAtual         = 0;
                        lPosicao.QtdeDisponivel    = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();
                        lPosicao.CodigoSerie       = lDataTable.Rows[i]["CD_SERIE"].DBToString();

                        string lSentido = lDataTable.Rows[i]["CD_NATOPE"].DBToString();

                        lPosicao.Sentido = lSentido;

                        if (lSentido.Equals("V"))
                        {
                            lPosicao.QtdeAExecVenda = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                            lPosicao.PrecoNegocioVenda = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                        }
                        else if (lSentido.Equals("C"))
                        {
                            lPosicao.QtdeAExecCompra = lDataTable.Rows[i]["QT_NEGOCIO"].DBToDecimal();

                            lPosicao.PrecoNegocioCompra = lDataTable.Rows[i]["PR_NEGOCIO"].DBToDecimal();
                        }
                        
                        lRetorno.Objeto.ListaPosicaoDia.Add(lPosicao);
                    }
            }

            return lRetorno;
        }

        public static ReceberObjetoResponse<MonitorRiscoGarantiaBMFInfo> 
            ConsultarFinanceiroGarantiaBMF(ReceberEntidadeRequest<MonitorRiscoGarantiaBMFInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();
            ReceberObjetoResponse<MonitorRiscoGarantiaBMFInfo>  lRetorno = new ReceberObjetoResponse<MonitorRiscoGarantiaBMFInfo>();

            lRetorno.Objeto = new MonitorRiscoGarantiaBMFInfo();

            lRetorno.Objeto.ListaGarantias = new List<MonitorRiscoGarantiaBMFInfo>();

            lRetorno.Objeto.CodigoClienteBmf = pParametros.Objeto.CodigoClienteBmf;

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.Objeto.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Objeto.ListaGarantias.Add(new MonitorRiscoGarantiaBMFInfo()
                        {
                            CodigoClienteBmf      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),

                        });
                }
            }

            ConsultarFinanceiroGarantiaDinheiro(ref lRetorno);

            return lRetorno;
        }

        public static ReceberObjetoResponse<MonitorRiscoGarantiaBMFOuroInfo>
            ReceberFinanceiroGarantiaBMFOuro(ReceberEntidadeRequest<MonitorRiscoGarantiaBMFOuroInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();
            ReceberObjetoResponse<MonitorRiscoGarantiaBMFOuroInfo> lRetorno = new ReceberObjetoResponse<MonitorRiscoGarantiaBMFOuroInfo>();

            lRetorno.Objeto                   = new MonitorRiscoGarantiaBMFOuroInfo();

            lRetorno.Objeto.ListaGarantias    = new List<MonitorRiscoGarantiaBMFOuroInfo>();

            lRetorno.Objeto.CodigoClienteBmf  = pParametros.Objeto.CodigoClienteBmf;

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GAR_OURO_BMF_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.Objeto.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Objeto.ListaGarantias.Add(new MonitorRiscoGarantiaBMFOuroInfo()
                        {
                            CodigoClienteBmf      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),

                        });
                }
            }

            return lRetorno;
        }

        private static void ConsultarFinanceiroGarantiaDinheiro(ref ReceberObjetoResponse<MonitorRiscoGarantiaBMFInfo> lRetorno)
        {
            var lAcessaDados = new AcessaDados();
            
            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BMF_DIN_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, lRetorno.Objeto.CodigoClienteBmf);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                    {
                        if (lDataTable.Rows[i]["VL_DINHEIRO"].DBToDecimal() != 0)
                        {
                            lRetorno.Objeto.ListaGarantias.Add(new MonitorRiscoGarantiaBMFInfo()
                            {
                                CodigoClienteBmf = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                                DescricaoGarantia = "VALOR DINHEIRO",
                                ValorGarantiaDeposito = lDataTable.Rows[i]["VL_DINHEIRO"].DBToDecimal(),
                            });
                        }

                    }
                }
            }

        }

        public static ReceberObjetoResponse<MonitorRiscoGarantiaBovespaInfo>
            ConsultarFinanceiroGarantiaBovespa(ReceberEntidadeRequest<MonitorRiscoGarantiaBovespaInfo> pParametros)
        {
            var lAcessaDados = new AcessaDados();
            ReceberObjetoResponse<MonitorRiscoGarantiaBovespaInfo> lRetorno = new ReceberObjetoResponse<MonitorRiscoGarantiaBovespaInfo>();

            lRetorno.Objeto = new MonitorRiscoGarantiaBovespaInfo();

            lRetorno.Objeto.ListaGarantias = new List<MonitorRiscoGarantiaBovespaInfo>();

            lRetorno.Objeto.CodigoClienteBov = pParametros.Objeto.CodigoClienteBov;

            lAcessaDados.ConnectionStringName = gNomeConexaoOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_GARANTIAS_BOV_DET_SEL"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "pCliente", DbType.Int32, pParametros.Objeto.CodigoClienteBov);

                var lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < lDataTable.Rows.Count; i++)
                        lRetorno.Objeto.ListaGarantias.Add(new MonitorRiscoGarantiaBovespaInfo()
                        {
                            CodigoClienteBov      = lDataTable.Rows[i]["CD_CLIENTE"].DBToInt32(),
                            DescricaoGarantia     = lDataTable.Rows[i]["DESCRICAO"].DBToString(),
                            ValorGarantiaDeposito = lDataTable.Rows[i]["VL_GARANTIA"].DBToDecimal(),
                            DtDeposito            = lDataTable.Rows[i]["DT_DEPOSITO"].DBToDateTime(),
                            Quantidade            = lDataTable.Rows[i]["QTDE_GARN"].DBToInt32(),
                            CodigoAtividade       = lDataTable.Rows[i]["COD_ATIV"].DBToString(),
                            FinalidadeGarantia    = lDataTable.Rows[i]["DESC_FINL_GARN"].DBToString(),
                            CodigoIsin            = lDataTable.Rows[i]["COD_ISIN"].DBToString(),
                            CodigoDistribuicao    = lDataTable.Rows[i]["NUM_DIST"].DBToInt32(),
                            NomeEmpresa           = lDataTable.Rows[i]["NOME_EMPR"].DBToString(),

                        });
                }
            }


            return lRetorno;
        }


    }
}
