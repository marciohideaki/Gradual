using System;
using System.Data;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;
using Gradual.OMS.Library;
using System.Data.Common;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class PessoaVinculadaDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClientePessoaVinculadaPorClienteInfo> ConsultarPessoaVinculadaPorCliente(ConsultarEntidadeRequest<ClientePessoaVinculadaPorClienteInfo> pParametros)
        {
            try
            {
                var lRetorno = new ConsultarObjetosResponse<ClientePessoaVinculadaPorClienteInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);

                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "pessoa_vinculada_por_cliente_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                    var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            lRetorno.Resultado.Add(new ClientePessoaVinculadaPorClienteInfo()
                            {
                                DsCpfCnpj = lDataTable.Rows[i]["ds_cpfcnpj"].DBToString(),
                                DsNome = lDataTable.Rows[i]["ds_nome"].DBToString(),
                                IdCliente = lDataTable.Rows[i]["id_cliente"].DBToInt32(),
                                StAtivo = lDataTable.Rows[i]["st_ativo"].DBToBoolean(),
                            });
                        }
                    }
                }

                return lRetorno;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }
    }
}
