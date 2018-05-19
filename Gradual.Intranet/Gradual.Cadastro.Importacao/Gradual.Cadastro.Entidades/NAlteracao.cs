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
    public class NAlteracao
    {
        /// <summary>
        /// Informa se o filtro será por Todas, Realizadas ou Pendentes
        /// </summary>
        public enum eStatus
        {
            Todas,
            Realizadas,
            Pendentes
        }

        /// <summary>
        /// Informa se a alteração já foi realizada
        /// </summary>
        public enum eRealizada
        {
            Sim,
            Não
        }




        /// <summary>
        /// Lista as Solicitações de Alteração
        /// </summary>
        /// <param name="status">Tipo de Filtro: Todas, Pendentes ou Realizadas</param>
        /// <returns>Lista com as alterações constantes no filtro</returns>
        public BindingList<EAlteracao> Listar(eStatus status)
        {
            try
            {
                BindingList<EAlteracao> _EAlteracao = new BindingList<EAlteracao>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Alteracao, ID_Cliente, Data, Campo,Alteracao.Ip, Alteracao.Tipo, Descricao, DataRealizada, Alteracao.ID_Administrador, Nome ");
                sbSQL.Append("FROM (Administrador INNER JOIN Login ON Administrador.ID_Login = Login.ID_Login) ");
                sbSQL.Append("RIGHT JOIN Alteracao ON Administrador.ID_Administrador = Alteracao.ID_Administrador ");

                switch (status)
                {
                    case eStatus.Todas:
                        sbSQL.Append("ORDER BY ID_Alteracao DESC ");
                        break;
                    case eStatus.Realizadas:
                        sbSQL.Append("where realizada='S' ORDER BY ID_Alteracao DESC");
                        break;
                    case eStatus.Pendentes:
                        sbSQL.Append("where realizada='N' ORDER BY ID_Alteracao DESC");
                        break;
                    default:
                        break;
                }


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAlteracao _Alt = new EAlteracao();
                    _Alt.Campo = Conversao.ToString(item["Campo"]);
                    _Alt.Data = Conversao.ToDateTime(item["Data"]).Value;
                    _Alt.Descricao = Conversao.ToString(item["Descricao"]);
                    _Alt.ID_Alteracao = Conversao.ToInt(item["ID_Alteracao"]).Value;
                    _Alt.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _Alt.ID_Administrador = Conversao.ToInt(item["ID_Administrador"]).Value;
                    _Alt.NomeAdministrador = item["Nome"].ToString();
                    _Alt.DataRealizada = Conversao.ToDateTime(item["DataRealizada"]).Value;
                    _Alt.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _Alt.Ip = Conversao.ToString(item["Ip"]);
                    _EAlteracao.Add(_Alt);
                }

                return _EAlteracao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lista as Solicitações de Alteração
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <param name="status">Tipo de Filtro: Todas, Pendentes ou Realizadas</param>
        /// <returns>Lista com as alterações constantes no filtro</returns>
        public BindingList<EAlteracao> Listar(int _ID_Cliente, eStatus status)
        {
            try
            {
                BindingList<EAlteracao> _EAlteracao = new BindingList<EAlteracao>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Alteracao, ID_Cliente, Data, Campo,Alteracao.Ip, Alteracao.Tipo, Descricao, DataRealizada, Alteracao.ID_Administrador, Nome ");
                sbSQL.Append("FROM (Administrador INNER JOIN Login ON Administrador.ID_Login = Login.ID_Login) ");
                sbSQL.Append("RIGHT JOIN Alteracao ON Administrador.ID_Administrador = Alteracao.ID_Administrador ");
                sbSQL.Append("WHERE ID_Cliente = " + _ID_Cliente.ToString());


                switch (status)
                {
                    case eStatus.Todas:
                        break;
                    case eStatus.Realizadas:
                        sbSQL.Append(" AND realizada='S' ");
                        break;
                    case eStatus.Pendentes:
                        sbSQL.Append(" AND realizada='N' ");
                        break;
                    default:
                        break;
                }

                sbSQL.Append(" ORDER BY ID_Alteracao DESC");


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAlteracao _Alt = new EAlteracao();
                    _Alt.ID_Alteracao = Conversao.ToInt(item["ID_Alteracao"]).Value;
                    _Alt.Data = Conversao.ToDateTime(item["Data"]).Value;
                    _Alt.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _Alt.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _Alt.Campo = Conversao.ToString(item["Campo"]);
                    _Alt.Descricao = Conversao.ToString(item["Descricao"]);
                    _Alt.ID_Administrador = Conversao.ToInt(item["ID_Administrador"]);
                    _Alt.NomeAdministrador = item["Nome"].ToString();
                    _Alt.DataRealizada = Conversao.ToDateTime(item["DataRealizada"]);
                    _Alt.Ip = Conversao.ToString(item["Ip"]);
                    _EAlteracao.Add(_Alt);
                }

                return _EAlteracao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        /// <summary>
        /// Lista as Solicitações de Alteração
        /// </summary>
        /// <param name="_ID_Cliente">Id do Cliente</param>
        /// <param name="status">Tipo de Filtro: Todas, Pendentes ou Realizadas</param>
        /// <returns>Lista com as alterações constantes no filtro</returns>
        public BindingList<EAlteracao> Listar(string cpf)
        {
            try
            {
                BindingList<EAlteracao> _EAlteracao = new BindingList<EAlteracao>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Alteracao, ID_Cliente, Data, Campo,Alteracao.Ip, Alteracao.Tipo, Descricao, DataRealizada, Alteracao.ID_Administrador, Nome ");
                sbSQL.Append("FROM (Administrador INNER JOIN Login ON Administrador.ID_Login = Login.ID_Login) ");
                sbSQL.Append("RIGHT JOIN Alteracao ON Administrador.ID_Administrador = Alteracao.ID_Administrador ");
                sbSQL.Append("WHERE ID_Cliente = (select id_cliente from cliente where cpf = '" + cpf + "')");
               sbSQL.Append(" ORDER BY ID_Alteracao DESC");


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAlteracao _Alt = new EAlteracao();
                    _Alt.ID_Alteracao = Conversao.ToInt(item["ID_Alteracao"]).Value;
                    _Alt.Data = Conversao.ToDateTime(item["Data"]).Value;
                    _Alt.ID_Cliente = Conversao.ToInt(item["ID_Cliente"]).Value;
                    _Alt.Tipo = Conversao.ToChar(item["Tipo"]).Value;
                    _Alt.Campo = Conversao.ToString(item["Campo"]);
                    _Alt.Descricao = Conversao.ToString(item["Descricao"]);
                    _Alt.ID_Administrador = Conversao.ToInt(item["ID_Administrador"]);
                    _Alt.NomeAdministrador = item["Nome"].ToString();
                    _Alt.DataRealizada = Conversao.ToDateTime(item["DataRealizada"]);
                    _Alt.Ip = Conversao.ToString(item["Ip"]);
                    _EAlteracao.Add(_Alt);
                }

                return _EAlteracao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        /// <summary>
        /// Seleciona uma solicitação de Alteração
        /// </summary>
        /// <param name="_ID_Alteracao">Id da Solicitação de Alteração</param>
        /// <returns>Entidade Alteração com a Solicitação de Alteração Selecionada</returns>
        public EAlteracao Selecionar(int _ID_Alteracao)
        {
            try
            {

                EAlteracao _EAlteracao = new EAlteracao();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Alteracao, ID_Cliente, Data, Campo, Alteracao.Tipo,Ip, Descricao, DataRealizada, Alteracao.ID_Administrador, Nome ");
                sbSQL.Append("FROM (Administrador INNER JOIN Login ON Administrador.ID_Login = Login.ID_Login) ");
                sbSQL.Append("RIGHT JOIN Alteracao ON Administrador.ID_Administrador = Alteracao.ID_Administrador ");
                sbSQL.Append("WHERE ID_Alteracao = " + _ID_Alteracao.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count > 0)
                {
                    _EAlteracao.ID_Alteracao = Conversao.ToInt(_table.Rows[0]["ID_Alteracao"]).Value;
                    _EAlteracao.Campo = Conversao.ToString(_table.Rows[0]["Campo"]);
                    _EAlteracao.Data = Conversao.ToDateTime(_table.Rows[0]["Data"]);
                    _EAlteracao.Tipo = Conversao.ToChar(_table.Rows[0]["Tipo"]).Value;
                    _EAlteracao.Descricao = Conversao.ToString( _table.Rows[0]["Descricao"]);
                    _EAlteracao.ID_Cliente = Conversao.ToInt(_table.Rows[0]["ID_Cliente"]);
                    _EAlteracao.ID_Administrador = Conversao.ToInt(_table.Rows[0]["ID_Administrador"]);
                    _EAlteracao.NomeAdministrador = _table.Rows[0]["Nome"].ToString();
                    _EAlteracao.DataRealizada = Conversao.ToDateTime(_table.Rows[0]["DataRealizada"]);
                    _EAlteracao.Ip = Conversao.ToString(_table.Rows[0]["Ip"]);
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }

                return _EAlteracao;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inclui uma solicitação de Alteração
        /// </summary>
        /// <param name="alteracao">Entidade Alteração populada para a inclusão</param>
        /// <returns>A quantidade de registros incluidos</returns>
        public int Inserir(EAlteracao alteracao)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("INSERT INTO Alteracao ");
                sbSQL.Append("(ID_Alteracao ");
                sbSQL.Append(",ID_Cliente ");
                sbSQL.Append(",Data ");
                sbSQL.Append(",DataRealizada ");
                sbSQL.Append(",Campo ");
                sbSQL.Append(",Tipo ");
                sbSQL.Append(",Descricao ");
                sbSQL.Append(",Ip) ");
                sbSQL.Append("VALUES (");
                sbSQL.Append("seqalteracao.nextval ");
                sbSQL.Append("," + alteracao.ID_Cliente.ToString());
                sbSQL.Append("," + Conversao.ToDateTimeOracle(DateTime.Now));
                sbSQL.Append(",NULL");
                sbSQL.Append(",'" + alteracao.Campo + "'");
                sbSQL.Append(",'" + alteracao.Tipo + "'");
                sbSQL.Append(",'" + alteracao.Descricao + "'");
                sbSQL.Append(",'" + alteracao.Ip + "')");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Altera a Solicitação de Alteração
        /// </summary>
        /// <param name="_EAlteracao">Entidade à ser alterada</param>
        /// <param name="_Realizada">Status de realização</param>
        /// <param name="_ID_Login">Usuário que realizou a alteração</param>
        public void Alterar(EAlteracao _EAlteracao, eRealizada _Realizada, int _ID_Login)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                DbConnection conn = ConexaoAntigo.CreateIConnection();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("SELECT ID_Administrador ,Nome FROM Administrador ");
                sbSQL.Append("INNER JOIN Login ON Administrador.ID_Login = Login.ID_Login ");
                sbSQL.Append("WHERE Administrador.ID_Login = " + _ID_Login.ToString());

                DbCommand _DbCommandLogin = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommandLogin);

                if (_table.Rows.Count > 0)
                {
                    _EAlteracao.ID_Administrador = Conversao.ToInt(_table.Rows[0]["ID_Administrador"]);
                    _EAlteracao.NomeAdministrador = _table.Rows[0]["Nome"].ToString();
                    _EAlteracao.DataRealizada = DateTime.Now;
                }
                else
                    throw new Exception("Usuário sem permissão de administrador!");

                sbSQL = new StringBuilder();
                sbSQL.Append("UPDATE Alteracao ");
                if (_Realizada == eRealizada.Não)
                {
                    sbSQL.Append("SET DataRealizada = NULL ");
                    sbSQL.Append(", ID_Administrador = NULL");
                }
                else
                {
                    sbSQL.Append("SET DataRealizada = " + Conversao.ToDateTimeOracle(_EAlteracao.DataRealizada));
                    sbSQL.Append(", ID_Administrador = " + _EAlteracao.ID_Administrador.ToString());
                }
                
                sbSQL.Append(" WHERE ID_Alteracao = " + _EAlteracao.ID_Alteracao.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                
                _AcessaDados.ExecuteNonQuery(_DbCommand);

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
