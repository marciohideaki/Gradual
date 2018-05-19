using System;
using System.Data;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data.Common;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<PendenciaClienteAssessorInfo> ConsultarPendenciaClienteAssessor(ConsultarEntidadeRequest<PendenciaClienteAssessorInfo> pParametros)
        {
            try
            {
                var lRetorno = new ConsultarObjetosResponse<PendenciaClienteAssessorInfo>();
                var lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (var lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PendenciaClienteAssessor_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable) foreach (DataRow linha in lDataTable.Rows)
                            lRetorno.Resultado.Add(new PendenciaClienteAssessorInfo()
                            {
                                CdBmfBovespa = linha["cd_codigo"].DBToInt32(),
                                TpPendenciaDescricao = linha["ds_pendencia"].DBToString(),
                                DescricaoPendencia = linha["ds_pendenciacadastral"].DBToString(),
                                NomeCliente = linha["ds_nome"].DBToString(),
                                CpfCnpjCliente = linha["ds_cpfcnpj"].DBToString(),
                                EmailCliente = linha["ds_emailcliente"].DBToString(),
                                IdCliente = linha["id_cliente"].DBToInt32(),
                            });
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }
    }
}
