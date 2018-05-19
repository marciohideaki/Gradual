using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Gradual.Generico.Dados
{
    public static class LogEventError
    {
        public static int Write(string value)
        {
            int IdErro = 0;

            //try
            //{
            //    string Erro = value.ToString();
            //    string Classe = new StackTrace().GetFrame(2).GetMethod().ReflectedType.FullName;
            //    string Metodo = new StackTrace().GetFrame(2).GetMethod().ToString();

            //    AcessaDados _AcessaDados = new AcessaDados();

            //    _AcessaDados.ConnectionStringName = Conexao.ConnectionName;

            //    DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "inserirErro");
            //    _AcessaDados.AddInParameter(_DbCommand, "pData", DbType.Date, DateTime.Now.ToString("dd-MMM-yy hh:mm:ss"));
            //    _AcessaDados.AddInParameter(_DbCommand, "pClasse", DbType.AnsiString, Classe);
            //    _AcessaDados.AddInParameter(_DbCommand, "pMetodo", DbType.AnsiString, Metodo);
            //    _AcessaDados.AddInParameter(_DbCommand, "pMensagem", DbType.AnsiString, Erro);
            //    _AcessaDados.AddOutParameter(_DbCommand, "pID_Erro", DbType.Int32, 8);

            //    _AcessaDados.ExecuteScalar(_DbCommand);

            //    IdErro = (int)(_AcessaDados.GetParameterValue(_DbCommand, "pID_Erro"));
            //    return IdErro;
            //}
            //catch (Exception ex)
            //{
            //    EventLog.WriteEntry(Conexao.ConnectionName, ex.Message, EventLogEntryType.Error );
            //    return IdErro;
            //}

            return IdErro;
        }
    }
}
