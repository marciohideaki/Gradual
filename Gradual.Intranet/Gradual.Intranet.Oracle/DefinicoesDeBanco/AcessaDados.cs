#region Includes
using System;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.OracleClient;
//using Gradual.Generico.Geral;
#endregion

namespace Gradual.Generico.Dados
{
    public class AcessaDados
    {
        #region Beans
        public string REGISTRONAOENCONTRADO = "Registro não Encontrado !";
        public string ConnectionStringName { set; get; }
        public string CursorRetorno { set; get; }
        #endregion

        #region Constructor
        
        /// <summary>
        /// Se o nome dos cursores de retorno nas procedures for diferente de Retorno criar no web.config a chave CursorRetorno no appsetings com o nome dos cursores
        /// </summary>
        public AcessaDados()
        {
           
            if (null == ConfigurationManager.AppSettings.Get("CursorRetorno"))
                this.CursorRetorno = "Retorno";
            else
                this.CursorRetorno = ConfigurationManager.AppSettings.Get("CursorRetorno");
        }

        public AcessaDados(string cursorRetorno)
        {
            this.CursorRetorno = cursorRetorno;
        }
        #endregion

        #region ValidaInjection
        public string ValidaInjection(string value)
        {
            string strIniciao, strFinal;
            strIniciao = value;
            strFinal = strIniciao.Replace("--", string.Empty);
            strFinal = strIniciao.Replace(";", string.Empty);
            strFinal = strIniciao.Replace("//", string.Empty);
            if (strIniciao.Length != strFinal.Length)
                throw new Exception("Caracter Inválido");
            return strFinal;
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(DbCommand scdCommand)
        {
            try
            {
                DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
                scdCommand.Connection = Conn;

                return scdCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }

        public object ExecuteScalar(DbCommand scdCommand, DbConnection Conn)
        {
            try
            {
                scdCommand.Connection = Conn;
                return scdCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region ExecuteDbDataTable
        public DataTable ExecuteDbDataTable(DbCommand scdCommand)
        {
            DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
            scdCommand.Connection = Conn;
            
            DbDataAdapter daAdapter = Conexao.obterDataAdapter(scdCommand.Connection);
            try
            {
                DataTable dtTable = new DataTable();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dtTable);

                return dtTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                daAdapter.Dispose();
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }
        #endregion

        #region ExecuteDbDataTable
        public DataTable ExecuteDbDataTable(DbCommand scdCommand, DbConnection Conn)
        {
            scdCommand.Connection = Conn;

            DbDataAdapter daAdapter = Conexao.obterDataAdapter(scdCommand.Connection);
            try
            {
                DataTable dtTable = new DataTable();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dtTable);

                return dtTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                daAdapter.Dispose();
                scdCommand.Dispose();
            }
        }
        #endregion

        #region ExecuteDbDataSet
        public DataSet ExecuteDbDataSet(DbCommand scdCommand)
        {
            DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
            scdCommand.Connection = Conn;

            DbDataAdapter daAdapter = Conexao.obterDataAdapter(scdCommand.Connection);
            try
            {
                DataSet dsDados = new DataSet();
                daAdapter.SelectCommand = scdCommand;
                daAdapter.Fill(dsDados);

                daAdapter = null;
                return dsDados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                daAdapter.Dispose();
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }
        #endregion

        #region ExecuteOracleDataTable
        public DataTable ExecuteOracleDataTable(DbCommand scdCommand)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection cn = (OracleConnection)Conexao.abrirConexao(ConnectionStringName);
                cm.Connection = cn;
                cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                OracleDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }

        public DataTable ExecuteOracleDataTableWithoutCursor(DbCommand scdCommand)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection cn = (OracleConnection)Conexao.abrirConexao(ConnectionStringName);
                cm.Connection = cn;
                //cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                OracleDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
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
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro Descrição: {0}", ex.Message));
            }

        }
        #endregion

        #region InserirOracle
        public int InserirOracle(DbCommand scdCommand)
        {
            try
            {
                DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
                scdCommand.Connection = Conn;
                this.AddOutParameter(scdCommand, "IDENTITY", DbType.Int32, 4);
                scdCommand.ExecuteNonQuery();
                return (int)scdCommand.Parameters["IDENTITY"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }
        #endregion

        #region "ExecuteNonQuery"

        public int ExecuteNonQuery(DbCommand scdCommand)
        {
            try
            {
                DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
                scdCommand.Connection = Conn;

                return scdCommand.ExecuteNonQuery();

            }
            catch (DbException ex)
            {
                throw ex;
                ////TrataException.SetError(ex);
                //int numeroErro = LogEventError.Write(ex.Message);
                //throw new Exception("Ocorreu um erro ao tentar acessar o Banco de Dados. Err.ID " + numeroErro);
            }
            catch (Exception ex2)
            {
                string x = ex2.Message;
                throw new Exception(ex2.Message, ex2);
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
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
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery(DbCommand scdCommand, DbTransaction Trans)
        {
            if (Trans != null)
            {
                try
                {
                    scdCommand.Connection = Trans.Connection;
                    scdCommand.Transaction = Trans;
                    return scdCommand.ExecuteNonQuery();
                }
                catch (DbException ex)
                {
                    throw ex;
                }
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
                DbConnection Conn = Conexao.abrirConexao(ConnectionStringName);
                scdCommand.Connection = Conn;

                return scdCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conexao.fecharConexao(scdCommand.Connection);
                scdCommand.Dispose();
            }
        }

        #region Parameter


        public DbParameter createParameter(DbCommand command, string name, DbType type, object value)
        {
            try
            {
                DbParameter p = command.CreateParameter();
                p.ParameterName = name;
                p.DbType = type;
                p.Value = value;
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object GetParameterValue(DbCommand command,
                                                string name)
        {
            try
            {
                return command.Parameters[BuildParameterName(name)].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DbParameter createParameter(DbCommand command, string name, DbType type, object value, int size)
        {
            try
            {
                DbParameter p = createParameter(command, name, type, value);
                p.Size = size;
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DbParameter createOutputParameter(DbCommand command, string name, DbType type)
        {
            try
            {
                DbParameter p = command.CreateParameter();
                p.ParameterName = name;
                p.DbType = type;
                p.Direction = ParameterDirection.ReturnValue;
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType,
                                   object value)
        {
            try
            {
                AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddOutParameter(DbCommand command,
                                  string name,
                                  DbType dbType,
                                  int size)
        {
            try
            {
                AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void AddParameter(DbCommand command,
                                string name,
                                DbType dbType,
                                ParameterDirection direction,
                                string sourceColumn,
                                DataRowVersion sourceVersion,
                                object value)
        {
            try
            {
                AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
                command.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                DbParameter param = CreateParameter(name);
                ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
                return param;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            try
            {
                param.DbType = dbType;
                param.Size = size;
                param.Value = value ?? DBNull.Value;
                param.Direction = direction;
                param.IsNullable = nullable;
                param.SourceColumn = sourceColumn;
                param.SourceVersion = sourceVersion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected DbParameter CreateParameter(string name)
        {
            try
            {
                DbProviderFactory factory = this.GetFactoty();

                DbParameter param = factory.CreateParameter();
                param.ParameterName = BuildParameterName(name);

                return param;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DbProviderFactory GetFactoty()
        {
            try
            {
                string strProviderName = ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName;

                DbProviderFactory factory = DbProviderFactories.GetFactory(strProviderName);

                return factory;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        #endregion

        public DbCommand CreateCommand(System.Data.CommandType _CommandType, string _CommandToExecute)
        {
            Conexao._ConnectionStringName = this.ConnectionStringName;
            //DbProviderFactory Factory = Conexao.GetDbProviderFactory();

            string strProviderName = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ProviderName;
            DbProviderFactory Factory = DbProviderFactories.GetFactory(strProviderName);

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

        #region ExecuteReader
        public DbDataReader ExecuteReader(DbCommand scdCommand)
        {
            try
            {
                OracleCommand cm = (OracleCommand)scdCommand;
                OracleConnection cn = (OracleConnection)Conexao.abrirConexao(ConnectionStringName);
                cm.Connection = cn;
                cm.Parameters.Add(CursorRetorno, OracleType.Cursor).Direction = ParameterDirection.Output;
                return cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Conexao.fecharConexao(scdCommand.Connection);
                //scdCommand.Dispose();
            }
        }
        #endregion
    }
}
