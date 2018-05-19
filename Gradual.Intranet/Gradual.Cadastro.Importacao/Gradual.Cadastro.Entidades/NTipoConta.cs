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

    public class NTipoConta
    {

        private string strConn = "CadastroOracle";

        public Boolean Inserir(ETipoConta objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_tipoconta_ins");
            _AcessaDados.AddInParameter(_DbCommand, "Nome", DbType.String, objEntidade.Nome);
            _AcessaDados.AddOutParameter(_DbCommand, "Id_tipoconta", DbType.Int32, 22);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            objEntidade.ID_TipoConta = Convert.ToInt32(_AcessaDados.GetParameterValue(_DbCommand, "ID_TIPOCONTA"));
            return true;
        }

        public Boolean Alterar(ETipoConta objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_tipoconta_upd");
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, objEntidade.ID_TipoConta);
            _AcessaDados.AddInParameter(_DbCommand, "Nome", DbType.String, objEntidade.Nome);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }

        public Boolean Excluir(int _Id_tipoconta)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_tipoconta_del");
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, _Id_tipoconta);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }

        public BindingList<ETipoConta> Listar()
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_tipoconta_lst");
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<ETipoConta> BLRetorno = new BindingList<ETipoConta>();
            foreach (DataRow item in _table.Rows)
            {
                ETipoConta linha = new ETipoConta();
                linha.ID_TipoConta = Conversao.ToInt(item["Id_tipoconta"]).Value;
                linha.Nome = Conversao.ToString(item["Nome"]);
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }

        public ETipoConta Selecionar(int _Id_tipoconta)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_tipoconta_sel");
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, _Id_tipoconta);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            ETipoConta Retorno = new ETipoConta();
            if (_table.Rows.Count > 0)
            {
                Retorno.ID_TipoConta = Conversao.ToInt(_table.Rows[0]["Id_tipoconta"]).Value;
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
