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
    public class NEmitente
    {
        /// <summary>
        /// Inseri um emitente de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_emitente">Entidade contendo todos os dados do Emitente a ser inserido</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EEmitente _emitente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO Emitente ");
                sbSQL.Append(" (id_emitente ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Cpf ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,Sistema ");
                sbSQL.Append(") VALUES ");
                sbSQL.Append(" ( seqEmitente.nextval ");
                sbSQL.Append(" , " + _emitente.ID_Cliente.ToString());
                sbSQL.Append(" , '" + _emitente.Principal.ToString() + "'");
                sbSQL.Append(" , '" + _emitente.Nome + "'");
                sbSQL.Append(" , '" + _emitente.Cpf + "'");
                sbSQL.Append(" , '" + _emitente.Identidade + "'");
                sbSQL.Append(" , '" + _emitente.Sistema + "')");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera um emitente de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_emitente">Entidade contendo todos os dados do Emitente a ser alterado</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EEmitente _emitente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE Emitente ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" Principal = '" + _emitente.Principal.ToString() + "'");
                sbSQL.Append(" ,Nome = '" + _emitente.Nome + "'");
                sbSQL.Append(" ,Cpf = '" + _emitente.Cpf + "'");
                sbSQL.Append(" ,Identidade = '" + _emitente.Identidade + "'");
                sbSQL.Append(" ,Sistema = '" + _emitente.Sistema + "'");
                sbSQL.Append("  WHERE id_emitente = " + _emitente.ID_Emitente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exclui um emitente de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Emitente">Id do Emitente</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int _ID_Emitente)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" delete from emitente ");
                sbSQL.Append("  WHERE ID_emitente = " + _ID_Emitente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Seleciona um emitente de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Emitente">Id do Emitente</param>
        /// <returns>Entidade contendo todos os dados do emitente selecionado</returns>
        public EEmitente Selecionar(int _ID_Emitente)
        {
            try
            {
                EEmitente _EEmitente = new EEmitente();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,id_emitente ");
                sbSQL.Append(" ,id_cliente ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Cpf ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,Sistema ");
                sbSQL.Append(" FROM emitente ");
                sbSQL.Append("where id_emitente = " + _ID_Emitente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EEmitente.ID_Emitente = Conversao.ToInt(_table.Rows[0]["id_emitente"]);
                    _EEmitente.ID_Cliente = Conversao.ToInt(_table.Rows[0]["id_cliente"]);
                    _EEmitente.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]);
                    _EEmitente.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
                    _EEmitente.Cpf = Conversao.ToString(_table.Rows[0]["Cpf"]);
                    _EEmitente.Identidade = Conversao.ToString(_table.Rows[0]["Identidade"]);
                    _EEmitente.Sistema = Conversao.ToString(_table.Rows[0]["Sistema"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EEmitente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista todos os emitentes de um cliente PJ ou Clubes/Fundos
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <returns>Lista contendo todos os emitentes do cliente informado</returns>
        public BindingList<EEmitente> Listar(int _ID_Cliente)
        {
            try
            {
                BindingList<EEmitente> _EEmitente = new BindingList<EEmitente>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT  ");
                sbSQL.Append(" id_emitente ");
                sbSQL.Append(" ,id_cliente ");
                sbSQL.Append(" ,Principal ");
                sbSQL.Append(" ,Nome ");
                sbSQL.Append(" ,Cpf ");
                sbSQL.Append(" ,Identidade ");
                sbSQL.Append(" ,Sistema ");
                sbSQL.Append(" FROM emitente ");
                sbSQL.Append(" where ID_CLiente = " + _ID_Cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EEmitente _EEmit = new EEmitente();
                    _EEmit.ID_Emitente = Conversao.ToInt(item["id_emitente"]);
                    _EEmit.ID_Cliente = Conversao.ToInt(item["id_cliente"]);
                    _EEmit.Principal = Conversao.ToChar(item["Principal"]);
                    _EEmit.Nome = Conversao.ToString(item["Nome"]);
                    _EEmit.Cpf = Conversao.ToString(item["Cpf"]);
                    _EEmit.Identidade = Conversao.ToString(item["Identidade"]);
                    _EEmit.Sistema = Conversao.ToString(item["Sistema"]);
                    _EEmitente.Add(_EEmit);
                }

                return _EEmitente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
