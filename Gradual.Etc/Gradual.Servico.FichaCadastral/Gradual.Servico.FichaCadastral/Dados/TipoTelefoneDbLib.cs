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
    public class TipoTelefoneDbLib : DbLibBase
    {
        public ConsultarObjetosResponse<TipoTelefoneInfo> ConsultarTipoTelefone(ConsultarEntidadeRequest<TipoTelefoneInfo> pParametros)
        {
            try
            {
                var resposta =
                    new ConsultarObjetosResponse<TipoTelefoneInfo>();

                var lAcessaDados = new AcessaDados();

                lAcessaDados.ConnectionStringName = gNomeConexaoCadastro;

                using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "tipo_telefone_lst_sp"))
                {
                    DataTable lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                    if (null != lDataTable && lDataTable.Rows.Count > 0)
                    {
                        for (int i = 0; i <= lDataTable.Rows.Count - 1; i++)
                        {
                            DataRow linha = lDataTable.NewRow();

                            linha["id_tipo_telefone"] = Convert.ToInt32(lDataTable.Rows[i]["id_tipo_telefone"]);
                            linha["ds_telefone"] = Convert.ToString(lDataTable.Rows[i]["ds_telefone"]);
                            resposta.Resultado.Add(CriarRegistroTipoTelefone(linha));
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

        private TipoTelefoneInfo CriarRegistroTipoTelefone(DataRow linha)
        {
            TipoTelefoneInfo lTipoDependencia = new TipoTelefoneInfo();

            lTipoDependencia.IdTipoTelefone = linha["id_tipo_telefone"].DBToInt32();
            lTipoDependencia.DsTelefone = linha["ds_telefone"].DBToString();

            return lTipoDependencia;

        }
    }
}
