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
    public class NEndereco{
        /// <summary>
        /// Tipo do Endereção: Comercial, Reseidencial
        /// </summary>
        public enum eTipo
        {
            Todos,
            Comercial,
            Residencial,
            Outros
        }
        /// <summary>
        /// Endereço de Correspondência: Sim, Não, Todos
        /// </summary>
        public enum eCorrespondencia
        {
            Todos,
            Sim,
            Nao
        }
        /// <summary>
        /// Lista todos os endereços de um cliente
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <returns>Lista contendo todos os endereços do cliente</returns>
        public BindingList<EEndereco> Listar(int _ID_Cliente)
        {
            try
            {
                BindingList<EEndereco> _EEndereco = new BindingList<EEndereco>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" FROM Endereco ");
                sbSQL.Append(" where ID_CLiente = " + _ID_Cliente.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EEndereco _End = new EEndereco();
                    _End.Bairro = Conversao.ToString(item["Bairro"]);
                    _End.CEP = Conversao.ToString(item["CEP"]);
                    _End.Cidade = Conversao.ToString(item["Cidade"]);
                    _End.Complemento = Conversao.ToString(item["Complemento"]);
                    _End.Correspondencia = Conversao.ToChar(item["Correspondencia"]).Value;
                    _End.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _End.ID_Endereco = Conversao.ToInt(item["ID_Endereco"]).Value;
                    _End.Logradouro = Conversao.ToString(item["Logradouro"]);
                    _End.Numero = Conversao.ToString(item["Numero"]);
                    _End.Pais = Conversao.ToString(item["Pais"]);
                    _End.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _End.UF = Conversao.ToString(item["UF"]);
                    _EEndereco.Add(_End);
                }

                return _EEndereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista os endereços de um cliente
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <param name="_Tipo">Tipo do endereço</param>
        /// <param name="Correspondencia">Endereço de correspondência?</param>
        /// <returns>Lista os endereços do cliente</returns>
        public BindingList<EEndereco> Listar(int _ID_Cliente, eTipo _Tipo, eCorrespondencia Correspondencia)
        {
            try
            {
                BindingList<EEndereco> _EEndereco = new BindingList<EEndereco>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" FROM Endereco ");
                sbSQL.Append(" where ID_CLiente = " + _ID_Cliente.ToString());
                switch (_Tipo)
                {
                    case eTipo.Todos:
                        break;
                    case eTipo.Comercial:
                        sbSQL.Append(" and Tipo = 'C' ");
                        break;
                    case eTipo.Residencial:
                        sbSQL.Append(" and Tipo = 'R' ");
                        break;
                    case eTipo.Outros:
                        sbSQL.Append(" and Tipo = 'O' ");
                        break;
                    default:
                        break;
                }
                switch (Correspondencia)
                {
                    case eCorrespondencia.Todos:
                        break;
                    case eCorrespondencia.Sim:
                        sbSQL.Append(" and Correspondencia = 'S' ");
                        break;
                    case eCorrespondencia.Nao:
                        sbSQL.Append(" and Correspondencia = 'N' ");
                        break;
                    default:
                        break;
                }


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EEndereco _End = new EEndereco();
                    _End.Bairro = Conversao.ToString(item["Bairro"]);
                    _End.CEP = Conversao.ToString(item["CEP"]);
                    _End.Cidade = Conversao.ToString(item["Cidade"]);
                    _End.Complemento = Conversao.ToString(item["Complemento"]);
                    _End.Correspondencia = Conversao.ToChar(item["Correspondencia"]).Value;
                    _End.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _End.ID_Endereco = Conversao.ToInt(item["ID_Endereco"]).Value;
                    _End.Logradouro = Conversao.ToString(item["Logradouro"]);
                    _End.Numero = Conversao.ToString(item["Numero"]);
                    _End.Pais = Conversao.ToString(item["Pais"]);
                    _End.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _End.UF = Conversao.ToString(item["UF"]);
                    _EEndereco.Add(_End);
                }
                if (_EEndereco.Count == 0)
                    throw new Exception("REGISTRONAOENCONTRADO");

                return _EEndereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Seleciona um Endereço
        /// </summary>
        /// <param name="_ID_Endereco">Id do Endereço</param>
        /// <returns>Entidade contendo todos os dados do Endereço</returns>
        public EEndereco Selecionar(int _ID_Endereco)
        {
            try
            {
                EEndereco _EEndereco = new EEndereco();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" FROM Endereco ");
                sbSQL.Append("where ID_Endereco = " + _ID_Endereco.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EEndereco.Bairro = Conversao.ToString(_table.Rows[0]["Bairro"]);
                    _EEndereco.CEP = Conversao.ToString(_table.Rows[0]["CEP"]);
                    _EEndereco.Cidade = Conversao.ToString(_table.Rows[0]["Cidade"]);
                    _EEndereco.Complemento = Conversao.ToString(_table.Rows[0]["Complemento"]);
                    _EEndereco.Correspondencia = Conversao.ToChar(_table.Rows[0]["Correspondencia"]).Value;
                    _EEndereco.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EEndereco.ID_Endereco = Conversao.ToInt(_table.Rows[0]["ID_Endereco"]).Value;
                    _EEndereco.Logradouro = Conversao.ToString(_table.Rows[0]["Logradouro"]);
                    _EEndereco.Numero = Conversao.ToString(_table.Rows[0]["Numero"]);
                    _EEndereco.Pais = Conversao.ToString(_table.Rows[0]["Pais"]);
                    _EEndereco.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EEndereco.UF = Conversao.ToString(_table.Rows[0]["UF"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EEndereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Seleciona um Endereço
        /// </summary>
        /// <param name="_id_cliente">Id do Cliente</param>
        /// <param name="_tipo">Tipo do Endereço</param>
        /// <returns>Entidade contendo todos os dados do Endereço</returns>
        public EEndereco Selecionar(int _id_cliente, string _tipo)
        {
            try
            {
                EEndereco _EEndereco = new EEndereco();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" FROM Endereco ");
                sbSQL.AppendFormat("WHERE ID_CLIENTE = {0}", _id_cliente.ToString());
                sbSQL.AppendFormat("AND LOWER(TIPO) = '{0}'", _tipo.ToLower());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EEndereco.Bairro = Conversao.ToString(_table.Rows[0]["Bairro"]);
                    _EEndereco.CEP = Conversao.ToString(_table.Rows[0]["CEP"]);
                    _EEndereco.Cidade = Conversao.ToString(_table.Rows[0]["Cidade"]);
                    _EEndereco.Complemento = Conversao.ToString(_table.Rows[0]["Complemento"]);
                    _EEndereco.Correspondencia = Conversao.ToChar(_table.Rows[0]["Correspondencia"]).Value;
                    _EEndereco.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EEndereco.ID_Endereco = Conversao.ToInt(_table.Rows[0]["ID_Endereco"]).Value;
                    _EEndereco.Logradouro = Conversao.ToString(_table.Rows[0]["Logradouro"]);
                    _EEndereco.Numero = Conversao.ToString(_table.Rows[0]["Numero"]);
                    _EEndereco.Pais = Conversao.ToString(_table.Rows[0]["Pais"]);
                    _EEndereco.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EEndereco.UF = Conversao.ToString(_table.Rows[0]["UF"]);
                }
                else
                {
                    _EEndereco = new EEndereco();
                    //throw new Exception(CFormatacao.REGISTRONAOENCONTRADO);
                }

                return _EEndereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Seleciona o endereço de correspondência de um cliente
        /// </summary>
        /// <param name="id_cliente">Id do Cliente</param>
        /// <returns>Entidade contendo o Principal endereço do cliente</returns>
        public EEndereco ListarPrincipal(int id_cliente)
        {
            try
            {
                EEndereco _EEndereco = new EEndereco();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" SELECT ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo ");
                sbSQL.Append(" FROM Endereco ");
                //sbSQL.Append("where  Principal = 'S' and id_cliente = " + id_cliente.ToString() + " and rownum < 2");]
                sbSQL.Append("where id_cliente = " + id_cliente.ToString() + " and rownum < 2");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EEndereco.Bairro = Conversao.ToString(_table.Rows[0]["Bairro"]);
                    _EEndereco.CEP = Conversao.ToString(_table.Rows[0]["CEP"]);
                    _EEndereco.Cidade = Conversao.ToString(_table.Rows[0]["Cidade"]);
                    _EEndereco.Complemento = Conversao.ToString(_table.Rows[0]["Complemento"]);
                    _EEndereco.Correspondencia = Conversao.ToChar(_table.Rows[0]["Correspondencia"]).Value;
                    _EEndereco.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]).Value;
                    _EEndereco.ID_Endereco = Conversao.ToInt(_table.Rows[0]["ID_Endereco"]).Value;
                    _EEndereco.Logradouro = Conversao.ToString(_table.Rows[0]["Logradouro"]);
                    _EEndereco.Numero = Conversao.ToString(_table.Rows[0]["Numero"]);
                    _EEndereco.Pais = Conversao.ToString(_table.Rows[0]["Pais"]);
                    _EEndereco.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EEndereco.UF = Conversao.ToString(_table.Rows[0]["UF"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EEndereco;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Inseri um endereço de um cliente
        /// </summary>
        /// <param name="endereco">Entidade contendo todos os dados do Endereço</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Inserir(EEndereco endereco)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO Endereco ");
                sbSQL.Append(" (ID_Endereco ");
                sbSQL.Append(" ,ID_Cliente ");
                sbSQL.Append(" ,CEP ");
                sbSQL.Append(" ,Logradouro ");
                sbSQL.Append(" ,Numero ");
                sbSQL.Append(" ,Complemento ");
                sbSQL.Append(" ,Bairro ");
                sbSQL.Append(" ,Cidade ");
                sbSQL.Append(" ,UF ");
                sbSQL.Append(" ,Pais ");
                sbSQL.Append(" ,Correspondencia ");
                sbSQL.Append(" ,Tipo) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqEndereco.nextval ");
                sbSQL.Append(" , " + endereco.ID_Cliente.ToString());
                sbSQL.Append(" , '" + endereco.CEP + "'");
                sbSQL.Append(" , '" + endereco.Logradouro + "'");
                sbSQL.Append(" , '" + endereco.Numero + "'");
                sbSQL.Append(" , '" + endereco.Complemento + "'");
                sbSQL.Append(" , '" + endereco.Bairro + "'");
                sbSQL.Append(" , '" + endereco.Cidade + "'");
                sbSQL.Append(" , '" + endereco.UF + "'");
                sbSQL.Append(" , '" + endereco.Pais + "'");
                sbSQL.Append(" , '" + endereco.Correspondencia + "'");
                sbSQL.Append(" , '" + endereco.Tipo + "')");


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Altera um endereço
        /// </summary>
        /// <param name="endereco">Entidade contendo todos os dados de Endereço</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Alterar(EEndereco endereco)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" UPDATE Endereco ");
                sbSQL.Append(" SET ");
                sbSQL.Append(" CEP = '" + endereco.CEP + "'");
                sbSQL.Append(" ,Logradouro = '" + endereco.Logradouro + "'");
                sbSQL.Append(" ,Numero = '" + endereco.Numero + "'");
                sbSQL.Append(" ,Complemento = '" + endereco.Complemento + "'");
                sbSQL.Append(" ,Bairro = '" + endereco.Bairro + "'");
                sbSQL.Append(" ,Cidade = '" + endereco.Cidade + "'");
                sbSQL.Append(" ,UF = '" + endereco.UF + "'");
                sbSQL.Append(" ,Pais = '" + endereco.Pais + "'");
                sbSQL.Append(" ,Correspondencia = '" + endereco.Correspondencia + "'");
                sbSQL.Append(" ,Tipo = '" + endereco.Tipo + "'");
                sbSQL.Append("  WHERE ID_Endereco = " + endereco.ID_Endereco.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exclui um endereço
        /// </summary>
        /// <param name="_ID_Endereco">Id do Endereço</param>
        /// <returns>Quantidade de linhas alteradas no Banco de Dados</returns>
        public int Excluir(int _ID_Endereco)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" delete from endereco ");
                sbSQL.Append("  WHERE ID_Endereco = " + _ID_Endereco.ToString());

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
