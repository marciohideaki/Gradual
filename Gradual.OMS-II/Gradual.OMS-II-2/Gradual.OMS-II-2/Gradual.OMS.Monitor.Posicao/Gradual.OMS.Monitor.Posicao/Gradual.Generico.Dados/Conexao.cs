using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Gradual.Generico.Dados
{
    public class Conexao : IDisposable
    {
        #region | Atributos

        private string _connectionStringName = null;

        private DbConnection _dbConnection = null;

        #endregion

        #region | Propriedades

        public string _ConnectionStringName
        {
            get { return this._connectionStringName; }
            set { this._connectionStringName = value; }
        }

        public string ConnectionName
        {
            get { return "CadastroOracle"; }
        }

        public string Exportacao
        {
            get { return "Exportacao"; }
        }

        public string Educacional
        {
            get { return "Educacional"; }
        }

        #endregion

        #region | Construtores

        public Conexao()
        {

        }

        #endregion

        #region | Métodos

        public DbConnection AbrirConexao(string connectionString)
        {
            this._ConnectionStringName = connectionString;

            return this.AbrirConexao();
        }

        public DbConnection AbrirConexao()
        {
            if (string.IsNullOrEmpty(this._ConnectionStringName))
            {
                Console.Error.Write("Ocorreu um erro ao acessar um membro da classe Conexao , a propriedade _ConnectionStringName esta nula.");
                throw new Exception("Ocorreu um erro ao tentar se conectar com o Banco de Dados");
            }

            string strConexao = ConfigurationManager.ConnectionStrings[this._ConnectionStringName].ConnectionString;
            string strProviderName = ConfigurationManager.ConnectionStrings[this._ConnectionStringName].ProviderName;

            this._dbConnection = DbProviderFactories.GetFactory(strProviderName).CreateConnection();
            this._dbConnection.ConnectionString = strConexao;
            this._dbConnection.Open();
            return this._dbConnection;
        }

        public DbConnection CreateIConnection()
        {
            if (string.IsNullOrEmpty(this._ConnectionStringName))
            {
                Console.Error.Write("Ocorreu um erro ao acessar um membro da classe Conexao , a propriedade _ConnectionStringName esta nula.");
                throw new Exception("Ocorreu um erro ao tentar se conectar com o Banco de Dados.");
            }

            string strConexao = ConfigurationManager.ConnectionStrings[this._ConnectionStringName].ConnectionString;
            string strProviderName = ConfigurationManager.ConnectionStrings[this._ConnectionStringName].ProviderName;

            this._dbConnection = DbProviderFactories.GetFactory(strProviderName).CreateConnection();
            this._dbConnection.ConnectionString = strConexao;

            return this._dbConnection;
        }

        public void FecharConexao(DbConnection conn)
        {
            if (conn == null)
            {
                return;
            }

            try
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            finally
            {
                conn.Dispose();
            }
        }

        public DbProviderFactory GetDbProviderFactory()
        {
            if (string.IsNullOrEmpty(this._ConnectionStringName))
                throw new Exception("Ocorreu um erro ao acessar um membro da classe Conexao, a propriedade _ConnectionStringName esta nula.");

            var strProviderName = ConfigurationManager.ConnectionStrings[this._ConnectionStringName].ProviderName;
            var Factory = DbProviderFactories.GetFactory(strProviderName);

            return Factory;
        }

        public DbDataAdapter ObterDataAdapter(DbConnection scnConn)
        {
            if (scnConn is System.Data.SqlClient.SqlConnection)
                return new System.Data.SqlClient.SqlDataAdapter();

            else if (scnConn is System.Data.OracleClient.OracleConnection)
                return new System.Data.OracleClient.OracleDataAdapter();

            else
                Console.Error.Write("Operacao não suportada pela Biblioteca");

            throw new Exception("Ocorreu um erro ao acessar a estrutura do banco de dados.");
        }

        #endregion

        #region | Implementação interface IDisposable

        public void Dispose()
        {
            this.FecharConexao(this._dbConnection);
        }

        #endregion
    }
}