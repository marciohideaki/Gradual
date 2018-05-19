using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Common;
using Gradual.Monitores.Compliance.Lib;
using Gradual.Generico.Dados;
using System.Data;
using Gradual.Monitores.Compliance;

namespace Gradual.Monitores.Persistencia
{
   
    public class PersistenciaCompliance
    {
        private const string gNomeConexaoSinacor = "SINACOR";
        private const string gNomeConexaoSQL = "Risco";
        private const string gNomeConexaoContaMargem = "CONTAMARGEM";

        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PersistenciaCompliance()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public List<OrdensAlteradasCabecalhoInfo> ObterCabecalhoOrdensAlteradasDayTrade()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<OrdensAlteradasCabecalhoInfo> lstOrdensAlteradaCabecalho = new List<OrdensAlteradasCabecalhoInfo>();
            OrdensAlteradasCabecalhoInfo OrdensAlteradaCabecalho;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_header_ordensalteradas_c1"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdensAlteradaCabecalho = new OrdensAlteradasCabecalhoInfo();

                            string DayTarde = (lDataTable.Rows[i]["in_negocio"]).DBToString();


                            OrdensAlteradaCabecalho.NumeroSeqOrdem = (lDataTable.Rows[i]["NR_SEQORD"]).DBToInt32();
                            OrdensAlteradaCabecalho.DayTrade = true;
                            OrdensAlteradaCabecalho.Justificativa = (lDataTable.Rows[i]["nm_justif"]).DBToString();
                            OrdensAlteradaCabecalho.DataHoraOrdem = (lDataTable.Rows[i]["DT_horord"]).DBToDateTime();
                            OrdensAlteradaCabecalho.TipoMercado = (lDataTable.Rows[i]["CD_MERCAD"]).DBToString();

                            lstOrdensAlteradaCabecalho.Add(OrdensAlteradaCabecalho);

                        }
                    }

                }

                return lstOrdensAlteradaCabecalho;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdensAlteradaCabecalho;

        }

        public List<OrdensAlteradasInfo> ObterOrdensAlteradasIntraday()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<OrdensAlteradasInfo> lstOrdensAlterada = new List<OrdensAlteradasInfo>();
            OrdensAlteradasInfo OrdensAlterada;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_corpo_ordensalteradas_c1"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdensAlterada = new OrdensAlteradasInfo();
                            
                            OrdensAlterada.NumeroSeqOrdem  = (lDataTable.Rows[i]["NR_SEQORD"]).DBToInt32();

                            OrdensAlterada.DataAlteracao   = (lDataTable.Rows[i]["DataAlteracao"]).DBToDateTime();
                            OrdensAlterada.Assessor        = (lDataTable.Rows[i]["NM_ASSESSOR"]).DBToString();
                            OrdensAlterada.CodigoCliente   = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();

                            string ContaErro = 
                                (lDataTable.Rows[i]["CTA_ERRO"]).DBToString();

                            if (ContaErro != "")
                            {
                                OrdensAlterada.ContaErro = true;
                            }
                            else
                            {
                                OrdensAlterada.ContaErro = false;
                            }

                            string Vinculado =
                                (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();

                            if (Vinculado == "S")
                            {
                                OrdensAlterada.Vinculado = true;
                            }
                            else
                            {
                                OrdensAlterada.Vinculado = false;
                            }

                            OrdensAlterada.DescontoCorretagem = (lDataTable.Rows[i]["PC_REDACR"]).DBToDecimal();
                            OrdensAlterada.Instrumento        = (lDataTable.Rows[i]["CD_NEGOCIO"]).DBToString();
                            OrdensAlterada.Quantidade         = (lDataTable.Rows[i]["QT_ORDEM"]).DBToInt32();
                            OrdensAlterada.Sentido            = (lDataTable.Rows[i]["CD_NATOPE"]).DBToString();
                            OrdensAlterada.TipoPessoa         = (lDataTable.Rows[i]["TP_PESSOA"]).DBToString();
                            OrdensAlterada.Usuario            = (lDataTable.Rows[i]["NM_CLIENTE"]).DBToString();
                            OrdensAlterada.UsuarioAlteracao   = (lDataTable.Rows[i]["NM_USUARIO_ALT"]).DBToString();                            

                            lstOrdensAlterada.Add(OrdensAlterada);


                        }
                    }

                }

                return lstOrdensAlterada;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdensAlterada;

        }

        public List<EstatisticaDayTradeBovespaInfo> ObterEstatisticaDayTradeBovespa()
        {
            AcessaDados lAcessaDados = new AcessaDados();
            List<EstatisticaDayTradeBovespaInfo> lstOrdens = new List<EstatisticaDayTradeBovespaInfo>();
            EstatisticaDayTradeBovespaInfo OrdemInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, " prc_estatistica_daytrade_bov"))
                {                  
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdemInfo = new EstatisticaDayTradeBovespaInfo();

                            OrdemInfo.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();

                            string TipoBolsa =  (lDataTable.Rows[i]["bolsa"]).DBToString();

                            if (TipoBolsa == "BVSP"){
                                OrdemInfo.TipoBolsa = EnumBolsaDayTrade.BOVESPA;
                            }
                            else{
                                OrdemInfo.TipoBolsa = EnumBolsaDayTrade.BMF;
                            }

                            OrdemInfo.PessoaVinculada = (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();
                            OrdemInfo.Idade           = (lDataTable.Rows[i]["idade"]).DBToInt32();
                            OrdemInfo.NomeCliente     = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            OrdemInfo.CodigoAssessor  = (lDataTable.Rows[i]["cd_assessor"]).DBToInt32();
                            OrdemInfo.NomeAssessor    = (lDataTable.Rows[i]["nm_assessor"]).DBToString();
                            OrdemInfo.QuantidadeDayTrade = (lDataTable.Rows[i]["Quantidade"]).DBToInt32();
                            OrdemInfo.QuantidadeDayTradePositivo = (lDataTable.Rows[i]["QTDE_POSITIVO"]).DBToInt32();
                            OrdemInfo.PercentualPositivo = (lDataTable.Rows[i]["PERCENT_POSITIVO"]).DBToDecimal();
                            OrdemInfo.ValorPositivo = (lDataTable.Rows[i]["vlr_positivo"]).DBToDecimal();
                            OrdemInfo.ValorNegativo = (lDataTable.Rows[i]["vlr_negativo"]).DBToDecimal();
                            OrdemInfo.NET = (lDataTable.Rows[i]["net"]).DBToDecimal();

                            lstOrdens.Add(OrdemInfo);

                        }
                    }

                }

                return lstOrdens;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdens;

        }

        public List<NegociosDiretosInfo> ObterNegociosDiretos()
        {

            AcessaDados lAcessaDados = new AcessaDados();
            List<NegociosDiretosInfo> lstOrdens = new List<NegociosDiretosInfo>();
            NegociosDiretosInfo OrdemInfo;

            try
            {
                lAcessaDados.ConnectionStringName = gNomeConexaoSinacor;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_operacoesDiretas"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteOracleDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            OrdemInfo = new NegociosDiretosInfo();

                            OrdemInfo.CodigoCliente = (lDataTable.Rows[i]["CD_CLIENTE"]).DBToInt32();
                            OrdemInfo.DataNegocio = (lDataTable.Rows[i]["DT_NEGOCIO"]).DBToDateTime();
                            OrdemInfo.Instrumento = (lDataTable.Rows[i]["cd_negocio"]).DBToString();
                            OrdemInfo.Sentido = (lDataTable.Rows[i]["cd_natope"]).DBToString();
                            OrdemInfo.NomeCliente = (lDataTable.Rows[i]["nm_cliente"]).DBToString();
                            OrdemInfo.PessoaVinculada = (lDataTable.Rows[i]["IN_PESS_VINC"]).DBToString();
                            OrdemInfo.NumeroNegocio = (lDataTable.Rows[i]["NR_NEGOCIO"]).DBToInt32();
                                       
        
                            lstOrdens.Add(OrdemInfo);

                        }
                    }

                }

                return lstOrdens;
            }
            catch (Exception ex)
            {
                throw (ex);

            }

            return lstOrdens;

        }

    }
}
