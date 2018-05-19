using System.ComponentModel;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Text;
using System.Data.Common;
using System.Data;
using Gradual.Generico.Geral;
using System;

namespace Gradual.Cadastro.Negocios
{
    public class NFilial
    {
        /// <summary>
        /// Lista todas as Filiais
        /// </summary>
        /// <returns>Lista contendo todas Filiais</returns>
        public BindingList<EFilial> Listar()
        {
            try
            {
                BindingList<EFilial> _EFilial = new BindingList<EFilial>();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Filial ");
                sbSQL.Append(",nome ");
                sbSQL.Append("FROM filial order by nome");
           
                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                foreach (DataRow item in _table.Rows)
                {
                    EFilial _Fil = new EFilial();
                    _Fil.ID_Filial= Conversao.ToInt(item["id_filial"]).Value;
                    _Fil.Nome = Conversao.ToString(item["nome"]);
                   

                    _EFilial.Add(_Fil);
                }

                return _EFilial;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Seleciona uma determinada Filial
        /// </summary>
        /// <param name="_ID_Filial">Id da Filial a ser Listada</param>
        /// <returns>Entidade contendo todos os dados da Filial</returns>
        public EFilial Selecionar(int _ID_Filial)
        {
            try
            {
                EFilial _EFilial = new EFilial();
                AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();

                _AcessaDados.ConnectionStringName = ConexaoAntigo.ConnectionName;

                StringBuilder sbSQL = new StringBuilder();

                sbSQL.Append("SELECT ID_Filial ");
                sbSQL.Append(",nome ");
                sbSQL.Append("FROM filial ");
                sbSQL.Append("where ID_Filial = " + _ID_Filial.ToString());

                DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.Text, sbSQL.ToString());

                DataTable _table = _AcessaDados.ExecuteDbDataTable(_DbCommand);

                if (_table.Rows.Count == 0)
                    throw new Exception("REGISTRONAOENCONTRADO");

                _EFilial.ID_Filial = Conversao.ToInt(_table.Rows[0]["id_filial"]);
                _EFilial.Nome= Conversao.ToString(_table.Rows[0]["nome"]);

                return _EFilial;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
