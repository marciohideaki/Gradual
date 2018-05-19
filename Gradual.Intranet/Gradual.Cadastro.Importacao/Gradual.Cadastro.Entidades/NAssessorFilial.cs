using System.ComponentModel;
using Gradual.Generico.Dados;
using Gradual.Cadastro.Entidades;
using System.Text;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Geral;
using System;

namespace Gradual.Cadastro.Negocios
{

    public class NAssessorFilial
    {
        /// <summary>
        /// Tipo de Filtro: Nome do Assessor, Nome da Filial
        /// </summary>
        public enum eTipoFiltro
        {
            NomeAssessor,
            NomeFilial
        }
        /// <summary>
        /// Lista os AssessoresFilial
        /// </summary>
        /// <param name="_tipoFiltro">Filtro por Nome ou Filial</param>
        /// <param name="_Filtro">Filtro</param>
        /// <returns>Lista com todos os AssessoresFilial</returns>
        public BindingList<EAssessorFilial> Listar(eTipoFiltro _tipoFiltro, string _Filtro)
        {
            try
            {

                BindingList<EAssessorFilial> _EAssessorFilial = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial and ");
                if (_tipoFiltro == eTipoFiltro.NomeAssessor)
                    sbSQL.Append(" upper(l.nome) like'%" + _Filtro.ToUpper() + "%' ");
                else
                    sbSQL.Append(" upper(f.nome) like '%" + _Filtro.ToUpper() + "%' ");
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _af = new EAssessorFilial();
                    _af.ID_Assessor = Conversao.ToInt(item["id_assessor"]).Value;
                    _af.ID_AssessorFilial = Conversao.ToInt(item["ID_AssessorFilial"]).Value;
                    _af.ID_Filial = Conversao.ToInt(item["ID_Filial"]).Value;
                    _af.NomeAssessor = Conversao.ToString(item["nome"]);
                    _af.NomeFilial = Conversao.ToString(item["filial"]);
                    _af.NomeFilial_NomeAssessor = Conversao.ToString(item["filialAssessor"]);

                    _EAssessorFilial.Add(_af);
                }

                return _EAssessorFilial;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista todos os AssessoresFilial
        /// </summary>
        /// <returns>Lista contendo todos os AssessoresFilial</returns>
        public BindingList<EAssessorFilial> Listar()
        {
            try
            {
                BindingList<EAssessorFilial> _EassFil = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT distinct af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(", f.nome || ' - ' || a.id_assessorsinacor || ' - ' || l.nome as filialSinacorAssessor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial ");
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _af = new EAssessorFilial();
                    _af.ID_Assessor = Conversao.ToInt(item["id_assessor"]).Value;
                    _af.ID_AssessorFilial = Conversao.ToInt(item["ID_AssessorFilial"]).Value;
                    _af.ID_Filial = Conversao.ToInt(item["ID_Filial"]).Value;
                    _af.NomeAssessor = Conversao.ToString(item["nome"]);
                    _af.NomeFilial = Conversao.ToString(item["filial"]);
                    _af.NomeFilial_NomeAssessor = Conversao.ToString(item["filialAssessor"]);
                    _af.NomeFilial_CodigoSinacorAssessor_NomeAssessor = Conversao.ToString(item["filialSinacorAssessor"]);
                    _EassFil.Add(_af);
                }
                return _EassFil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista todos os AssessoresFilial de um determinado Login
        /// </summary>
        /// <param name="_id_Login">Id Login</param>
        /// <returns>Lista com todos os AssessoresFilial de um determinado Login</returns>
        public BindingList<EAssessorFilial> Listar(int _id_Login)
        {
            try
            {
                BindingList<EAssessorFilial> _EassFil = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(", f.nome || ' - ' || a.id_assessorsinacor || ' - ' || l.nome as filialSinacorAssessor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial and ");
                sbSQL.Append(" l.id_login=" + _id_Login.ToString());
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _af = new EAssessorFilial();
                    _af.ID_Assessor = Conversao.ToInt(item["id_assessor"]).Value;
                    _af.ID_AssessorFilial = Conversao.ToInt(item["ID_AssessorFilial"]).Value;
                    _af.ID_Filial = Conversao.ToInt(item["ID_Filial"]).Value;
                    _af.NomeAssessor = Conversao.ToString(item["nome"]);
                    _af.NomeFilial = Conversao.ToString(item["filial"]);
                    _af.NomeFilial_NomeAssessor = Conversao.ToString(item["filialAssessor"]);
                    _af.NomeFilial_CodigoSinacorAssessor_NomeAssessor = Conversao.ToString(item["filialSinacorAssessor"]);

                    _EassFil.Add(_af);
                }
                return _EassFil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista todos os AssessoresFilial de um determinado Assessor
        /// </summary>
        /// <param name="_ID_Assessor">Id do Assessor</param>
        /// <returns>Lista contendo todos os AssessoresFilial do Assessor listado</returns>
        public BindingList<EAssessorFilial> ListarPorAsessor(int _ID_Assessor)
        {
            try
            {
                BindingList<EAssessorFilial> _EassFil = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial ");
                sbSQL.Append(" and a.id_assessor = " + _ID_Assessor);
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _af = new EAssessorFilial();
                    _af.ID_Assessor = Conversao.ToInt(item["id_assessor"]).Value;
                    _af.ID_AssessorFilial = Conversao.ToInt(item["ID_AssessorFilial"]).Value;
                    _af.ID_Filial = Conversao.ToInt(item["ID_Filial"]).Value;
                    _af.NomeAssessor = Conversao.ToString(item["nome"]);
                    _af.NomeFilial = Conversao.ToString(item["filial"]);
                    _af.NomeFilial_NomeAssessor = Conversao.ToString(item["filialAssessor"]);

                    _EassFil.Add(_af);
                }

                return _EassFil;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lista todos os AssessoresFilial de uma determinada Filial
        /// </summary>
        /// <param name="_ID_Filial">Id da Filial</param>
        /// <returns>Lista contendo todos os AssessoresFilial da Filial listada</returns>
        public BindingList<EAssessorFilial> ListarPorFilial(int _ID_Filial)
        {
            try
            {
                BindingList<EAssessorFilial> _EassFil = new BindingList<EAssessorFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial ");
                sbSQL.Append(" and f.id_filial = " + _ID_Filial);
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);
                foreach (DataRow item in _table.Rows)
                {
                    EAssessorFilial _af = new EAssessorFilial();
                    _af.ID_Assessor = Conversao.ToInt(item["id_assessor"]).Value;
                    _af.ID_AssessorFilial = Conversao.ToInt(item["ID_AssessorFilial"]).Value;
                    _af.ID_Filial = Conversao.ToInt(item["ID_Filial"]).Value;
                    _af.NomeAssessor = Conversao.ToString(item["nome"]);
                    _af.NomeFilial = Conversao.ToString(item["filial"]);
                    _af.NomeFilial_NomeAssessor = Conversao.ToString(item["filialAssessor"]);

                    _EassFil.Add(_af);
                }

                return _EassFil;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Seleciona um AssessorFilial
        /// </summary>
        /// <param name="_ID_AssessorFilial">Id do AssessorFilial</param>
        /// <returns>Entidade contendo todos os dados do AssessorFilial informado no filtro</returns>
        public EAssessorFilial Selecionar(int _ID_AssessorFilial)
        {
            try
            {
                EAssessorFilial _EassFil = new EAssessorFilial();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT af.ID_AssessorFilial ");
                sbSQL.Append(", a.ID_Assessor ");
                sbSQL.Append(", l.nome ");
                sbSQL.Append(", f.id_filial ");
                sbSQL.Append(", f.nome as filial ");
                sbSQL.Append(", f.nome || ' - ' || l.nome as filialAssessor ");
                sbSQL.Append(", id_assessorSinacor ");
                sbSQL.Append(" FROM filial f, assessor a, assessorfilial af, login l ");
                sbSQL.Append(" where l.id_login = a.id_login and ");
                sbSQL.Append(" a.id_assessor = af.id_assessor and ");
                sbSQL.Append(" af.id_filial = f.id_filial ");
                sbSQL.Append(" and af.ID_AssessorFilial = " + _ID_AssessorFilial);
                sbSQL.Append(" order by f.nome,l.nome ");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count == 0)
                    throw new Exception("REGISTRONAOENCONTRADO");

                _EassFil.ID_Assessor = Conversao.ToInt(_table.Rows[0]["id_assessor"]).Value;
                _EassFil.ID_AssessorFilial = Conversao.ToInt(_table.Rows[0]["ID_AssessorFilial"]).Value;
                _EassFil.ID_Filial = Conversao.ToInt(_table.Rows[0]["ID_Filial"]).Value;
                _EassFil.NomeAssessor = Conversao.ToString(_table.Rows[0]["nome"]);
                _EassFil.NomeFilial = Conversao.ToString(_table.Rows[0]["filial"]);
                _EassFil.NomeFilial_NomeAssessor = Conversao.ToString(_table.Rows[0]["filialAssessor"]);
                _EassFil.ID_AssessorSinacor = Conversao.ToInt(_table.Rows[0]["id_assessorSinacor"]);
                return _EassFil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Inseri um AssessorFilial
        /// </summary>
        /// <param name="_AssessorFilial">Entidade contendo todos os dados de AssessorFilial</param>
        /// <returns>Quantidade de linhas afetadas no Banco de Dados</returns>
        public int Inserir(EAssessorFilial _AssessorFilial)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append(" INSERT INTO AssessorFilial ");
                sbSQL.Append(" (ID_AssessorFilial ");
                sbSQL.Append(" ,ID_Assessor ");
                sbSQL.Append(" ,ID_Filial) ");
                sbSQL.Append(" VALUES ");
                sbSQL.Append(" ( seqAssessorFilial.nextval ");
                sbSQL.Append(" ," + _AssessorFilial.ID_Assessor.ToString());
                sbSQL.Append(" ," + _AssessorFilial.ID_Filial.ToString() + ")");

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Excluí um registro de AssessorFilial
        /// </summary>
        /// <param name="_ID_AssessorFilial">Id do AssessorFilial a ser excluído</param>
        /// <returns>Quantidade de registros excluídos no Banco de Dados</returns>
        public int Excluir(int _ID_AssessorFilial)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" delete from assessorfilial ");
                sbSQL.Append("  WHERE ID_AssessorFilial = " + _ID_AssessorFilial.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                return _AcessaDados.ExecuteNonQuery(_DbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable MigrarClientes(int _AssessorFilialDe)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" select nome,cpf from cliente,login where login.id_login=cliente.id_login and ");
                sbSQL.Append(" id_assessorfilial = " + _AssessorFilialDe.ToString());
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());
                DataTable dt = _AcessaDados.ExecuteDbDataTable (_DbCommand);
                 return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int MigrarClientes(int _AssessorFilialDe, int _AssessorFilialPara)
        {
            try
            {
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;
                StringBuilder sbSQL = new StringBuilder();
                sbSQL = new StringBuilder();
                sbSQL.Append(" update cliente set id_assessorfilial = " + _AssessorFilialPara.ToString());
                sbSQL.Append(" where id_assessorfilial = " + _AssessorFilialDe.ToString());
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
