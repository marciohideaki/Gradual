using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Gradual.OMS.Library;
using log4net;

namespace Gradual.OMS.Persistencias.Seguranca.Entidades
{
    public class DbLib
    {
        private string _NomeConexao = "Default";
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string NomeConexao
        {
            set
            {
                _NomeConexao = value;
            }
        }

        public DbLib(string NomeConexao)
        {
            this._NomeConexao = NomeConexao;
        }

        #region Rotinas Locais

        /// <summary>
        /// Executa procudure
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public DataSet ExecutarProcedure(string nomeProcedure, params object[] parametros)
        {
            Dictionary<string, object> parametros2 = new Dictionary<string, object>();
            for (int i = 0; i < parametros.Length; i += 2)
                parametros2.Add((string)parametros[i], parametros[i + 1]);
            return ExecutarProcedure(nomeProcedure, parametros2, null);
        }

        /// <summary>
        /// Executa uma procedure no banco de dados.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <param name="outputParams"></param>
        /// <returns></returns>
        public DataSet ExecutarProcedure(string nomeProcedure, Dictionary<string, object> parametros, List<string> outputParams)
        {
            SqlConnection cn = null;
            try
            {
                // Recebe a conexao
                cn = receberConexao();

                // Cria o comando
                SqlCommand cm = new SqlCommand(nomeProcedure, cn);
                cm.CommandType = CommandType.StoredProcedure;

                // Adiciona parametros, caso existam
                if (parametros != null)
                    foreach (KeyValuePair<string, object> item in parametros)
                        cm.Parameters.AddWithValue(item.Key, item.Value);

                // Seta os parametros de output
                if (outputParams != null)
                    foreach (string outputParam in outputParams)
                        if (cm.Parameters.Contains(outputParam))
                            cm.Parameters[outputParam].Direction = ParameterDirection.Output;

                // Executa
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // Preenche parametros de output
                foreach (SqlParameter param in cm.Parameters)
                    if (param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Output)
                        if (parametros.ContainsKey(param.ParameterName))
                            parametros[param.ParameterName] = param.Value;

                // Retorna
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                if(cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        /// <summary>
        /// Executa uma consulta no banco de dados.
        /// Overload para receber os parametros como duplas de parametro e valor.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public DataSet ExecutarConsulta(string sql, params object[] parametros)
        {
            // Transforma a lista de parametros em um dicionario
            Dictionary<string, object> parametros2 = new Dictionary<string, object>();
            for (int i = 0; i < parametros.Length; i += 2)
                parametros2.Add((string)parametros[i], parametros[i + 1]);

            // Repassa a chamada
            return ExecutarConsulta(sql, parametros2);
        }

        /// <summary>
        /// Executa uma consulta no sinacor
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private DataSet ExecutarConsulta(string sql, Dictionary<string, object> parametros)
        {
            // Recebe a conexao
            SqlConnection cn = null;

            try
            {

                cn = receberConexao();

                // Cria o comando
                SqlCommand cm = new SqlCommand(sql, cn);
                cm.CommandType = CommandType.Text;

                // Adiciona parametros, caso existam
                if (parametros != null)
                    foreach (KeyValuePair<string, object> item in parametros)
                        cm.Parameters.AddWithValue(item.Key, item.Value);

                // Executa
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataSet ds = new DataSet();
                da.Fill(ds);

                // Retorna
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        /// <summary>
        /// Retorna uma conexão com o banco de dados do sinacor
        /// </summary>
        /// <returns></returns>
        private SqlConnection receberConexao()
        {
            // Cria a conexão
            SqlConnection cn = new SqlConnection(
                ConfigurationManager.ConnectionStrings[_NomeConexao].ConnectionString);

            // Abre
            cn.Open();

            // Retorna
            return cn;
        }

        #endregion
    }
}
