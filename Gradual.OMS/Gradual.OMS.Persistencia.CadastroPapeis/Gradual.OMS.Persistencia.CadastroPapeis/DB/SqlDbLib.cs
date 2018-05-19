using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Gradual.OMS.Persistencia.CadastroPapeis.DB
{
    public class SqlDbLib
    {
        /// <summary>
        /// Referencia para uma conexão SQL
        /// </summary>
        private SqlConnection _cn = null;

        private string gNomeDaConexao = "default";

        public SqlDbLib()
        {
        }

        public SqlDbLib(string pNomeDaConexao)
        {
            this.gNomeDaConexao = pNomeDaConexao;
        }


        /// <summary>
        /// Pede inicialização da instancia
        /// </summary>
        /// <param name="parametros"></param>
        public void Inicializar(object parametros)
        {
        }

        /// <summary>
        /// Pede finalização da instancia
        /// </summary>
        public void Finalizar()
        {
        }

        /// <summary>
        /// Solicita a execução de uma procedure.
        /// Os parametros são passados em duplas de nome do parametro, valor
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
        /// Overload da execução de procedure que recebe a lista de parametros através de 
        /// um dicionário.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public DataSet ExecutarProcedure(string nomeProcedure, Dictionary<string, object> parametros)
        {
            // Repassa a chamada
            return this.ExecutarProcedure(nomeProcedure, parametros, new List<string>());
        }

        /// <summary>
        /// Executa uma procedure no banco de dados.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public DataSet ExecutarProcedure(string nomeProcedure, Dictionary<string, object> parametros, List<string> outputParams)
        {
            // Recebe a conexao
            SqlConnection cn = ReceberConexao();

            try
            {
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
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
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
        /// Executa uma consulta 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public DataSet ExecutarConsulta(string sql, Dictionary<string, object> parametros)
        {
            // Recebe a conexao
            SqlConnection cn = ReceberConexao();
            try
            {
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
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        /// <summary>
        /// Retorna uma conexão com o banco de dados 
        /// </summary>
        /// <returns></returns>
        public SqlConnection ReceberConexao()
        {
            return OnReceberConexao();
        }

        /// <summary>
        /// Método virtual para retornar conexão com o banco de dados
        /// </summary>
        /// <returns></returns>
        protected virtual SqlConnection OnReceberConexao()
        {
            // Cria a conexão
            if (_cn == null)
            {
                // Cria
                _cn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["default"].ConnectionString);

                // Abre
                _cn.Open();
            }

            // Retorna
            return _cn;
        }
    }
}
