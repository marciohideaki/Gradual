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
    public class NAssessor{


        public  const int  IDASSESSORDELA = 305;


         /// <summary>
        /// Lista todos os assessores
        /// </summary>
        /// <returns>Lista com todos os assessores</returns>
        public BindingList<EAssessor> Listar()
        {
            try
            {

                BindingList<EAssessor> _EAssessor = new BindingList<EAssessor>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Assessor,ID_Login,filial FROM Assessor");


                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAssessor _Ass = new EAssessor();
                    _Ass.ID_Assessor = Conversao.ToInt(item["ID_Assessor"]).Value;
                    _Ass.ID_Login = Conversao.ToInt(item["ID_Login"]).Value;
                    _Ass.ID_AssessorSinacor = Conversao.ToInt(_table.Rows[0]["ID_AssessorSinacor"]).Value;

                    _EAssessor.Add(_Ass);
                }

                return _EAssessor;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Inclui todos os assessores em uma filial
        /// </summary>
        /// <param name="_ID_Filial">Id da Filial</param>
        /// <param name="_ID_AssessorIncluir">Lista  com o Id de todos os assessores a serem incluídos</param>
        /// <returns>Informação se a inclusão foi realizada com sucesso</returns>
        public Boolean Salvar(int _ID_Filial, List<int> _ID_AssessorIncluir)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                ConexaoAntigo._ConnectionStringName = ConexaoAntigo.ConnectionName;
                using (DbConnection conn = ConexaoAntigo.CreateIConnection())
                {
                    conn.Open();
                    DbTransaction _Transaction = conn.BeginTransaction();

                    try
                    {
                        DbCommand _DbCommand;
                        if (_ID_AssessorIncluir.Count == 0)
                            _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, "delete from assessorfilial where id_filial = " + _ID_Filial.ToString());
                        else
                        {
                            string naoExcluir = "";
                            foreach (int item in _ID_AssessorIncluir)
                            {
                                naoExcluir += item.ToString() + ",";
                            }
                            naoExcluir = naoExcluir.Substring(0, naoExcluir.Length - 1);
                            _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, "delete from assessorfilial where id_filial = " + _ID_Filial.ToString() + " and id_assessor not in (" + naoExcluir + ")");
                        }
                        _DbCommand.Transaction = _Transaction;
                        if (_ID_AssessorIncluir.Count > 0)
                        {
                            _AcessaDados.ExecuteScalar(_DbCommand, conn);
                            StringBuilder sbSQLIncluir = null;
                            foreach (int item in _ID_AssessorIncluir)
                            {
                                AcessaDadosAntigo _AcessaDadosValidar = new AcessaDadosAntigo();
                                _AcessaDadosValidar.ConnectionStringName = ConexaoAntigo.ConnectionName;
                                DbCommand _ComandoValida = _AcessaDadosValidar.CreateCommand(CommandType.Text, "select count(*) from assessorfilial where id_assessor=" + item.ToString() + " and id_filial=" + _ID_Filial.ToString());
                                int existe = Conversao.ToInt(_AcessaDadosValidar.ExecuteScalar(_ComandoValida)).Value;
                                if (existe == 0)
                                {

                                    sbSQLIncluir = new StringBuilder();
                                    sbSQLIncluir.Append("insert into assessorfilial(id_assessorfilial,id_assessor,id_filial) ");
                                    sbSQLIncluir.Append(" values ");
                                    sbSQLIncluir.Append("( seqAssessorFilial.nextval," + item.ToString() + "," + _ID_Filial.ToString() + ") ");
                                    DbCommand _Command = _AcessaDados.CreateCommand(CommandType.Text, sbSQLIncluir.ToString());
                                    _Command.Transaction = _Transaction;
                                    _AcessaDados.ExecuteScalar(_Command, conn);
                                    _Command = null;
                                    sbSQLIncluir = null;
                                }
                            }
                        }
                        _Transaction.Commit();
                        _Transaction.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _Transaction.Rollback();
                        _Transaction.Dispose();
                        throw (ex);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }              
        /// <summary>
        /// Lista todos os assessores de uma determinada Filial
        /// </summary>
        /// <param name="_ID_Filial">Id da Filial</param>
        /// <returns>Lista com todos os assessores da filial selecionada</returns>
        public BindingList<EAssessorFilial> ListarPorFilial(int _ID_Filial)
        {
            try
            {

                BindingList<EAssessorFilial> _EAssessorFilial = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT a.ID_Assessor,l.nome ");
                sbSQL.Append(" FROM Assessor a, assessorFilial af,login l ");
                sbSQL.Append(" where a.id_assessor = af.id_assessor and l.ID_login = a.ID_login ");
                sbSQL.Append(" and af.id_filial = " + _ID_Filial.ToString());
                sbSQL.Append(" order by l.nome ");
                
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _Ass = new EAssessorFilial();
                    _Ass.ID_Assessor = Conversao.ToInt(item["ID_Assessor"]).Value;
                    _Ass.NomeAssessor = Conversao.ToString(item["nome"]);
                    _EAssessorFilial.Add(_Ass);
                }

                return _EAssessorFilial;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        /// <summary>
        /// Lista todos os assessores que não estão em uma determinada filial
        /// </summary>
        /// <param name="_ID_Filial">Id da Filial</param>
        /// <returns>Lista com todos os assessores que não estão na filial informada</returns>
        public BindingList<EAssessorFilial> ListarForaDaFilial(int _ID_Filial)
        {
            try
            {

                BindingList<EAssessorFilial> _EAssessorFilial = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT a.ID_Assessor,l.nome ");
                sbSQL.Append(" FROM Assessor a, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and a.id_assessor not in ");
                sbSQL.Append(" (select id_assessor from assessorfilial where id_filial = " + _ID_Filial + " ) ");
                sbSQL.Append(" order by l.nome ");
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _Ass = new EAssessorFilial();
                    _Ass.ID_Assessor = Conversao.ToInt(item["ID_Assessor"]).Value;
                    _Ass.NomeAssessor = Conversao.ToString(item["nome"]);

                    _EAssessorFilial.Add(_Ass);
                }

                return _EAssessorFilial;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Seleciona um assessor
        /// </summary>
        /// <param name="_ID_Assessor">Id do Assessor à ser selecionado</param>
        /// <returns>Entidade contentdo todos os dados do Assessor selecionado</returns>
        public EAssessor Selecionar(int _ID_Assessor)
        {
            try
            {
                EAssessor _EAssessor = new EAssessor();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("SELECT ID_Assessor,ID_Login , filial FROM Assessor where ID_Assessor = " + _ID_Assessor.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                if (_table.Rows.Count > 0)
                {
                    _EAssessor.ID_Assessor = Conversao.ToInt(_table.Rows[0]["ID_Assessor"]).Value;
                    _EAssessor.ID_Login = Conversao.ToInt(_table.Rows[0]["ID_Login"]).Value;
                    _EAssessor.ID_AssessorSinacor = Conversao.ToInt(_table.Rows[0]["ID_AssessorSinacor"]).Value;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }
                return _EAssessor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Excluí um assessor
        /// </summary>
        /// <param name="_ID_Assessor">Id do assessor à ser excluído</param>
        /// <returns>Quantidade de registros excluídos no Banco de Dados</returns>
        public int Excluir(int _ID_Assessor)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("delete from Assessor ");
                sbSQL.Append("where ID_Assessor = " + _ID_Assessor.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Retorna o código do assessor no Sinacor de um determinado Cliente
        /// </summary>
        /// <param name="_CodigoClienteDuc">Código do Cliente</param>
        /// <returns>Código do assessor no Sinacor</returns>
        public string GetCodigoAssessorSinacor(int _CodigoClienteDuc) {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("SELECT Assessor.ID_AssessorSinacor ");
                sbSQL.Append("FROM Assessor, assessorfilial, cliente ");
                sbSQL.Append(" where assessorfilial.id_assessorfilial=cliente.id_assessorfilial and ");
                sbSQL.Append(" assessor.id_assessor= assessorfilial.id_assessor and ");
                sbSQL.Append(" cliente.ID_cliente = " + _CodigoClienteDuc.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                string retorno =Conversao.ToString( _AcessaDados.ExecuteScalar(_DbCommand));
                if (retorno.Trim().Length>0)
                {
                    return retorno;
                }
                else
                {
                    throw new Exception("REGISTRONAOENCONTRADO");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
