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
using System.Data.Odbc;
using System.Configuration;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA
{
    public class ImportacaoDbLib
    {
        #region Atributos
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["SIANBIMA43"].ConnectionString;
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

        public void SalvarCadastroFundos(FundosInfo pRequest)
        {
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                try
                {
                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_FUNDOS_INS"))
                    {
                        gLogger.InfoFormat("Importando fundo Codigo [{0}] nome [{1}] ", pRequest.CodigoFundo, pRequest.NomeFantasia);

                        Nullable<DateTime> lDataFimFundo = null;
                        Nullable<DateTime> lDataInfo = null;

                        //(pRequest.DataFimFundo== DateTime.MinValue ? pRequest.DataFimFundo.Value : new Nullable<DateTime>()  )
                        //(pRequest.DataInfo.HasValue ? pRequest.DataInfo.Value : new Nullable<DateTime>())
                        if (pRequest.DataFimFundo.HasValue)
                        {
                            if (!pRequest.DataFimFundo.Equals(DateTime.MinValue))
                            {
                                lDataFimFundo = pRequest.DataFimFundo;
                            }
                            
                        }

                        if (pRequest.DataInfo.HasValue)
                        {
                            if (!pRequest.DataInfo.Equals(DateTime.MinValue))
                            {
                                lDataInfo = pRequest.DataInfo;
                            }
                        }

                        lAcessaDados.AddInParameter(lCommand,"@CodigoFundo" ,       DbType.String,      pRequest.CodigoFundo);
                        lAcessaDados.AddInParameter(lCommand,"@CodigoInstituicao" , DbType.String,      pRequest.CodInst);
                        lAcessaDados.AddInParameter(lCommand,"@NomeFanatasia" ,     DbType.String,      pRequest.NomeFantasia);
                        lAcessaDados.AddInParameter(lCommand,"@Gestor" ,            DbType.String,      pRequest.Gestor);
                        lAcessaDados.AddInParameter(lCommand,"@CodigoTipo" ,        DbType.Int32,       pRequest.CodigoTipo);
                        lAcessaDados.AddInParameter(lCommand,"@DataInicial" ,       DbType.DateTime,    pRequest.DataInicioFundo);
                        lAcessaDados.AddInParameter(lCommand,"@DataFim" ,           DbType.DateTime,    lDataFimFundo);
                        lAcessaDados.AddInParameter(lCommand,"@DataInfo" ,          DbType.DateTime,    lDataInfo);
                        lAcessaDados.AddInParameter(lCommand,"@PerfilCota" ,        DbType.String,      pRequest.PerfilCota);
                        lAcessaDados.AddInParameter(lCommand,"@DataDiv" ,           DbType.DateTime,    pRequest.DataDiv);
                        lAcessaDados.AddInParameter(lCommand,"@RazaoSocial" ,       DbType.String,      pRequest.RazaoSocial);
                        lAcessaDados.AddInParameter(lCommand,"@CNPJ" ,              DbType.String,      pRequest.DsCnpj);
                        lAcessaDados.AddInParameter(lCommand,"@Aberto" ,            DbType.String,      pRequest.Aberto);
                        lAcessaDados.AddInParameter(lCommand,"@Exclusivo" ,         DbType.String,      pRequest.Exclusivo);
                        lAcessaDados.AddInParameter(lCommand,"@PrazoEmissaoCotas" , DbType.String,      pRequest.PrazoEmissaoCotas);
                        lAcessaDados.AddInParameter(lCommand,"@PrazoConvResg" ,     DbType.String,      pRequest.PrazoConvResg);
                        lAcessaDados.AddInParameter(lCommand,"@PrazoPagtoResg" ,    DbType.String,      pRequest.PrazoPgtoResg);
                        lAcessaDados.AddInParameter(lCommand,"@CarenciaUniversal" , DbType.Int32,       pRequest.CarenciaUniversal);
                        lAcessaDados.AddInParameter(lCommand,"@CarenciaCiclica" ,   DbType.Int32,       pRequest.CarenciaCiclica);
                        lAcessaDados.AddInParameter(lCommand,"@CotaAbertura" ,      DbType.String,      pRequest.CotaAbertura);
                        lAcessaDados.AddInParameter(lCommand,"@PeriodoDivulgacao",  DbType.Int32,       pRequest.PeriodoDivulg);
                        lAcessaDados.AddInParameter(lCommand,"@DataHora",           DbType.DateTime,    pRequest.DataHora);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
                catch (Exception ex)
                {
                    gLogger.ErrorFormat("Erro encontrado no método SalvarCadastroFundos  - [{0}]", ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Importação das informações de Instituições da anbima para o sql
        /// </summary>
        public void ImportarFundosTipo()
        {
            List<ANBIMAFundosTipoInfo> ListaFundoTipo = null;

            string lSql = "Select * from fundos_tipo"; //SqlANBIMA.ListaFundoTipo;

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaFundoTipo = new List<Lib.ANBIMA.ANBIMAFundosTipoInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosTipoInfo lInfo = new ANBIMAFundosTipoInfo();

                lInfo.CodigoTipo    = Convert.ToInt32(lReader["Codtipo"].ToString());
                lInfo.Descricao     = lReader["descricao"].ToString();
                lInfo.Sigla         = lReader["sigla"].ToString();
                lInfo.DataIni       = Convert.ToDateTime(lReader["DataIni"].ToString());
                lInfo.DataFim       = lReader["DataFim"].DBToDateTime(eDateNull.Permite);
                lInfo.DataHora      = Convert.ToDateTime(lReader["DataHora"].ToString());

                ListaFundoTipo.Add(lInfo);
            }

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var tipo in ListaFundoTipo)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_FUNDOSTIPO_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoTipo",   DbType.Int32,       tipo.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@Descricao",    DbType.String,      tipo.Descricao);
                        lAcessaDados.AddInParameter(lComm, "@Sigla",        DbType.String,      tipo.Sigla);
                        lAcessaDados.AddInParameter(lComm, "@DataIni",      DbType.DateTime,    tipo.DataIni);
                        lAcessaDados.AddInParameter(lComm, "@DataFim",      DbType.DateTime,    tipo.DataFim);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime,    tipo.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        /// <summary>
        /// Importação das informações de Instituições da anbima para o sql
        /// </summary>
        public void ImportarInstituicoes()
        {
            try
            {
                List<ANBIMAInstituicoesInfo> ListaInstituicao = null;

                string lSql = SqlANBIMA.ListaInstituicao;

                using (OdbcConnection lConexao = new OdbcConnection(ConnectionString))
                {
                    using (OdbcCommand lCommand = new OdbcCommand(lSql, lConexao))
                    {
                        lConexao.Open();
                        using (OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            ListaInstituicao = new List<Lib.ANBIMA.ANBIMAInstituicoesInfo>();

                            while (lReader.Read())
                            {
                                ANBIMAInstituicoesInfo lInfo = new ANBIMAInstituicoesInfo();

                                lInfo.CodigoInstituicao = lReader["CodInst"].ToString();
                                lInfo.NomeFantasia      = lReader["fantasia"].ToString();
                                lInfo.CodigoMae         = lReader["CodMae"].ToString();
                                lInfo.InstituicaoMae    = lReader["instmae"].ToString();
                                lInfo.InstituicaoFinanceira = lReader["instfin"].ToString();
                                lInfo.Empab             = lReader["empab"].ToString();
                                lInfo.CodigoCvm         = lReader["codcvm"].ToString();
                                lInfo.InstituicaoAdm    = lReader["instadm"].ToString();
                                lInfo.DataHora          = Convert.ToDateTime(lReader["datahora"].ToString());

                                ListaInstituicao.Add(lInfo);

                            }
                        }
                    }
                }
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    foreach (var Inst in ListaInstituicao)
                    {
                        using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_INSTITUICAO_INS"))
                        {
                            lAcessaDados.AddInParameter(lComm, "@CodigoInstituicao",    DbType.String, Inst.CodigoInstituicao);
                            lAcessaDados.AddInParameter(lComm, "@NomeFantasia",         DbType.String, Inst.NomeFantasia);
                            lAcessaDados.AddInParameter(lComm, "@CodigoMae",            DbType.String, Inst.CodigoMae);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoMae",       DbType.String, Inst.InstituicaoMae);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoFinanceira", DbType.String, Inst.InstituicaoFinanceira);
                            lAcessaDados.AddInParameter(lComm, "@Empab",                DbType.String, Inst.Empab);
                            lAcessaDados.AddInParameter(lComm, "@CodigoCvm",            DbType.String, Inst.CodigoCvm);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoAdm",       DbType.String, Inst.InstituicaoAdm);
                            lAcessaDados.AddInParameter(lComm, "@DataHora",             DbType.DateTime, Inst.DataHora);

                            DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Importar Fundos do banco de dados da anbima para o sql
        /// </summary>
        public void ImportarFundos()
        {
            try
            {
                List<ANBIMAFundosInfo> ListaFundos = null;

                string lSql = "select * from fundos where datafim is null";

                OdbcConnection lConexao = new OdbcConnection(ConnectionString);

                OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

                lConexao.Open();

                OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

                ListaFundos = new List<Lib.ANBIMA.ANBIMAFundosInfo>();

                while (lReader.Read())
                {
                    ANBIMAFundosInfo lInfo = new ANBIMAFundosInfo();

                    lInfo.CodigoFundo       = lReader["codFundo"].ToString().PadLeft(6, '0');
                    lInfo.CodigoInstituicao = lReader["CodInst"].ToString();
                    lInfo.NomeFanatasia     = lReader["fantasia"].ToString();
                    lInfo.Gestor            = lReader["gestor"].ToString();
                    lInfo.CodigoTipo        = Convert.ToInt32(lReader["Codtipo"].ToString());
                    lInfo.DataInicial       = Convert.ToDateTime(lReader["DataIni"].ToString());
                    lInfo.DataFim           = lReader["DataFim"].DBToDateTime(eDateNull.Permite);
                    lInfo.DataInfo          = Convert.ToDateTime(lReader["DataInfo"].ToString());
                    lInfo.PerfilCota        = lReader["perfil_cota"].ToString();
                    lInfo.DataDiv           = Convert.ToDateTime(lReader["DataDiv"].ToString());
                    lInfo.RazaoSocial       = lReader["razaosoc"].ToString();
                    lInfo.CNPJ              = lReader["cnpj"].ToString();
                    lInfo.Aberto            = lReader["aberto"].ToString();
                    lInfo.Exclusivo         = lReader["exclusivo"].ToString();
                    lInfo.PrazoEmissaoCotas = lReader["prazo_emis_cotas"].ToString();
                    lInfo.PrazoConvResg     = lReader["prazo_conv_resg"].ToString();
                    lInfo.PrazoPagtoResg    = lReader["prazo_pgto_resg"].ToString();
                    lInfo.CarenciaUniversal = lReader["carencia_universal"].DBToInt32();
                    lInfo.CarenciaCiclica   = lReader["carencia_ciclica"].DBToInt32();
                    lInfo.CotaAbertura      = lReader["cota_abertura"].ToString();
                    lInfo.PeriodoDivulgacao = Convert.ToInt32(lReader["periodo_divulg"].ToString());
                    lInfo.DataHora          = Convert.ToDateTime(lReader["DataHora"].ToString());

                    ListaFundos.Add(lInfo);
                }

                lConexao.Close();

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    foreach (var fundos in ListaFundos)
                    {
                        using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_FUNDOS_INS"))
                        {
                            lAcessaDados.AddInParameter(lComm, "@CodigoFundo",          DbType.String,      fundos.CodigoFundo);
                            lAcessaDados.AddInParameter(lComm, "@CodigoInstituicao",    DbType.String,      fundos.CodigoInstituicao);
                            lAcessaDados.AddInParameter(lComm, "@NomeFanatasia",        DbType.String,      fundos.NomeFanatasia);
                            lAcessaDados.AddInParameter(lComm, "@Gestor",               DbType.String,      fundos.Gestor);
                            lAcessaDados.AddInParameter(lComm, "@CodigoTipo",           DbType.Int32,       fundos.CodigoTipo);
                            lAcessaDados.AddInParameter(lComm, "@DataInicial",          DbType.DateTime,    fundos.DataInicial);
                            lAcessaDados.AddInParameter(lComm, "@DataFim",              DbType.DateTime,    fundos.DataFim);
                            lAcessaDados.AddInParameter(lComm, "@DataInfo",             DbType.DateTime,    fundos.DataInfo);
                            lAcessaDados.AddInParameter(lComm, "@PerfilCota",           DbType.String,      fundos.PerfilCota);
                            lAcessaDados.AddInParameter(lComm, "@DataDiv",              DbType.DateTime,    fundos.DataDiv);
                            lAcessaDados.AddInParameter(lComm, "@RazaoSocial",          DbType.String,      fundos.RazaoSocial);
                            lAcessaDados.AddInParameter(lComm, "@CNPJ",                 DbType.String,      fundos.CNPJ);
                            lAcessaDados.AddInParameter(lComm, "@Aberto",               DbType.String,      fundos.Aberto);
                            lAcessaDados.AddInParameter(lComm, "@Exclusivo",            DbType.String,      fundos.Exclusivo);
                            lAcessaDados.AddInParameter(lComm, "@PrazoEmissaoCotas",    DbType.String,      fundos.PrazoEmissaoCotas);
                            lAcessaDados.AddInParameter(lComm, "@PrazoConvResg",        DbType.String,      fundos.PrazoConvResg);
                            lAcessaDados.AddInParameter(lComm, "@PrazoPagtoResg",       DbType.String,      fundos.PrazoPagtoResg);
                            lAcessaDados.AddInParameter(lComm, "@CarenciaUniversal",    DbType.Int32,       fundos.CarenciaUniversal);
                            lAcessaDados.AddInParameter(lComm, "@CarenciaCiclica",      DbType.Int32,       fundos.CarenciaCiclica);
                            lAcessaDados.AddInParameter(lComm, "@CotaAbertura",         DbType.String,      fundos.CotaAbertura);
                            lAcessaDados.AddInParameter(lComm, "@PeriodoDivulgacao",    DbType.Int32,       fundos.PeriodoDivulgacao);
                            lAcessaDados.AddInParameter(lComm, "@DataHora",             DbType.DateTime,    fundos.DataHora);

                            DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Importar Taxa de Adm do banco de dados da anbima para o sql
        /// </summary>
        public void ImportarTaxaAdm()
        {
            List<ANBIMATaxaAdmInfo> ListaTaxaAdm = null;

            string lSql = string.Format("Select * from taxa_adm");

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            try
            {
                lConexao.Open();

                OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

                ListaTaxaAdm = new List<Lib.ANBIMA.ANBIMATaxaAdmInfo>();

                while (lReader.Read())
                {
                    try
                    {
                        ANBIMATaxaAdmInfo lInfo = new ANBIMATaxaAdmInfo();

                        lInfo.CodigoFundo       = lReader["codFundo"].ToString().PadLeft(6, '0');
                        lInfo.Data              = Convert.ToDateTime(lReader["data"].ToString());
                        lInfo.TaxaFixa          = lReader["taxa_fixa"].ToString() == string.Empty ? 0 :  Convert.ToDecimal(lReader["taxa_fixa"].ToString());
                        lInfo.CobraTaxaPerform  = lReader["cobra_taxa_perform"].ToString();
                        lInfo.TaxaPerformance   = lReader["taxa_perform"].ToString();
                        lInfo.RegraTaxaPerform  = lReader["regra_taxa_perform"].ToString();
                        lInfo.TaxaEntrada       = lReader["taxa_entrada"].ToString();
                        lInfo.TaxaSaida         = lReader["taxa_saida"].ToString();
                        lInfo.PeriodoCobrancaTaxaPerform = lReader["period_cob_tx_perf"].DBToInt32();
                        lInfo.Unidade           = lReader["unidade"].ToString();
                        lInfo.TaxaComposta      = lReader["taxa_composta"].ToString();
                        lInfo.DataHora          = Convert.ToDateTime(lReader["DataHora"].ToString());

                        ListaTaxaAdm.Add(lInfo);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                lConexao.Close();

                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    foreach (var taxa in ListaTaxaAdm)
                    {
                        using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_TAXAADM_INS"))
                        {
                            try
                            {
                                lAcessaDados.AddInParameter(lComm, "@CodigoFundo",                  DbType.String,      taxa.CodigoFundo);
                                lAcessaDados.AddInParameter(lComm, "@Data",                         DbType.DateTime,    taxa.Data);
                                lAcessaDados.AddInParameter(lComm, "@TaxaFixa",                     DbType.Decimal,     taxa.TaxaFixa);
                                lAcessaDados.AddInParameter(lComm, "@CobraTaxaPerform",             DbType.String,      taxa.CobraTaxaPerform);
                                lAcessaDados.AddInParameter(lComm, "@TaxaPerformance",              DbType.String,      taxa.TaxaPerformance);
                                lAcessaDados.AddInParameter(lComm, "@RegraTaxaPerform",             DbType.String,      taxa.RegraTaxaPerform);
                                lAcessaDados.AddInParameter(lComm, "@TaxaEntrada",                  DbType.String,      taxa.TaxaEntrada);
                                lAcessaDados.AddInParameter(lComm, "@TaxaSaida",                    DbType.String,      taxa.TaxaSaida);
                                lAcessaDados.AddInParameter(lComm, "@PeriodoCobrancaTaxaPerform",   DbType.Int32,       taxa.PeriodoCobrancaTaxaPerform);
                                lAcessaDados.AddInParameter(lComm, "@Unidade",                      DbType.String,      taxa.Unidade);
                                lAcessaDados.AddInParameter(lComm, "@TaxaComposta",                 DbType.String,      taxa.TaxaComposta);
                                lAcessaDados.AddInParameter(lComm, "@DataHora",                     DbType.DateTime,    taxa.DataHora);

                                DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                            }
                            catch(Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lConexao.Close();

                throw ex;
            }
        }

        /// <summary>
        /// Importar Movimento Cota do banco de dados da anbima para o sql
        /// </summary>
        public void ImportarFundosMovimentoCota()
        {
            List<ANBIMAMovimentoCotaInfo> ListaMovimentoCota = null;

            string lSql = string.Format("Select * from fundos_mov_cota");

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaMovimentoCota = new List<Lib.ANBIMA.ANBIMAMovimentoCotaInfo>();

            while (lReader.Read())
            {
                ANBIMAMovimentoCotaInfo lInfo = new ANBIMAMovimentoCotaInfo();

                lInfo.CodigoFundo                   = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Data                          = lReader["data"].DBToDateTime();
                lInfo.ValorMinAplicacaoInicial      = lReader["vlr_min_aplic_ini"].DBToDecimal();
                lInfo.ValorMiniAplicacaoAdicional   = lReader["vlr_min_aplic_adic"].DBToDecimal();
                lInfo.ValorMiniResgate              = lReader["vlr_min_resgate"].DBToDecimal();
                lInfo.ValorMiniAplicacao            = lReader["vlr_min_aplic"].DBToDecimal();
                lInfo.Identificador                 = lReader["identificador"].ToString();
                lInfo.DataHora                      = lReader["DataHora"].DBToDateTime();

                ListaMovimentoCota.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var cota in ListaMovimentoCota)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_MOVIMENTOCOTA_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo",              DbType.String, cota.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data",                     DbType.DateTime, cota.Data);
                        lAcessaDados.AddInParameter(lComm, "@ValorMinAplicacaoInicial", DbType.Decimal, cota.ValorMinAplicacaoInicial);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacaoAdicional", DbType.Decimal, cota.ValorMiniAplicacaoAdicional);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniResgate",         DbType.Decimal, cota.ValorMiniResgate);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacao",       DbType.Decimal, cota.ValorMiniAplicacao);
                        lAcessaDados.AddInParameter(lComm, "@Identificador",            DbType.String, cota.Identificador);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",                 DbType.DateTime, cota.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        /// <summary>
        /// Importar Fundos Status do banco de dados da anbima para o sql
        /// </summary>
        public void ImportarFundosStatus()
        {
            List<ANBIMAFundosStatusInfo> ListaStatus = null;

            string lSql = "select * from fundos_status";

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaStatus = new List<Lib.ANBIMA.ANBIMAFundosStatusInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosStatusInfo lInfo = new ANBIMAFundosStatusInfo();

                lInfo.CodigoFundo   = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Status        = lReader["status"].ToString();
                lInfo.OpcoesStatus  = lReader["status"].ToString();
                lInfo.DataInicial   = lReader["dataini"].DBToDateTime();
                lInfo.DataFim       = lReader["datafim"].DBToDateTime(eDateNull.Permite);
                lInfo.DataHora      = lReader["DataHora"].DBToDateTime();

                ListaStatus.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();

                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var status in ListaStatus)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_STATUS_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo",  DbType.String, status.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Status",       DbType.String, status.Status);
                        lAcessaDados.AddInParameter(lComm, "@OpcoesStatus", DbType.String, status.OpcoesStatus);
                        lAcessaDados.AddInParameter(lComm, "@DataInicial",  DbType.DateTime, status.DataInicial);
                        lAcessaDados.AddInParameter(lComm, "@DataFim",      DbType.DateTime, status.DataFim);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, status.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        /// <summary>
        /// Importar dados de Rentabilidade Dia do banco de dados da anbima para o Sql
        /// </summary>
        public void ImportarRentabilidadeDia(DateTime DataInicial)
        {
            List<ANBIMARentabilidadeDiaInfo> ListaRentabilidadeDia = null;

            DateTime lDataInicial = new DateTime(2016, 1, 1);

            DateTime DataFinal = new DateTime(2016, 6, 30);

            string lSql = string.Format("Select * from fundos_dia where data between '{0}' and '{1}'", lDataInicial.ToString("yyyy-MM-dd"), DataFinal.ToString("yyyy-MM-dd") );

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeDia = new List<ANBIMARentabilidadeDiaInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeDiaInfo lInfo = new ANBIMARentabilidadeDiaInfo();

                lInfo.CodigoFundo   = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Data          = lReader["data"].DBToDateTime();
                lInfo.PL            = lReader["pl"].DBToDecimal();
                lInfo.ValorCota     = lReader["valcota"].DBToDecimal();
                lInfo.RentDia       = lReader["rentdia"].DBToDecimal();
                lInfo.RentMes       = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno       = lReader["rentano"].DBToDecimal();
                lInfo.DataHora      = lReader["datahora"].DBToDateTime();

                ListaRentabilidadeDia.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var rent in ListaRentabilidadeDia)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_RENTDIA_INS"))
                    {
                        try 
                        { 
                            lAcessaDados.AddInParameter(lComm, "@CodigoFundo",  DbType.String, rent.CodigoFundo);
                            lAcessaDados.AddInParameter(lComm, "@Data",         DbType.DateTime, rent.Data);
                            lAcessaDados.AddInParameter(lComm, "@PL",           DbType.Decimal, rent.PL);
                            lAcessaDados.AddInParameter(lComm, "@ValorCota",    DbType.Decimal, rent.ValorCota);
                            lAcessaDados.AddInParameter(lComm, "@RentDia",      DbType.Decimal, rent.RentDia);
                            lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal, rent.RentMes);
                            lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal, rent.RentAno);
                            lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, rent.DataHora);

                            DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                        }
                        catch (Exception ex) 
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Importar dados de Rentabilidade Mes do banco de dados da anbima para o Sql
        /// </summary>
        public void ImportarRentabilidadeMes(DateTime DataInicial)
        {
            List<ANBIMARentabilidadeMesInfo> ListaRentabilidadeMes = null;

            string lSql = string.Format("Select * from fundos_mes where ano > '{0}' and mes > '{1}'", 2011, 6);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeMes = new List<ANBIMARentabilidadeMesInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeMesInfo lInfo = new ANBIMARentabilidadeMesInfo();

                lInfo.CodigoFundo   = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Mes           = lReader["mes"].DBToInt32();
                lInfo.Ano           = lReader["ano"].DBToInt32();
                lInfo.PL            = lReader["pl"].DBToDecimal();
                lInfo.ValorCota     = lReader["valcota"].DBToDecimal();
                lInfo.RentMes       = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno       = lReader["rentano"].DBToDecimal();
                lInfo.CodigoTipo    = lReader["codtipo"].DBToInt32();
                lInfo.DataHora      = lReader["datahora"].DBToDateTime();

                ListaRentabilidadeMes.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var rent in ListaRentabilidadeMes)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_RENTMES_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo",  DbType.String, rent.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Mes",          DbType.Int32, rent.Mes);
                        lAcessaDados.AddInParameter(lComm, "@Ano",          DbType.Int32, rent.Ano);
                        lAcessaDados.AddInParameter(lComm, "@PL",           DbType.Decimal, rent.PL);
                        lAcessaDados.AddInParameter(lComm, "@ValorCota",    DbType.Decimal, rent.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal, rent.RentMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal, rent.RentAno);
                        lAcessaDados.AddInParameter(lComm, "@CodigoTipo",   DbType.Int32, rent.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, rent.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }
    }
}
