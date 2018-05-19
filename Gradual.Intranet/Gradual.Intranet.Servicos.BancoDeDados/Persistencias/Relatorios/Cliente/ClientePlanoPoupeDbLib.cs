using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.Cliente;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<ClientePlanoPoupeInfo> ConsultarClienteProdutoPoupeRel(ConsultarEntidadeRequest<ClientePlanoPoupeInfo> pParametros)
        {
            var lRetorno = new ConsultarObjetosResponse<ClientePlanoPoupeInfo>();

            ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();
            lAcessaDados.ConnectionStringName = gNomeConexaoRendaFixa;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_plano_lst"))
            {
                lAcessaDados.AddInParameter(lDbCommand, "@cd_cliente", DbType.Int32, pParametros.Objeto.ConsultaCdCliente);
                lAcessaDados.AddInParameter(lDbCommand, "@id_produto", DbType.Int32, pParametros.Objeto.ConsultaIdProduto);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_inicio", DbType.DateTime, pParametros.Objeto.ConsultaDtInicio);
                lAcessaDados.AddInParameter(lDbCommand, "@dt_fim", DbType.DateTime, pParametros.Objeto.ConsultaDtFim);

                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                        lRetorno.Resultado.Add(new ClientePlanoPoupeInfo()
                        {
                            DsNome = lLinha["ds_nome"].DBToString(),
                           
                            DsAtivo = lLinha["id_ativo"].DBToString(),
                            CdCliente = lLinha["id_cliente"].DBToInt32(),
                            IdProduto = lLinha["id_produto"].DBToInt32(),
                            DsCpfCnpj = lLinha["ds_cpfcnpj"].DBToString(),
                            DsProduto = lLinha["ds_produto"].DBToString(),
                            DtAdesao = lLinha["dt_adesao"].DBToDateTime(),
                            DtVencimento = lLinha["dt_vencimento"].DBToDateTime(),
                            DtRetroTrocaAtivo = lLinha["dt_retro_troca_ativo"].DBToDateTime(),
                        });
            }

            return lRetorno;
        }
    }
}
