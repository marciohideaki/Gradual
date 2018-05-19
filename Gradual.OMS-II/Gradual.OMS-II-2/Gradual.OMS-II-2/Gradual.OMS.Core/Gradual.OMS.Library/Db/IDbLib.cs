using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Library.Db
{
    /// <summary>
    /// Interface para bibliotecas de acesso a banco de dados relacional
    /// </summary>
    public interface IDbLib
    {
        /// <summary>
        /// Pede inicialização da instancia
        /// </summary>
        /// <param name="parametros"></param>
        void Inicializar(object parametros);

        /// <summary>
        /// Pede finalização da instancia
        /// </summary>
        void Finalizar();

        /// <summary>
        /// Solicita a execução de uma procedure.
        /// Os parametros são passados em duplas de nome do parametro, valor
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        DataSet ExecutarProcedure(string nomeProcedure, params object[] parametros);

        /// <summary>
        /// Overload da execução de procedure que recebe a lista de parametros através de 
        /// um dicionário.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        DataSet ExecutarProcedure(string nomeProcedure, Dictionary<string, object> parametros);

        /// <summary>
        /// Overload da execução de procedure que recebe os parametros num dicionário e permite
        /// o retorno de parametros de output.
        /// O valor dos parametros de output são retornados na propria coleção de parametros.
        /// A lista outputParams indica quais são os parametros de output.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <param name="parametros"></param>
        /// <param name="outputParams"></param>
        /// <returns></returns>
        DataSet ExecutarProcedure(string nomeProcedure, Dictionary<string, object> parametros, List<string> outputParams);

        /// <summary>
        /// Solicita a execução de uma string sql.
        /// Os parametros são passados em duplas de nome do parametro, valor
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        DataSet ExecutarConsulta(string sql, params object[] parametros);

        /// <summary>
        /// Overload da execução de string sql que recebe os parametros num dicionário
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        DataSet ExecutarConsulta(string sql, Dictionary<string, object> parametros);
    }
}
