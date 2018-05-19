using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

using log4net;
using Gradual.Core.OMS.LimiteManager.Lib.Dados;
using System.Data;

namespace Gradual.Core.OMS.LimiteManager.Db.Database
{
    public class DbAccess
    {
        #region log4net declaration
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion


        #region Private Variables
        private SqlConnection _sqlConn;
        private SqlCommand _sqlCommand;
        string _strConnectionStringDefault;
        #endregion

        #region Properties
        public bool ConexaoIniciada
        {
            get
            {
                return !(_sqlConn == null);
            }
        }
        public bool ConexaoAberta
        {
            get
            {
                return (_sqlConn != null && _sqlConn.State == System.Data.ConnectionState.Open);
            }
        }

        #endregion


        public DbAccess()
        {
            _sqlConn = null;
            _sqlCommand = null;
            _strConnectionStringDefault = ConfigurationManager.ConnectionStrings["GradualSpider"].ConnectionString;
        }
        
        ~DbAccess()
        {
            if (null != _sqlCommand)
            {
                _sqlCommand.Dispose();
                _sqlCommand = null;
            }
            if (null!=_sqlConn)
            {
                _fecharConexao();
            }

        }

        private void _abrirConexao()
        {
            this._abrirConexao(_strConnectionStringDefault);
        }

        private void _abrirConexao(string strConnectionString)
        {
            if (!this.ConexaoAberta)
            {
                _sqlConn = new SqlConnection(strConnectionString);
                _sqlConn.Open();
            }
        }

        private void _fecharConexao()
        {
            try
            {
                _sqlConn.Close();
                _sqlConn.Dispose();
            }
            catch { }
        }


        public bool AtualizarMvtoBvsp(OperatingLimitInfo opLimit)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_lmt_atualiza_posicao_limite_oms", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente_parametro", opLimit.CodigoParametroCliente));
                _sqlCommand.Parameters.Add(new SqlParameter("@ValorAlocado", opLimit.ValorAlocado));
                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                if (rows > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de limite Bovespa: " + ex.Message, ex);
                return false;
            }
        }

        public bool InserirMvtoBvsp(OperatingLimitInfo opLimit)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_lmt_insere_cliente_parametro_valor", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@id_cliente_parametro", opLimit.CodigoParametroCliente));
                _sqlCommand.Parameters.Add(new SqlParameter("@ValorMovimento", opLimit.ValorMovimento));
                _sqlCommand.Parameters.Add(new SqlParameter("@ValorAlocado", opLimit.ValorAlocado));
                _sqlCommand.Parameters.Add(new SqlParameter("@ValorDisponivel", opLimit.ValorDisponivel));
                _sqlCommand.Parameters.Add(new SqlParameter("@Descricao", "ATUALIZACAO LIMITE OPERACIONAL"));
                _sqlCommand.Parameters.Add(new SqlParameter("@StNatureza", opLimit.StNatureza));
                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                if (rows > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de limite Bovespa: " + ex.Message, ex);
                return false;
            }
        }

        public bool AtualizarMvtoBMFContrato(ClientLimitContractBMFInfo contractLimit)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_lmt_atualiza_limite_bmf_contrato", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@idClienteParametroBMF", contractLimit.IdClienteParametroBMF));
                _sqlCommand.Parameters.Add(new SqlParameter("@qtDisponivel", contractLimit.QuantidadeDisponivel));
                _sqlCommand.Parameters.Add(new SqlParameter("@dtMovimento", contractLimit.DataMovimento));
                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                if (rows > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de limite de contrato BMF: " + ex.Message, ex);
                return false;
            }
        }

        public bool AtualizarMvtoBMFInstrumento(ClientLimitInstrumentBMFInfo instLimit)
        {
            try
            {
                _abrirConexao(_strConnectionStringDefault);
                _sqlCommand = new SqlCommand("prc_lmt_atualiza_limite_bmf_instrumento", _sqlConn);
                _sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                _sqlCommand.Parameters.Add(new SqlParameter("@idClienteParametroInstrumento", instLimit.IdClienteParametroInstrumento));
                _sqlCommand.Parameters.Add(new SqlParameter("@idClienteParametroBMF", instLimit.IdClienteParametroBMF));
                _sqlCommand.Parameters.Add(new SqlParameter("@qtDisponivel", instLimit.QtDisponivel));
                _sqlCommand.Parameters.Add(new SqlParameter("@dtMovimento", instLimit.dtMovimento));
                int rows = _sqlCommand.ExecuteNonQuery();
                _fecharConexao();
                if (rows > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Problemas na atualizacao de movimentacao de limite de instrumento BMF: " + ex.Message, ex);
                throw ex;
            }
        }


    }
}
