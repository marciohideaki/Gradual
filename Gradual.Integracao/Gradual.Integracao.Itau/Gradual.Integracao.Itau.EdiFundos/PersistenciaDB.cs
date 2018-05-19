using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Integracao.Itau.EdiFundos.Lib.Dados;
using log4net;
using Gradual.Generico.Dados;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace Gradual.Integracao.Itau.EdiFundos
{
    public class PersistenciaDB
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<CadastroFundoInfo> ObterListaFundos()
        {
            List<CadastroFundoInfo> ret = new List<CadastroFundoInfo>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlDataAdapter lAdapter;

                DataSet dtSet = new DataSet();

                SqlCommand sqlCmd = new SqlCommand("prc_sel_fundos", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = conn;

                lAdapter = new SqlDataAdapter(sqlCmd);

                lAdapter.Fill(dtSet);

                for( int i=0; i < dtSet.Tables[0].Rows.Count; i++ )
                {
                    DataRow lRow = dtSet.Tables[0].Rows[i];

                    CadastroFundoInfo info = new CadastroFundoInfo();

                    info.Agencia = lRow["Agencia"].ToString();
                    info.Ativo = lRow["stAtivo"].ToString();
                    info.CNPJ = lRow["dsCnpj"].ToString();
                    info.CodFundo = lRow["dsCodFundo"].ToString();
                    info.Conta = lRow["Conta"].ToString();
                    info.DataAtualizacao = lRow["dtAtualizacao"].DBToDateTime();
                    info.DigitoConta = lRow["DigitoConta"].ToString();
                    info.Gestor = lRow["dsGestor"].ToString();
                    info.GestorCli = lRow["dsGestorCli"].ToString();
                    info.IDDistribuidor = lRow["idDistribuidor"].DBToInt32();
                    info.IDFundo = Convert.ToInt32(lRow["idFundo"].ToString());
                    info.NomeFantasia = lRow["dsNomeFantasia"].ToString();
                    info.NumeroCotas = lRow["NumeroCotas"].DBToInt64();
                    info.PatrimonioLiquido = lRow["PatrimonioLiquido"].DBToDecimal();
                    info.RazaoSocial = lRow["dsRazaoSocial"].ToString();
                    info.SubConta = lRow["SubConta"].ToString();
                    info.CodFundoFC = lRow["CodFundoFC"].DBToString();

                    ret.Add(info);
                }

                logger.Debug("Obteve lista de fundos com [" + dtSet.Tables[0].Rows.Count + "] itens");
            }
            catch(Exception ex)
            {
                logger.Error("ObterListaFundos(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return ret;
        }


        /// <summary>
        /// Retornar o conteudo da tabela tbClientePosicao
        /// </summary>
        /// <returns></returns>
        public List<PosicaoClienteInfo> ObterPosicaoCotistas()
        {
            List<PosicaoClienteInfo> ret = new List<PosicaoClienteInfo>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlDataAdapter lAdapter;

                DataSet dtSet = new DataSet();

                SqlCommand sqlCmd = new SqlCommand("prc_sel_posicao_cotistas", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = conn;

                lAdapter = new SqlDataAdapter(sqlCmd);

                lAdapter.Fill(dtSet);

                for (int i = 0; i < dtSet.Tables[0].Rows.Count; i++)
                {
                    DataRow lRow = dtSet.Tables[0].Rows[i];

                    PosicaoClienteInfo info = new PosicaoClienteInfo();

                    info.Agencia = lRow["Agencia"].ToString();
                    info.BancoCli = lRow["Banco"].ToString();
                    info.Conta = lRow["Conta"].ToString();
                    info.CPFCNPJ = lRow["dsCpfCnpj"].ToString();
                    info.DataProcessamento = lRow["dtProcessamento"].DBToDateTime();
                    info.DataReferencia = lRow["dtReferencia"].DBToDateTime();
                    info.DigitoConta = lRow["DigitoConta"].ToString();
                    info.IDCotista = lRow["idCotista"].ToString();
                    info.IDFundo = lRow["idFundo"].DBToInt32();
                    info.IDMovimento = lRow["idMovimento"].DBToInt32();
                    info.IDProcessamento = lRow["idProcessamento"].DBToInt32();
                    info.QtdeCotas = lRow["quantidadeCotas"].DBToDecimal();
                    info.ValorBruto = lRow["valorBruto"].DBToDecimal();
                    info.ValorCota = lRow["valorCota"].DBToDecimal();
                    info.ValorIR = lRow["valorIR"].DBToDecimal();
                    info.ValorIOF = lRow["valorIOF"].DBToDecimal();
                    info.ValorLiquido = lRow["valorLiquido"].DBToDecimal();

                    ret.Add(info);
                }

                logger.Debug("Obteve posicao de cotistas com [" + dtSet.Tables[0].Rows.Count + "] itens");
            }
            catch (Exception ex)
            {
                logger.Error("ObterPosicaoCotistas(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return ret;
        }

        public bool InserirCadastroFundo(CadastroFundoInfo fundo)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("prc_ins_fundo", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@dsRazaoSocial", fundo.RazaoSocial ));
	            sqlCmd.Parameters.Add(new SqlParameter("@dsNomeFantasia", fundo.NomeFantasia ));
	            sqlCmd.Parameters.Add(new SqlParameter("@dsCnpj", fundo.CNPJ ));
	            sqlCmd.Parameters.Add(new SqlParameter("@stAtivo", "S"));
	            sqlCmd.Parameters.Add(new SqlParameter("@dtAtualizacao", fundo.DataAtualizacao));
                sqlCmd.Parameters.Add(new SqlParameter("@idDistribuidor", fundo.IDDistribuidor));
	            sqlCmd.Parameters.Add(new SqlParameter("@PatrimonioLiquido", fundo.PatrimonioLiquido));
	            sqlCmd.Parameters.Add(new SqlParameter("@NumeroCotas", fundo.NumeroCotas));
	            sqlCmd.Parameters.Add(new SqlParameter("@dsGestor", fundo.Gestor));
	            sqlCmd.Parameters.Add(new SqlParameter("@dsCodFundo", fundo.CodFundo));
	            sqlCmd.Parameters.Add(new SqlParameter("@dsGestorCli", fundo.GestorCli));
	            sqlCmd.Parameters.Add(new SqlParameter("@Agencia", fundo.Agencia));
	            sqlCmd.Parameters.Add(new SqlParameter("@Conta", fundo.Conta));
	            sqlCmd.Parameters.Add(new SqlParameter("@DigitoConta", fundo.DigitoConta));
	            sqlCmd.Parameters.Add(new SqlParameter("@SubConta", fundo.SubConta));
                sqlCmd.Parameters.Add(new SqlParameter("@CodFundoFC", fundo.CodFundoFC));

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("InserirCadastroFundo():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }


            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundo"></param>
        /// <returns></returns>
        public bool AtualizarCadastroFundo(CadastroFundoInfo fundo)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("prc_atualiza_fundo", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@dsRazaoSocial", fundo.RazaoSocial));
                sqlCmd.Parameters.Add(new SqlParameter("@dsNomeFantasia", fundo.NomeFantasia));
                sqlCmd.Parameters.Add(new SqlParameter("@dsCnpj", fundo.CNPJ));
                sqlCmd.Parameters.Add(new SqlParameter("@stAtivo", "S"));
                sqlCmd.Parameters.Add(new SqlParameter("@dtAtualizacao", fundo.DataAtualizacao));
                sqlCmd.Parameters.Add(new SqlParameter("@idDistribuidor", fundo.IDDistribuidor));
                sqlCmd.Parameters.Add(new SqlParameter("@PatrimonioLiquido", fundo.PatrimonioLiquido));
                sqlCmd.Parameters.Add(new SqlParameter("@NumeroCotas", fundo.NumeroCotas));
                sqlCmd.Parameters.Add(new SqlParameter("@dsGestor", fundo.Gestor));
                sqlCmd.Parameters.Add(new SqlParameter("@dsCodFundo", fundo.CodFundo));
                sqlCmd.Parameters.Add(new SqlParameter("@dsGestorCli", fundo.GestorCli));
                sqlCmd.Parameters.Add(new SqlParameter("@Agencia", fundo.Agencia));
                sqlCmd.Parameters.Add(new SqlParameter("@Conta", fundo.Conta));
                sqlCmd.Parameters.Add(new SqlParameter("@DigitoConta", fundo.DigitoConta));
                sqlCmd.Parameters.Add(new SqlParameter("@SubConta", fundo.SubConta));
                sqlCmd.Parameters.Add(new SqlParameter("@CodFundoFC", fundo.CodFundoFC));

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarCadastroFundo():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }


            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundo"></param>
        /// <returns></returns>
        public bool InserirPosicaoCotista(PosicaoClienteInfo fundo)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("prc_ins_posicao_cotista", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@idProcessamento", fundo.IDProcessamento));
                sqlCmd.Parameters.Add(new SqlParameter("@dsCpfCnpj", fundo.CPFCNPJ));
                sqlCmd.Parameters.Add(new SqlParameter("@agencia", fundo.Agencia));
                sqlCmd.Parameters.Add(new SqlParameter("@conta", fundo.Conta));
                sqlCmd.Parameters.Add(new SqlParameter("@idCotista", fundo.IDCotista));
                sqlCmd.Parameters.Add(new SqlParameter("@idFundo", fundo.IDFundo));
                sqlCmd.Parameters.Add(new SqlParameter("@quantidadeCotas", fundo.QtdeCotas));
                sqlCmd.Parameters.Add(new SqlParameter("@valorCota", fundo.ValorCota));
                sqlCmd.Parameters.Add(new SqlParameter("@valorBruto", fundo.ValorBruto));
                sqlCmd.Parameters.Add(new SqlParameter("@valorIR", fundo.ValorIR));
                sqlCmd.Parameters.Add(new SqlParameter("@valorIOF", fundo.ValorIOF));
                sqlCmd.Parameters.Add(new SqlParameter("@valorLiquido", fundo.ValorLiquido));
                sqlCmd.Parameters.Add(new SqlParameter("@dtReferencia", fundo.DataReferencia));
                sqlCmd.Parameters.Add(new SqlParameter("@dtProcessamento", fundo.DataProcessamento));
                sqlCmd.Parameters.Add(new SqlParameter("@DigitoConta", fundo.DigitoConta));
                sqlCmd.Parameters.Add(new SqlParameter("@Subconta", fundo.SubConta));
                sqlCmd.Parameters.Add(new SqlParameter("@Banco", fundo.BancoCli));

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("InserirPosicaoCotista():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundo"></param>
        /// <returns></returns>
        public bool AtualizarPosicaoCotista(PosicaoClienteInfo fundo)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("prc_atualiza_posicao_cotista", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@idMovimento", fundo.IDMovimento));
                sqlCmd.Parameters.Add(new SqlParameter("@idProcessamento", fundo.IDProcessamento));
                sqlCmd.Parameters.Add(new SqlParameter("@dsCpfCnpj", fundo.CPFCNPJ));
                sqlCmd.Parameters.Add(new SqlParameter("@agencia", fundo.Agencia));
                sqlCmd.Parameters.Add(new SqlParameter("@conta", fundo.Conta));
                sqlCmd.Parameters.Add(new SqlParameter("@idCotista", fundo.IDCotista));
                sqlCmd.Parameters.Add(new SqlParameter("@idFundo", fundo.IDFundo));
                sqlCmd.Parameters.Add(new SqlParameter("@quantidadeCotas", fundo.QtdeCotas));
                sqlCmd.Parameters.Add(new SqlParameter("@valorCota", fundo.ValorCota));
                sqlCmd.Parameters.Add(new SqlParameter("@valorBruto", fundo.ValorBruto));
                sqlCmd.Parameters.Add(new SqlParameter("@valorIR", fundo.ValorIR));
                sqlCmd.Parameters.Add(new SqlParameter("@valorIOF", fundo.ValorIOF));
                sqlCmd.Parameters.Add(new SqlParameter("@valorLiquido", fundo.ValorLiquido));
                sqlCmd.Parameters.Add(new SqlParameter("@dtReferencia", fundo.DataReferencia));
                sqlCmd.Parameters.Add(new SqlParameter("@dtProcessamento", fundo.DataProcessamento));
                sqlCmd.Parameters.Add(new SqlParameter("@DigitoConta", fundo.DigitoConta));
                sqlCmd.Parameters.Add(new SqlParameter("@Subconta", fundo.SubConta));
                sqlCmd.Parameters.Add(new SqlParameter("@Banco", fundo.BancoCli));

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("AtualizarPosicaoCotista():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundo"></param>
        /// <returns></returns>
        public bool LimparTabelaPosicaoCotista()
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("DELETE FROM tbClientePosicao", conn);
                sqlCmd.CommandType = System.Data.CommandType.Text;

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("LimparTabelaPosicaoCotista():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retorna todos os registros do cadastro de cotistas
        /// </summary>
        /// <returns></returns>
        public List<CotistaInfo> ObterCadastroCotistas()
        {
            List<CotistaInfo> ret = new List<CotistaInfo>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlDataAdapter lAdapter;

                DataSet dtSet = new DataSet();

                SqlCommand sqlCmd = new SqlCommand("prc_sel_cotistas", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Connection = conn;

                lAdapter = new SqlDataAdapter(sqlCmd);

                lAdapter.Fill(dtSet);

                for (int i = 0; i < dtSet.Tables[0].Rows.Count; i++)
                {
                    DataRow lRow = dtSet.Tables[0].Rows[i];

                    CotistaInfo info = new CotistaInfo();

                    info.Agencia = lRow["Agencia"].ToString();
                    info.BancoCli = lRow["Banco"].ToString();
                    info.Conta = lRow["Conta"].ToString();
                    info.SubConta = lRow["SubConta"].ToString();
                    info.CPFCNPJ = lRow["dsCpfCnpj"].ToString();
                    info.DataImportacao = lRow["dtImportacao"].DBToDateTime();
                    info.DigitoConta = lRow["DigitoConta"].ToString();
                    info.IdCotista = lRow["idCotista"].DBToInt32();
                    info.IdCotistaItau = lRow["idCotistaItau"].DBToString();
                    info.Nome = lRow["dsNome"].DBToString();

                    ret.Add(info);
                }

                logger.Debug("Obteve lista de cotistas com [" + dtSet.Tables[0].Rows.Count + "] itens");
            }
            catch (Exception ex)
            {
                logger.Error("ObterCadastroCotistas(): " + ex.Message, ex);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return ret;
        }



        public bool InserirCotista(CotistaInfo cotista)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubesFundos"].ConnectionString);

                conn.Open();

                SqlCommand sqlCmd = new SqlCommand("prc_ins_cotista", conn);

                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

                sqlCmd.Parameters.Add(new SqlParameter("@idCotistaItau", cotista.IdCotistaItau));
                sqlCmd.Parameters.Add(new SqlParameter("@dsNome", cotista.Nome));
                sqlCmd.Parameters.Add(new SqlParameter("@dsCpfCnpj", cotista.CPFCNPJ));
                sqlCmd.Parameters.Add(new SqlParameter("@stAtivo", cotista.StatusAtivo));
                sqlCmd.Parameters.Add(new SqlParameter("@dtImportacao", cotista.DataImportacao));
                sqlCmd.Parameters.Add(new SqlParameter("@Banco", cotista.BancoCli));
                sqlCmd.Parameters.Add(new SqlParameter("@Agencia", cotista.Agencia));
                sqlCmd.Parameters.Add(new SqlParameter("@Conta", cotista.Conta));
                sqlCmd.Parameters.Add(new SqlParameter("@DigitoConta", cotista.DigitoConta));
                sqlCmd.Parameters.Add(new SqlParameter("@SubConta",cotista.SubConta));

                sqlCmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                logger.Error("InserirCadastroCotista():" + ex.Message, ex);
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
                return false;
            }


            return true;
        }
    }
}
