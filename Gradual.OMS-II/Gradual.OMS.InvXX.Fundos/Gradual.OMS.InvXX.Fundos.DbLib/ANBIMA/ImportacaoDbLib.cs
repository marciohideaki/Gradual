using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA.LeituraArquivos.Info;
using Gradual.Generico.Dados;
using log4net;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA
{
    public class ImportacaoDbLib
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public DateTime SelecionaUltimoPregao()
        {
            DateTime lRetorno = new DateTime();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "SINACOR";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ULTIMO_PREGAO_SEL"))
                    {
                        DataTable dt = lAcessaDados.ExecuteOracleDataTable(lCommand);

                        foreach (DataRow dr in dt.Rows)
                        {
                            lRetorno = Convert.ToDateTime(dr["dt_ultimopregao"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SelecionaUltimoPregao = [{0}]", ex.StackTrace);
            }

            return lRetorno;
        }

        public List<ProdutosInfo> PesquisarProduto()
        {
            List<ProdutosInfo> lRes = new List<ProdutosInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_PESQUISAR_PRODUTO"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        foreach (DataRow dr in dt.Rows)
                        {
                            ProdutosInfo lpi         = new ProdutosInfo();
                            lpi.IdCodigoAnbima       = dr["idCodigoAnbima"].ToString();
                            lpi.IdProduto            = Convert.ToInt32(dr["idProduto"]);
                            lpi.NomeProduto          = dr["dsNomeProduto"].ToString();
                            lpi.Risco                = dr["dsRisco"].ToString();
                            lpi.HorarioLimite        = dr["hrLimMovimento"].ToString();
                            lpi.IdCodigoAnbima       = dr["IdCodigoAnbima"].ToString();
                            lpi.NomeArquivoProspecto = dr["pathProspecto"].ToString();
                            lpi.CpfCnpj              = dr["CPFCNPJ"].ToString();

                            lRes.Add(lpi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método PesquisarProduto = [{0}]", ex.StackTrace);
            }

            return lRes;
        }

        public List<ProdutosInfo> ListarProduto()
        {
            List<ProdutosInfo> lRes = new List<ProdutosInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto, idCodigoAnbima from tbProduto"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        foreach (DataRow dr in dt.Rows)
                        {
                            ProdutosInfo lpi = new ProdutosInfo();
                            lpi.IdCodigoAnbima = dr["idCodigoAnbima"].ToString();
                            lpi.IdProduto            = Convert.ToInt32(dr["idProduto"]);
                            //lpi.NomeProduto          = dr["dsNomeProduto"].ToString();
                            //lpi.Risco                = dr["dsRisco"].ToString();
                            //lpi.HorarioLimite        = dr["hrLimMovimento"].ToString();
                            //lpi.IdCodigoAnbima       = dr["IdCodigoAnbima"].ToString();
                            //lpi.NomeArquivoProspecto = dr["pathProspecto"].ToString();
                            //lpi.CpfCnpj              = dr["CPFCNPJ"].ToString();

                            lRes.Add(lpi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método PesquisarProduto = [{0}]", ex.StackTrace);
            }

            return lRes;
        }

        public void SalvarRentabilidadeDiaria(FundosDiaInfo pRequest)
        {
            
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RENTABILIDADE_DIARIA_INS"))
                {
                    lAcessaDados.AddInParameter(lCommand, "@CodigoANBIMA", DbType.String,   pRequest.CodigoFundo);
                    lAcessaDados.AddInParameter(lCommand, "@data",         DbType.DateTime, pRequest.Data);
                    lAcessaDados.AddInParameter(lCommand, "@vlrCota",      DbType.Decimal,  pRequest.ValorCota);
                    lAcessaDados.AddInParameter(lCommand, "@rentDia",      DbType.Decimal,  pRequest.RentabilidadeDia);
                    lAcessaDados.AddInParameter(lCommand, "@rentMes",      DbType.Decimal,  pRequest.RentabilidadeMes);
                    lAcessaDados.AddInParameter(lCommand, "@rentAno",      DbType.Decimal,  pRequest.RentabilidadeAno);
                    lAcessaDados.AddInParameter(lCommand, "@patrLiquido",  DbType.Decimal,  pRequest.Pl);

                    try
                    {
                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                    catch (Exception ex)
                    {
                        gLogger.ErrorFormat("Erro encontrado em SalvarRentabilidadeDiaria - [{0}]", ex.StackTrace);
                    }
                }
            }
        }

        public void SalvarProdutoMovimento(FundosMovCotaInfo pResquest)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TBPRODUTOMOVIMENTOCOTA_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@idProduto",             DbType.Int32,       pResquest.IdProduto);
                        lAcessaDados.AddInParameter(lCommand, "@data",                  DbType.DateTime,    pResquest.Data);
                        lAcessaDados.AddInParameter(lCommand, "@VlrMinAplicInicial",    DbType.Decimal,     pResquest.ValorMinimoAplicacaoInicial);
                        lAcessaDados.AddInParameter(lCommand, "@VlrMinAplicAdicional",  DbType.Decimal,     pResquest.ValorMinimoAplicacaoAdicional);
                        lAcessaDados.AddInParameter(lCommand, "@VlrMinResgate",         DbType.Decimal,     pResquest.ValorMinimoResgate);
                        lAcessaDados.AddInParameter(lCommand, "@VlrMinSaldo",           DbType.Decimal,     pResquest.ValorMinimoAplicacao);
                        lAcessaDados.AddInParameter(lCommand, "@dataAtualizacao",       DbType.DateTime,    pResquest.DataHora);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.ErrorFormat("Erro encontrado no método SalvarProdutoMovimento - [{0}] ", ex.StackTrace);
            }
        }

        public void  SalvarRentabilidadeMes(FundosMesInfo pRequest)
        {
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                try
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RENTABILIDADE_MES_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodANBIMA", DbType.String,   pRequest.CodigoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@mes",       DbType.Int32,    pRequest.DataMes);
                        lAcessaDados.AddInParameter(lCommand, "@ano",       DbType.Int32,    pRequest.DataAno);
                        lAcessaDados.AddInParameter(lCommand, "@pl",        DbType.Decimal,  pRequest.ValorPL);
                        lAcessaDados.AddInParameter(lCommand, "@valCota",   DbType.Decimal,  pRequest.ValorCota);
                        lAcessaDados.AddInParameter(lCommand, "@rentMes",   DbType.Decimal,  pRequest.RentabilidadeMes);
                        lAcessaDados.AddInParameter(lCommand, "@rentAno",   DbType.Decimal,  pRequest.RentabilidadeAno);
                        lAcessaDados.AddInParameter(lCommand, "@datahora",  DbType.DateTime, pRequest.DataHora);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
                catch (Exception ex)
                {
                    gLogger.ErrorFormat("Erro encontrado no método SalvarRentabilidadeMes  - [{0}]", ex.StackTrace);
                }
            }
            
        }

        public void SalvarTaxaAdministrador(TaxaAdministracaoInfo pRequest)
        {
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                try
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_TAXAADMINISTRADOR_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoFundo",           DbType.String,  pRequest.CodigoFundo);
                        lAcessaDados.AddInParameter(lCommand, "@DataInicio",            DbType.DateTime,pRequest.DataInicio);
                        lAcessaDados.AddInParameter(lCommand, "@TaxaFixa",              DbType.Double,  pRequest.TaxaFixa);
                        lAcessaDados.AddInParameter(lCommand, "@CobraTaxaPerfomance",   DbType.String,  pRequest.CobraTaxaPerfomance);
                        lAcessaDados.AddInParameter(lCommand, "@TaxaPerfomance",        DbType.String,  pRequest.TaxaPerfomance);
                        lAcessaDados.AddInParameter(lCommand, "@RegraTaxaPerformance",  DbType.String,  pRequest.RegraTaxaPerformance);
                        lAcessaDados.AddInParameter(lCommand, "@TaxaEntrada",           DbType.String,  pRequest.TaxaEntrada);
                        lAcessaDados.AddInParameter(lCommand, "@TaxaSaida",             DbType.String,  pRequest.TaxaSaida);
                        lAcessaDados.AddInParameter(lCommand, "@PeriodoCobTxPerf",      DbType.Int32,   pRequest.PeriodoCobTxPerf);
                        lAcessaDados.AddInParameter(lCommand, "@Unidade",               DbType.String,  pRequest.Unidade);
                        lAcessaDados.AddInParameter(lCommand, "@TaxaComposta",          DbType.String,  pRequest.TaxaComposta);
                        lAcessaDados.AddInParameter(lCommand, "@DataHora",              DbType.DateTime,pRequest.DataHora);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
                catch (Exception ex)
                {
                    gLogger.ErrorFormat("Erro encontrado no método SalvarRentabilidadeMes  - [{0}]", ex.StackTrace);
                }
            }
        }

        public void SalvarIndicadoresMes(ANBIMAIndicadoresMesInfo pRequest)
        {
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                try
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_INDICADORESMES_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoIndicador",   DbType.Int32,       pRequest.CodigoIndicador);
                        lAcessaDados.AddInParameter(lCommand, "@Mes",               DbType.Int32,       pRequest.Mes);
                        lAcessaDados.AddInParameter(lCommand, "@Ano",               DbType.Int32,       pRequest.Ano);
                        lAcessaDados.AddInParameter(lCommand, "@Volume",            DbType.Decimal,     pRequest.Volume);
                        lAcessaDados.AddInParameter(lCommand, "@Taxa",              DbType.Decimal,     pRequest.Taxa);
                        lAcessaDados.AddInParameter(lCommand, "@Indice",            DbType.Decimal,     pRequest.Indice);
                        lAcessaDados.AddInParameter(lCommand, "@Quantidade",        DbType.Decimal,     pRequest.Quantidade);
                        lAcessaDados.AddInParameter(lCommand, "@DataHora",          DbType.DateTime,    pRequest.DataHora);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
                catch (Exception ex)
                {
                    gLogger.ErrorFormat("Erro encontrado no método SalvarIndicadoresMes  - [{0}]", ex.StackTrace);
                }
            }
        }
    }
}
