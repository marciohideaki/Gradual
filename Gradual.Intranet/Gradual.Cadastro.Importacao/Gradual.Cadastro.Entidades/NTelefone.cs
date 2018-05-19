using System;
using System.Collections.Generic;
using System.Data;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Negocios
{
    public class NTelefone
    {
        /// <summary>
        /// Método para listar todos os telefones do cliente.
        /// </summary>
        /// <param name="_ID_Cliente">Código do Cliente</param>
        /// <returns>Números de contato do cliente.</returns>
        public BindingList<ETelefone> Listar(int _ID_Cliente)
        {
            try
            {

                BindingList<ETelefone> _ETelefone = new BindingList<ETelefone>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Telefone ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,DDD ");
                sbSQL.Append(" ,Telefone ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Tipo ,ramal ");
                sbSQL.Append(" FROM Telefone ");
                sbSQL.Append(" where  ID_Cliente = " + _ID_Cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    ETelefone _Tel = new ETelefone();
                    _Tel.DDD =Conversao.ToString( item["DDD"]);
                    _Tel.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _Tel.ID_Telefone = Conversao.ToInt(item["ID_Telefone"]).Value;
                    _Tel.Principal = Conversao.ToChar(item["Principal"]).Value;
                    _Tel.Telefone = Conversao.ToString( item["Telefone"]);
                    _Tel.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _Tel.Ramal = Conversao.ToString(item["Ramal"]);

                    _ETelefone.Add(_Tel);
                }

                return _ETelefone;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para selecionar um telefone em específico
        /// </summary>
        /// <param name="_ID_Telefone">Código do Telefone</param>
        /// <returns>Informações do contato</returns>
        public ETelefone Selecionar(int _ID_Telefone)
        {
            try
            {
                ETelefone _ETelefone = new ETelefone();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Telefone ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,DDD ");
                sbSQL.Append(" ,Telefone ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Tipo,ramal");
                sbSQL.Append(" FROM Telefone ");
                sbSQL.Append(" where  ID_Telefone = " + _ID_Telefone.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _ETelefone.DDD = Conversao.ToString(_table.Rows[0]["DDD"]);
                    _ETelefone.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _ETelefone.ID_Telefone = Conversao.ToInt(_table.Rows[0]["ID_Telefone"]).Value;
                    _ETelefone.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                    _ETelefone.Telefone = Conversao.ToString(_table.Rows[0]["Telefone"]);
                    _ETelefone.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _ETelefone.Ramal = Conversao.ToString(_table.Rows[0]["Ramal"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _ETelefone;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método para selecionar um tipo de telefone do cliente
        /// </summary>
        /// <param name="_id_cliente">Código do cliente</param>
        /// <param name="_tipo">Tipo de contato</param>
        /// <returns>Informações do contato</returns>
        public ETelefone Selecionar(int _id_cliente, string _tipo)
        { 
            try
            {
                ETelefone _ETelefone = new ETelefone();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Telefone ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,DDD ");
                sbSQL.Append(" ,Telefone ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Tipo,ramal");
                sbSQL.Append(" FROM Telefone ");
                sbSQL.AppendFormat(" where  ID_CLIENTE = {0}", _id_cliente.ToString());
                sbSQL.AppendFormat(" AND  LOWER(TIPO) = '{0}'", _tipo.ToLower());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _ETelefone.DDD = Conversao.ToString(_table.Rows[0]["DDD"]);
                    _ETelefone.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _ETelefone.ID_Telefone = Conversao.ToInt(_table.Rows[0]["ID_Telefone"]).Value;
                    _ETelefone.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                    _ETelefone.Telefone = Conversao.ToString(_table.Rows[0]["Telefone"]);
                    _ETelefone.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _ETelefone.Ramal = Conversao.ToString(_table.Rows[0]["Ramal"]);
                }
                else
                {
                    //throw new Exception(CFormatacao.REGISTRONAOENCONTRADO);
                    _ETelefone = new ETelefone();
                }

                return _ETelefone;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista o contato principal do cliente
        /// </summary>
        /// <param name="_ID_Cliente">Código do Cliente</param>
        /// <returns>Dados do telefone principal do cliente</returns>
        public ETelefone ListarPrincipal(int? _ID_Cliente)
        {
            try
            {
                if (_ID_Cliente == null)
                    throw new Exception("Código do Cliente não fornecido!");

                ETelefone _ETelefone = new ETelefone();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Telefone ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,DDD ");
                sbSQL.Append(" ,Telefone ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" ,Ramal ");
                sbSQL.Append(" FROM Telefone ");
                sbSQL.Append(" where  ID_Cliente = " + _ID_Cliente.ToString());
                sbSQL.Append(" and  Principal = 'S'" );

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _ETelefone.DDD = Conversao.ToString(_table.Rows[0]["DDD"]);
                    _ETelefone.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _ETelefone.ID_Telefone = Conversao.ToInt(_table.Rows[0]["ID_Telefone"]).Value;
                    _ETelefone.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                    _ETelefone.Telefone = Conversao.ToString(_table.Rows[0]["Telefone"]);
                    _ETelefone.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _ETelefone.Ramal = Conversao.ToString(_table.Rows[0]["Ramal"]);
                }
                else {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _ETelefone;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Alterar o telefone principal do cliente.
        /// </summary>
        /// <param name="telefone">Dados do telefone</param>
        /// <returns>Número de linhas afetadas</returns>
        public int AlterarPrincipal(ETelefone telefone)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE Telefone ");
                sbSQL.Append(" SET ");
                sbSQL.Append("  DDD = '" + telefone.DDD.ToString() + "'");
                sbSQL.Append(" ,Telefone = '" + telefone.Telefone.ToString() + "'");
                sbSQL.Append(" ,Tipo = '" + Conversao.ToString(telefone.Tipo) + "'");
                sbSQL.Append(" ,ramal = '" + Conversao.ToString(telefone.Ramal) + "'");
                sbSQL.Append(" WHERE ID_Cliente = " + telefone.ID_Cliente + " ");
                sbSQL.Append(" AND Principal = 'S'");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Cadastra um telefone no banco de dados.
        /// </summary>
        /// <param name="telefone">Informações do telefone</param>
        /// <returns>Número de linhas afetadas</returns>
        public int Inserir(ETelefone telefone)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO Telefone ");
                sbSQL.Append(" (ID_Telefone ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,DDD ");
                sbSQL.Append(" ,Telefone ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Tipo , Ramal) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqTelefone.nextval ");
                sbSQL.Append(" , " + telefone.ID_Cliente.ToString());
                sbSQL.Append(" , '" + telefone.DDD.ToString() + "'");
                sbSQL.Append(" , '" + telefone.Telefone.ToString() + "'");
                sbSQL.Append(" , '" + telefone.Principal.ToString() + "'");
                sbSQL.Append(" , '" + telefone.Tipo.ToString() + "'");
                sbSQL.Append(" , '" + telefone.Ramal.ToString() + "')");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Altera os dados de um telefone
        /// </summary>
        /// <param name="telefone">Informações do telefone</param>
        /// <returns>Número de linhas afetadas.</returns>
        public int Alterar(ETelefone telefone)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE Telefone ");
                sbSQL.Append(" SET ");
                sbSQL.Append("  DDD = '"+telefone.DDD.ToString()+"'");
                sbSQL.Append(" ,Telefone = '" + telefone.Telefone.ToString()+"'");
                sbSQL.Append(" ,Principal = '"+telefone.Principal+"'");
                sbSQL.Append(" ,Tipo = '"+telefone.Tipo.ToString()+"'");
                sbSQL.Append(" ,ramal = '" + telefone.Ramal.ToString() + "'");
                sbSQL.Append(" WHERE ID_Telefone = " + telefone.ID_Telefone.ToString() );

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exclui um telefone
        /// </summary>
        /// <param name="_ID_Telefone">Código do telefone</param>
        /// <returns>Número de linhas afetadas</returns>
        public int Excluir(int _ID_Telefone)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" delete from telefone where ID_Telefone = " + _ID_Telefone.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
