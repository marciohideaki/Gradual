using System;
using System.Data;
using System.Configuration;
using System.Text;
using System.ComponentModel;
using System.Data.SqlClient;
using Gradual.Cadastro.Entidades;
using Gradual.Generico.Dados;
using System.Data.Common;
using Gradual.Generico.Geral;
using System.Data.OracleClient;

namespace Gradual.Cadastro.Negocios
{

    public class NSistema
    {

        private string strConn = "CadastroOracle";

        public Boolean Inserir(ESistema objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_sistema_ins");
            _AcessaDados.AddInParameter(_DbCommand, "Nome", DbType.String, objEntidade.Nome);
            _AcessaDados.AddOutParameter(_DbCommand, "Id_sistema", DbType.Int32, 22);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            objEntidade.ID_Sistema = Convert.ToInt32(_AcessaDados.GetParameterValue(_DbCommand, "ID_SISTEMA"));
            return true;
        }

        public Boolean Alterar(ESistema objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_sistema_upd");
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, objEntidade.ID_Sistema);
            _AcessaDados.AddInParameter(_DbCommand, "Nome", DbType.String, objEntidade.Nome);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }

        public Boolean Excluir(int _Id_sistema)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_sistema_del");
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, _Id_sistema);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }

        public BindingList<ESistema> Listar()
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_sistema_lst");
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<ESistema> BLRetorno = new BindingList<ESistema>();
            foreach (DataRow item in _table.Rows)
            {
                ESistema linha = new ESistema();
                linha.ID_Sistema = Conversao.ToInt(item["Id_sistema"]).Value;
                linha.Nome = Conversao.ToString(item["Nome"]);
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }

        public ESistema Selecionar(int _Id_sistema)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_sistema_sel");
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, _Id_sistema);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            ESistema Retorno = new ESistema();
            if (_table.Rows.Count > 0)
            {
                Retorno.ID_Sistema = Conversao.ToInt(_table.Rows[0]["Id_sistema"]).Value;
                Retorno.Nome = Conversao.ToString(_table.Rows[0]["Nome"]);
            }
            else
            {
                throw new Exception("Registro não encontrado!");
            }
            return Retorno;
        }

    }
}
