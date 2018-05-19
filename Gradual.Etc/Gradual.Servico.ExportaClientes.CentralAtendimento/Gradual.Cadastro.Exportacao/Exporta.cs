using System;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Gradual.Cadastro.Exportacao
{
    public class Exporta
    {
        #region | Propriedades

        public System.Collections.Hashtable gHtAssessores { get; set; }

        #endregion

        #region | Métodos

        public void ExportarClientes(string pPath, string pConexaoSql, string pConexaoOracle)
        {
            StringBuilder lConteudoArquivo = new StringBuilder();
            this.SetAssessores(pConexaoOracle);

            using (SqlConnection lSqlConnection = new SqlConnection(pConexaoSql))
            {
                using (SqlCommand lSqlCommand = new SqlCommand("exportacao_central_atendimento_lst_sp", lSqlConnection))
                {
                    lSqlCommand.CommandType = CommandType.StoredProcedure;

                    lSqlConnection.Open();

                    SqlDataReader dr = lSqlCommand.ExecuteReader();

                    while (dr.Read())
                    {
                        try
                        {
                            lConteudoArquivo.AppendFormat("{0};", dr["NomeCliente"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["CodigoCBLC"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["CPF"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["TipoCliente"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["StatusCadastro"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["CodigoAssessor"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", this.GetNomeAssessor(dr["NomeAssessor"].DBToInt32()));
                            lConteudoArquivo.AppendFormat("{0};", dr["DataCadastro"].DBToDateTime().ToString("dd/MM/yyyy"));
                            lConteudoArquivo.AppendFormat("{0};", dr["Email"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["DataNascimento"].DBToDateTime().ToString("dd/MM/yyyy"));
                            lConteudoArquivo.AppendFormat("{0};", dr["Logradouro"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Numero"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Complemento"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Bairro"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Cidade"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["UF"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Pais"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["TipoEndereco"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["DDD"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Telefone"].DBToString());
                            lConteudoArquivo.AppendFormat("{0};", dr["Ramal"].DBToString());
                            lConteudoArquivo.AppendFormat("{0}\r\n", dr["TipoTelefone"].DBToString());
                        }
                        catch {  }
                    }
                }
            }

            using (StreamWriter lStreamWriter = new StreamWriter(pPath))
            {
                lStreamWriter.Write(lConteudoArquivo.ToString());
            }
        }

        public void ExportarAssessores(string pPath, string pConexaoOracle)
        {
            StringBuilder lConteudoArquivo = new StringBuilder();

            using (OracleConnection lOracleConnection = new OracleConnection(pConexaoOracle))
            {
                using (OracleCommand lOracleCommand = new OracleCommand("SELECT cd_assessor, nm_assessor FROM tscasses ORDER BY cd_assessor", lOracleConnection))
                {
                    lOracleCommand.CommandType = CommandType.Text;

                    lOracleConnection.Open();

                    OracleDataReader dr = lOracleCommand.ExecuteReader();

                    while (dr.Read())
                        lConteudoArquivo.AppendFormat("{0};{1};Todas\r\n", TrataCaracter(dr["NM_ASSESSOR"].ToString()), TrataCaracter(dr["CD_ASSESSOR"].ToString()));
                }
            }

            using (StreamWriter lStreamWriter = new StreamWriter(@pPath))
            {
                lStreamWriter.Write(lConteudoArquivo.ToString());
            }
        }

        #endregion

        #region | Métodos de apoio

        private string TrataData(string value)
        {
            return DateTime.Parse(value).ToString("dd/MM/yyyy HH:mm:ss");
        }

        private string TrataSomenteData(string value)
        {
            return DateTime.Parse(value).ToString("dd/MM/yyyy");
        }

        private void SetAssessores(string pConexaoOracle)
        {
            using (OracleConnection conCadastro = new OracleConnection(pConexaoOracle))
            {
                using (OracleCommand comCadastro = new OracleCommand("select CD_ASSESSOR, NM_ASSESSOR from tscasses order by CD_ASSESSOR", conCadastro))
                {
                    gHtAssessores = new System.Collections.Hashtable();
                    conCadastro.Open();

                    OracleDataReader dr = comCadastro.ExecuteReader();

                    while (dr.Read())
                        gHtAssessores.Add(int.Parse(dr["CD_ASSESSOR"].ToString()), dr["NM_ASSESSOR"].ToString());
                }
            }
        }

        private string GetNomeAssessor(int pIdAssessor)
        {
            try
            {
                return gHtAssessores[pIdAssessor].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string TrataCaracter(string dado)
        {
            return dado.Replace(((char)150).ToString(), "-").Replace(";", "-").Replace("´", "").Replace("'", "").Trim();
        }

        private string TrataTipoCliente(string p)
        {
            string lTipoCliente = this.TrataCaracter(p).Trim();

            switch (lTipoCliente)
            {
                case "1":
                case "2":
                case "3":
                    lTipoCliente = "1"; //--> PF
                    break;
                case "4":
                case "6":
                case "9":
                case "11":
                case "13":
                case "18":
                case "20":
                case "21":
                case "23":
                case "25":
                case "26":
                case "29":
                case "99":
                    lTipoCliente = "2"; //--> PJ
                    break;
                case "8":
                case "15":
                case "17":
                    lTipoCliente = "3"; //--> Clubes e Fundos
                    break;
            }

            return lTipoCliente;
        }

        #endregion
    }
}
