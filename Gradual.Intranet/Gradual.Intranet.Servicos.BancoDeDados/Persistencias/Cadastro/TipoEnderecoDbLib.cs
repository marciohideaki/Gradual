using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;
using System.Data;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;


namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias
{
    public static partial class ClienteDbLib
    {
        public static ConsultarObjetosResponse<TipoEnderecoInfo> ConsultarTipoEndereco(ConsultarEntidadeRequest<TipoEnderecoInfo> pParametros)
        {
            try
            {
                ConsultarObjetosResponse<TipoEnderecoInfo> resposta =
                    new ConsultarObjetosResponse<TipoEnderecoInfo>();

                ConexaoDbHelper lAcessaDados = new ConexaoDbHelper();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_endereco_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_tipo_endereco"] = Convert.ToInt32(lDataTable.Rows[i]["id_tipo_endereco"]);
                            linha["ds_endereco"] = Convert.ToString(lDataTable.Rows[i]["ds_endereco"]);
                            resposta.Resultado.Add(CriarRegistroTipoEndereco(linha));
                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                LogCadastro.Logar(pParametros.Objeto, pParametros.IdUsuarioLogado, pParametros.DescricaoUsuarioLogado, LogCadastro.eAcao.Consultar, ex);
                throw ex;
            }
        }

        private static TipoEnderecoInfo CriarRegistroTipoEndereco(DataRow linha)
        {
            TipoEnderecoInfo lTipoDependencia = new TipoEnderecoInfo();

            lTipoDependencia.IdTipoEndereco = linha["id_tipo_endereco"].DBToInt32();
            lTipoDependencia.DsEndereco = linha["ds_endereco"].DBToString();

            return lTipoDependencia;

        }


    }
}
