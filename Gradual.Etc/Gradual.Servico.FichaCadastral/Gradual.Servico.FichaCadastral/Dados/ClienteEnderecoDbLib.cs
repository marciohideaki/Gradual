using System;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;
using Gradual.Servico.FichaCadastral.Lib;

namespace Gradual.Servico.FichaCadastral.Dados
{
    public class ClienteEnderecoDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<ClienteEnderecoInfo> ConsultarClienteEndereco(ConsultarEntidadeRequest<ClienteEnderecoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<ClienteEnderecoInfo> resposta =
                    new ConsultarObjetosResponse<ClienteEnderecoInfo>();

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "cliente_endereco_lst_sp"))
                {
                    lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, pParametros.Objeto.IdCliente);

                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        DataRow linha;
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            linha = lDataTable.NewRow();

                            linha["ds_bairro"] = (lDataTable.Rows[i]["ds_bairro"]).DBToString();
                            linha["ds_cidade"] = (lDataTable.Rows[i]["ds_cidade"]).DBToString();
                            linha["ds_complemento"] = (lDataTable.Rows[i]["ds_complemento"]).DBToString();
                            linha["ds_logradouro"] = (lDataTable.Rows[i]["ds_logradouro"]).DBToString();
                            linha["ds_numero"] = (lDataTable.Rows[i]["ds_numero"]).DBToString();
                            linha["cd_pais"] = (lDataTable.Rows[i]["cd_pais"]).DBToString();
                            linha["cd_uf"] = (lDataTable.Rows[i]["cd_uf"]).DBToString();
                            linha["id_cliente"] = (lDataTable.Rows[i]["id_cliente"]).DBToInt32();
                            linha["id_endereco"] = (lDataTable.Rows[i]["id_endereco"]).DBToInt32();
                            linha["id_tipo_endereco"] = (lDataTable.Rows[i]["id_tipo_endereco"]).DBToInt32();
                            linha["cd_cep"] = (lDataTable.Rows[i]["cd_cep"]).DBToInt32();
                            linha["cd_cep_ext"] = (lDataTable.Rows[i]["cd_cep_ext"]).DBToInt32();
                            linha["st_principal"] = bool.Parse(lDataTable.Rows[i]["st_principal"].ToString());

                            resposta.Resultado.Add(CriarRegistroClienteEnderecoInfo(linha));
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

        private static ClienteEnderecoInfo CriarRegistroClienteEnderecoInfo(DataRow linha)
        {
            ClienteEnderecoInfo lClienteEnderecoInfo = new ClienteEnderecoInfo();

            lClienteEnderecoInfo.DsBairro = linha["ds_bairro"].DBToString();
            lClienteEnderecoInfo.DsCidade = linha["ds_cidade"].DBToString();
            lClienteEnderecoInfo.DsComplemento = linha["ds_complemento"].DBToString();
            lClienteEnderecoInfo.DsLogradouro = linha["ds_logradouro"].DBToString();
            lClienteEnderecoInfo.DsNumero = linha["ds_numero"].DBToString();
            lClienteEnderecoInfo.CdPais = linha["cd_pais"].DBToString();
            lClienteEnderecoInfo.CdUf = linha["cd_uf"].DBToString();
            lClienteEnderecoInfo.IdCliente = linha["id_cliente"].DBToInt32();
            lClienteEnderecoInfo.IdEndereco = linha["id_endereco"].DBToInt32();
            lClienteEnderecoInfo.IdTipoEndereco = linha["id_tipo_endereco"].DBToInt32();
            lClienteEnderecoInfo.NrCep = linha["cd_cep"].DBToInt32();
            lClienteEnderecoInfo.NrCepExt = linha["cd_cep_ext"].DBToInt32();
            lClienteEnderecoInfo.StPrincipal = bool.Parse(linha["st_principal"].ToString());

            return lClienteEnderecoInfo;
        }
    }
}
