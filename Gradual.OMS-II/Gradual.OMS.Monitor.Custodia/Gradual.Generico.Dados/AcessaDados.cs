using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace Gradual.Generico.Dados
{
    public class AcessaDados : IDisposable
    {
        #region | Atributos

        public const string REGISTRONAOENCONTRADO = "Registro não Encontrado !";

        private Conexao _conexao;
        
        #endregion

        #region | Propriedades

        public string ConnectionStringName { set; get; }

        public string CursorRetorno { set; get; }

        public Conexao Conexao
        {
            get { return _conexao; }
            set { _conexao = value; }
        }

        #endregion

        #region | Constructores

        /// <summary>
        /// Se o nome dos cursores de retorno nas procedures for diferente de Retorno criar no web.config a chave CursorRetorno no appsetings com o nome dos cursores
        /// </summary>
        public AcessaDados()
        {
            this.Conexao = new Conexao();

            this.CursorRetorno = "Retorno";

            if (null != ConfigurationManager.AppSettings.Get("CursorRetorno"))
                this.CursorRetorno = ConfigurationManager.AppSettings.Get("CursorRetorno");
        }

        public AcessaDados(string cursorRetorno)
        {
            this.Conexao = new Conexao();

            this.CursorRetorno = cursorRetorno;
        }

        #endregion

        #region | ValidaInjection

        public string ValidaInjection(string value)
        {
            string strInicial, strFinal;

            strInicial = value;
            strFinal = strInicial.Replace("--", string.Empty)
                                 .Replace(";", string.Empty)
                                 .Replace("//", string.Empty);
            
            if (strInicial.Length != strFinal.Length)
                throw new Exception("Caracter Inválido");

            return strFinal;
        }
        
        #endregion

        #region | ExecuteScalar

        public object ExecuteScalar(DbCommand scdCommand)
        {
            scdCommand.Connection = this.Conexao.AbrirConexao();

            return scdCommand.ExecuteScalar();
        }

        public object ExecuteScalar(DbCommand scdCommand, DbConnection Conn)
        {
            scdCommand.Connection = Conn;
            return scdCommand.ExecuteScalar();
        }

        #endregion

        #region | ExecuteDbDataTable

        public DataTable ExecuteDbDataTable(DbCommand scdCommand)
        {
            scdCommand.Connection = this.Conexao.AbrirConexao();

            DbDataAdapter daAdapter = this.Conexao.ObterDataAdapter(scdCommand.Connection);

            try
            {
                var dtTable = new DataTable();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dtTable);

                return dtTable;
            }
            finally
            {
                daAdapter.Dispose();
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | ExecuteDbDataTable

        public DataTable ExecuteDbDataTable(DbCommand scdCommand, DbConnection Conn)
        {
            scdCommand.Connection = Conn;

            DbDataAdapter daAdapter = Conexao.ObterDataAdapter(scdCommand.Connection);

            try
            {
                var dtTable = new DataTable();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dtTable);

                return dtTable;
            }
            finally
            {
                daAdapter.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | ExecuteDbDataSet

        public DataSet ExecuteDbDataSet(DbCommand scdCommand)
        {
            scdCommand.Connection = this.Conexao.AbrirConexao();

            DbDataAdapter daAdapter = this.Conexao.ObterDataAdapter(scdCommand.Connection);

            try
            {
                var dsDados = new DataSet();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dsDados);

                daAdapter = null;
                return dsDados;
            }
            finally
            {
                daAdapter.Dispose();
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | ExecuteOracleDataTable

        public DataTable ExecuteOracleDataTable(DbCommand scdCommand)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection cn = (OracleConnection)this.Conexao.AbrirConexao();
                cm.Connection = cn;
                cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                OracleDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            finally
            {
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        public DataTable ExecuteOracleDataTable(DbCommand scdCommand, DbConnection Conn)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection Cnn = (OracleConnection)Conn;

                cm.Connection = Cnn;
                cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                OracleDataReader dr = cm.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            finally 
            {
                Conn.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | InserirOracle

        public int InserirOracle(DbCommand scdCommand)
        {
            try
            {
                DbConnection Conn = this.Conexao.AbrirConexao();
                scdCommand.Connection = Conn;
                this.AddOutParameter(scdCommand, "IDENTITY", DbType.Int32, 4);
                scdCommand.ExecuteNonQuery();
                return (int)scdCommand.Parameters["IDENTITY"].Value;
            }
            finally
            {
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | ExecuteNonQuery

        public int ExecuteNonQuery(DbCommand scdCommand)
        {
            try
            {
                scdCommand.Connection = this.Conexao.AbrirConexao();

                return scdCommand.ExecuteNonQuery();
            }
            finally
            {
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        public int ExecuteNonQuery(DbCommand scdCommand, DbConnection Conn)
        {
            try
            {
                scdCommand.Connection = Conn;
                return scdCommand.ExecuteNonQuery();
            }
            finally 
            {
                scdCommand.Dispose();
                Conn.Dispose();
            }
        }

        public int ExecuteNonQuery(DbCommand scdCommand, DbTransaction Trans)
        {
            if (Trans != null)
            {
                scdCommand.Transaction = Trans;
                return scdCommand.ExecuteNonQuery();
            }
            else
            {
                return this.ExecuteNonQuery(scdCommand);
            }
        }

        public object ExecuteNonQuery(DbCommand scdCommand, bool bolHasReturnValue)
        {
            try
            {
                scdCommand.Connection = this.Conexao.AbrirConexao();

                return scdCommand.ExecuteNonQuery();
            }
            finally
            {
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        #region Parameter

        public DbParameter createParameter(DbCommand command, string name, DbType type, object value)
        {
            DbParameter dbParameter = command.CreateParameter();
            dbParameter.ParameterName = name;
            dbParameter.DbType = type;
            dbParameter.Value = value;
            return dbParameter;
        }

        public object GetParameterValue(DbCommand command, string name)
        {
            return command.Parameters[BuildParameterName(name)].Value;
        }

        public DbParameter createParameter(DbCommand command, string name, DbType type, object value, int size)
        {
            DbParameter dbParameter = createParameter(command, name, type, value);
            dbParameter.Size = size;
            return dbParameter;
        }

        public DbParameter createOutputParameter(DbCommand command, string name, DbType type)
        {
            DbParameter dbParameter = command.CreateParameter();
            dbParameter.ParameterName = name;
            dbParameter.DbType = type;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            return dbParameter;
        }

        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        public virtual void AddParameter(DbCommand command,
                                        string name,
                                        DbType dbType,
                                        int size,
                                        ParameterDirection direction,
                                        bool nullable,
                                        byte precision,
                                        byte scale,
                                        string sourceColumn,
                                        DataRowVersion sourceVersion,
                                        object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        protected DbParameter CreateParameter(string name,
                                            DbType dbType,
                                            int size,
                                            ParameterDirection direction,
                                            bool nullable,
                                            byte precision,
                                            byte scale,
                                            string sourceColumn,
                                            DataRowVersion sourceVersion,
                                            object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        protected virtual void ConfigureParameter(DbParameter param,
                                                 string name,
                                                 DbType dbType,
                                                 int size,
                                                 ParameterDirection direction,
                                                 bool nullable,
                                                 byte precision,
                                                 byte scale,
                                                 string sourceColumn,
                                                 DataRowVersion sourceVersion,
                                                 object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        protected DbParameter CreateParameter(string name)
        {
            DbProviderFactory factory = this.GetFactoty();

            DbParameter bParameter = factory.CreateParameter();
            bParameter.ParameterName = this.BuildParameterName(name);

            return bParameter;
        }

        private DbProviderFactory GetFactoty()
        {
            string strProviderName = ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName;

            DbProviderFactory factory = DbProviderFactories.GetFactory(strProviderName);

            return factory;
        }

        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        #endregion

        public DbCommand CreateCommand(System.Data.CommandType _CommandType, string _CommandToExecute)
        {
            this.Conexao._ConnectionStringName = this.ConnectionStringName;
            DbProviderFactory Factory = this.Conexao.GetDbProviderFactory();

            DbCommand _DbCommand = Factory.CreateCommand();
            _DbCommand.CommandText = _CommandToExecute;
            _DbCommand.CommandType = _CommandType;

            return _DbCommand;
        }

        public DbCommand CreateCommand(DbTransaction _Transaction, System.Data.CommandType _CommandType, string _CommandToExecute)
        {
            if (_Transaction != null)
            {
                DbCommand _DbCommand = _Transaction.Connection.CreateCommand();
                _DbCommand.CommandText = _CommandToExecute;
                _DbCommand.CommandType = _CommandType;
                return _DbCommand;
            }
            else
            {
                return this.CreateCommand(_CommandType, _CommandToExecute);
            }
        }

        #endregion

        #region | ExecuteReader

        public DbDataReader ExecuteReader(DbCommand scdCommand)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection cn = (OracleConnection)this.Conexao.AbrirConexao();
                cm.Connection = cn;
                cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                return cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            finally
            {
                this.Conexao.Dispose();
                scdCommand.Dispose();
            }
        }

        #endregion

        #region | Implementação da interface IDisposable

        public void Dispose()
        {
            this.Conexao.Dispose();
        }

        #endregion
    }
}
