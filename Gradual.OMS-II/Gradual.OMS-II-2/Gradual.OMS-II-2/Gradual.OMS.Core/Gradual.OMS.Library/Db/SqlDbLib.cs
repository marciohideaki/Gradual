using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Db
{
    /// <summary>
    /// Classe de auxilio para chamadas em Banco de Dados SQL Server.
    /// Possui facilitadores para chamada de procedures e execução de comandos sql.
    /// Atualmente trabalha com a string de conexao "default" do arquivo de configurações.
    /// </summary>
    public class SqlDbLib : IDbLib
    {
        /// <summary>
        /// Referencia para uma conexão SQL
        /// </summary>
        private SqlConnection _cn = null;

        #region Funcionalidades Estáticas

        /// <summary>
        /// Instancia estática da classe para ser oferecida como default
        /// </summary>
        private static IDbLib _default = null;
        
        /// <summary>
        /// Acesso estático à instancia default
        /// </summary>
        public static IDbLib Default
        {
            get 
            {
                if (_default == null)
                    _default = new SqlDbLib();
                return _default;
            }
        }

        #endregion

        #region IDbLib Members

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

        #endregion
    }
}
