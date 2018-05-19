using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Gradual.Generico.Dados
{
    public static class Conexao
    {
        private static string _connectionStringName = null;

        public static string _ConnectionStringName
        {
            get { return _connectionStringName; }
            set { _connectionStringName = value; }
        }

        static Conexao()
        {
        
        }

        public static string ConnectionName
        {
            get { return "CadastroOracle"; }
        } 

        public static string Exportacao
        {
            get { return "Exportacao"; }
        }

        public static string Educacional
        {
            get { return "Educacional"; }
        } 

        #region "Abrir conexão"

        public static DbConnection abrirConexao(string connectionString)
        {
            _ConnectionStringName = connectionString;

            return abrirConexao();
        }

        public static DbConnection abrirConexao()
        {
            try
            {
                if (string.IsNullOrEmpty(_ConnectionStringName))
                {
                    Console.Error.Write("Ocorreu um erro ao acessar um membro da classe Conexao , a propriedade _ConnectionStringName esta nula.");
                    throw new Exception("Ocorreu um erro ao tentar se conectar com o Banco de Dados");
                }

                string strConexao = ConfigurationManager.ConnectionStrings[_ConnectionStringName].ConnectionString;
                string strProviderName = ConfigurationManager.ConnectionStrings[_ConnectionStringName].ProviderName;

                DbProviderFactory factory = DbProviderFactories.GetFactory(strProviderName);
                DbConnection conn = factory.CreateConnection();
                conn.ConnectionString = strConexao;
                conn.Open();
                return conn;
            }
            catch (Exception ex){
                Console.Error.Write(ex.Message);
                throw ex;
            }
		}

        public static DbConnection CreateIConnection()
        {
            try
            {
                if (string.IsNullOrEmpty(_ConnectionStringName))
                {
                    Console.Error.Write("Ocorreu um erro ao acessar um membro da classe Conexao , a propriedade _ConnectionStringName esta nula.");
                    throw new Exception("Ocorreu um erro ao tentar se conectar com o Banco de Dados.");
                }

                string strConexao = ConfigurationManager.ConnectionStrings[_ConnectionStringName].ConnectionString;
                string strProviderName = ConfigurationManager.ConnectionStrings[_ConnectionStringName].ProviderName;

                DbProviderFactory factory = DbProviderFactories.GetFactory(strProviderName);
                DbConnection conn = factory.CreateConnection();
                conn.ConnectionString = strConexao;

                return conn;
            }
            catch (Exception ex)
            {
                Console.Error.Write(ex.Message);
                throw (ex);
            }
        }

        #endregion

        #region "Fechar conexão"

        public static void fecharConexao(DbConnection conn)
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
            catch (Exception ex)
            {
                Console.Error.Write(ex.Message);
                throw (ex);
            }
            finally
            {
                conn.Dispose();
            }
        }

        #endregion

        #region "obterDbProviderFactory"

        public static DbProviderFactory GetDbProviderFactory()
        {
            try
            {
                if (string.IsNullOrEmpty(_ConnectionStringName))
                {
                    throw new Exception("Ocorreu um erro ao acessar um membro da classe Conexao , a propriedade _ConnectionStringName esta nula.");
                }

                string strProviderName = ConfigurationManager.ConnectionStrings[_ConnectionStringName].ProviderName;
                DbProviderFactory Factory = DbProviderFactories.GetFactory(strProviderName);

                return Factory;
            }
            catch (Exception ex)
            {
                Console.Error.Write(ex.Message);
                throw (ex);
            }
        }

        #endregion

        public static DbDataAdapter obterDataAdapter(DbConnection scnConn)
        {
            if (scnConn is System.Data.SqlClient.SqlConnection)
                return new System.Data.SqlClient.SqlDataAdapter();
            else if (scnConn is System.Data.OracleClient.OracleConnection)
                return new System.Data.OracleClient.OracleDataAdapter();
            else
                Console.Error.Write("Operacao não suportada pela Biblioteca");

            throw new Exception("Ocorreu um erro ao acessar a estrutura do banco de dados.");


        }
    }
}