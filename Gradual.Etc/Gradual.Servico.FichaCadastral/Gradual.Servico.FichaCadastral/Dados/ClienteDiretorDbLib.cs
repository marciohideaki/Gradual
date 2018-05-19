using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Library;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteDiretorDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteDiretorInfo> ConsultarClienteDiretor(ConsultarEntidadeRequest<ClienteDiretorInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteDiretorInfo> resposta =
                    new ConsultarObjetosResponse<ClienteDiretorInfo>();

                CondicaoInfo info = new CondicaoInfo("@id_cliente", CondicaoTipoEnum.Igual, pParametros.Objeto.IdCliente);
                pParametros.Condicoes.Add(info);

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_diretor_lst_sp"))
                {
                    foreach (CondicaoInfo condicao in pParametros.Condicoes)
                    {
                        lAcessaDados.AddInParameter(lDbCommand, condicao.Propriedade, DbType.Int32, condicao.Valores[0]);

                        DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                        if (null != lDataTable && lDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                            {
                                DataRow linha = lDataTable.NewRow();

                                linha["ds_cpfcnpj"] = (lDataTable.Rows[i]["ds_cpfcnpj"]).DBToString();
                                linha["ds_identidade"] = (lDataTable.Rows[i]["ds_identidade"]).DBToString();
                                linha["ds_nome"] = (lDataTable.Rows[i]["ds_nome"]).DBToString();
                                linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                                linha["id_cliente_diretor"] = (lDataTable.Rows[i]["id_cliente_diretor"]).DBToInt32();
                                linha["cd_orgaoemissor"] = (lDataTable.Rows[i]["cd_orgaoemissor"]).DBToString();
                                linha["cd_uforgaoemissordocumento"] = (lDataTable.Rows[i]["cd_uforgaoemissordocumento"]).DBToString();

                                resposta.Resultado.Add(CriarRegistroClienteDiretorInfo(linha));
                            }

                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                //LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static ClienteDiretorInfo CriarRegistroClienteDiretorInfo(DataRow linha)
        {
            var lClienteDiretorInfo = new ClienteDiretorInfo();

            lClienteDiretorInfo.NrCpfCnpj = linha["ds_cpfcnpj"].DBToString();
            lClienteDiretorInfo.DsIdentidade = linha["ds_identidade"].DBToString();
            lClienteDiretorInfo.DsNome = linha["ds_nome"].DBToString();
            lClienteDiretorInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteDiretorInfo.IdClienteDiretor = linha["id_cliente_diretor"].DBToInt32();
            lClienteDiretorInfo.CdUfOrgaoEmissor = linha["cd_uforgaoemissordocumento"].DBToString();
            lClienteDiretorInfo.CdOrgaoEmissor = linha["cd_orgaoemissor"].DBToString();
            
            return lClienteDiretorInfo;
        }
    }
}
