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
    public class TipoEnderecoDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<TipoEnderecoInfo> ConsultarTipoEndereco(ConsultarEntidadeRequest<TipoEnderecoInfo> pParametros)
        {
            try
            {
                var resposta = new ConsultarObjetosResponse<TipoEnderecoInfo>();

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_endereco_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        DataRow linha;
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            linha = lDataTable.NewRow();

                            linha["id_tipo_endereco"] = Convert.ToInt32(lDataTable.Rows[i]["id_tipo_endereco"]);
                            linha["ds_endereco"] = Convert.ToString(lDataTable.Rows[i]["ds_endereco"]);
                            resposta.Resultado.Add(this.CriarRegistroTipoEndereco(linha));
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

        private TipoEnderecoInfo CriarRegistroTipoEndereco(DataRow linha)
        {
            var lTipoDependencia = new TipoEnderecoInfo();

            lTipoDependencia.IdTipoEndereco = linha["id_tipo_endereco"].DBToInt32();
            lTipoDependencia.DsEndereco = linha["ds_endereco"].DBToString();

            return lTipoDependencia;
        }
    }
}
