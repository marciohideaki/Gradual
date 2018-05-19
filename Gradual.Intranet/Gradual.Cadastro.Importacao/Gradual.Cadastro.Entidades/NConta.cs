using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using Gradual.Generico.Geral;

namespace Gradual.Cadastro.Negocios
{
    public class NConta
    {
        /// <summary>
        /// Lista todas as contas bancárias de um cliente
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <returns></returns>
        public BindingList<EConta> Listar(int _ID_Cliente)
        {
            var _EConta = new BindingList<EConta>();
            var _AcessaDados = new AcessaDadosAntigo();

            _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

            var sbSQL = new StringBuilder();

            sbSQL.AppendLine(" SELECT id_conta");
            sbSQL.AppendLine(" ,      id_cliente");
            sbSQL.AppendLine(" ,      banco");
            sbSQL.AppendLine(" ,      agencia");
            sbSQL.AppendLine(" ,      conta");
            sbSQL.AppendLine(" ,      digito");
            sbSQL.AppendLine(" ,      tipo");
            sbSQL.AppendLine(" ,      principal");
            sbSQL.AppendLine(" FROM   conta");
            sbSQL.AppendFormat(" WHERE id_cliente = {0}", _ID_Cliente.ToString());

            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

            DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
            EConta _Con;
            foreach (DataRow item in _table.Rows)
            {
                _Con = new EConta();
                _Con.Agencia = Conversao.ToInt(item["Agencia"]).Value;
                _Con.AgenciaDigito = Gradual.Generico.Geral.Conversao.RecuperarDadosAgencia(_Con.Agencia, Conversao.ToInt(item["Agencia"]).Value, false);
                _Con.Banco = Gradual.Generico.Geral.Conversao.RecuperarDadosAgencia(_Con.Agencia, Conversao.ToInt(item["Agencia"]).Value, true);
                _Con.Conta = Conversao.ToString(item["Conta"]);
                _Con.ContaDigito = Conversao.ToString(item["Digito"]);
                _Con.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                _Con.ID_Conta = Conversao.ToInt(item["ID_Conta"]).Value;
                _Con.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                _Con.Principal = Conversao.ToChar(item["Principal"]).Value;
                _EConta.Add(_Con);
            }

            return _EConta;
        }

        /// <summary>
        /// Seleciona a principal conta bancária de um cliente
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        /// <returns>Entidade contendo a Principal conta do cliente</returns>
        public EConta ListarPrincipal(int id_cliente)
        {
            try
            {
                EConta _EConta = new EConta();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Conta ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Banco ");
                sbSQL.Append(",Agencia ");
                sbSQL.Append(",Conta ");
                sbSQL.Append(",Digito ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Principal ");
                sbSQL.Append("FROM Conta ");
                sbSQL.Append("where principal ='S' and id_cliente = " + id_cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EConta.Agencia = Conversao.ToInt(_table.Rows[0]["Agencia"]).Value;
                    _EConta.Banco = Conversao.ToInt(_table.Rows[0]["Banco"]).Value;
                    _EConta.Conta = Conversao.ToString(_table.Rows[0]["Conta"]);
                    _EConta.ContaDigito = Conversao.ToString(_table.Rows[0]["Digito"]);
                    _EConta.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EConta.ID_Conta = Conversao.ToInt(_table.Rows[0]["ID_Conta"]).Value;
                    _EConta.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EConta.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EConta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Seleciona uma conta
        /// </summary>
        /// <param name="_ID_Conta">Id da Conta bancária</param>
        /// <returns>Entidade contendo a conta selecionada</returns>
        public EConta Selecionar(int _ID_Conta)
        {
            try
            {
                EConta _EConta = new EConta();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Conta ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Banco ");
                sbSQL.Append(",Agencia ");
                sbSQL.Append(",Conta ");
                sbSQL.Append(",Digito ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Principal ");
                sbSQL.Append("FROM Conta ");
                sbSQL.Append("where ID_Conta = " + _ID_Conta.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EConta.Agencia = Conversao.ToInt(_table.Rows[0]["Agencia"]).Value;
                    _EConta.Banco = Conversao.ToInt(_table.Rows[0]["Banco"]).Value;
                    _EConta.Conta = Conversao.ToString(_table.Rows[0]["Conta"]);
                    _EConta.ContaDigito = Conversao.ToString(_table.Rows[0]["Digito"]);
                    _EConta.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EConta.ID_Conta = Conversao.ToInt(_table.Rows[0]["ID_Conta"]).Value;
                    _EConta.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EConta.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EConta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inseri uma conta bancária
        /// </summary>
        /// <param name="conta">Entidade contando os dados da conta bancária</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EConta conta)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO Conta ");
                sbSQL.Append(" (ID_Conta ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,Banco ");
                sbSQL.Append(" ,Agencia ");
                sbSQL.Append(" ,Conta ");
                sbSQL.Append(" ,Digito ");
                sbSQL.Append(" ,Tipo , Principal) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqConta.nextval ");
                sbSQL.Append(" ," + conta.ID_Cliente.ToString());
                sbSQL.Append(" ," + conta.Banco.ToString());
                sbSQL.Append(" ," + conta.Agencia.ToString());
                sbSQL.Append(" , '" + conta.Conta.ToString() + "'");
                sbSQL.Append(" , '" + conta.ContaDigito.ToString() + "'");
                sbSQL.Append(" , '" + conta.Tipo.ToString() + "'");
                sbSQL.Append(" , '" + conta.Principal.ToString() + "') ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera uma conta bancária
        /// </summary>
        /// <param name="conta">Entidade contando os dados da conta bancária</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EConta conta)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE Conta ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" Banco = " + conta.Banco.ToString());
                sbSQL.Append(" ,Agencia = " + conta.Agencia.ToString());
                sbSQL.Append(" ,Conta = '" + conta.Conta + "'");
                sbSQL.Append(" ,Digito = '" + conta.ContaDigito + "'");
                sbSQL.Append(" ,Tipo = '" + conta.Tipo.ToString() + "'");
                sbSQL.Append(" ,Principal = '" + conta.Principal.ToString() + "'");

                sbSQL.Append("  WHERE ID_Conta = " + conta.ID_Conta.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// exclui uma conta bancária
        /// </summary>
        /// <param name="_ID_Conta">Id da conta bancária</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int _ID_Conta)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" delete from conta ");
                sbSQL.Append("  WHERE ID_Conta = " + _ID_Conta.ToString());
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
