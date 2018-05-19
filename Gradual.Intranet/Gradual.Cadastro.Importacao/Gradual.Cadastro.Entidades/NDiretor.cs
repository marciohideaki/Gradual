using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.Cadastro.Entidades;
using System.ComponentModel;
using Gradual.Generico.Dados;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Negocios
{
    public class NDiretor
    {
        /// <summary>
        /// Inseri um diretor de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_diretor">Entidade contendo os dados do Diretor</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EDiretor _diretor)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" INSERT INTO Diretor ");
                sbSQL.Append(" (ID_Diretor ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,CPF ");
                sbSQL.Append(" ) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqDiretor.nextval ");
                sbSQL.Append(" , " + _diretor.ID_Cliente.ToString());
                sbSQL.Append(" , '" + _diretor.Nome + "'");
                sbSQL.Append(" , '" + _diretor.Identidade + "'");
                sbSQL.Append(" , '" + _diretor.Cpf + "')");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera o Diretor
        /// </summary>
        /// <param name="_diretor">Entidade contendo os dados do Diretor</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EDiretor _diretor)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" UPDATE Diretor ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" Nome = '" + _diretor.Nome + "'");
                sbSQL.Append(" ,Identidade = '" + _diretor.Identidade + "'");
                sbSQL.Append(" ,Cpf = '" + _diretor.Cpf + "'");
                sbSQL.Append("  WHERE id_diretor = " + _diretor.ID_Diretor.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui um diretor de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Diretor">Id do Diretor</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int _ID_Diretor)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" delete from diretor ");
                sbSQL.Append("  WHERE ID_diretor = " + _ID_Diretor.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Seleciona um diretor de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Diretor">Id do Diretor</param>
        /// <returns>Entidade contendo os dados do Diretor selecionado</returns>
        public EDiretor Selecionar(int _ID_Diretor)
        {
            try
            {
                EDiretor _EDiretor = new EDiretor();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" SELECT id_diretor ");
                sbSQL.Append(" ,id_cliente ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,Cpf ");
                sbSQL.Append(" FROM Diretor ");
                sbSQL.Append("where ID_Diretor = " + _ID_Diretor.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EDiretor.ID_Diretor = Conversao.ToInt(_table.Rows[0]["ID_Diretor"]);
                    _EDiretor.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]);
                    _EDiretor.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
                    _EDiretor.Identidade = Conversao.ToString(_table.Rows[0]["Identidade"]);
                    _EDiretor.Cpf = Conversao.ToString(_table.Rows[0]["Cpf"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EDiretor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista todos os diretores de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <returns>Lista contendo todos os Diretores do Cliente informado</returns>
        public BindingList<EDiretor> Listar(int _ID_Cliente)
        {
            try
            {
                BindingList<EDiretor> _EDiretor = new BindingList<EDiretor>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" SELECT id_diretor ");
                sbSQL.Append(" ,id_cliente ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,Cpf ");
                sbSQL.Append(" FROM Diretor ");
                sbSQL.Append("where ID_Cliente = " + _ID_Cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EDiretor _Edir = new EDiretor();
                    _Edir.ID_Diretor = Conversao.ToInt(item["ID_Diretor"]);
                    _Edir.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]);
                    _Edir.Nome = Conversao.ToString(item["Nome"]);
                    _Edir.Identidade = Conversao.ToString(item["Identidade"]);
                    _Edir.Cpf = Conversao.ToString(item["Cpf"]);
                    _EDiretor.Add(_Edir);
                }

                return _EDiretor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
