using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados.Cadastro;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteNaoOperaPorContaPropriaDbLib : DbLibBase
    {
        public ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo> ConsultarClienteNaoOperaPorContaPropria(ReceberEntidadeRequest<ClienteNaoOperaPorContaPropriaInfo> pParametros)
        {
            var lRetorno = new ReceberObjetoResponse<ClienteNaoOperaPorContaPropriaInfo>();
            var lAcessaDados = new AcessaDados();

            lRetorno.Objeto = new ClienteNaoOperaPorContaPropriaInfo();
            lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_NaoOperaPorContaPropria_sel_sp"))
            {
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
            }

            return lRetorno;
        }
    }
}
