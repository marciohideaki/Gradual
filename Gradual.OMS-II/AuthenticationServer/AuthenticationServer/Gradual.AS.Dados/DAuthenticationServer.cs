using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using AS.Messages;
using System.Globalization;
using log4net;

namespace AS.Dados
{
    public class DAuthenticationServer
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private enum ERetorno{
            Sucesso = 1,
            FalhaAutenticacao = 0,
            UsuarioLogado = 2
        };

        private string ConnectionString{
            get{
                return ConfigurationSettings.AppSettings["MDS"].ToString();
            }
        }   

        public virtual object InsertAccess(SignIn _SignIn){

            SqlConnection _Conn = null;
            try
            {
                _Conn = new SqlConnection(ConnectionString);
                _Conn.Open();

                SqlCommand _Command = new SqlCommand("prc_ins_MdsAuthentication", _Conn);
                _Command.CommandType = CommandType.StoredProcedure;

                _Command.Parameters.Add(new SqlParameter("@IdCliente", int.Parse(_SignIn.pStrIdCliente.Trim())));
                _Command.Parameters.Add(new SqlParameter("@idSistema", int.Parse(_SignIn.pStrIdSistema)));
                _Command.Parameters.Add(new SqlParameter("@InitialDateTime", DateTime.ParseExact(_SignIn.pStrInitialDateTime.Trim().Substring(0,14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture)));                
                _Command.Parameters.Add(new SqlParameter("@FlgBlocked", "S"));

                SqlParameter OutputParameter = new SqlParameter();
                OutputParameter.Direction = ParameterDirection.Output;
                OutputParameter.Size = 40;
                OutputParameter.DbType = DbType.String;
                OutputParameter.ParameterName = "@UniqueId";
                _Command.Parameters.Add(OutputParameter);
                OutputParameter = null;

                OutputParameter = new SqlParameter();
                OutputParameter.Direction = ParameterDirection.Output;
                OutputParameter.Size = 4;
                OutputParameter.DbType = DbType.Int32;
                OutputParameter.ParameterName = "@ErrorCode";
                _Command.Parameters.Add(OutputParameter);

               _Command.ExecuteNonQuery();

                int ErrorCode = int.Parse(_Command.Parameters["@ErrorCode"].Value.ToString());

                if (ErrorCode == (int)ERetorno.Sucesso){
                    return _Command.Parameters["@UniqueId"].Value.ToString();
                }
                else if (ErrorCode == (int)ERetorno.UsuarioLogado){
                    return  ERetorno.UsuarioLogado;
                }
            
            }
            catch (SqlException ex)
            {
                logger.Error("Erro em InsertAccess(): " + ex.Message, ex);
            }
            finally
            {
                _Conn.Close();
                _Conn.Dispose();
            }
            return ERetorno.FalhaAutenticacao;
        }

        public virtual bool UpdateAccess(SignOut _SignOut)
        {
            SqlConnection _Conn = null;
            try
            {
                _Conn = new SqlConnection(ConnectionString);
                _Conn.Open();

                SqlCommand _Command = new SqlCommand("prc_upd_MdsAuthentication", _Conn);
                _Command.CommandType = CommandType.StoredProcedure;

                _Command.Parameters.Add(new SqlParameter("@idSistema", int.Parse(_SignOut.pStrIdSistema)));
                _Command.Parameters.Add(new SqlParameter("@idCliente", int.Parse(_SignOut.pStrIdCliente)));
                _Command.Parameters.Add(new SqlParameter("@OutDateTime", DateTime.Parse(_SignOut.pStrOutDateTime)));
                _Command.Parameters.Add(new SqlParameter("@FlgBlocked", "N"));

                _Command.ExecuteNonQuery();
    
                return true;

            }

            catch (SqlException ex)
            {
                logger.Error("Erro em InsertAccess(): " + ex.Message, ex);
            }
            finally
            {
                _Conn.Close();
                _Conn.Dispose();
            }
            return false;
        }
    }
}
