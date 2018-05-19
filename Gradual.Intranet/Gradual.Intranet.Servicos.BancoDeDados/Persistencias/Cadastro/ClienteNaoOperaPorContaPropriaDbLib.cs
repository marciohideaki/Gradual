using System;
using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> ConsultarClienteNaoOperaPorContaPropria(ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros, DbTransaction pTransaction = null)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno.Objeto = new ClienteNaoOperaPorContaPropriaInfo();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            DbCommand lDbCommand;

            if(pTransaction == null)
            {
                lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_NaoOperaPorContaPropria_sel_sp");
            }
            else
            {
                lDbCommand = lAcessaDados.CreateCommand(pTransaction, CommandType.StoredProcedure, "cliente_NaoOperaPorContaPropria_sel_sp");
            }

            lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

            var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

            if (null != lDataTable && lDataTable.Rows.Count > 0)
                lRetorno.Objeto = new ClienteNaoOperaPorContaPropriaInfo()
                {
                    DsCpfCnpjClienteRepresentado = lDataTable.Rows[0]["ds_cpfcnpjClienteRepresentado"].DBToString(),
                    DsNomeClienteRepresentado = lDataTable.Rows[0]["ds_nomeClienteRepresentado"].DBToString(),
                    IdCliente = pParametros.Objeto.IdCliente,
                    IdClienteNaoOperaPorContaPropria = lDataTable.Rows[0]["id_cliente_NaoOperaPorContaPropria"].DBToInt32(),
                };

            return lRetorno;
        }

        public static ClienteNaoOperaPorContaPropriaInfo ConsultarClienteNaoOperaPorContaPropria(int IdCliente)
        {
            var lRetorno = new ClienteNaoOperaPorContaPropriaInfo();
            var lAcessaDados = new ConexaoDbHelper();

            lRetorno = new ClienteNaoOperaPorContaPropriaInfo();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_NaoOperaPorContaPropria_sel_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, IdCliente);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lRetorno = new ClienteNaoOperaPorContaPropriaInfo()
                    {
                        DsCpfCnpjClienteRepresentado     = lDataTable.Rows[0]["ds_cpfcnpjClienteRepresentado"].DBToString(),
                        DsNomeClienteRepresentado        = lDataTable.Rows[0]["ds_nomeClienteRepresentado"].DBToString(),
                        IdCliente                        = IdCliente,
                        IdClienteNaoOperaPorContaPropria = lDataTable.Rows[0]["id_cliente_NaoOperaPorContaPropria"].DBToInt32(),
                    };
            }

            return lRetorno;
        }

        public static SalvarObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> SalvarClienteNaoOperaPorContaPropria(SalvarObjetoRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros)
        {
            var lRetorno = new SalvarObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();

            return lRetorno;
        }

        public static SalvarObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> SalvarClienteNaoOperaPorContaPropria(SalvarObjetoRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros, DbTransaction pDbTransaction)
        {
            var lRetorno = new SalvarObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();
            var lAcessaDados = new ConexaoDbHelper();
            var lNomeProc = string.Empty;

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            lNomeProc = ValidarInsersao(pParametros.Objeto.IdCliente, pDbTransaction)
                      ? "cliente_NaoOperaPorContaPropria_ins_sp"
                      : "cliente_NaoOperaPorContaPropria_upd_sp";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, lNomeProc))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_nomeClienteRepresentado", DbType.String, pParametros.Objeto.DsNomeClienteRepresentado);
                lAcessaDados.AddInParameter(lDbCommand, "@ds_cpfcnpjClienteRepresentado", DbType.String, pParametros.Objeto.DsCpfCnpjClienteRepresentado.ToCpfCnpjSemPontuacao());
                lAcessaDados.AddInParameter(lDbCommand, "@dt_inclusao", DbType.DateTime, DateTime.Now);
                lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
            }

            return lRetorno;
        }

        public static RemoverObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> ExcluirClienteNaoOperaPorContaPropria(RemoverEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros)
        {
            var lRetorno = new RemoverObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();

            return lRetorno;
        }

        public static RemoverObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> ExcluirClienteNaoOperaPorContaPropria(RemoverEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros, DbTransaction pDbTransaction)
        {
            var lRetorno = new RemoverObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(pDbTransaction, CommandType.StoredProcedure, "cliente_NaoOperaPorContaPropria_del_sp"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);
                lAcessaDados.ExecuteNonQuery(lDbCommand, pDbTransaction);
            }
            
            return lRetorno;
        }

        private static bool ValidarInsersao(int pIdCliente, DbTransaction pTransaction)
        {
            var lDadosOperaPor =  ConsultarClienteNaoOperaPorContaPropria(new ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo>() { Objeto = new ClienteNaoOperaPorContaPropriaInfo() { IdCliente = pIdCliente } }, pTransaction);

            return string.IsNullOrWhiteSpace(lDadosOperaPor.Objeto.DsCpfCnpjClienteRepresentado) || string.IsNullOrWhiteSpace(lDadosOperaPor.Objeto.DsNomeClienteRepresentado);
        }
    }
}
