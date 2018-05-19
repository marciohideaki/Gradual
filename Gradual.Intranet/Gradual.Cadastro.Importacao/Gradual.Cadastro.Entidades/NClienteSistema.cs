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

    public class NClienteSistema
    {


        
        /// TODO
        /// Retirar Assessor
        /// fazer o papel da Trigger de alterar/incluir para principal




        private string strConn = "CadastroOracle";

        public Boolean Inserir(EClienteSistema objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_ins");
            _AcessaDados.AddInParameter(_DbCommand, "Id_cliente", DbType.Int32, objEntidade.ID_Cliente);
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, objEntidade.ID_Sistema);
            _AcessaDados.AddInParameter(_DbCommand, "Conta", DbType.String, objEntidade.Conta);
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, objEntidade.ID_TipoConta);
            _AcessaDados.AddInParameter(_DbCommand, "AssessorSinacor", DbType.Int32, objEntidade.AssessorSinacor);
            OracleParameter pPrincipal = new OracleParameter("Principal", OracleType.Char);
            pPrincipal.Value = objEntidade.Principal;
            pPrincipal.Direction = ParameterDirection.Input;
            _DbCommand.Parameters.Add(pPrincipal);
            OracleParameter pAtiva = new OracleParameter("Ativa", OracleType.Char);
            pAtiva.Value = objEntidade.Ativa;
            pAtiva.Direction = ParameterDirection.Input;
            _DbCommand.Parameters.Add(pAtiva);
            _AcessaDados.AddOutParameter(_DbCommand, "Id_clientesistema", DbType.Int32, 22);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            objEntidade.ID_ClienteSistema = Convert.ToInt32(_AcessaDados.GetParameterValue(_DbCommand, "ID_CLIENTESISTEMA"));
            AtualizaPrincipal(objEntidade.ID_Cliente.Value, objEntidade.ID_ClienteSistema.Value, objEntidade.Principal.Value,objEntidade.ID_Sistema.Value);
            return true;
        }


        private Boolean AtualizaPrincipal(int id_cliente, int id_clientesistema,char principal,int id_sistema) {
            if (principal != 'S')
                return true;
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clisis_atl_princ");
            _AcessaDados.AddInParameter(_DbCommand, "Id_clientesistema", DbType.Int32, id_clientesistema);
            _AcessaDados.AddInParameter(_DbCommand, "Id_cliente", DbType.Int32, id_cliente);
            _AcessaDados.AddInParameter(_DbCommand, "id_sistema", DbType.Int32, id_sistema);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }


        public Boolean Alterar(EClienteSistema objEntidade)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_upd");
            _AcessaDados.AddInParameter(_DbCommand, "Id_clientesistema", DbType.Int32, objEntidade.ID_ClienteSistema);
            _AcessaDados.AddInParameter(_DbCommand, "Id_cliente", DbType.Int32, objEntidade.ID_Cliente);
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, objEntidade.ID_Sistema);
            _AcessaDados.AddInParameter(_DbCommand, "Conta", DbType.String, objEntidade.Conta);
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, objEntidade.ID_TipoConta);
            _AcessaDados.AddInParameter(_DbCommand, "AssessorSinacor", DbType.Int32, objEntidade.AssessorSinacor);
            OracleParameter pPrincipal = new OracleParameter("Principal", OracleType.Char);
            pPrincipal.Value = objEntidade.Principal;
            pPrincipal.Direction = ParameterDirection.Input;
            _DbCommand.Parameters.Add(pPrincipal);
            OracleParameter pAtiva = new OracleParameter("Ativa", OracleType.Char);
            pAtiva.Value = objEntidade.Ativa;
            pAtiva.Direction = ParameterDirection.Input;
            _DbCommand.Parameters.Add(pAtiva);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            AtualizaPrincipal(objEntidade.ID_Cliente.Value, objEntidade.ID_ClienteSistema.Value, objEntidade.Principal.Value, objEntidade.ID_Sistema.Value);
            return true;
        }

        public Boolean Excluir(int _Id_clientesistema)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_del");
            _AcessaDados.AddInParameter(_DbCommand, "Id_clientesistema", DbType.Int32, _Id_clientesistema);
            _AcessaDados.ExecuteNonQuery(_DbCommand);
            return true;
        }

        public BindingList<EClienteSistema> Listar()
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_lst");
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<EClienteSistema> BLRetorno = new BindingList<EClienteSistema>();
            foreach (DataRow item in _table.Rows)
            {
                EClienteSistema linha = new EClienteSistema();
                linha.ID_ClienteSistema = Conversao.ToInt(item["Id_clientesistema"]).Value;
                linha.ID_Cliente = Conversao.ToInt(item["Id_cliente"]).Value;
                linha.ID_Sistema = Conversao.ToInt(item["Id_sistema"]).Value;
                linha.Conta = Conversao.ToString(item["Conta"]);
                linha.ID_TipoConta = Conversao.ToInt(item["Id_tipoconta"]).Value;
                linha.Principal = Conversao.ToChar(item["Principal"]).Value;
                linha.Ativa = Conversao.ToChar(item["Ativa"]).Value;
                linha.AssessorSinacor = Conversao.ToInt(item["AssessorSinacor"]).Value;
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }

        public EClienteSistema Selecionar(int _Id_clientesistema)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_sel");
            _AcessaDados.AddInParameter(_DbCommand, "Id_clientesistema", DbType.Int32, _Id_clientesistema);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            EClienteSistema Retorno = new EClienteSistema();
            if (_table.Rows.Count > 0)
            {
                Retorno.ID_ClienteSistema = Conversao.ToInt(_table.Rows[0]["Id_clientesistema"]).Value;
                Retorno.ID_Cliente = Conversao.ToInt(_table.Rows[0]["Id_cliente"]).Value;
                Retorno.ID_Sistema = Conversao.ToInt(_table.Rows[0]["Id_sistema"]).Value;
                Retorno.Conta = Conversao.ToString(_table.Rows[0]["Conta"]);
                Retorno.ID_TipoConta = Conversao.ToInt(_table.Rows[0]["Id_tipoconta"]).Value;
                Retorno.Principal = Conversao.ToChar(_table.Rows[0]["Principal"]).Value;
                Retorno.Ativa = Conversao.ToChar(_table.Rows[0]["Ativa"]).Value;
                Retorno.AssessorSinacor = Conversao.ToInt(_table.Rows[0]["AssessorSinacor"]).Value;
            }
            else
            {
                throw new Exception("Registro não encontrado!");
            }
            return Retorno;
        }

        public BindingList<EClienteSistema> ListarPorSistema(int _Id_sistema)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clientesistema_lst_sistema");
            _AcessaDados.AddInParameter(_DbCommand, "Id_sistema", DbType.Int32, _Id_sistema);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<EClienteSistema> BLRetorno = new BindingList<EClienteSistema>();
            foreach (DataRow item in _table.Rows)
            {
                EClienteSistema linha = new EClienteSistema();
                linha.ID_Cliente = Conversao.ToInt(item["Id_cliente"]).Value;
                linha.Conta = Conversao.ToString(item["Conta"]);
                linha.Principal = Conversao.ToChar(item["Principal"]).Value;
                linha.ID_TipoConta = Conversao.ToInt(item["Id_tipoconta"]).Value;
                linha.ID_Sistema = Conversao.ToInt(item["Id_sistema"]).Value;
                linha.ID_ClienteSistema = Conversao.ToInt(item["Id_clientesistema"]).Value;
                linha.Ativa = Conversao.ToChar(item["Ativa"]).Value;
                linha.AssessorSinacor = Conversao.ToInt(item["AssessorSinacor"]).Value;
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }

        public BindingList<EClienteSistema> ListarPorTipoConta(int _Id_tipoconta)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.prc_clisistema_lst_tpconta");
            _AcessaDados.AddInParameter(_DbCommand, "Id_tipoconta", DbType.Int32, _Id_tipoconta);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<EClienteSistema> BLRetorno = new BindingList<EClienteSistema>();
            foreach (DataRow item in _table.Rows)
            {
                EClienteSistema linha = new EClienteSistema();
                linha.ID_Cliente = Conversao.ToInt(item["Id_cliente"]).Value;
                linha.Conta = Conversao.ToString(item["Conta"]);
                linha.Principal = Conversao.ToChar(item["Principal"]).Value;
                linha.ID_TipoConta = Conversao.ToInt(item["Id_tipoconta"]).Value;
                linha.ID_Sistema = Conversao.ToInt(item["Id_sistema"]).Value;
                linha.ID_ClienteSistema = Conversao.ToInt(item["Id_clientesistema"]).Value;
                linha.Ativa = Conversao.ToChar(item["Ativa"]).Value;
                linha.AssessorSinacor = Conversao.ToInt(item["AssessorSinacor"]).Value;
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }


        /// <summary>
        /// Lista as contas de um cliente
        /// </summary>
        /// <param name="_Id_cliente">Id do Cliente</param>
        /// <param name="ativo">S=Lista ativos,N=Losta inativos,null=lista todos</param>
        /// <returns>Lista com as contas do cliente levando em conta o filtro: S=Lista ativos,N=Losta inativos,null=lista todos</returns>
        public BindingList<EClienteSistema> ListarPorCliente(int _Id_cliente,char? ativo)
        {
            AcessaDadosAntigo _AcessaDados = new AcessaDadosAntigo();
            _AcessaDados.ConnectionStringName = this.strConn;
            DbCommand _DbCommand = _AcessaDados.CreateCommand(CommandType.StoredProcedure, "cadastro.PRC_CLISISTEMA_LST_CLIENTE");
            _AcessaDados.AddInParameter(_DbCommand, "Id_cliente", DbType.Int32, _Id_cliente);
            OracleParameter pAtiva = new OracleParameter("Ativa", OracleType.Char);
            pAtiva.Value = ativo;
            pAtiva.Direction = ParameterDirection.Input;
            _DbCommand.Parameters.Add(pAtiva);
            DataTable _table = _AcessaDados.ExecuteOracleDataTable(_DbCommand);
            BindingList<EClienteSistema> BLRetorno = new BindingList<EClienteSistema>();
            foreach (DataRow item in _table.Rows)
            {
                EClienteSistema linha = new EClienteSistema();
                linha.ID_Cliente = Conversao.ToInt(item["Id_cliente"]).Value;
                linha.Conta = Conversao.ToString(item["Conta"]);
                linha.Principal = Conversao.ToChar(item["Principal"]).Value;
                linha.ID_TipoConta = Conversao.ToInt(item["Id_tipoconta"]).Value;
                linha.ID_Sistema = Conversao.ToInt(item["Id_sistema"]).Value;
                linha.ID_ClienteSistema = Conversao.ToInt(item["Id_clientesistema"]).Value;
                linha.Ativa = Conversao.ToChar(item["Ativa"]).Value;
                linha.AssessorSinacor = Conversao.ToInt(item["AssessorSinacor"]).Value;
                BLRetorno.Add(linha);
            }
            return BLRetorno;
        }

    }
}
