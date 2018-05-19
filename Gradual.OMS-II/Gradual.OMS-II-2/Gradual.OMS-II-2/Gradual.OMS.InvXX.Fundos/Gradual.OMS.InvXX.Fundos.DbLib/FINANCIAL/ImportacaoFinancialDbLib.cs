using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Gradual.Generico.Dados;
using System.Data;
using System.Data.Common;
using Gradual.OMS.InvXX.Fundos.Lib;
using Gradual.OMS.InvXX.Fundos.Lib.FINANCIAL;

namespace Gradual.OMS.InvXX.Fundos.DbLib.FINANCIAL
{
    public class ImportacaoFinancialDbLib
    {
        #region Atributos
        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Métodos

        public void ImportaPosicaoClienteFundosFinancial(PosicaoClienteFinancialInfo info)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_INS_POSICAO_COTISTAS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCliente"  , DbType.Int32      , info.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@Agencia"        , DbType.String     , info.Angencia);
                        lAcessaDados.AddInParameter(lCommand, "@Banco"          , DbType.String     , info.Banco);
                        lAcessaDados.AddInParameter(lCommand, "@Conta"          , DbType.String     , info.Conta);
                        lAcessaDados.AddInParameter(lCommand, "@DigitoConta"    , DbType.String     , info.DigitoConta);
                        lAcessaDados.AddInParameter(lCommand, "@DsCpfCnpj"      , DbType.String     , info.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lCommand, "@DtProcessamento", DbType.DateTime   , info.DtProcessamento);
                        lAcessaDados.AddInParameter(lCommand, "@DtReferencia"   , DbType.DateTime   , info.DtReferencia);
                        lAcessaDados.AddInParameter(lCommand, "@IdCotista"      , DbType.String     , info.IdCotista);
                        lAcessaDados.AddInParameter(lCommand, "@IdFundo"        , DbType.Int32      , info.IdFundo);
                        lAcessaDados.AddInParameter(lCommand, "@IdMovimento"    , DbType.Int32      , info.IdMovimento);
                        lAcessaDados.AddInParameter(lCommand, "@IdProcessamento", DbType.Int32      , info.IdProcessamento);
                        lAcessaDados.AddInParameter(lCommand, "@QuantidadeCotas", DbType.Decimal    , info.QuantidadeCotas);
                        lAcessaDados.AddInParameter(lCommand, "@SubConta"       , DbType.String     , info.SubConta);
                        lAcessaDados.AddInParameter(lCommand, "@ValorCota"      , DbType.Decimal    , info.ValorCota);
                        lAcessaDados.AddInParameter(lCommand, "@ValorBruto"     , DbType.Decimal    , info.ValorBruto);
                        lAcessaDados.AddInParameter(lCommand, "@ValorIR"        , DbType.Decimal    , info.ValorIR);
                        lAcessaDados.AddInParameter(lCommand, "@ValorIOF"       , DbType.Decimal    , info.ValorIOF);
                        lAcessaDados.AddInParameter(lCommand, "@ValorLiquido"   , DbType.Decimal    , info.ValorLiquido);

                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método ImportarPosicaoClienteFundosITAU", ex);
            }
        }

        public void ImportacaoClienteFinancial(ClienteFinancialInfo info)
        {
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_INS"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@CodigoCliente"  , DbType.Int32  , info.CodigoCliente);
                        lAcessaDados.AddInParameter(lCommand, "@NomeCliente"    , DbType.String , info.NomeCliente);
                        lAcessaDados.AddInParameter(lCommand, "@CodigoAssessor" , DbType.Int32  , info.CodigoAssessor);
                        lAcessaDados.AddInParameter(lCommand, "@StAtivo"        , DbType.String , info.StAtivo);
                        lAcessaDados.AddInParameter(lCommand, "@Telefone"       , DbType.String , info.Telefone);
                        lAcessaDados.AddInParameter(lCommand, "@Email"          , DbType.String , info.Email);
                        lAcessaDados.AddInParameter(lCommand, "@DsCpfCnpj"      , DbType.String , info.DsCpfCnpj);
                        lAcessaDados.AddInParameter(lCommand, "@TipoPessoa"     , DbType.String , info.TipoPessoa);
                        
                        lAcessaDados.ExecuteNonQuery(lCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método ImportacaoClienteFinancial", ex);
            }
        }

        public PosicaoClienteFinancialInfo SelecionaClienteDadosBancarios(int CodigoCliente)
        {
            PosicaoClienteFinancialInfo lRetorno = new PosicaoClienteFinancialInfo();
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DADOS_BANCARIOS_SEL"))
                    {
                        lAcessaDados.AddInParameter(lCommand, "@codCliente", DbType.Int32, CodigoCliente);

                        DataTable lDt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (lDt != null && lDt.Rows.Count > 0)
                        {
                            DataRow row = lDt.Rows[0];

                            PosicaoClienteFinancialInfo linfo = new PosicaoClienteFinancialInfo();

                            linfo.Banco       = row["banco"].ToString();
                            linfo.Angencia    = row["agencia"].ToString();
                            linfo.Conta       = row["conta"].ToString();
                            linfo.DigitoConta = row["DigitoConta"].ToString();
                            linfo.SubConta    = row["subconta"].ToString();

                            lRetorno = linfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaClienteDadosBancarios.", ex);
            }

            return lRetorno;
        }

        public List<ClienteFinancialInfo> SelecionaClienteImportacaoPosicaoFinancial()
        {
            List<ClienteFinancialInfo> lRetorno = new List<ClienteFinancialInfo>();
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_IMPORTACAO_SEL"))
                    {
                        DataTable lDt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (lDt != null && lDt.Rows.Count > 0)
                        {
                            foreach (DataRow row in lDt.Rows)
                            {
                                ClienteFinancialInfo linfo = new ClienteFinancialInfo();

                                linfo.CodigoAnbima   = int.Parse(row["codAnbima"].ToString());
                                linfo.CodigoCliente  = int.Parse(row["codCliente"].ToString());
                                linfo.CodigoAssessor = int.Parse(row["codAssessor"].ToString());
                                linfo.DsCpfCnpj      = row["CPFCNPJ"].ToString();
                                linfo.Email          = row["email"].ToString();
                                linfo.StAtivo        = row["stAtivo"].ToString();
                                linfo.Telefone       = row["telefone"].ToString();
                                linfo.TipoPessoa     = row["TipoPessoa"].ToString();

                                lRetorno.Add(linfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaClienteImportacaoFinancial.", ex);
            }

            return lRetorno;
        }

        public List<ClienteFinancialInfo> SelecionaClienteImportacaoFinancial()
        {
            List<ClienteFinancialInfo> lRetorno = new List<ClienteFinancialInfo>();

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_CLIENTE_FINANCIAL_IMPORTACAO_SEL"))
                    {
                        DataTable lDt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (lDt != null && lDt.Rows.Count > 0)
                        {
                            foreach (DataRow row in lDt.Rows)
                            {
                                ClienteFinancialInfo linfo = new ClienteFinancialInfo();

                                linfo.CodigoCliente  = int.Parse(row["codCliente"].ToString());
                                linfo.CodigoAssessor = int.Parse(row["codAssessor"].ToString());
                                linfo.NomeCliente    = row["ds_nome"].ToString();
                                linfo.DsCpfCnpj      = row["CPFCNPJ"].ToString();
                                linfo.Email          = row["email"].ToString();
                                linfo.StAtivo        = row["stAtivo"].ToString();
                                linfo.Telefone       = row["telefone"].ToString();
                                linfo.TipoPessoa     = row["TipoPessoa"].ToString();

                                lRetorno.Add(linfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaClienteImportacaoFinancial.", ex);
            }

            return lRetorno;
        }

        public string SelecionaCodigoFundo(string Dscpfcnpj)
        {
            string lRetorno = string.Empty;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto from tbProduto where cpfcnpj = '" + Dscpfcnpj.Substring(1, 14) + "'"))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = row["idProduto"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoFundo.", ex);
            }

            return lRetorno;
        }

        public int SelecionaCodigoProduto(int idproduto)
        {
            int lRetorno = 0;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao              = new Conexao();
                    lAcessaDados.ConnectionStringName = "ClubesFundos";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select dscnpj from tbCadastroFundo where idFundo = " + idproduto))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = Convert.ToInt32(SelecionaCodigoFundo(row["dscnpj"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoProduto.", ex);
            }

            return lRetorno;
        }

        public int SelecionaCodigoProdutoPorANBIMA(int CodAnbima)
        {
            int lRetorno = 0;

            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "PlataformaInviXX";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, "select idProduto from tbProduto where idCodigoAnbima = " + CodAnbima))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = Convert.ToInt32(row["idProduto"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoProduto.", ex);
            }

            return lRetorno;
        }

        public int SelecionaCodigoCliente(string Dscpfcnpj)
        {
            int lRetorno = 0;
            try
            {
                using (AcessaDados lAcessaDados = new AcessaDados())
                {
                    lAcessaDados.Conexao = new Conexao();
                    lAcessaDados.ConnectionStringName = "Cadastro";

                    string lSql = "select a.cd_codigo from tb_cliente_conta a, tb_cliente b where a.id_cliente = b.id_cliente and b.ds_cpfcnpj = '" + Dscpfcnpj.Substring(4, 11) + "' or  b.ds_cpfcnpj = '" + Dscpfcnpj.Substring(0, 15) + "'";

                    using (DbCommand lCommand = lAcessaDados.CreateCommand(CommandType.Text, lSql))
                    {
                        DataTable dt = lAcessaDados.ExecuteDbDataTable(lCommand);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            lRetorno = Convert.ToInt32(row["cd_codigo"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gLogger.Error("Erro encontrado no método SelecionaCodigoProduto", ex);
            }
            return lRetorno;
        }

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

        #endregion
    }
}
