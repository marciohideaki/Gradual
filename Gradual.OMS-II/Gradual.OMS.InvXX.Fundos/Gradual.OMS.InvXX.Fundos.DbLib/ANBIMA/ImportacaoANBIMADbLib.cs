using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.InvXX.Fundos.Lib.ANBIMA;
using System.Data.Odbc;
using log4net;
using System.Data;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Configuration;

namespace Gradual.OMS.InvXX.Fundos.DbLib.ANBIMA
{
    public class ImportacaoANBIMADbLib
    {
        #region Propriedades
        private const string ConnectionStringName  = "SIANBIMA43";
        private static string ConnectionString      = ConfigurationManager.ConnectionStrings["SIANBIMA43"].ConnectionString;
        private static readonly ILog gLogger       = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Métodos
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

                                lInfo.CodigoInstituicao     = lReader["CodInst"].ToString();
                                lInfo.NomeFantasia          = lReader["fantasia"].ToString();
                                lInfo.CodigoMae             = lReader["CodMae"].ToString();
                                lInfo.InstituicaoMae        = lReader["instmae"].ToString();
                                lInfo.InstituicaoFinanceira = lReader["instfin"].ToString();
                                lInfo.Empab                 = lReader["empab"].ToString();
                                lInfo.CodigoCvm             = lReader["codcvm"].ToString();
                                lInfo.InstituicaoAdm        = lReader["instadm"].ToString();
                                lInfo.DataHora              = Convert.ToDateTime(lReader["datahora"].ToString());

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
                            lAcessaDados.AddInParameter(lComm, "@CodigoInstituicao", DbType.String, Inst.CodigoInstituicao);
                            lAcessaDados.AddInParameter(lComm, "@NomeFantasia", DbType.String, Inst.NomeFantasia);
                            lAcessaDados.AddInParameter(lComm, "@CodigoMae", DbType.String, Inst.CodigoMae);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoMae", DbType.String, Inst.InstituicaoMae);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoFinanceira", DbType.String, Inst.InstituicaoFinanceira);
                            lAcessaDados.AddInParameter(lComm, "@Empab", DbType.String, Inst.Empab);
                            lAcessaDados.AddInParameter(lComm, "@CodigoCvm", DbType.String, Inst.CodigoCvm);
                            lAcessaDados.AddInParameter(lComm, "@InstituicaoAdm", DbType.String, Inst.InstituicaoAdm);
                            lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, Inst.DataHora);

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

        public void ImportarFundosTipo()
        {
            List<ANBIMAFundosTipoInfo> ListaFundoTipo = null;

            string lSql = SqlANBIMA.ListaFundoTipo;

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaFundoTipo = new List<Lib.ANBIMA.ANBIMAFundosTipoInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosTipoInfo lInfo = new ANBIMAFundosTipoInfo();

                lInfo.CodigoTipo         = Convert.ToInt32(lReader["Codtipo"].ToString());
                lInfo.Descricao          = lReader["descricao"].ToString();
                lInfo.Sigla              = lReader["sigla"].ToString();
                lInfo.DataIni            = Convert.ToDateTime( lReader["DataIni"].ToString());
                lInfo.DataFim            = lReader["DataFim"].DBToDateTime(eDateNull.Permite);
                lInfo.DataHora           = Convert.ToDateTime( lReader["DataHora"].ToString());

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
                        lAcessaDados.AddInParameter(lComm, "@CodigoTipo", DbType.Int32, tipo.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@Descricao", DbType.String, tipo.Descricao);
                        lAcessaDados.AddInParameter(lComm, "@Sigla", DbType.String, tipo.Sigla);
                        lAcessaDados.AddInParameter(lComm, "@DataIni", DbType.DateTime, tipo.DataIni);
                        lAcessaDados.AddInParameter(lComm, "@DataFim", DbType.DateTime, tipo.DataFim);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, tipo.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }

        }

        public void ImportarFundos(string CodigoAnbima)
        {
            List<ANBIMAFundosInfo> ListaFundos = null;

            string lSql = string.Format( SqlANBIMA.ListaFundos, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaFundos = new List<Lib.ANBIMA.ANBIMAFundosInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosInfo lInfo = new ANBIMAFundosInfo();

                lInfo.CodigoFundo       = lReader["codFundo"].ToString().PadLeft(6,'0');
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
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo", DbType.String, fundos.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@CodigoInstituicao", DbType.String, fundos.CodigoInstituicao);
                        lAcessaDados.AddInParameter(lComm, "@NomeFanatasia", DbType.String, fundos.NomeFanatasia);
                        lAcessaDados.AddInParameter(lComm, "@Gestor", DbType.String, fundos.Gestor);
                        lAcessaDados.AddInParameter(lComm, "@CodigoTipo", DbType.Int32, fundos.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@DataInicial", DbType.DateTime, fundos.DataInicial);
                        lAcessaDados.AddInParameter(lComm, "@DataFim", DbType.DateTime, fundos.DataFim);
                        lAcessaDados.AddInParameter(lComm, "@DataInfo", DbType.DateTime, fundos.DataInfo);
                        lAcessaDados.AddInParameter(lComm, "@PerfilCota", DbType.String, fundos.PerfilCota);
                        lAcessaDados.AddInParameter(lComm, "@DataDiv", DbType.DateTime, fundos.DataDiv);
                        lAcessaDados.AddInParameter(lComm, "@RazaoSocial", DbType.String, fundos.RazaoSocial);
                        lAcessaDados.AddInParameter(lComm, "@CNPJ", DbType.String, fundos.CNPJ);
                        lAcessaDados.AddInParameter(lComm, "@Aberto", DbType.String, fundos.Aberto);
                        lAcessaDados.AddInParameter(lComm, "@Exclusivo", DbType.String, fundos.Exclusivo);
                        lAcessaDados.AddInParameter(lComm, "@PrazoEmissaoCotas", DbType.String, fundos.PrazoEmissaoCotas);
                        lAcessaDados.AddInParameter(lComm, "@PrazoConvResg", DbType.String, fundos.PrazoConvResg);
                        lAcessaDados.AddInParameter(lComm, "@PrazoPagtoResg", DbType.String, fundos.PrazoPagtoResg);
                        lAcessaDados.AddInParameter(lComm, "@CarenciaUniversal", DbType.Int32, fundos.CarenciaUniversal);
                        lAcessaDados.AddInParameter(lComm, "@CarenciaCiclica", DbType.Int32, fundos.CarenciaCiclica);
                        lAcessaDados.AddInParameter(lComm, "@CotaAbertura", DbType.String, fundos.CotaAbertura);
                        lAcessaDados.AddInParameter(lComm, "@PeriodoDivulgacao", DbType.Int32, fundos.PeriodoDivulgacao);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, fundos.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarTaxaAdm(string CodigoAnbima)
        {
            List<ANBIMATaxaAdmInfo> ListaTaxaAdm = null;

            string lSql = string.Format(SqlANBIMA.ListaTaxaAdm, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaTaxaAdm = new List<Lib.ANBIMA.ANBIMATaxaAdmInfo>();

            while (lReader.Read())
            {
                ANBIMATaxaAdmInfo lInfo = new ANBIMATaxaAdmInfo();

                lInfo.CodigoFundo                = lReader["codFundo"].ToString().PadLeft(6,'0');
                lInfo.Data                       = Convert.ToDateTime(lReader["data"].ToString());
                lInfo.TaxaFixa                   = Convert.ToDecimal( lReader["taxa_fixa"].ToString());
                lInfo.CobraTaxaPerform           = lReader["cobra_taxa_perform"].ToString();
                lInfo.TaxaPerformance            = lReader["taxa_perform"].ToString();
                lInfo.RegraTaxaPerform           = lReader["regra_taxa_perform"].ToString();
                lInfo.TaxaEntrada                = lReader["taxa_entrada"].ToString();
                lInfo.TaxaSaida                  = lReader["taxa_saida"].ToString();
                lInfo.PeriodoCobrancaTaxaPerform = lReader["period_cob_tx_perf"].DBToInt32();
                lInfo.Unidade                    = lReader["unidade"].ToString();
                lInfo.TaxaComposta               = lReader["taxa_composta"].ToString();
                lInfo.DataHora                   = Convert.ToDateTime(lReader["DataHora"].ToString());

                ListaTaxaAdm.Add(lInfo);
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
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo", DbType.String, taxa.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data", DbType.DateTime, taxa.Data);
                        lAcessaDados.AddInParameter(lComm, "@TaxaFixa", DbType.Decimal, taxa.TaxaFixa);
                        lAcessaDados.AddInParameter(lComm, "@CobraTaxaPerform", DbType.String, taxa.CobraTaxaPerform);
                        lAcessaDados.AddInParameter(lComm, "@TaxaPerformance", DbType.String, taxa.TaxaPerformance);
                        lAcessaDados.AddInParameter(lComm, "@RegraTaxaPerform", DbType.String, taxa.RegraTaxaPerform);
                        lAcessaDados.AddInParameter(lComm, "@TaxaEntrada", DbType.String, taxa.TaxaEntrada);
                        lAcessaDados.AddInParameter(lComm, "@TaxaSaida", DbType.String, taxa.TaxaSaida);
                        lAcessaDados.AddInParameter(lComm, "@PeriodoCobrancaTaxaPerform", DbType.Int32, taxa.PeriodoCobrancaTaxaPerform);
                        lAcessaDados.AddInParameter(lComm, "@Unidade", DbType.String, taxa.Unidade);
                        lAcessaDados.AddInParameter(lComm, "@TaxaComposta", DbType.String, taxa.TaxaComposta);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, taxa.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarFundosMovimentoCota(string CodigoAnbima)
        {
            List<ANBIMAMovimentoCotaInfo> ListaMovimentoCota = null;

            string lSql = string.Format(SqlANBIMA.ListaMovimentoCota, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaMovimentoCota = new List<Lib.ANBIMA.ANBIMAMovimentoCotaInfo>();

            while (lReader.Read())
            {
                ANBIMAMovimentoCotaInfo lInfo = new ANBIMAMovimentoCotaInfo();

                lInfo.CodigoFundo                 = lReader["codFundo"].ToString().PadLeft(6,'0');
                lInfo.Data                        = lReader["data"].DBToDateTime();
                lInfo.ValorMinAplicacaoInicial    = lReader["vlr_min_aplic_ini"].DBToDecimal();
                lInfo.ValorMiniAplicacaoAdicional = lReader["vlr_min_aplic_adic"].DBToDecimal();
                lInfo.ValorMiniResgate            = lReader["vlr_min_resgate"].DBToDecimal();
                lInfo.ValorMiniAplicacao          = lReader["vlr_min_aplic"].DBToDecimal();
                lInfo.Identificador               = lReader["identificador"].ToString();
                lInfo.DataHora                    = lReader["DataHora"].DBToDateTime() ;

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
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo", DbType.String, cota.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data", DbType.DateTime, cota.Data);
                        lAcessaDados.AddInParameter(lComm, "@ValorMinAplicacaoInicial", DbType.Decimal, cota.ValorMinAplicacaoInicial);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacaoAdicional", DbType.Decimal, cota.ValorMiniAplicacaoAdicional);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniResgate", DbType.Decimal, cota.ValorMiniResgate);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacao", DbType.Decimal, cota.ValorMiniAplicacao);
                        lAcessaDados.AddInParameter(lComm, "@Identificador", DbType.String, cota.Identificador);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, cota.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarFundosStatus(string CodigoAnbima)
        {
            List<ANBIMAFundosStatusInfo> ListaStatus = null;

            string lSql = string.Format(SqlANBIMA.ListaFundoStatus, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaStatus = new List<Lib.ANBIMA.ANBIMAFundosStatusInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosStatusInfo lInfo = new ANBIMAFundosStatusInfo();

                lInfo.CodigoFundo  = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Status       = lReader["status"].ToString();
                lInfo.OpcoesStatus = lReader["status"].ToString();
                lInfo.DataInicial  = lReader["dataini"].DBToDateTime();
                lInfo.DataFim      = lReader["datafim"].DBToDateTime(eDateNull.Permite);
                lInfo.DataHora     = lReader["DataHora"].DBToDateTime();

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
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo", DbType.String, status.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Status", DbType.String, status.Status);
                        lAcessaDados.AddInParameter(lComm, "@OpcoesStatus", DbType.String, status.OpcoesStatus);
                        lAcessaDados.AddInParameter(lComm, "@DataInicial", DbType.DateTime, status.DataInicial);
                        lAcessaDados.AddInParameter(lComm, "@DataFim", DbType.DateTime, status.DataFim);
                        lAcessaDados.AddInParameter(lComm, "@DataHora", DbType.DateTime, status.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarRentabilidadeDia(string CodigoAnbima, DateTime DataInicial)
        {
            List<ANBIMARentabilidadeDiaInfo> ListaRentabilidadeDia = null;

            string lSql = string.Format(SqlANBIMA.ListaRentabilidadeDia, CodigoAnbima, DataInicial.ToString("yyyy-MM-dd"));

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeDia = new List<ANBIMARentabilidadeDiaInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeDiaInfo lInfo = new ANBIMARentabilidadeDiaInfo();

                lInfo.CodigoFundo = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Data        = lReader["data"].DBToDateTime();
                lInfo.PL          = lReader["pl"].DBToDecimal();
                lInfo.ValorCota   = lReader["valcota"].DBToDecimal();
                lInfo.RentDia     = lReader["rentdia"].DBToDecimal();
                lInfo.RentMes     = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno     = lReader["rentano"].DBToDecimal();
                lInfo.DataHora    = lReader["datahora"].DBToDateTime();

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
                        lAcessaDados.AddInParameter(lComm, "@CodigoFundo",  DbType.String,   rent.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data",         DbType.DateTime, rent.Data);
                        lAcessaDados.AddInParameter(lComm, "@PL",           DbType.Decimal,  rent.PL);
                        lAcessaDados.AddInParameter(lComm, "@ValorCota",    DbType.Decimal,  rent.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentDia",      DbType.Decimal,  rent.RentDia);
                        lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal,  rent.RentMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal,  rent.RentAno);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, rent.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarRentabilidadeMes(string CodigoAnbima, DateTime DataInicial)
        {
            List<ANBIMARentabilidadeMesInfo> ListaRentabilidadeMes = null;

            string lSql = string.Format(SqlANBIMA.ListaRentabilidadeMes, CodigoAnbima, DataInicial.ToString("yyyy-MM-dd"));

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeMes = new List<ANBIMARentabilidadeMesInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeMesInfo lInfo = new ANBIMARentabilidadeMesInfo();

                lInfo.CodigoFundo = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Mes         = lReader["mes"].DBToInt32();
                lInfo.Ano         = lReader["ano"].DBToInt32();
                lInfo.PL          = lReader["pl"].DBToDecimal();
                lInfo.ValorCota   = lReader["valcota"].DBToDecimal();
                lInfo.RentMes     = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno     = lReader["rentano"].DBToDecimal();
                lInfo.CodigoTipo  = lReader["codtipo"].DBToInt32();
                lInfo.DataHora    = lReader["datahora"].DBToDateTime();

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

        public void ImportarNotaExplicatoria()
        {}

        public void ImportarFundosNotaExplicatoria()
        {}

        public void ImportarFundosMes()
        {}

        public void ImportarFundosConsolidadoDia()
        {}

        public void ImportarFundosConsolidadoMes()
        {}

        public void ImportarPLInstituicao()
        {}

        public void ImportarEspecie()
        {}

        public void ImportarAplicacaoAgregada()
        {}

        public void ImportarIndicadores()
        {
            List<ANBIMAIndicadoresInfo> ListaIndicadores = null;

            string lSql = string.Format(SqlANBIMA.ListaIndicadores);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaIndicadores = new List<ANBIMAIndicadoresInfo>();

            while (lReader.Read())
            {
                var lInfo = new ANBIMAIndicadoresInfo();

                lInfo.CodigoIndicador    = lReader["CodInd"].DBToInt32();
                lInfo.DescricaoIndicador = lReader["descricao"].ToString();
                lInfo.Valores            = lReader["valores"].ToString();
                lInfo.Volume             = lReader["volume"].ToString();
                lInfo.Taxa               = lReader["taxa"].ToString();
                lInfo.Indice             = lReader["indice"].ToString();
                lInfo.Quantidade         = lReader["quantidade"].ToString();
                lInfo.DataHora           = lReader["datahora"].DBToDateTime();

                ListaIndicadores.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var ind in ListaIndicadores)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_INDICADORES_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoIndicador",      DbType.Int32,       ind.CodigoIndicador);
                        lAcessaDados.AddInParameter(lComm, "@DescricaoIndicador",   DbType.String,      ind.DescricaoIndicador);
                        lAcessaDados.AddInParameter(lComm, "@Valores",              DbType.String,      ind.Valores);
                        lAcessaDados.AddInParameter(lComm, "@Volume",               DbType.String,      ind.Volume);
                        lAcessaDados.AddInParameter(lComm, "@Taxa",                 DbType.String,      ind.Taxa);
                        lAcessaDados.AddInParameter(lComm, "@Indice",               DbType.String,      ind.Indice);
                        lAcessaDados.AddInParameter(lComm, "@Quantidade",           DbType.String,      ind.Quantidade);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",             DbType.DateTime,    ind.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarIndicadoresMes(DateTime DataInicial)
        {
            List<ANBIMAIndicadoresMesInfo> ListaIndicadores = null;

            string lSql = string.Format(SqlANBIMA.ListaIndicadoresMes, DataInicial.ToString("yyyy-MM-dd"));

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaIndicadores = new List<ANBIMAIndicadoresMesInfo>();

            while (lReader.Read())
            {
                var lInfo = new ANBIMAIndicadoresMesInfo();

                lInfo.CodigoIndicador    = lReader["CodInd"].DBToInt32();
                lInfo.Mes                = lReader["Mes"].DBToInt32();
                lInfo.Ano                = lReader["Ano"].DBToInt32();
                lInfo.Volume             = lReader["volume"].DBToDecimal();
                lInfo.Taxa               = lReader["taxa"].DBToDecimal();
                lInfo.Indice             = lReader["indice"].DBToDecimal();
                lInfo.Quantidade         = lReader["quantidade"].DBToDecimal();
                lInfo.DataHora           = lReader["datahora"].DBToDateTime();

                ListaIndicadores.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var ind in ListaIndicadores)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_INDICADORESMES_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoIndicador",  DbType.Int32,       ind.CodigoIndicador);
                        lAcessaDados.AddInParameter(lComm, "@Mes",              DbType.Int32,       ind.Mes);
                        lAcessaDados.AddInParameter(lComm, "@Ano",              DbType.Int32,       ind.Ano);
                        lAcessaDados.AddInParameter(lComm, "@Volume",           DbType.Decimal,     ind.Volume);
                        lAcessaDados.AddInParameter(lComm, "@Taxa",             DbType.Decimal,     ind.Taxa);
                        lAcessaDados.AddInParameter(lComm, "@Indice",           DbType.Decimal,     ind.Indice);
                        lAcessaDados.AddInParameter(lComm, "@Quantidade",       DbType.Decimal,     ind.Quantidade);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",         DbType.DateTime,    ind.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarFundosDia()
        {}

        public void ImportarFundosDia1()
        { }
        #endregion

        #region Métodos Importação Site
        public void ImportarFundosSite(string CodigoAnbima)
        {
            List<ANBIMAFundosSiteInfo> ListaFundos = null;

            string lSql = string.Format(SqlANBIMA.ListaFundosSite, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaFundos = new List<Lib.ANBIMA.ANBIMAFundosSiteInfo>();

            while (lReader.Read())
            {
                ANBIMAFundosSiteInfo lInfo    = new ANBIMAFundosSiteInfo();

                lInfo.CodigoFundo             = lReader["idCodigoAnbima"].ToString().PadLeft(6, '0');
                lInfo.NomeFanatasia           = lReader["dsNomeProduto"].ToString();
                lInfo.CNPJ                    = lReader["CPFCNPJ"].ToString();
                lInfo.DescricaoClienteAlvo    = lReader["dsClienteAlvo"].DBToString();
                lInfo.Gestor                  = lReader["gestor"].ToString();
                lInfo.Administrador           = lReader["Administrador"].DBToString();
                lInfo.Tributacao              = lReader["Tributacao"].DBToString();
                lInfo.IdTipoAnbima            = lReader["idTipoAmbima"].DBToInt32();
                lInfo.FocoAtuacao             = lReader["FocoAtuacao"].ToString();
                lInfo.ClasseCVM               = lReader["categoria"].ToString();
                lInfo.PermiteNovasAplicacaoes = lReader["stPermitirNovasAplicacoes"].ToString();
                lInfo.ClienteAlvo             = lReader["dsClienteAlvo"].ToString();
                lInfo.DataFim                 = lReader["DataFim"].DBToDateTime(eDateNull.Permite);
                lInfo.DataInicial             = lReader["dataini"].DBToDateTime();
                lInfo.DataInfo                = Convert.ToDateTime(lReader["DataInfo"].ToString());

                ListaFundos.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var fundos in ListaFundos)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_FUNDOS_SITE_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@idCodigoAnbima",               DbType.String,      fundos.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@dsNomeProduto",                DbType.String,      fundos.NomeFanatasia);
                        lAcessaDados.AddInParameter(lComm, "@CPFCNPJ",                      DbType.String,      fundos.CNPJ);
                        lAcessaDados.AddInParameter(lComm, "@dsClienteAlvo",                DbType.String,      fundos.DescricaoClienteAlvo);
                        lAcessaDados.AddInParameter(lComm, "@Administrador",                DbType.String,      fundos.Administrador);
                        lAcessaDados.AddInParameter(lComm, "@Gestor",                       DbType.String,      fundos.Gestor);
                        lAcessaDados.AddInParameter(lComm, "@Custodiante",                  DbType.String,      fundos.Custodiante);
                        lAcessaDados.AddInParameter(lComm, "@Auditoria",                    DbType.String,      fundos.Auditoria);
                        lAcessaDados.AddInParameter(lComm, "@Tributacao",                   DbType.String,      fundos.Tributacao);
                        lAcessaDados.AddInParameter(lComm, "@idTipoAnbima",                 DbType.Int32,       fundos.IdTipoAnbima);
                        //lAcessaDados.AddInParameter(lComm, "@Tributacao",                   DbType.String,      fundos.FocoAtuacao);
                        lAcessaDados.AddInParameter(lComm, "@stPermitirNovasAplicacoes",    DbType.String,      fundos.PermiteNovasAplicacaoes);
                        lAcessaDados.AddInParameter(lComm, "@dataInicio",                   DbType.DateTime,    fundos.DataInicial);
                        lAcessaDados.AddInParameter(lComm, "@dataFim",                      DbType.DateTime,    fundos.DataFim);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarMovimentoCotaSite(string CodigoAnbima)
        {
            List<ANBIMAMovimentoCotaSiteInfo> ListaMovimentoCota = null;

            string lSql = string.Format(SqlANBIMA.ListaMovimentoCotaSite, CodigoAnbima);

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaMovimentoCota = new List<Lib.ANBIMA.ANBIMAMovimentoCotaSiteInfo>();

            while (lReader.Read())
            {
                ANBIMAMovimentoCotaSiteInfo lInfo = new ANBIMAMovimentoCotaSiteInfo();
                lInfo.CodigoProduto                  = this.GetCodigoProduto(CodigoAnbima);
                lInfo.CodigoFundo                    = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Data                           = lReader["data"].DBToDateTime();
                lInfo.ValorMinAplicacaoInicial       = lReader["vlr_min_aplic_ini"].DBToDecimal();
                lInfo.ValorMiniAplicacaoAdicional    = lReader["vlr_min_aplic_adic"].DBToDecimal();
                lInfo.ValorMiniResgate               = lReader["vlr_min_resgate"].DBToDecimal();
                lInfo.ValorMiniAplicacao             = lReader["vlr_min_aplic"].DBToDecimal();
                lInfo.Identificador                  = lReader["identificador"].ToString();
                lInfo.DiasConversaoAplicacao         = "";
                lInfo.DiasConversaoResgate           = lReader["prazo_conv_resg"].ToString();
                lInfo.DiasConversaoResgateAntecipado = "";//lReader[""].ToString();
                lInfo.DiasPagamentoResgate           = lReader["prazo_pgto_resg"].ToString();
                lInfo.ValorTaxaAdministracao         = lReader["taxa_fixa"].DBToDecimal();
                lInfo.ValorTaxaAdministracaoMaxima   = "";// lReader[""].ToString();
                lInfo.ValorTaxaResgateAntecipado     = "";// lReader[""].ToString();
                lInfo.ValorPatrimonioLiquido         = lReader["PL"].DBToDecimal();
                lInfo.CobraTaxaPerformance           = lReader["cobra_taxa_perform"].ToString();
                lInfo.ValorTaxaPerformance           = lReader["taxa_perform"].ToString();
                lInfo.DataHora                       = lReader["DataHora"].DBToDateTime();

                ListaMovimentoCota.Add(lInfo);
            }

            lConexao.Close();
            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var cota in ListaMovimentoCota)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_ANBIMA_MOVIMENTOCOTA_SITE_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@idProduto",                        DbType.Int32,       cota.CodigoProduto);
                        lAcessaDados.AddInParameter(lComm, "@Data",                             DbType.DateTime,    cota.Data);
                        lAcessaDados.AddInParameter(lComm, "@ValorMinAplicacaoInicial",         DbType.Decimal,     cota.ValorMinAplicacaoInicial);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacaoAdicional",      DbType.Decimal,     cota.ValorMiniAplicacaoAdicional);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniResgate",                 DbType.Decimal,     cota.ValorMiniResgate);
                        lAcessaDados.AddInParameter(lComm, "@ValorMiniAplicacao",               DbType.Decimal,     cota.ValorMiniAplicacao);
                        lAcessaDados.AddInParameter(lComm, "@Identificador",                    DbType.String,      cota.Identificador);
                        lAcessaDados.AddInParameter(lComm, "@DiasConversaoAplicacao",           DbType.String,      cota.DiasConversaoAplicacao);
                        lAcessaDados.AddInParameter(lComm, "@DiasConversaoResgate",             DbType.String,      cota.DiasConversaoResgate);
                        lAcessaDados.AddInParameter(lComm, "@DiasConversaoResgateAntecipado",   DbType.String,      cota.DiasConversaoResgateAntecipado);
                        lAcessaDados.AddInParameter(lComm, "@DiasPagamentoResgate",             DbType.String,      cota.DiasPagamentoResgate);
                        lAcessaDados.AddInParameter(lComm, "@ValorTaxaAdministracao",           DbType.Decimal,     cota.ValorTaxaAdministracao);
                        lAcessaDados.AddInParameter(lComm, "@ValorTaxaAdministracaoMaxima",     DbType.String,      cota.ValorTaxaAdministracaoMaxima);
                        lAcessaDados.AddInParameter(lComm, "@ValorTaxaResgateAntecipado",       DbType.String,      cota.ValorTaxaResgateAntecipado);
                        lAcessaDados.AddInParameter(lComm, "@ValorPatrimonioLiquido",           DbType.Decimal,     cota.ValorPatrimonioLiquido);
                        lAcessaDados.AddInParameter(lComm, "@CobraTaxaPerformance",             DbType.String,      cota.CobraTaxaPerformance);
                        lAcessaDados.AddInParameter(lComm, "@ValorTaxaPerformance",             DbType.String,      cota.ValorTaxaPerformance);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",                         DbType.DateTime,    cota.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        public void ImportarRentabilidadeDiaSite(string CodigoAnbima, DateTime  DataInicial)
        {
            List<ANBIMARentabilidadeDiaInfo> ListaRentabilidadeDia = null;

            string lSql = string.Format(SqlANBIMA.ListaRentabilidadeDia, CodigoAnbima, DataInicial.ToString("yyyy-MM-dd"));

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeDia = new List<ANBIMARentabilidadeDiaInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeDiaInfo lInfo = new ANBIMARentabilidadeDiaInfo();

                lInfo.CodigoFundo = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Data        = lReader["data"].DBToDateTime();
                lInfo.PL          = lReader["pl"].DBToDecimal();
                lInfo.ValorCota   = lReader["valcota"].DBToDecimal();
                lInfo.RentDia     = lReader["rentdia"].DBToDecimal();
                lInfo.RentMes     = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno     = lReader["rentano"].DBToDecimal();
                lInfo.DataHora    = lReader["datahora"].DBToDateTime();

                ListaRentabilidadeDia.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var rent in ListaRentabilidadeDia)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RENTABILIDADE_DIARIA_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodigoANBIMA", DbType.String,      rent.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Data",         DbType.DateTime,    rent.Data);
                        lAcessaDados.AddInParameter(lComm, "@patrLiquido",  DbType.Decimal,     rent.PL);
                        lAcessaDados.AddInParameter(lComm, "@vlrCota",      DbType.Decimal,     rent.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentDia",      DbType.Decimal,     rent.RentDia);
                        lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal,     rent.RentMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal,     rent.RentAno);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }

        }

        public void ImportarRentabilidadeMesSite(string CodigoAnbima, DateTime DataInicial)
        {
            List<ANBIMARentabilidadeMesInfo> ListaRentabilidadeMes = null;

            string lSql = string.Format(SqlANBIMA.ListaRentabilidadeMes, CodigoAnbima, DataInicial.ToString("yyyy-MM-dd"));

            OdbcConnection lConexao = new OdbcConnection(ConnectionString);

            OdbcCommand lCommand = new OdbcCommand(lSql, lConexao);

            lConexao.Open();

            OdbcDataReader lReader = lCommand.ExecuteReader(CommandBehavior.CloseConnection);

            ListaRentabilidadeMes = new List<ANBIMARentabilidadeMesInfo>();

            while (lReader.Read())
            {
                ANBIMARentabilidadeMesInfo lInfo = new ANBIMARentabilidadeMesInfo();

                lInfo.CodigoFundo = lReader["codFundo"].ToString().PadLeft(6, '0');
                lInfo.Mes         = lReader["mes"].DBToInt32();
                lInfo.Ano         = lReader["ano"].DBToInt32();
                lInfo.PL          = lReader["pl"].DBToDecimal();
                lInfo.ValorCota   = lReader["valcota"].DBToDecimal();
                lInfo.RentMes     = lReader["rentmes"].DBToDecimal();
                lInfo.RentAno     = lReader["rentano"].DBToDecimal();
                lInfo.CodigoTipo  = lReader["codtipo"].DBToInt32();
                lInfo.DataHora    = lReader["datahora"].DBToDateTime();

                ListaRentabilidadeMes.Add(lInfo);
            }

            lConexao.Close();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                foreach (var rent in ListaRentabilidadeMes)
                {
                    using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_RENTABILIDADE_MES_INS"))
                    {
                        lAcessaDados.AddInParameter(lComm, "@CodANBIMA",    DbType.String,  rent.CodigoFundo);
                        lAcessaDados.AddInParameter(lComm, "@Mes",          DbType.Int32,   rent.Mes);
                        lAcessaDados.AddInParameter(lComm, "@Ano",          DbType.Int32,   rent.Ano);
                        lAcessaDados.AddInParameter(lComm, "@PL",           DbType.Decimal, rent.PL);
                        lAcessaDados.AddInParameter(lComm, "@valCota",      DbType.Decimal, rent.ValorCota);
                        lAcessaDados.AddInParameter(lComm, "@RentMes",      DbType.Decimal, rent.RentMes);
                        lAcessaDados.AddInParameter(lComm, "@RentAno",      DbType.Decimal, rent.RentAno);
                        //lAcessaDados.AddInParameter(lComm, "@CodigoTipo",   DbType.Int32,   rent.CodigoTipo);
                        lAcessaDados.AddInParameter(lComm, "@DataHora",     DbType.DateTime, rent.DataHora);

                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);
                    }
                }
            }
        }

        private int GetCodigoProduto(string CodigoAnbima)
        {
            int lRetorno = 0;

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto from tbProduto where idCodigoAnbima = " + CodigoAnbima))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);

                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            lRetorno = dr1["idProduto"].DBToInt32();
                        }
                    }
                }
            }

            return lRetorno;
        }

        public Dictionary<int, string> GetListaCodigoAnbimaImportacao()
        {
            Dictionary<int, string> lRetorno = new Dictionary<int,string>();

            using (AcessaDados lAcessaDados = new AcessaDados())
            {
                lAcessaDados.Conexao = new Conexao();
                lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                using (DbCommand lComm = lAcessaDados.CreateCommand(CommandType.Text, "Select idProduto ,idCodigoAnbima from tbProduto"))
                {
                    DataTable dt = lAcessaDados.ExecuteDbDataTable(lComm);

                    if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt.Rows)
                        {
                            lRetorno.Add(dr1["idProduto"].DBToInt32(),  dr1["idCodigoAnbima"].DBToString());
                        }
                    }
                }
            }

            return lRetorno;
        }


        #endregion
    }
}
