using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CelulaAtivacao;
using System.Data.Common;

namespace CelulaAtivacao
{
    public class DAOCelulaAtivacao : CDBase
    {
        /// <summary>
        ///  Retorna todos os clientes que completaram apenas o Cadastro de Visitante no Portal.
        /// </summary>
        /// <param name="DataInicial">Data inicial</param>
        /// <param name="DataFinal">Data final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessot</param>
        /// <returns> DataTable </returns>
        public DataTable GetDataM1(DateTime DataInicial, DateTime DataFinal, int? Bovespa, int? CodigoAssessor)
        {
            _AcessaDados.ConnectionStringName = ConexaoCadastro;

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CELULAATIVACAO_M1");
            _AcessaDados.AddInParameter(_DbCommand, "@DATAINICIAL", DbType.DateTime, DataInicial);
            _AcessaDados.AddInParameter(_DbCommand, "@DATAFINAL", DbType.DateTime, DataFinal);
            if (null != Bovespa)
                _AcessaDados.AddInParameter(_DbCommand, "@BOVESPA", DbType.Int32, Bovespa);
            if (null != CodigoAssessor)
                _AcessaDados.AddInParameter(_DbCommand, "@CODIGOASSESSOR", DbType.Int32, CodigoAssessor);

            return _AcessaDados.ExecuteDbDataTable(_DbCommand);
        }

        /// <summary>
        ///  Retorna todos os clientes que completaram o cadastro no portal porém que não foram exportados para o Sinacor
        /// </summary>
        /// <param name="DataInicial">Data inicial</param>
        /// <param name="DataFinal">Data final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessot</param>
        /// <returns> DataTable </returns>
        public DataTable GetDataM2(DateTime DataInicial, DateTime DataFinal, int? Bovespa, int? CodigoAssessor)
        {

            _AcessaDados.ConnectionStringName = ConexaoCadastro;

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CELULAATIVACAO_M2");
            _AcessaDados.AddInParameter(_DbCommand, "@DATAINICIAL", DbType.DateTime, DataInicial);
            _AcessaDados.AddInParameter(_DbCommand, "@DATAFINAL", DbType.DateTime, DataFinal);
            if (null != Bovespa)
                _AcessaDados.AddInParameter(_DbCommand, "@BOVESPA", DbType.Int32, Bovespa);
            if (null != CodigoAssessor)
                _AcessaDados.AddInParameter(_DbCommand, "@CODIGOASSESSOR", DbType.Int32, CodigoAssessor);

            return _AcessaDados.ExecuteDbDataTable(_DbCommand);
        }

        /// <summary>
        ///  Retorna todos os clientes que foram exportados para o sistema sinacor.
        /// </summary>
        /// <param name="DataInicial">Data inicial</param>
        /// <param name="DataFinal">Data final</param>
        /// <param name="Bovespa">Código Bovespa</param>
        /// <param name="CodigoAssessor">Código do Assessot</param>
        /// <returns> DataTable </returns>
        public DataTable GetDataM3(DateTime DataInicial, DateTime DataFinal, int? Bovespa, int? CodigoAssessor)
        {
            _AcessaDados.ConnectionStringName = ConexaoCadastro;

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_SEL_CELULAATIVACAO_M3");
            _AcessaDados.AddInParameter(_DbCommand, "@DATAINICIAL", DbType.DateTime, DataInicial);
            _AcessaDados.AddInParameter(_DbCommand, "@DATAFINAL", DbType.DateTime, DataFinal);
            if (null != Bovespa)
                _AcessaDados.AddInParameter(_DbCommand, "@BOVESPA", DbType.Int32, Bovespa);
            if (null != CodigoAssessor)
                _AcessaDados.AddInParameter(_DbCommand, "@CODIGOASSESSOR", DbType.Int32, CodigoAssessor);
            return _AcessaDados.ExecuteDbDataTable(_DbCommand);
        }

        /// <summary>
        ///  Retorna todos os assessores do sistema sinacor.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAssessor()
        {
            _AcessaDados.ConnectionStringName = ConexaoCadastroOracle;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "prc_ListaComboSinacor");//"prc_sel_celula_assessor");
            _AcessaDados.AddInParameter(_DbCommand, "Informacao", DbType.Int32, 9);//9=Assessor
            return _AcessaDados.ExecuteOracleDataTable(_DbCommand);
        }

    }
}
